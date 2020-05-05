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
    
    public bool PerformsTrick = false;
    public bool PerformsFlipTrick = false;
    public bool PerformsGrindTrick = false;

    private int tricksArrLimit = 2;

    private float eraseTrickNameTimeFail = 0.2f;
    private float eraseTrickNameTimeSuccess = 1.5f;

    private int respectForTrick = 0;
    private string performedTrickName = "";
    public int GainRespect = 0;

    public int performedTricks = 0;
    private List<string> performedTricksArr = new List<string>();

    private List<TrickCombination> trickCombinationsArr = new List<TrickCombination>();

    private float buttonResetTime = 0.4f;

    // Main button ints
    public int whichDirectionPressed = -1;
    public int whichModifierPressed = -1;

    private bool directionPressed = false;
    private bool modifierPressed = false;

    private bool squarePressed = false;
    private bool trianglePressed = false;

    // REWIRED
    private bool SquareButton = false;
    private bool TriangleButton = false;


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
            if (trickCombinationsArr.Count < tricksArrLimit) {
                SetNewCombination();
            }
            ResetButtons();
        }

        // Check for new combinations in array and perform trick
        if (!PerformsTrick) {
            if (trickCombinationsArr.Count > 0) {
                PerformTrick();
            }
        }

        // Check for success or failure when landing
        if (SkateboardControllerScript.IsGrounded) {
            if (PerformsFlipTrick) {
                Bail();
            } else {
                AwardRespect();
            }

            if (PerformsGrindTrick) {
                AwardRespect();
            }
        }

        // Set grind to done when player gets off the rail
        if (!SkateboardControllerScript.IsOnRail) {
            if (PerformsGrindTrick) {
                TrickDone();
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
        trickCombinationsArr.Add(newCombination);
    }


    private void PerformTrick() {
        int getDirBtn = trickCombinationsArr[0].directionValue;
        int getModBtn = trickCombinationsArr[0].modifierValue;

        switch (getModBtn) {

            // FLIP TRICK
            case 0:
                if (!SkateboardControllerScript.IsGrounded) {
                    DoFlipTrick(getDirBtn);
                    DisplayTrickName();
                }
                break;

            // GRIND TRICK
            case 1:
                if (SkateboardControllerScript.IsOnRail) {
                    DoGrindTrick(getDirBtn);
                    DisplayTrickName();
                }
                break;
            
        }
    }


    private void DisplayTrickName() {
        performedTricks++;

        GainRespect += respectForTrick;
        performedTricksArr.Add(performedTrickName);

        string allPerformedTricksText = "";
        
        for (int i = 0; i < performedTricksArr.Count; i++) {
            if (i > 0) allPerformedTricksText += " + ";
            allPerformedTricksText += performedTricksArr[i];
        }

        // Display trick name(s)
        TrickName.text = allPerformedTricksText;
    }


    private void DoFlipTrick(int getDirBtn) {
        PerformsTrick = true;
        PerformsFlipTrick = true;

        List<FlipTricks> whichDirection = TricksManager.FlipTricksArr[getDirBtn];
        FlipTricks whichFlipTrick = whichDirection[TricksManager.FlipTricksLevel[getDirBtn]];

        whichFlipTrick.PlayAnimation(SkateboardAnim);

        respectForTrick = whichFlipTrick.respectGain;
        performedTrickName = whichFlipTrick.trickName;
    }


    private void DoGrindTrick(int getDirBtn) {
        PerformsTrick = true;
        PerformsGrindTrick = true;

        List<GrindTricks> whichDirection = TricksManager.GrindTricksArr[getDirBtn];
        GrindTricks whichFlipTrick = whichDirection[TricksManager.GrindTricksLevel[getDirBtn]];

        // SkateboardControllerScript.rb.useGravity = false;
        // SkateboardControllerScript.rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        whichFlipTrick.PlayAnimation(SkateboardAnim);

        respectForTrick = whichFlipTrick.respectGain;
        performedTrickName = whichFlipTrick.trickName;
    }


    private void TrickDone() {
        // Remove trick from array
        if (trickCombinationsArr.Count > 0) {
            trickCombinationsArr.RemoveAt(0);
        }

        // SkateboardControllerScript.rb.useGravity = true;
        // SkateboardControllerScript.rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        PerformsTrick = false;
        PerformsFlipTrick = false;
        PerformsGrindTrick = false;

        // SkateboardControllerScript.gameObject.transform.rotation = Quaternion.Euler(
        //     0.0f,
        //     0.0f,
        //     SkateboardControllerScript.gameObject.transform.eulerAngles.z
        // );
    }

    private void Bail() {
        IsBailing = true;
        PerformsTrick = false;
        PerformsFlipTrick = false;

        GainRespect = 0;
        performedTricks = 0;
        performedTricksArr.Clear();
        trickCombinationsArr.Clear();

        // Disable rotation and position constraints
        // SkateboardControllerScript.rb.constraints = RigidbodyConstraints.None;

        // Instantly erase trick names when player bails
        StartCoroutine(EraseTrickName(eraseTrickNameTimeFail));

        SkateboardAnim.SetTrigger("Bail");
    }


    private void BailDone() {
        IsBailing = false;

        // Enable rotation and position constraints
        // SkateboardControllerScript.rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        SkateboardControllerScript.RespawnAfterBail();
    }


    private void AwardRespect() {
        if (GainRespect > 0) {
            SkateboardControllerScript.CharacterSheetScript.NewRespectValue += GainRespect;
            SkateboardControllerScript.CharacterSheetScript.IncreasingRespect = true;

            GainRespect = 0;
            performedTricks = 0;
            performedTricksArr.Clear();
            trickCombinationsArr.Clear();

            // Erase trick names after a few seconds
            StartCoroutine(EraseTrickName(eraseTrickNameTimeSuccess));
        }
    }


    private IEnumerator EraseTrickName(float eraseTrickTime) {
        yield return new WaitForSeconds(eraseTrickTime);
        
        if (!PerformsTrick) {
            TrickName.text = "";
        }
    }


    private void GetInput() {
        SquareButton = player.GetButton("Square");
        TriangleButton = player.GetButton("Triangle");
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
