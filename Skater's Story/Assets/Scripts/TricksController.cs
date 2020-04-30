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

    public bool PerformsTrick = false;
    public int GainRespect = 0;

    private int performedTricks = 0;
    private List<string> performedTricksArr = new List<string>();

    private float buttonResetTime = 0.2f;
    private float eraseTrickNameSuccess = 1.0f;
    private float eraseTrickNameFail = 0.2f;

    public bool IsBailing = false;

    // Extra button bools
    private int whichDirectionPressed = -1;
    // private bool leftPressed = false;
    // private bool upPressed = false;
    // private bool rightPressed = false;
    // private bool downPressed = false;

    private bool squarePressed = false;
    private bool trianglePressed = false;
    private bool circlePressed = false;

    // REWIRED
    private bool dPadLeft = false;
    private bool dPadUp = false;
    private bool dPadRight = false;
    private bool dPadDown = false;

    private bool SquareButton = false;
    private bool TriangleButton = false;
    private bool CircleButton = false;


    private void Awake () {
        player = ReInput.players.GetPlayer(SkateboardControllerScript.PlayerID);
    }


    private void Update() {
        GetInput();

        if (!SkateboardControllerScript.IsGrounded) {

            ActivateBools();

            // FLIP TRICKS
            if (!PerformsTrick) {

                if (squarePressed) {

                    if (whichDirectionPressed > -1) {
                        PerformTrick(whichDirectionPressed);
                        whichDirectionPressed = -1;
                    }

                }

            }

        }

        // Check if player is still performing trick when landing
        if (SkateboardControllerScript.IsGrounded) {
            if (PerformsTrick) {
                IsBailing = true;
                PerformsTrick = false;
                
                GainRespect = 0;
                performedTricks = 0;
                performedTricksArr.Clear();

                // Disable rotation and position constraints
                SkateboardControllerScript.rb.constraints = RigidbodyConstraints.None;

                // Instantly erase trick names when player bails
                StartCoroutine(EraseTrickName(eraseTrickNameFail));

                SkateboardAnim.SetTrigger("Bail");
            } else {
                if (GainRespect > 0) {
                    // Give exp to player
                    SkateboardControllerScript.CharacterSheetScript.NewRespectValue += GainRespect;
                    SkateboardControllerScript.CharacterSheetScript.IncreasingRespect = true;

                    GainRespect = 0;
                    performedTricks = 0;
                    performedTricksArr.Clear();

                    // Erase trick names after a few seconds
                    StartCoroutine(EraseTrickName(eraseTrickNameSuccess));
                }
            }
        }
    }


    private void GetInput() {
        dPadLeft = player.GetButton("DPad Left");
        dPadUp = player.GetButton("DPad Up");
        dPadRight = player.GetButton("DPad Right");
        dPadDown = player.GetButton("DPad Down");
        
        SquareButton = player.GetButton("Square");
        TriangleButton = player.GetButton("Triangle");
        CircleButton = player.GetButton("Circle");
    }


    private void ActivateBools() {
        if (dPadLeft) {
            whichDirectionPressed = 0;
            StartCoroutine(ResetDirectionBtn());
        }

        if (dPadUp) {
            whichDirectionPressed = 1;
            StartCoroutine(ResetDirectionBtn());
        }

        if (dPadRight) {
            whichDirectionPressed = 2;
            StartCoroutine(ResetDirectionBtn());
        }

        if (dPadDown) {
            whichDirectionPressed = 3;
            StartCoroutine(ResetDirectionBtn());
        }

        //////////////////////////////////////////////////////////////////////////////////////

        if (SquareButton) {
            squarePressed = true;
            StartCoroutine(ResetSquareBtn());
        }
    }

    private IEnumerator ResetDirectionBtn() {
        yield return new WaitForSeconds(buttonResetTime);
        whichDirectionPressed = -1;
    }

    //////////////////////////////////////////////////////////////////////////////////////

    private IEnumerator ResetSquareBtn() {
        yield return new WaitForSeconds(buttonResetTime);
        squarePressed = false;
    }


    private void PerformTrick(int dpadDirection) {
        PerformsTrick = true;
        performedTricks++;

        List<FlipTricks> whichDirection = TricksManager.FlipTricksArr[dpadDirection];
        FlipTricks whichFlipTrick = whichDirection[TricksManager.TricksLevel[dpadDirection]];

        whichFlipTrick.PlayAnimation(SkateboardAnim);
        GainRespect += whichFlipTrick.respectGain;

        performedTricksArr.Add(whichFlipTrick.trickName);

        string allPerformedTricksText = "";
        
        for (int i = 0; i < performedTricksArr.Count; i++) {
            if (i > 0) allPerformedTricksText += " + ";
            allPerformedTricksText += performedTricksArr[i];
        }

        TrickName.text = allPerformedTricksText;
    }


    private void TrickDone() {
        PerformsTrick = false;

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


    private IEnumerator EraseTrickName(float eraseTrickTime) {
        yield return new WaitForSeconds(eraseTrickTime);
        
        if (!PerformsTrick) {
            TrickName.text = "";
        }
    }

}
