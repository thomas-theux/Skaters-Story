using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rewired;

public class TricksController : MonoBehaviour {

    public SkateboardController SkateboardControllerScript;
    public Animator SkateboardAnim;
    private Player player;

    public TMP_Text TrickPoints;
    public TMP_Text TrickName;

    public bool IsBailing = false;
    public bool IsPerformingTrick = false;

    private float eraseTrickNameTimeFail = 0.2f;
    private float eraseTrickNameTimeSuccess = 1.5f;

    public int GainRespect = 0;

    public int performedTricks = 0;
    private List<string> performedTricksArr = new List<string>();

    private List<TrickCombination> TrickCombinationsArr = new List<TrickCombination>();

    private float buttonResetTime = 0.4f;

    // Main button ints
    public int whichDirectionPressed = -1;
    public int whichModifierPressed = -1;

    private bool directionPressed = false;
    private bool modifierPressed = false;

    private bool squarePressed = false;

    // REWIRED
    private bool SquareButton = false;


    private void Awake () {
        player = ReInput.players.GetPlayer(SkateboardControllerScript.PlayerID);
    }


    private void Update() {
        GetInput();

        // Check for DPad activity
        if (getFirstActiveDpad() > -1) {
            whichDirectionPressed = getFirstActiveDpad();
            directionPressed = true;
            StartCoroutine(ButtonResetDelay());
        }

        // Check for modifier button activity
        if (getFirstActiveModifier() > -1) {
            whichModifierPressed = getFirstActiveModifier();
            modifierPressed = true;
            StartCoroutine(ButtonResetDelay());
        }

        // Write combination into array if both buttons have been pressed
        if (directionPressed && modifierPressed) {
            SetNewCombination();
            ResetButtons();
        }

        // Check for new combinations in array and perform trick
        if (!IsPerformingTrick) {
            if (TrickCombinationsArr.Count > 0) {
                PerformTrick();
            }
        }

        // Check for success or failure when landing
        if (SkateboardControllerScript.IsGrounded) {
            if (IsPerformingTrick) {
                Bail();
            } else {
                AwardRespect();
            }
        }
    }


    private IEnumerator ButtonResetDelay() {
        yield return new WaitForSeconds(buttonResetTime);
        ResetButtons();
    }


    private void ResetButtons() {
        directionPressed = false;
        modifierPressed = false;

        whichDirectionPressed = -1;
        whichModifierPressed = -1;
    }


    private void SetNewCombination() {
        TrickCombination newCombination = new TrickCombination(whichDirectionPressed, whichModifierPressed);
        TrickCombinationsArr.Add(newCombination);
        // print(newCombination.directionValue + " + " + newCombination.modifierValue);
    }


    private void PerformTrick() {
        IsPerformingTrick = true;

        performedTricks++;

        int getDirBtn = TrickCombinationsArr[0].directionValue;
        int getModBtn = TrickCombinationsArr[0].modifierValue;

        int respectForTrick = 0;
        string performedTrickName = "";

        switch (getModBtn) {
            // FLIP TRICK
            case 0:
                List<FlipTricks> whichDirection = TricksManager.FlipTricksArr[getDirBtn];
                FlipTricks whichFlipTrick = whichDirection[TricksManager.FlipTricksLevel[getDirBtn]];

                whichFlipTrick.PlayAnimation(SkateboardAnim);

                respectForTrick = whichFlipTrick.respectGain;
                performedTrickName = whichFlipTrick.trickName;
                break;
            // GRIND TRICK
            case 1:
                print("some grind trick");
                break;
        }

        GainRespect += respectForTrick;
        performedTricksArr.Add(performedTrickName);

        string allPerformedTricksText = "";
        
        for (int i = 0; i < performedTricksArr.Count; i++) {
            if (i > 0) allPerformedTricksText += " + ";
            allPerformedTricksText += performedTricksArr[i];
        }

        // Display trick name(s)
        TrickName.text = allPerformedTricksText;

        // Remove trick from array
        TrickCombinationsArr.RemoveAt(0);
    }


    private void TrickDone() {
        IsPerformingTrick = false;

        SkateboardControllerScript.gameObject.transform.rotation = Quaternion.Euler(
            0.0f,
            0.0f,
            SkateboardControllerScript.gameObject.transform.eulerAngles.z
        );
    }

    private void BailDone() {
        IsBailing = false;

        // Enable rotation and position constraints
        SkateboardControllerScript.rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        SkateboardControllerScript.RespawnAfterBail();
    }


    private void Bail() {
        IsBailing = true;
        IsPerformingTrick = false;

        GainRespect = 0;
        performedTricks = 0;
        performedTricksArr.Clear();
        TrickCombinationsArr.Clear();

        // Disable rotation and position constraints
        SkateboardControllerScript.rb.constraints = RigidbodyConstraints.None;

        // Instantly erase trick names when player bails
        StartCoroutine(EraseTrickName(eraseTrickNameTimeFail));

        SkateboardAnim.SetTrigger("Bail");
    }


    private void AwardRespect() {
        if (GainRespect > 0) {
            SkateboardControllerScript.CharacterSheetScript.NewRespectValue += GainRespect;
            SkateboardControllerScript.CharacterSheetScript.IncreasingRespect = true;

            GainRespect = 0;
            performedTricks = 0;
            performedTricksArr.Clear();
            TrickCombinationsArr.Clear();

            // Erase trick names after a few seconds
            StartCoroutine(EraseTrickName(eraseTrickNameTimeSuccess));
        }
    }


    private IEnumerator EraseTrickName(float eraseTrickTime) {
        yield return new WaitForSeconds(eraseTrickTime);
        
        if (!IsPerformingTrick) {
            TrickName.text = "";
        }
    }


    private void GetInput() {        
        SquareButton = player.GetButton("Square");
    }


    public class TrickCombination {
        public int directionValue;
        public int modifierValue;

        public TrickCombination(int dir, int mod) {
                directionValue = dir;
                modifierValue = mod;
            }
    }


    public bool[] getDpad() {
        return new bool[4] {
            player.GetButton("DPad Left"),
            player.GetButton("DPad Up"),
            player.GetButton("DPad Right"),
            player.GetButton("DPad Down")
        };
    }


    public int getFirstActiveDpad() {
        bool[] dpad = this.getDpad();

        int returnValue = -1;

        for (int i = 0; i < 4; i++) {
            if (dpad[i]) returnValue = i;
        }

        return returnValue;
    }


    public bool[] getModifier() {
        return new bool[2] {
            player.GetButtonDown("Square"),
            player.GetButtonDown("Triangle")
        };
    }


    public int getFirstActiveModifier() {
        bool[] modifier = this.getModifier();

        int returnValue = -1;

        for (int i = 0; i < 2; i++) {
            if (modifier[i]) returnValue = i;
        }

        return returnValue;
    }

}
