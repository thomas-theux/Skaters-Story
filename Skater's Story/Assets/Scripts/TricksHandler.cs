using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rewired;

public class TricksHandler : MonoBehaviour {

    public SkateboardController SkateboardControllerScript;
    public TimeManager TimeManagerScript;
    public Animator SkateboardAnim;
    private Player player;

    public TMP_Text TrickName;

    public bool PerformsTrick = false;
    public bool PerformsManual = false;
    public bool IsBailing = false;

    private int performedTricks = 0;
    private int tricksArrLimit = 2;
    private List<string> performedTricksArr = new List<string>();

    private int respectForTrick = 0;
    public int GainRespect = 0;

    private float eraseTrickNameTimeFail = 0.2f;
    private float eraseTrickNameTimeSuccess = 1.5f;

    private float resetDelayTime = 0.3f;

    // Definition for tricks combination values
    // -1 = no direction; 0 = left; 1 = up; 2 = right; 3 = down
    // 0 = !IsGrounded; 1 = CanGrind; 2 = IsGrounded

    private List<TrickCombination> trickCombinationsArr = new List<TrickCombination>();

    public class TrickCombination {
        public int whichTrick;
        public int trickCondition;

        public TrickCombination(int trick, int cond) {
            whichTrick = trick;
            trickCondition = cond;
        }
    }

    // Button presses
    public bool SquareButtonPressed = false;
    public bool TriangleButtonPressed = false;

    public bool DPadLeftPressed = false;
    public bool DPadUpPressed = false;
    public bool DPadRightPressed = false;
    public bool DPadDownPressed = false;

    // REWIRED
    private bool SquareButton = false;
    private bool TriangleButton = false;

    private bool DPadLeft = false;
    private bool DPadUp = false;
    private bool DPadRight = false;
    private bool DPadDown = false;


    void Awake() {
        player = ReInput.players.GetPlayer(SkateboardControllerScript.PlayerID);
    }


    private void Update() {
        GetInput();
        ActivateButtonPresses();
        ResetButtonPresses();
        CheckForCombination();

        StartPerforming();
        AwardOrBail();
    }


    private void GetInput() {
        SquareButton = player.GetButtonDown("Square");
        TriangleButton = player.GetButtonDown("Triangle");

        DPadLeft = player.GetButton("DPad Left");
        DPadUp = player.GetButton("DPad Up");
        DPadRight = player.GetButton("DPad Right");
        DPadDown = player.GetButton("DPad Down");
    }


    private void ActivateButtonPresses() {
        if (SquareButton) SquareButtonPressed = true;
        if (TriangleButton) TriangleButtonPressed = true;

        if (DPadLeft) DPadLeftPressed = true;
        if (DPadUp) DPadUpPressed = true;
        if (DPadRight) DPadRightPressed = true;
        if (DPadDown) DPadDownPressed = true;
    }


    private void ResetButtonPresses() {
        if (SquareButtonPressed) StartCoroutine(ResetAfterDelay(0));
        if (TriangleButtonPressed) StartCoroutine(ResetAfterDelay(1));

        if (DPadLeftPressed) StartCoroutine(ResetAfterDelay(2));
        if (DPadUpPressed) StartCoroutine(ResetAfterDelay(3));
        if (DPadRightPressed) StartCoroutine(ResetAfterDelay(4));
        if (DPadDownPressed) StartCoroutine(ResetAfterDelay(5));
    }


    private IEnumerator ResetAfterDelay(int resetThisButton) {
        yield return new WaitForSeconds(resetDelayTime);

        switch (resetThisButton) {
            case 0:
                SquareButtonPressed = false;
                break;
            case 1:
                TriangleButtonPressed = false;
                break;

            case 2:
                DPadLeftPressed = false;
                break;
            case 3:
                DPadUpPressed = false;
                break;
            case 4:
                DPadRightPressed = false;
                break;
            case 5:
                DPadDownPressed = false;
                break;
        }

        // ResetButtons();
    }


    private void CheckForCombination() {
        if (trickCombinationsArr.Count < tricksArrLimit) {

            ///////////////////////////
            // FLIP TRICKS
            ///////////////////////////

            if (SquareButtonPressed && DPadLeftPressed) {
                // print("Kickflip");
                TrickCombination newCombination = new TrickCombination(0, 0);
                trickCombinationsArr.Add(newCombination);
                ResetButtons();
            }

            if (SquareButtonPressed && DPadUpPressed) {
                // print("360 Flip");
                TrickCombination newCombination = new TrickCombination(1, 0);
                trickCombinationsArr.Add(newCombination);
                ResetButtons();
            }

            if (SquareButtonPressed && DPadRightPressed) {
                // print("Impossible");
                TrickCombination newCombination = new TrickCombination(2, 0);
                trickCombinationsArr.Add(newCombination);
                ResetButtons();
            }

            if (SquareButtonPressed && DPadDownPressed) {
                // print("Hardflip");
                TrickCombination newCombination = new TrickCombination(3, 0);
                trickCombinationsArr.Add(newCombination);
                ResetButtons();
            }

            ///////////////////////////
            // GRIND TRICKS
            ///////////////////////////

            // if (TriangleButtonPressed && DPadRightPressed) {
            //     // print("Nose Grind");
            //     TrickCombination newCombination = new TrickCombination(1, 1);
            //     trickCombinationsArr.Add(newCombination);
            //     ResetButtons();
            // }

            // if (TriangleButtonPressed) {
            //     // print("Nose Grind");
            //     TrickCombination newCombination = new TrickCombination(-1, 1);
            //     trickCombinationsArr.Add(newCombination);
            //     ResetButtons();
            // }

            ///////////////////////////
            // MANUAL TRICKS
            ///////////////////////////

            if (DPadLeftPressed && DPadRightPressed) {
                // print("Casper");
                TrickCombination newCombination = new TrickCombination(0, 2);
                trickCombinationsArr.Add(newCombination);
                ResetButtons();
            }

            if (DPadUpPressed && DPadDownPressed) {
                // print("Manual");
                TrickCombination newCombination = new TrickCombination(1, 2);
                trickCombinationsArr.Add(newCombination);
                ResetButtons();
            }

            // if (DPadRightPressed && DPadLeftPressed) {
            //     // print("Primo");
            //     TrickCombination newCombination = new TrickCombination(2, 2);
            //     trickCombinationsArr.Add(newCombination);
            //     ResetButtons();
            // }

            // if (DPadDownPressed && DPadUpPressed) {
            //     // print("Nose Manual");
            //     TrickCombination newCombination = new TrickCombination(3, 2);
            //     trickCombinationsArr.Add(newCombination);
            //     ResetButtons();
            // }

        }
    }


    private void ResetButtons() {
        SquareButtonPressed = false;
        TriangleButtonPressed = false;

        DPadLeftPressed = false;
        DPadUpPressed = false;
        DPadRightPressed = false;
        DPadDownPressed = false;
    }


    private void StartPerforming() {
        if (!PerformsTrick) {
            if (trickCombinationsArr.Count > 0) {
                int getCond = trickCombinationsArr[0].trickCondition;

                switch (getCond) {
                    case 0:
                        if (!SkateboardControllerScript.IsGrounded) DoFlipTrick();
                        break;
                    case 1:
                        if (SkateboardControllerScript.IsOnRail) DoGrindTrick();
                        break;
                    case 2:
                        if (SkateboardControllerScript.IsGrounded) DoManualTrick();
                        break;
                }

            }
        }
    }


    private void DoFlipTrick() {
        PerformsTrick = true;

        int getTrick = trickCombinationsArr[0].whichTrick;

        List<FlipTricks> whichDirection = TricksManager.FlipTricksArr[getTrick];
        FlipTricks whichFlipTrick = whichDirection[TricksManager.FlipTricksLevel[getTrick]];

        if (GameSettings.SlowMotionTricks == 2) {
            TimeManagerScript.DoSlowmotion();
        }

        whichFlipTrick.PlayAnimation(SkateboardAnim);

        respectForTrick = whichFlipTrick.respectGain;
        string performedTrickName = whichFlipTrick.trickName;
        
        DisplayTrickName(performedTrickName);
    }

    
    private void DoGrindTrick() {
        PerformsTrick = true;
    }


    private void DoManualTrick() {
        PerformsTrick = true;
        PerformsManual = true;

        SkateboardAnim.SetBool("Can Manual", true);

        int getTrick = trickCombinationsArr[0].whichTrick;

        List<ManualTricks> whichDirection = TricksManager.ManualTricksArr[getTrick];
        ManualTricks whichManualTrick = whichDirection[TricksManager.ManualTricksLevel[getTrick]];

        whichManualTrick.PlayAnimation(SkateboardAnim);

        respectForTrick = whichManualTrick.respectGain;
        string performedTrickName = whichManualTrick.trickName;
        
        DisplayTrickName(performedTrickName);
    }


    private void DisplayTrickName(string addTrickName){
        performedTricks++;

        GainRespect += respectForTrick;
        performedTricksArr.Add(addTrickName);

        string allPerformedTricksText = "";

        for (int i = 0; i < performedTricksArr.Count; i++) {
            if (i > 0) allPerformedTricksText += " + ";
            allPerformedTricksText += performedTricksArr[i];
        }

        TrickName.text = allPerformedTricksText;
    }


    public void TrickDone() {
        trickCombinationsArr.RemoveAt(0);
        PerformsTrick = false;
    }


    private void AwardOrBail() {
        // Check for success or failure when landing
        if (SkateboardControllerScript.IsGrounded) {
            if (!PerformsManual) {
                if (PerformsTrick) {
                    Bail();
                } else {
                    AwardRespect();
                }
            }
        }
    }


    private void Bail() {
        IsBailing = true;
        PerformsTrick = false;

        GainRespect = 0;
        performedTricks = 0;
        performedTricksArr.Clear();
        trickCombinationsArr.Clear();

        // Instantly erase trick names when player bails
        StartCoroutine(EraseTrickName(eraseTrickNameTimeFail));

        SkateboardAnim.SetTrigger("Bail");
        AudioManager.instance.Play("Bail");
    }


    private void BailDone() {
        IsBailing = false;
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
    
}
