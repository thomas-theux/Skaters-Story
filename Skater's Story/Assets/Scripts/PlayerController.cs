using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour {

    public CharacterController2D CharacterController2DScript;

    public int playerID = 0;

    public float PushSpeed = 40.0f;
    private float horizontalMove = 0;
    private bool jump = false;
    private bool crouch = false;

    private Player player;

    // REWIRED
    private bool xBTN = false;


    private void Awake() {
        player = ReInput.players.GetPlayer(playerID);
    }


    private void Update() {
        GetInput();
        ProcessInput();
        MoveSkater();
    }


    private void FixedUpdate() {
    }


    private void GetInput() {
        xBTN = player.GetButton("X");
        // jump = player.GetButtonUp("X");
    }


    private void ProcessInput() {
        if (xBTN) {
            horizontalMove = 1.0f * PushSpeed;
        } else {
            horizontalMove = 0.0f;
        }
    }


    private void MoveSkater() {
        CharacterController2DScript.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

}
