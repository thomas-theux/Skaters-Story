﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rewired;

public class SkateboardController : MonoBehaviour {

    public int PlayerID = 0;
    private Player player;

    public Rigidbody rb;
    public CharacterSheet CharacterSheetScript;
    public TricksController TricksControllerScript;

    // private float slowMotionSpeed = 0.8f;
    // private bool timeIsSlowedDown = false;

    public int direction = 1;
    private bool directionSwitchR = false;
    private bool directionSwitchL = false;

    private float minBoardSpeed = 0;             // The min speed of the board
    private float maxBoardSpeed = 0;             // The max speed when holding the X button
    private float ollieForce = 0f;               // How high the skater can jump

    private Quaternion rotCur;
    private float raycastDistance = 1.5f;
    private float alignSpeed = 10.0f;
    private bool aligning = false;

    // Ollie multiplier depending on current speed
    private float ollieMultiplierMin = 0.5f;
    private float ollieMultiplierMax = 1.0f;

    // Variables for ground checking
    public bool IsGrounded = false;
    public bool IsOnRail = false;
    // public bool CanGrind = false;

    public Transform GroundChecker;

    private float GroundDistance = 0.025f;
    private float RailDistance = 0.1f;

    [SerializeField] public LayerMask GroundedLayer;
    [SerializeField] public LayerMask RailLayer;

    private float currentBoardSpeed;

    // DEV
    public TMP_Text BoardSpeedText;


    // REWIRED
    private float horizontalAxis;

    private bool XButton = false;
    private bool OptionsButton = false;


    private void Awake () {
        player = ReInput.players.GetPlayer(PlayerID);
    }


    private void Start() {
        GetStats();
        directionSwitchR = true;
    }


    private void GetStats() {
        maxBoardSpeed = CharacterSheetScript.StatSpeed;
        ollieForce = CharacterSheetScript.StatOllie;
    }


    private void Update () {
        GetInput();

        CheckDirection();
        CheckIfGrounded();
        AlignToSurface();

        //////////////////////////////////////////////////////////////////////////////////////

        if (OptionsButton) CheckpointRespawn();
        
        if (XButton) {
            if (IsGrounded || IsOnRail) {
                ApplyOllieForce();
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////

        currentBoardSpeed = rb.velocity.magnitude;
        BoardSpeedText.text = currentBoardSpeed.ToString("F1");

        //////////////////////////////////////////////////////////////////////////////////////

        // Slow down time when jumping
        // if (!IsGrounded) {
        //     if (!timeIsSlowedDown) {
        //         timeIsSlowedDown = true;
        //         Time.timeScale = slowMotionSpeed;
        //     }
        // } else {
        //     if (timeIsSlowedDown) {
        //         timeIsSlowedDown = false;
        //         Time.timeScale = 1.0f;
        //     }
        // }
    }


    private void FixedUpdate() {
        // rb.AddRelativeForce(horizontalAxis * maxBoardSpeed, 0, 0);
        // rb.AddRelativeForce(0, 0, horizontalAxis * maxBoardSpeed);

        // rb.AddForce(transform.forward * horizontalAxis * maxBoardSpeed);
        if (!TricksControllerScript.IsBailing) {
            // rb.AddForce(horizontalAxis * maxBoardSpeed, 0, 0);
            if (IsGrounded) {
                rb.AddForce(transform.right * horizontalAxis * maxBoardSpeed);
            } else {
                rb.AddForce(horizontalAxis * maxBoardSpeed, 0, 0);
            }
        }
    }


    private void GetInput() {
        // Only be able to move the skater when he's not bailing or when he's grounded
        if (!TricksControllerScript.IsBailing) {
            if (IsGrounded) {
                horizontalAxis = player.GetAxis("Horizontal");
            }

            XButton = player.GetButtonDown("X");
        }

        OptionsButton = player.GetButtonUp("Options");
    }


    private void CheckIfGrounded() {
        IsGrounded = Physics.CheckSphere(GroundChecker.position, GroundDistance, GroundedLayer, QueryTriggerInteraction.Ignore);

        IsOnRail = Physics.CheckSphere(GroundChecker.position, RailDistance, RailLayer, QueryTriggerInteraction.Ignore);
        TricksControllerScript.SkateboardAnim.SetBool("Can Grind", IsOnRail);
    }


    private void AlignToSurface() {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, raycastDistance) == true) {
            rotCur = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            aligning = true;
        } else {
            aligning = false;
        }

        if (aligning) {
            transform.rotation = Quaternion.Lerp(transform.rotation, rotCur, Time.deltaTime * alignSpeed);
        }
    }


    private void CheckDirection() {
        if (rb.velocity.x > 0) {

            direction = 1;

            if (directionSwitchL) directionSwitchL = false;

            if (!directionSwitchR) {
                TricksControllerScript.SkateboardAnim.SetTrigger("Turn Right");
                directionSwitchR = true;
            }

        } else if (rb.velocity.x < 0) {

            direction = -1;

            if (directionSwitchR) directionSwitchR = false;

            if (!directionSwitchL) {
                TricksControllerScript.SkateboardAnim.SetTrigger("Turn Left");
                directionSwitchL = true;
            }

        }
    }


    private void ApplyOllieForce() {
        TricksControllerScript.SkateboardAnim.SetTrigger("Ollie");

        float ollieForceMultiplier = MapSpeed();
        float calculatedOllieForce = ollieForce * ollieForceMultiplier;

        Vector3 newOllieForce = new Vector3(0f, calculatedOllieForce, 0f);
        rb.AddForce(newOllieForce);
    }


    private float MapSpeed() {
        float multiplierRange = ollieMultiplierMax - ollieMultiplierMin;
        float speedRange = maxBoardSpeed - minBoardSpeed;
        float inputSpeed = currentBoardSpeed - minBoardSpeed;

        float firstPart = multiplierRange / speedRange;
        float secondPart = firstPart * inputSpeed;

        float mappedValue = ollieMultiplierMin + secondPart;

        return mappedValue;
    }


    public void CheckpointRespawn() {
        horizontalAxis = 0;
        rb.velocity = Vector3.zero;

        float respawnPosX = 0f;

        if (direction == 1) {
            respawnPosX = this.transform.position.x - 2.0f;
        } else if (direction == -1) {
            respawnPosX = this.transform.position.x + 2.0f;
        }

        this.transform.position = new Vector3(respawnPosX, 1.0f, 0);
        this.transform.rotation = Quaternion.Euler(Vector3.zero);
    }


    public void RespawnAfterBail() {
        horizontalAxis = 0;
        rb.velocity = Vector3.zero;

        float respawnPosX = this.transform.position.x;

        this.transform.position = new Vector3(respawnPosX, 1.0f, 0);
        this.transform.rotation = Quaternion.Euler(Vector3.zero);
    }


    // private void OnTriggerEnter(Collider other) {
    //     if (other.tag == "Rail") {
    //         CanGrind = true;
    //         TricksControllerScript.SkateboardAnim.SetBool("Can Grind", true);
    //     }
    // }


    // private void OnTriggerExit(Collider other) {
    //     if (other.tag == "Rail") {
    //         CanGrind = false;
    //         TricksControllerScript.SkateboardAnim.SetBool("Can Grind", false);
    //     }
    // }


    protected void LateUpdate() {
        rb.transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z);
    }

}