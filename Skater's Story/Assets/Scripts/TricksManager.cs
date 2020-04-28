using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rewired;

public class TricksManager : MonoBehaviour {

    public SkateboardController SkateboardControllerScript;
    public Animator SkateboardAnim;
    private Player player;

    public TMP_Text TrickPoints;
    public TMP_Text TrickName;

    public bool PerformsTrick = false;
    public int ExpForTrick = 0;

    private float buttonResetTime = 0.2f;
    private float eraseTrickNameSuccess = 1.0f;
    private float eraseTrickNameFail = 0.2f;

    public bool IsBailing = false;

    // Extra button bools
    private bool leftPressed = false;
    private bool upPressed = false;
    private bool rightPressed = false;
    private bool downPressed = false;

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

                    if (leftPressed) {
                        squarePressed = false;
                        leftPressed = false;
                        PerformsTrick = true;

                        SkateboardAnim.SetTrigger("Kickflip");
                        TrickName.text = "Kickflip";
                        ExpForTrick += 12;
                    }
                    
                    if (upPressed) {
                        squarePressed = false;
                        upPressed = false;
                        PerformsTrick = true;

                        SkateboardAnim.SetTrigger("360 Flip");
                    
                        TrickName.text = "360 Flip";
                        ExpForTrick += 20;
                    }
                    
                    if (rightPressed) {
                        squarePressed = false;
                        rightPressed = false;
                        PerformsTrick = true;

                        SkateboardAnim.SetTrigger("Impossible");
                        TrickName.text = "Impossible";
                        ExpForTrick += 12;
                    }
                    
                    if (downPressed) {
                        squarePressed = false;
                        downPressed = false;
                        PerformsTrick = true;

                        SkateboardAnim.SetTrigger("Hardflip");
                        TrickName.text = "Hardflip";
                        ExpForTrick += 20;
                    }

                    // SkateboardAnim.SetTrigger("Pop Shove-It");


                }

            }

        }

        // Check if player is still performing trick when landing
        if (SkateboardControllerScript.IsGrounded) {
            if (PerformsTrick) {
                IsBailing = true;
                PerformsTrick = false;
                ExpForTrick = 0;

                StartCoroutine(EraseTrickName(eraseTrickNameFail));

                SkateboardAnim.SetTrigger("Bail");
            } else {
                if (ExpForTrick > 0) {
                    // Give exp to player
                    SkateboardControllerScript.CharacterSheetScript.CurrentSkaterExp += ExpForTrick;
                    SkateboardControllerScript.CharacterSheetScript.DisplayExpBar();

                    ExpForTrick = 0;

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
            leftPressed = true;
            StartCoroutine(ResetLeftBtn());
        }

        if (dPadUp) {
            upPressed = true;
            StartCoroutine(ResetUpBtn());
        }

        if (dPadRight) {
            rightPressed = true;
            StartCoroutine(ResetRightBtn());
        }

        if (dPadDown) {
            downPressed = true;
            StartCoroutine(ResetDownBtn());
        }

        //////////////////////////////////////////////////////////////////////////////////////

        if (SquareButton) {
            squarePressed = true;
            StartCoroutine(ResetSquareBtn());
        }
    }

    private IEnumerator ResetLeftBtn() {
        yield return new WaitForSeconds(buttonResetTime);
        leftPressed = false;
    }

    private IEnumerator ResetUpBtn() {
        yield return new WaitForSeconds(buttonResetTime);
        upPressed = false;
    }

    private IEnumerator ResetRightBtn() {
        yield return new WaitForSeconds(buttonResetTime);
        rightPressed = false;
    }

    private IEnumerator ResetDownBtn() {
        yield return new WaitForSeconds(buttonResetTime);
        downPressed = false;
    }

    //////////////////////////////////////////////////////////////////////////////////////

    private IEnumerator ResetSquareBtn() {
        yield return new WaitForSeconds(buttonResetTime);
        squarePressed = false;
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
        SkateboardControllerScript.RespawnAfterBail();
    }


    private IEnumerator EraseTrickName(float eraseTrickTime) {
        yield return new WaitForSeconds(eraseTrickTime);
        
        TrickName.text = "";
    }

}
