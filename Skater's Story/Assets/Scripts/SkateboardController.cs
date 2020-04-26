using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rewired;

public class SkateboardController : MonoBehaviour {

    public int PlayerID = 0;
    private Player player;

    private Rigidbody rb;
    public CharacterSheet CharacterSheetScript;

    public int direction = 1;

    private float minBoardSpeed = 0;             // The min speed of the board
    private float maxBoardSpeed = 0;             // The max speed when holding the X button
    private float ollieForce = 0f;               // How high the skater can jump

    // Ollie multiplier depending on current speed
    private float minMultiplier = 0.5f;
    private float maxMultiplier = 1.0f;

    // Variables for ground checking
    public bool isGrounded = false;
    public Transform GroundChecker;
    private float GroundDistance = 0.1f;
    [SerializeField] public LayerMask GroundedLayer;

    private float currentBoardSpeed;
    public TMP_Text BoardSpeedText;


    // REWIRED
    private float axisInput;

    private bool XButton = false;
    private bool SquareButton = false;
    private bool TriangleButton = false;


    private void Awake () {
        player = ReInput.players.GetPlayer(PlayerID);
        rb = GetComponent <Rigidbody>();
    }


    private void Start() {
        GetStats();
    }


    private void GetStats() {
        maxBoardSpeed = CharacterSheetScript.StatSpeed;
        ollieForce = CharacterSheetScript.StatOllie;
    }


    private void Update () {
        GetInput();
        CheckDirection();
        CheckIfGrounded();

        //////////////////////////////////////////////////////////////////////////////////////

        if (TriangleButton) CheckpointRespawn();
        
        if (XButton) {
            if (isGrounded) {
                ApplyOllieForce();
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////

        currentBoardSpeed = rb.velocity.magnitude;
        BoardSpeedText.text = currentBoardSpeed.ToString("F1");
    }


    private void FixedUpdate() {
        // rb.AddRelativeForce(axisInput * maxBoardSpeed, 0, 0);
        // rb.AddRelativeForce(0, 0, axisInput * maxBoardSpeed);

        // rb.AddForce(transform.forward * axisInput * maxBoardSpeed);
        rb.AddForce(axisInput * maxBoardSpeed, 0, 0);
    }


    private void GetInput() {
        // axisInput = Input.GetAxis("Horizontal");
        axisInput = player.GetAxis("DPad Horizontal");

        XButton = player.GetButtonDown("X");
        SquareButton = player.GetButtonUp("Square");
        TriangleButton = player.GetButtonUp("Triangle");
    }


    private void CheckIfGrounded() {
        isGrounded = Physics.CheckSphere(GroundChecker.position, GroundDistance, GroundedLayer, QueryTriggerInteraction.Ignore);
    }


    private void CheckDirection() {
        if (rb.velocity.x > 0) {
            direction = 1;
        } else if (rb.velocity.x < 0) {
            direction = -1;
        }
    }


    private void ApplyOllieForce() {
        float ollieForceMultiplier = MapSpeed();
        float calculatedOllieForce = ollieForce * ollieForceMultiplier;

        Vector3 newOllieForce = new Vector3(0f, calculatedOllieForce, 0f);
        rb.AddForce(newOllieForce);
    }


    private float MapSpeed() {
        float multiplierRange = maxMultiplier - minMultiplier;
        float speedRange = maxBoardSpeed - minBoardSpeed;
        float inputSpeed = currentBoardSpeed - minBoardSpeed;

        float firstPart = multiplierRange / speedRange;
        float secondPart = firstPart * inputSpeed;

        float mappedValue = minMultiplier + secondPart;

        return mappedValue;
    }


    public void CheckpointRespawn() {
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

}