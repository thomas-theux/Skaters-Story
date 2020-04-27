using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class TricksManager : MonoBehaviour {

    public SkateboardController SkateboardControllerScript;
    public Animator SkateboardAnim;
    private Player player;

    public bool PerformsTrick = false;

    // REWIRED
    private bool dPadLeft = false;
    private bool dPadRight = false;

    private bool SquareButton = false;
    private bool TriangleButton = false;


    private void Awake () {
        player = ReInput.players.GetPlayer(SkateboardControllerScript.PlayerID);
    }


    private void Update() {
        GetInput();

        if (!SkateboardControllerScript.IsGrounded) {

            // FLIP TRICKS
            if (SquareButton) {
                PerformsTrick = true;

                // SkateboardAnim.SetTrigger("Pop Shove-It");
                SkateboardAnim.SetTrigger("Kickflip");
            }

        }

        // Check if player is still performing trick when landing
        if (PerformsTrick) {
            if (SkateboardControllerScript.IsGrounded) {
                // print("BAIL!");
            }
        }
    }


    private void GetInput() {
        dPadLeft = player.GetButtonDown("DPad Left");
        dPadRight = player.GetButtonDown("DPad Right");
        
        SquareButton = player.GetButtonDown("Square");
        TriangleButton = player.GetButtonDown("Triangle");
    }


    private void TrickDone() {
        PerformsTrick = false;
    }

}
