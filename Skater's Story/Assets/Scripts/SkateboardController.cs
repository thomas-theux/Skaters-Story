using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rewired;

public class SkateboardController : MonoBehaviour {

    public int PlayerID = 0;
    private Player player;

    public Rigidbody rb;
    public CharacterSheet CharacterSheetScript;
    // public TricksController TricksControllerScript;
    public TricksHandler TricksHandlerScript;

    public SphereCollider BoardCollider;

    // private float slowMotionSpeed = 0.8f;
    // private bool timeIsSlowedDown = false;

    public int Direction = 1;
    private bool directionSwitchR = false;
    private bool directionSwitchL = false;

    private float minBoardSpeed = 0;             // The min speed of the board
    private float maxBoardSpeed = 0;             // The max speed when holding the X button
    private float ollieForce = 0f;               // How high the skater can jump

    private Quaternion rotCur;
    private float raycastDistance = 1.5f;
    private float alignSpeed = 10.0f;
    private bool aligning = false;

    public string CurrentSurface = "";

    private bool playedRollingSound = false;
    private bool playedLandingSound = false;
    private bool playedGrindingSound = false;

    private string rollingSoundName = "";
    private string grindingSoundName = "";

    // Ollie multiplier depending on current speed
    private float ollieMultiplierMin = 0.5f;
    private float ollieMultiplierMax = 1.0f;

    // Variables for ground checking
    public bool IsGrounded = false;
    public bool IsOnRail = false;
    // public bool CanGrind = false;

    public Transform GroundChecker;

    private float GroundDistance = 0.05f;
    private float RailDistance = 0.1f;

    [SerializeField] public LayerMask GroundedLayer;
    [SerializeField] public LayerMask RailLayer;

    private float currentBoardSpeed;

    // DEV
    public TMP_Text BoardSpeedText;


    // REWIRED
    private float horizontalAxis;
    private float horizontalMovement;
    private float XButtonDown;

    private bool XButtonUp = false;
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
        CheckSurface();
        AlignToSurface();

        //////////////////////////////////////////////////////////////////////////////////////

        if (OptionsButton) CheckpointRespawn();
        
        if (XButtonUp) {
            if (IsGrounded || IsOnRail) {
                ApplyOllieForce();
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////

        currentBoardSpeed = rb.velocity.magnitude;
        BoardSpeedText.text = currentBoardSpeed.ToString("F1");

        //////////////////////////////////////////////////////////////////////////////////////

        PlayRollingSound();
        PlayLandingSound();
        PlayGrindingSound();

        //////////////////////////////////////////////////////////////////////////////////////

        // Only stop manualling when skater is in the air
        if (!IsGrounded) {
            if (TricksHandlerScript.PerformsManual) {
                TricksHandlerScript.PerformsManual = false;
                TricksHandlerScript.SkateboardAnim.SetBool("Can Manual", false);
                TricksHandlerScript.TrickDone();
            }
        }

        if (!IsOnRail) {
            if (TricksHandlerScript.PerformsGrind) {
                TricksHandlerScript.PerformsGrind = false;
                TricksHandlerScript.TrickDone();
            }
        }
    }


    private void FixedUpdate() {
        if (!TricksHandlerScript.IsBailing) {
            if (IsGrounded) {
                rb.AddForce(transform.right * horizontalMovement * maxBoardSpeed);
            } else {
                rb.AddForce(horizontalMovement * maxBoardSpeed, 0, 0);
            }
        }
    }


    private void GetInput() {
        // Only be able to move the skater when he's not bailing or when he's grounded
        if (GameSettings.ClassicControls) {
            ClassicControls();
        } else if (!GameSettings.ClassicControls) {
            NewControls();
        }

        OptionsButton = player.GetButtonUp("Options");
    }


    // New controls: Shoulder buttons to roll left or right
    private void NewControls() {
        if (!TricksHandlerScript.IsBailing) {
            horizontalAxis = player.GetAxis("Horizontal Shoulder");
            XButtonUp = player.GetButtonDown("X");

            if (IsGrounded) {
                horizontalMovement = horizontalAxis;
            }
        }
    }


    // Classic THPS controls: Press X to push and left/right to brake
    private void ClassicControls() {
        if (!TricksHandlerScript.IsBailing) {
            horizontalAxis = player.GetAxis("Horizontal DPad");
            XButtonUp = player.GetButtonUp("X");
            XButtonDown = player.GetAxis("XDown");
            
            if (IsGrounded) {
                if (horizontalAxis != 0) {
                    horizontalMovement = horizontalAxis;
                } else {
                    horizontalMovement = XButtonDown * Direction;
                }
            }
        }
    }


    private void CheckDirection() {
        if (rb.velocity.x > 0) {

            Direction = 1;

            if (directionSwitchL) directionSwitchL = false;

            if (!directionSwitchR) {
                TricksHandlerScript.SkateboardAnim.SetTrigger("Turn Right");
                AudioManager.instance.Play("Switch Direction");
                directionSwitchR = true;
            }

        } else if (rb.velocity.x < 0) {

            Direction = -1;

            if (directionSwitchR) directionSwitchR = false;

            if (!directionSwitchL) {
                TricksHandlerScript.SkateboardAnim.SetTrigger("Turn Left");
                AudioManager.instance.Play("Switch Direction");
                directionSwitchL = true;
            }

        }
    }


    private void CheckIfGrounded() {
        IsGrounded = Physics.CheckSphere(GroundChecker.position, GroundDistance, GroundedLayer, QueryTriggerInteraction.Ignore);

        IsOnRail = Physics.CheckSphere(GroundChecker.position, RailDistance, RailLayer, QueryTriggerInteraction.Ignore);
        TricksHandlerScript.SkateboardAnim.SetBool("Can Grind", IsOnRail);
    }


    private void CheckSurface() {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.25f) == true) {
            CurrentSurface = hit.collider.tag;
        }

        // switch (rayHit.collider.tag) {
        //     case "Concrete":
        //         break;
        //     case "Wood":
        //         break;
        // }
    }


    private void AlignToSurface() {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance) == true) {
            rotCur = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            aligning = true;
        } else {
            aligning = false;
        }

        if (aligning) {
            transform.rotation = Quaternion.Lerp(transform.rotation, rotCur, Time.deltaTime * alignSpeed);
        }
    }


    private void ApplyOllieForce() {
        AudioManager.instance.Play("Ollie");

        // // If the skater performs a manual stop the manual
        // if (TricksHandlerScript.PerformsManual) {
        //     TricksHandlerScript.PerformsManual = false;
        //     TricksHandlerScript.SkateboardAnim.SetBool("Can Manual", false);
        //     TricksHandlerScript.TrickDone();
        // }

        float ollieForceMultiplier = MapSpeed();
        float calculatedOllieForce = ollieForce * ollieForceMultiplier;

        // Vector3 newOllieForce = new Vector3(0f, calculatedOllieForce, 0f);
        // rb.AddForce(newOllieForce);

        Vector3 newOllieForce = new Vector3(rb.velocity.x, calculatedOllieForce, rb.velocity.z);
        rb.velocity = newOllieForce;
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
        horizontalMovement = 0;
        rb.velocity = Vector3.zero;

        float respawnPosX = 0f;

        if (Direction == 1) {
            respawnPosX = this.transform.position.x - 2.0f;
        } else if (Direction == -1) {
            respawnPosX = this.transform.position.x + 2.0f;
        }

        this.transform.position = new Vector3(respawnPosX, 1.0f, 0);
        this.transform.rotation = Quaternion.Euler(Vector3.zero);
    }


    public void RespawnAfterBail() {
        horizontalMovement = 0;
        rb.velocity = Vector3.zero;

        float respawnPosX = this.transform.position.x;

        this.transform.position = new Vector3(respawnPosX, 1.0f, 0);
        this.transform.rotation = Quaternion.Euler(Vector3.zero);
    }


    private void PlayRollingSound() {
        if (IsGrounded) {
            if (currentBoardSpeed > 0.1f) {
                if (!playedRollingSound) {
                    playedRollingSound = true;
                    rollingSoundName = "Rolling " + CurrentSurface;
                    AudioManager.instance.Play(rollingSoundName);
                }
            } else {
                if (playedRollingSound) {
                    playedRollingSound = false;
                    AudioManager.instance.Stop(rollingSoundName);
                }
            }
        } else {
            if (playedRollingSound) {
                playedRollingSound = false;
                AudioManager.instance.Stop(rollingSoundName);
            }
        }
    }


    private void PlayLandingSound() {
        if (IsGrounded || IsOnRail) {
            if (!playedLandingSound) {
                playedLandingSound = true;
                AudioManager.instance.Play("Land " + CurrentSurface);
            }
        } else if (!IsGrounded) {
            if (playedLandingSound) {
                playedLandingSound = false;
            }
        }
    }


    private void PlayGrindingSound() {
        if (IsOnRail) {
            if (!playedGrindingSound) {
                playedGrindingSound = true;
                grindingSoundName = "Grind " + CurrentSurface;
                AudioManager.instance.Play(grindingSoundName);
            }
        } else {
            if (playedGrindingSound) {
                playedGrindingSound = false;
                AudioManager.instance.Stop(grindingSoundName);
            }
        }
    }


    protected void LateUpdate() {
        rb.transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z);
    }

}