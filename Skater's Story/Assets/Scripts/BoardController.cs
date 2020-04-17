using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rewired;

public class BoardController : MonoBehaviour {

    public int playerID = 0;

    private Player player;
    private Rigidbody2D rb;

    public CharacterSheet CharacterSheetScript;
    public CameraController CameraControllerScript;

    // 0 = stop; 1 = increasing; 2 = skate; 3 = air; 4 = decreasing
    public int SkateMode = 0;

    private float MinBoardSpeed = 0;             // The min speed of the board
    private float MaxBoardSpeed = 0;             // The max speed when holding the X button
    private float OllieForce = 0f;               // How high the skater can jump
    private float appliedForce = 0.0f;           // Just an interims variable to calculate
    private float increaseForce = 0.3f;          // How fast the skateboard is accelerating until it reaches max speed
    private float decreaseForce = 0.01f;         // How fast the skateboard is decelerating until it reaches 0 speed
    private float appliedForceTolerance = 0.2f;  // Tolerance for board speed
    private float speedIncreaseVar = 20.0f;      // This determines how fast the speed of the board increases until it reaches full speed –> lower value = faster increase

    // private float drag = 0.5f;

    public int direction = 1;

    // Camera zooming out and smoothing values
    private float fovSmoothing = 1.2f;            // The higher this value, the faster the smoothing
    private float distanceMultiplier = 3.0f;      // The higher this value, the more zoomed out

    // private float ollieForceMin = 100.0f;
    // private float ollieForceMax = 500.0f;

    private float minMultiplier = 0.5f;
    private float maxMultiplier = 1.0f;

    // Variables for ground checking
    public bool isGrounded = false;
    public Transform GroundChecker;
    private float GroundDistance = 0.1f;
    [SerializeField] public LayerMask platformLayerMask;

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
    private bool TriangleButton = false;
    private bool CircleButton = false;
    private bool dPadLeft = false;
    private bool dPadRight = false;

    // DEV variables
    public TMP_Text BoardSpeedText;


    private void Awake() {
        player = ReInput.players.GetPlayer(playerID);
        rb = GetComponent<Rigidbody2D>();
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
        CheckDirection();

        ///////////////////////////////////////////////////////////////////////////////////////

        if (TriangleButton) LevelStartRespawn();               // Reset player to the beginning of the level
        if (CircleButton) CheckpointRespawn();                 // Reset player at where he is right now

        currentBoardSpeed = rb.velocity.magnitude;             // Get speed of the board in m/s
        BoardSpeedText.text = currentBoardSpeed.ToString("F1");

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

        // Remove the drag whenever the player is not in the decreasing mode
        if (SkateMode != 4) {
            if (rb.drag > 0) {
                rb.drag = 0f;
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

        CameraZoom();
    }


    private void GetInput() {
        XButtonDown = player.GetButton("X");
        XButtonUp = player.GetButtonUp("X");

        TriangleButton = player.GetButtonUp("Triangle");
        CircleButton = player.GetButtonUp("Circle");

        dPadLeft = player.GetButton("DPad Left");
        dPadRight = player.GetButton("DPad Right");
    }


    private void CheckIfGrounded() {
        isGrounded = Physics2D.OverlapCircle(GroundChecker.position, GroundDistance, platformLayerMask);
    }


    private void CheckDirection() {
        if (dPadRight) {
            direction = 1;
        } else if (dPadLeft) {
            direction = -1;
        }
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
        // rb.velocity = new Vector2(pushForce * direction, rb.velocity.y);

        // This if function only applies force when the skateboard is slower than the max speed
        // otherwise it would limit the speed to the max even if it's rolling down a slope
        if (currentBoardSpeed < MaxBoardSpeed) {
            Vector3 newForce = transform.right * pushForce * direction;
            newForce.y = rb.velocity.y;
            rb.velocity = newForce;
        }
    }


    private void ApplyOllieForce() {
        startedEnterDelay = false;
        startedExitDelay = false;

        float ollieForceMultiplier = MapSpeed();
        float calculatedOllieForce = OllieForce * ollieForceMultiplier;

        Vector2 newOllieForce = new Vector2(0f, calculatedOllieForce);
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
            // if (rb.drag < drag) {
            //     rb.drag = drag;
            // }

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


    private void CameraZoom() {
        Vector2 direction = new Vector2(0, -1);
        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, direction, Mathf.Infinity, platformLayerMask);

        if (hit.collider != null) {
            float desiredFOV = hit.distance * distanceMultiplier;
            float smoothedFOV = Mathf.Lerp(CameraControllerScript.FieldOfView, desiredFOV, fovSmoothing * Time.fixedDeltaTime);

            CameraControllerScript.FieldOfView = smoothedFOV;
        }
    }


    public void LevelStartRespawn() {
        savedSkateMode = 0;
        SkateMode = 0;

        direction = 1;

        appliedForce = 0;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;

        this.transform.position = new Vector3(0, 0.2f, 0);
        this.transform.rotation = Quaternion.Euler(Vector3.zero);
    }


    public void CheckpointRespawn() {
        savedSkateMode = 0;
        SkateMode = 0;

        appliedForce = 0;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;

        float respawnPosX = 0f;

        if (direction == 1) {
            respawnPosX = this.transform.position.x - 2.0f;
        } else if (direction == -1) {
            respawnPosX = this.transform.position.x + 2.0f;
        }

        this.transform.position = new Vector3(respawnPosX, 0, 0);
        this.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

}
