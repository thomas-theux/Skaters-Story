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
    public TricksManager TricksManagerScript;

    public int direction = 1;

    private float minBoardSpeed = 0;             // The min speed of the board
    private float maxBoardSpeed = 0;             // The max speed when holding the X button
    private float ollieForce = 0f;               // How high the skater can jump

    // Ollie multiplier depending on current speed
    private float minMultiplier = 0.5f;
    private float maxMultiplier = 1.0f;

    // Variables for ground checking
    public bool IsGrounded = false;
    public bool IsFlipped = false;
    public Transform GroundChecker;
    public Transform FlippedChecker;
    private float GroundDistance = 0.025f;
    [SerializeField] public LayerMask GroundedLayer;

    private float currentBoardSpeed;

    private float angleSmoothSpeed = 3.0f;

    // DEV
    public TMP_Text BoardSpeedText;


    // REWIRED
    private float horizontalAxis;

    private bool XButton = false;
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
        // Only be able to move the skater when he's not bailing or when he's grounded
        if (!TricksManagerScript.IsBailing) {
            GetInput();
        }

        CheckDirection();
        CheckIfGrounded();

        //////////////////////////////////////////////////////////////////////////////////////

        if (TriangleButton) CheckpointRespawn();
        
        if (XButton) {
            if (IsGrounded) {
                ApplyOllieForce();
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////

        currentBoardSpeed = rb.velocity.magnitude;
        BoardSpeedText.text = currentBoardSpeed.ToString("F1");
    }


    private void FixedUpdate() {
        // rb.AddRelativeForce(horizontalAxis * maxBoardSpeed, 0, 0);
        // rb.AddRelativeForce(0, 0, horizontalAxis * maxBoardSpeed);

        // rb.AddForce(transform.forward * horizontalAxis * maxBoardSpeed);
        if (!TricksManagerScript.IsBailing) {
            rb.AddForce(horizontalAxis * maxBoardSpeed, 0, 0);
        }

        // Smoothen the board out while in the air
        // if (!IsGrounded) {
        //     float desiredZAngle = 0;
        //     // float smoothedAngle = Mathf.Lerp(transform.eulerAngles.z, desiredZAngle, angleSmoothSpeed * Time.deltaTime);

        //     transform.rotation = Quaternion.AngleAxis(desiredZAngle, Vector3.forward * direction);            
        // }
    }


    private void GetInput() {
        if (IsGrounded) {
            horizontalAxis = player.GetAxis("Horizontal");
        }

        XButton = player.GetButtonDown("X");
        TriangleButton = player.GetButtonUp("Triangle");
    }


    private void CheckIfGrounded() {
        IsGrounded = Physics.CheckSphere(GroundChecker.position, GroundDistance, GroundedLayer, QueryTriggerInteraction.Ignore);
        IsFlipped = Physics.CheckSphere(FlippedChecker.position, GroundDistance, GroundedLayer, QueryTriggerInteraction.Ignore);
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


    public void RespawnAfterBail() {
        rb.velocity = Vector3.zero;

        float respawnPosX = this.transform.position.x;

        this.transform.position = new Vector3(respawnPosX, 1.0f, 0);
        this.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

}