﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rewired;

public class BoardController3D : MonoBehaviour {

    public int playerID = 0;

    private Player player;
    private Rigidbody rb;

    public CharacterSheet CharacterSheetScript;

    // 0 = stop; 1 = increasing; 2 = skate; 3 = air; 4 = decreasing
    public int SkateMode = 0;

    private float MinBoardSpeed = 0;             // The min speed of the board
    private float MaxBoardSpeed = 0;             // The max speed when holding the X button
    private float OllieForce = 0f;               // How high the skater can jump
    private float appliedForce = 0.0f;          // Just an interims variable to calculate
    private float increaseForce = 0.3f;         // How fast the skateboard is accelerating until it reaches max speed
    private float decreaseForce = 0.01f;        // How fast the skateboard is decelerating until it reaches 0 speed
    private float appliedForceTolerance = 0.2f; // Tolerance for board speed
    private float speedIncreaseVar = 20.0f;     // This determines how fast the speed of the board increases until it reaches full speed –> lower value = faster increase

    private float ollieForceMin = 100.0f;
    private float ollieForceMax = 500.0f;

    private float minMultiplier = 0.5f;
    private float maxMultiplier = 1.0f;

    // Variables for ground checking
    public bool isGrounded = false;
    public Transform GroundChecker;
    private float GroundDistance = 0.1f;
    [SerializeField] private LayerMask platformLayerMask;

    // Variables for the delays for entering and exiting the skate mode 
    private bool startedEnterDelay = false;
    private bool startedExitDelay = false;
    private float enterIncrease = 0.2f;
    private float exitIncrease = 0.05f;
    private float delayCounter = 0.0f;
    private float delayThreshold = 1.0f;


    private float currentBoardSpeed = 0;
    private int savedSkateMode = 0;        // When the player ollies the current skate mode will be saved and will be applied again after landing

    // REWIRED
    private bool XButtonDown = false;
    private bool XButtonUp = false;
    private bool CircleButton = false;
    private bool dPadLeft = false;
    private bool dPadRight = false;

    // DEV variables
    public TMP_Text BoardSpeedText;


    private void Awake() {
        player = ReInput.players.GetPlayer(playerID);
        rb = GetComponent<Rigidbody>();
    }


    private void Start() {
        GetStats();
    }


    private void GetStats() {
        MaxBoardSpeed = CharacterSheetScript.StatSpeed;
        OllieForce = CharacterSheetScript.StatOllie;

        increaseForce = MaxBoardSpeed / speedIncreaseVar;
    }


    private void Update() {
        GetInput();
        CheckIfGrounded();

        ///////////////////////////////////////////////////////////////////////////////////////

        if (CircleButton) CheckpointRespawn();               // Reset player to the beginning of the level
        currentBoardSpeed = rb.velocity.magnitude;           // Get speed of the board in m/s
        BoardSpeedText.text = currentBoardSpeed.ToString("F2");

        ///////////////////////////////////////////////////////////////////////////////////////

        // Handle jumping
        if (XButtonUp) {
            if (isGrounded) {
                ApplyOllieForce();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////

        // Set skate mode to AIR whenever the player is not grounded
        if (!isGrounded) {
            if (SkateMode != 3) {
                savedSkateMode = SkateMode;
                SkateMode = 3;
            }
        } else {
            if (SkateMode == 3) {
                SkateMode = savedSkateMode;

                // Check for bails
            }
        }
    }


    private void FixedUpdate() {
        switch (SkateMode) {
            case 0:
                if (XButtonDown) { EnterSkateModeDelay(); }
                break;
            case 1:
                if (XButtonDown) { IncreaseSpeed(); }
                break;
            case 2:
                if (XButtonDown) { ApplyPushForce(MaxBoardSpeed); }
                if (!XButtonDown) { ExitSkateModeDelay(); }
                break;
            // case 3:
                // if (isGrounded) { ExitSkateModeDelay(); }
                // break;
            case 4:
                DecreaseSpeed();
                // if (XButtonDown) { IncreaseSpeed(); }
                if (XButtonDown) { EnterSkateModeDelay(); }
                break;
        }
    }


    private void GetInput() {
        XButtonDown = player.GetButton("X");
        XButtonUp = player.GetButtonUp("X");

        CircleButton = player.GetButtonUp("Circle");

        dPadLeft = player.GetButton("DPad Left");
        dPadRight = player.GetButton("DPad Right");
    }


    private void CheckIfGrounded() {
        isGrounded = Physics.CheckSphere(GroundChecker.position, GroundDistance, platformLayerMask, QueryTriggerInteraction.Ignore);
    }


    private void EnterSkateModeDelay() {
        if (!startedEnterDelay) {
            delayCounter = 0.0f;
            startedEnterDelay = true;
        }

        delayCounter += enterIncrease;

        if (delayCounter >= delayThreshold) {
            SkateMode = 1;
            startedEnterDelay = false;
        }
    }


    private void IncreaseSpeed() {
        if (currentBoardSpeed < MaxBoardSpeed - appliedForceTolerance) {
            appliedForce = currentBoardSpeed + increaseForce;

            ApplyPushForce(appliedForce);
        } else {
            SkateMode = 2;
        }
    }


    private void ApplyPushForce(float pushForce) {
        rb.velocity = new Vector3(pushForce, rb.velocity.y, 0f);
    }


    private void ApplyOllieForce() {
        startedEnterDelay = false;
        startedExitDelay = false;

        float ollieForceMultiplier = MapSpeed();
        float calculatedOllieForce = OllieForce * ollieForceMultiplier;

        Vector3 newOllieForce = new Vector3(0f, calculatedOllieForce, 0f);
        rb.AddForce(newOllieForce);
    }


    private float MapSpeed() {
        float multiplierRange = maxMultiplier - minMultiplier;
        float speedRange = MaxBoardSpeed - MinBoardSpeed;
        float inputSpeed = currentBoardSpeed - MinBoardSpeed;

        float firstPart = multiplierRange / speedRange;
        float secondPart = firstPart * inputSpeed;

        float mappedValue = minMultiplier + secondPart;

        return mappedValue;
    }


    private void ExitSkateModeDelay() {
        if (!startedExitDelay) {
            delayCounter = 0.0f;
            startedExitDelay = true;
        }

        delayCounter += exitIncrease;

        ApplyPushForce(MaxBoardSpeed);

        if (delayCounter >= delayThreshold) {
            SkateMode = 4;
            startedExitDelay = false;
        }
    }


    private void DecreaseSpeed() {
        if (rb.velocity.x > 0) {
            appliedForce = currentBoardSpeed - decreaseForce;

            ApplyPushForce(appliedForce);
        } else {
            ResetOnStop();
        }
    }


    private void ResetOnStop() {
        startedEnterDelay = false;
        startedExitDelay = false;

        savedSkateMode = 0;
        SkateMode = 0;

        appliedForce = 0;
    }


    public void CheckpointRespawn() {
        savedSkateMode = 0;
        SkateMode = 0;

        appliedForce = 0;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        this.transform.position = new Vector3(0, 0.2f, 0);
        this.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

}