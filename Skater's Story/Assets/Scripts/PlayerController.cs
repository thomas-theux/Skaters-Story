using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;
    private Player player;

    public int playerID = 0;

    public float push = 1.0f;

    // REWIRED
    private bool xBTN = false;


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        player = ReInput.players.GetPlayer(playerID);
    }


    private void Update() {
        GetInput();

        if (xBTN) {
            MoveSkater();
        }

        print(rb.velocity.magnitude);
    }


    private void GetInput() {
        xBTN = player.GetButton("X");
    }


    private void MoveSkater() {
        rb.AddForce(transform.right * push);
    }

}
