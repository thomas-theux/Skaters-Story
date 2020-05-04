using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rewired;

public class TricksPerformer : MonoBehaviour {

    public SkateboardController SkateboardControllerScript;
    public Animator SkateboardAnim;
    private Player player;

    public TMP_Text TrickPoints;
    public TMP_Text TrickName;

    public bool PerformsFlipTrick = false;
    public bool PerformsGrindTrick = false;
    public int GainRespect = 0;

    private int performedTricks = 0;
    private List<string> performedTricksArr = new List<string>();

    private float buttonResetTime = 0.4f;
    private float eraseTrickNameSuccess = 1.5f;
    private float eraseTrickNameFail = 0.2f;

    public bool IsBailing = false;

    // Extra button bools
    private int whichDirectionPressed = -1;
    // private bool leftPressed = false;
    // private bool upPressed = false;
    // private bool rightPressed = false;
    // private bool downPressed = false;

    private bool squarePressed = false;
    public bool trianglePressed = false;
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

        CheckIfIsOnRail();

        if (!SkateboardControllerScript.IsGrounded) {

            if (!PerformsFlipTrick && !PerformsGrindTrick) {
                ActivateBools();
            }

            // FLIP TRICKS
            // if (!PerformsFlipTrick) {

                if (squarePressed) {

                    if (whichDirectionPressed > -1) {
                        PerformFlipTrick(whichDirectionPressed);
                        whichDirectionPressed = -1;
                    } else {
                        PerformFlipTrick(0);
                    }

                }

            // }

            if (!PerformsGrindTrick) {

                if (trianglePressed) {

                    if (whichDirectionPressed > -1) {
                        if (SkateboardControllerScript.IsOnRail) {
                            PerformGrindTrick(whichDirectionPressed);
                            whichDirectionPressed = -1;
                        }
                    } else {
                        if (SkateboardControllerScript.IsOnRail) {
                            PerformGrindTrick(0);
                        }
                    }

                }

            }

        }

        // Check if player is still performing trick when landing
        if (SkateboardControllerScript.IsGrounded) {
            if (PerformsFlipTrick) {
                IsBailing = true;
                PerformsFlipTrick = false;
                
                GainRespect = 0;
                performedTricks = 0;
                performedTricksArr.Clear();

                // Disable rotation and position constraints
                SkateboardControllerScript.rb.constraints = RigidbodyConstraints.None;

                // Instantly erase trick names when player bails
                StartCoroutine(EraseTrickName(eraseTrickNameFail));

                SkateboardAnim.SetTrigger("Bail");
            } else {
                AwardRespect();
            }
        }

        // Set grind to done when player gets off the rail
        if (!SkateboardControllerScript.IsOnRail) {
            if (PerformsGrindTrick) {
                GrindTrickDone();
            }
        }
    }


    private void AwardRespect() {
        if (GainRespect > 0) {
            // Give respect to player
            SkateboardControllerScript.CharacterSheetScript.NewRespectValue += GainRespect;
            SkateboardControllerScript.CharacterSheetScript.IncreasingRespect = true;

            GainRespect = 0;
            performedTricks = 0;
            performedTricksArr.Clear();

            // Erase trick names after a few seconds
            StartCoroutine(EraseTrickName(eraseTrickNameSuccess));
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


    private void CheckIfIsOnRail() {
        SkateboardAnim.SetBool("Is On Rail", SkateboardControllerScript.IsOnRail);
    }


    private void ActivateBools() {
        if (dPadLeft) {
            whichDirectionPressed = 1;
            StartCoroutine(ResetDirectionBtn());
        }

        if (dPadUp) {
            whichDirectionPressed = 2;
            StartCoroutine(ResetDirectionBtn());
        }

        if (dPadRight) {
            whichDirectionPressed = 3;
            StartCoroutine(ResetDirectionBtn());
        }

        if (dPadDown) {
            whichDirectionPressed = 4;
            StartCoroutine(ResetDirectionBtn());
        }

        //////////////////////////////////////////////////////////////////////////////////////

        if (SquareButton) {
            squarePressed = true;
            StartCoroutine(ResetSquareBtn());
        }

        if (TriangleButton) {
            trianglePressed = true;
            StartCoroutine(ResetTriangleBtn());
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

    private IEnumerator ResetTriangleBtn() {
        yield return new WaitForSeconds(buttonResetTime);
        trianglePressed = false;
    }


    private void PerformFlipTrick(int dpadDirection) {
        squarePressed = false;
        PerformsFlipTrick = true;
        performedTricks++;

        List<FlipTricks> whichDirection = TricksManager.FlipTricksArr[dpadDirection];
        FlipTricks whichFlipTrick = whichDirection[TricksManager.FlipTricksLevel[dpadDirection]];

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


    private void PerformGrindTrick(int dpadDirection) {
        trianglePressed = false;
        PerformsGrindTrick = true;
        performedTricks++;

        // Additionally constrain the z rotation so the grinds actually work
        SkateboardControllerScript.rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        List<GrindTricks> whichDirection = TricksManager.GrindTricksArr[dpadDirection];
        GrindTricks whichGrindTrick = whichDirection[TricksManager.GrindTricksLevel[dpadDirection]];

        whichGrindTrick.PlayAnimation(SkateboardAnim);
        GainRespect += whichGrindTrick.respectGain;

        performedTricksArr.Add(whichGrindTrick.trickName);

        string allPerformedTricksText = "";

        for (int i = 0; i < performedTricksArr.Count; i++) {
            if (i > 0) allPerformedTricksText += " + ";
            allPerformedTricksText += performedTricksArr[i];
        }

        TrickName.text = allPerformedTricksText;
    }


    private void TrickDone() {
        PerformsFlipTrick = false;

        SkateboardControllerScript.gameObject.transform.rotation = Quaternion.Euler(
            0.0f,
            0.0f,
            SkateboardControllerScript.gameObject.transform.eulerAngles.z
        );
    }


    private void GrindTrickDone() {
        PerformsGrindTrick = false;

        SkateboardControllerScript.rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

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
        
        if (!PerformsFlipTrick) {
            TrickName.text = "";
        }
    }

}
