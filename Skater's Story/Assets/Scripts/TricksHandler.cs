using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rewired;

public class TricksHandler : MonoBehaviour {

    public SkateboardController SkateboardControllerScript;
    public CharacterSheet CharacterSheetScript;
    public TimeManager TimeManagerScript;
    public Animator SkateboardAnim;

    private Player player;

    public TMP_Text TrickPoints;
    public TMP_Text TrickName;

    public GameObject BalancingGO;
    public GameObject GrindBalanceGO;
    public GameObject ManualBalanceGO;

    public bool PerformsTrick = false;
    public bool PerformsManual = false;
    public bool PerformsGrind = false;
    public bool IsBailing = false;

    private int performedTricks = 0;
    private int tricksArrLimit = 2;

    private List<string> allTricksNamesArr = new List<string>();
    private int maxTrickNames = 10;

    public float respectForTrick = 0;
    private int respectIncrease = 0;
    private float increaseDelayDef = 0.2f;
    private float increaseDelayTimer = 0;
    public int GainRespect = 0;

    private float eraseTrickNameTimeDef = 1.5f;
    public float eraseTrickNameTime = 0;
    private bool erasedTrickTexts = false;

    // private float resetDelayTime = 0.2f;
    private float resetDelayTime = 0.4f;
    private float simpleTrickTimer = 0.05f;
    private float grindTimer = 0;
    private float flipTimer = 0;

    private bool switchTextColor = false;
    private bool addGap = false;
    public bool InsideRailTrigger = false;
    public bool AtTheRail = false;

    // Definition for tricks combination values
    // 0 = !IsGrounded; 1 = CanGrind; 2 = IsGrounded

    private enum directionsEnum {
        Left = 0,
        Up = 1,
        Right = 2,
        Down = 3,
        NoDir = 4
    }

    private List<TrickCombination> trickCombinationsArr = new List<TrickCombination>();

    public class TrickCombination {
        public int whichTrick;
        public int trickCondition;

        public TrickCombination(int trick, int cond) {
            whichTrick = trick;
            trickCondition = cond;
        }
    }

    // Button presses
    public bool SquareButtonPressed = false;
    public bool TriangleButtonPressed = false;

    public bool DPadLeftPressed = false;
    public bool DPadUpPressed = false;
    public bool DPadRightPressed = false;
    public bool DPadDownPressed = false;

    // REWIRED
    private bool SquareButton = false;
    private bool TriangleButton = false;

    private bool DPadLeft = false;
    private bool DPadUp = false;
    private bool DPadRight = false;
    private bool DPadDown = false;


    void Awake() {
        player = ReInput.players.GetPlayer(SkateboardControllerScript.PlayerID);

        // Set font colors
        TrickPoints.color = ColorManager.KeyYellow;
        TrickName.color = ColorManager.KeyGrey;
    }


    private void Update() {
        GetInput();
        ActivateButtonPresses();
        ResetButtonPresses();
        CheckForCombination();

        StartPerforming();
        ContinuousRespectGain();
        DisplayTrickPoints();
        EraseTrickTexts();
        AwardOrBail();

        MoveToRail();
        CheckIfNeedToRemove();
    }


    private void GetInput() {
        // Only be able to skate when the menu isn't open
        if (!CharacterSheetScript.MenuOpen) {
            if (!IsBailing) {
                SquareButton = player.GetButtonDown("Square");
                TriangleButton = player.GetButtonDown("Triangle");

                DPadLeft = player.GetButton("DPad Left");
                DPadUp = player.GetButton("DPad Up");
                DPadRight = player.GetButton("DPad Right");
                DPadDown = player.GetButton("DPad Down");
            }
        }
    }


    private void ActivateButtonPresses() {
        if (SquareButton) SquareButtonPressed = true;
        if (TriangleButton) TriangleButtonPressed = true;

        if (DPadLeft) DPadLeftPressed = true;
        if (DPadUp) DPadUpPressed = true;
        if (DPadRight) DPadRightPressed = true;
        if (DPadDown) DPadDownPressed = true;
    }


    private void ResetButtonPresses() {
        if (SquareButtonPressed) StartCoroutine(ResetAfterDelay(0));
        if (TriangleButtonPressed) StartCoroutine(ResetAfterDelay(1));

        if (DPadLeftPressed) StartCoroutine(ResetAfterDelay(2));
        if (DPadUpPressed) StartCoroutine(ResetAfterDelay(3));
        if (DPadRightPressed) StartCoroutine(ResetAfterDelay(4));
        if (DPadDownPressed) StartCoroutine(ResetAfterDelay(5));
    }


    private IEnumerator ResetAfterDelay(int resetThisButton) {
        yield return new WaitForSeconds(resetDelayTime);

        switch (resetThisButton) {
            case 0:
                SquareButtonPressed = false;
                break;
            case 1:
                TriangleButtonPressed = false;
                break;

            case 2:
                DPadLeftPressed = false;
                break;
            case 3:
                DPadUpPressed = false;
                break;
            case 4:
                DPadRightPressed = false;
                break;
            case 5:
                DPadDownPressed = false;
                break;
        }

        // ResetButtons();
    }


    private void CheckForCombination() {
        if (trickCombinationsArr.Count < tricksArrLimit) {

            ///////////////////////////
            // FLIP TRICKS
            ///////////////////////////

            if (SquareButtonPressed) {
                if (!DPadLeftPressed && !DPadUpPressed && !DPadRightPressed && !DPadDownPressed) {
                    flipTimer += Time.deltaTime;

                    if (flipTimer >= simpleTrickTimer) AddTrickCombination(directionsEnum.NoDir, 0);
                }

                else if (DPadLeftPressed) AddTrickCombination(directionsEnum.Left, 0);
                else if (DPadUpPressed) AddTrickCombination(directionsEnum.Up, 0);
                else if (DPadRightPressed) AddTrickCombination(directionsEnum.Right, 0);
                else if (DPadDownPressed) AddTrickCombination(directionsEnum.Down, 0);
            }
            
            ///////////////////////////
            // GRIND TRICKS
            ///////////////////////////

            if (TriangleButtonPressed) {
                if (!DPadLeftPressed && !DPadUpPressed && !DPadRightPressed && !DPadDownPressed) {
                    grindTimer += Time.deltaTime;

                    if (grindTimer >= simpleTrickTimer) AddTrickCombination(directionsEnum.NoDir, 1);
                }

                else if (DPadLeftPressed) AddTrickCombination(directionsEnum.Left, 1);
                else if (DPadUpPressed) AddTrickCombination(directionsEnum.Up, 1);
                else if (DPadRightPressed) AddTrickCombination(directionsEnum.Right, 1);
                else if (DPadDownPressed) AddTrickCombination(directionsEnum.Down, 1);
            }

            ///////////////////////////
            // MANUAL TRICKS
            ///////////////////////////

            // if (DPadLeftPressed && DPadRightPressed) // AddTrickCombination(directionsEnum.Left, 2);
            if (DPadUpPressed && DPadDownPressed) AddTrickCombination(directionsEnum.Up, 2);
            if (DPadRightPressed && DPadLeftPressed) AddTrickCombination(directionsEnum.Right, 2);
            // if (DPadDownPressed && DPadUpPressed) // AddTrickCombination(directionsEnum.Down, 2);

        }
    }


    private void AddTrickCombination(directionsEnum identifier, int cond) {
        TrickCombination newCombination = new TrickCombination((int)identifier, cond);
        trickCombinationsArr.Add(newCombination);
        ResetButtons();
    }


    private void ResetButtons() {
        SquareButtonPressed = false;
        TriangleButtonPressed = false;

        DPadLeftPressed = false;
        DPadUpPressed = false;
        DPadRightPressed = false;
        DPadDownPressed = false;
    }


    private void StartPerforming() {
        if (!PerformsTrick) {
            if (trickCombinationsArr.Count > 0) {
                grindTimer = 0;
                flipTimer = 0;

                int getCond = trickCombinationsArr[0].trickCondition;

                switch (getCond) {
                    case 0:
                        if (!SkateboardControllerScript.IsGrounded) DoFlipTrick();
                        break;
                    case 1:
                        if (SkateboardControllerScript.IsOnRail) DoGrindTrick();
                        // if (InsideRailTrigger) DoGrindTrick();
                        break;
                    case 2:
                        if (SkateboardControllerScript.IsGrounded) DoManualTrick();
                        break;
                }

            }
        }
    }


    private void DoFlipTrick() {
        PerformsTrick = true;

        int getTrick = trickCombinationsArr[0].whichTrick;

        List<FlipTricks> whichDirection = TricksManager.FlipTricksArr[getTrick];
        FlipTricks whichFlipTrick = whichDirection[CharacterSheetScript.FlipTricksLevel[getTrick]];

        if (GameSettings.SlowMotionTricks == 2) {
            TimeManagerScript.DoSlowmotion();
        }

        whichFlipTrick.PlayAnimation(SkateboardAnim);

        respectForTrick = whichFlipTrick.respectGain * CharacterSheetScript.StatCharisma;
        string performedTrickName = whichFlipTrick.trickName;
        
        DisplayTrickName(performedTrickName);
    }

    
    private void DoGrindTrick() {
        PerformsTrick = true;
        PerformsGrind = true;

        int getTrick = trickCombinationsArr[0].whichTrick;

        List<GrindTricks> whichDirection = TricksManager.GrindTricksArr[getTrick];
        GrindTricks whichGrindTrick = whichDirection[CharacterSheetScript.GrindTricksLevel[getTrick]];

        whichGrindTrick.PlayAnimation(SkateboardAnim);

        respectForTrick = whichGrindTrick.respectGain * CharacterSheetScript.StatCharisma;
        respectIncrease = whichGrindTrick.respectIncrease;
        string performedTrickName = whichGrindTrick.trickName;
        
        DisplayTrickName(performedTrickName);

        BalancingGO.SetActive(true);
        GrindBalanceGO.SetActive(true);
    }


    private void DoManualTrick() {
        PerformsTrick = true;
        PerformsManual = true;

        SkateboardAnim.SetBool("Can Manual", true);

        int getTrick = trickCombinationsArr[0].whichTrick;

        List<ManualTricks> whichDirection = TricksManager.ManualTricksArr[getTrick];
        ManualTricks whichManualTrick = whichDirection[CharacterSheetScript.ManualTricksLevel[getTrick]];

        whichManualTrick.PlayAnimation(SkateboardAnim);

        respectForTrick = whichManualTrick.respectGain * CharacterSheetScript.StatCharisma;
        respectIncrease = whichManualTrick.respectIncrease;
        string performedTrickName = whichManualTrick.trickName;
        
        DisplayTrickName(performedTrickName);

        BalancingGO.SetActive(true);
        ManualBalanceGO.SetActive(true);
    }


    private void DisplayTrickName(string addTrickName){
        performedTricks++;

        allTricksNamesArr.Add(addTrickName);

        int startIndex = 0;
        string allPerformedTricksText = "";

        if (allTricksNamesArr.Count > maxTrickNames) {
            startIndex = allTricksNamesArr.Count - maxTrickNames;
        }

        for (int i = startIndex; i < allTricksNamesArr.Count; i++) {
            if (startIndex != 0) {
                if (i == startIndex) {
                    allPerformedTricksText += " ... ";
                }
            }

            if (i > 0) allPerformedTricksText += " + ";

            allPerformedTricksText += allTricksNamesArr[i];
        }

        TrickName.text = allPerformedTricksText;
    }


    private void DisplayTrickPoints() {
        if (PerformsTrick || addGap) {
            float currentCombo = respectForTrick + GainRespect;

            if (performedTricks == 1) {
                TrickPoints.text = currentCombo.ToString("N0");
            } else {
                TrickPoints.text = currentCombo.ToString("N0") + "<size=30%> </size>" + "<size=42>X</size>" + "<size=30%> </size>" + performedTricks;
            }

            if (switchTextColor) {
                // TrickPoints.colorGradient = new VertexGradient(ColorManager.TricksBlueTop, ColorManager.TricksBlueTop, ColorManager.TricksBlueBottom, ColorManager.TricksBlueBottom);
                TrickPoints.color = ColorManager.KeyYellow;
                switchTextColor = false;
            }
        }
    }


    private void ContinuousRespectGain() {
        if (PerformsGrind || PerformsManual) {
            increaseDelayTimer += Time.deltaTime;

            if (increaseDelayTimer > increaseDelayDef) {
                respectForTrick += respectIncrease;
                increaseDelayTimer = 0;
            }
        }
    }


    public void TrickDone() {
        if (GrindBalanceGO) GrindBalanceGO.SetActive(false);
        if (ManualBalanceGO) ManualBalanceGO.SetActive(false);

        GainRespect += (int)respectForTrick;

        if (trickCombinationsArr.Count > 0) {
            trickCombinationsArr.RemoveAt(0);
        }


        if (addGap) addGap = false;
        PerformsTrick = false;

        respectForTrick = 0;
        respectIncrease = 0;
    }


    private void AwardOrBail() {
        // Check for success or failure when landing
        if (SkateboardControllerScript.IsGrounded) {
            if (!PerformsManual) {
                if (PerformsTrick) {
                    Bail();
                } else {
                    AwardRespect();
                }
            }
        }
    }


    public void Bail() {
        if (GrindBalanceGO) GrindBalanceGO.SetActive(false);
        if (ManualBalanceGO) ManualBalanceGO.SetActive(false);

        BalancingGO.SetActive(false);

        IsBailing = true;
        PerformsTrick = false;
        PerformsGrind = false;
        PerformsManual = false;

        // Move skater to Z = 0 if he was grinding
        if (AtTheRail) {
            AtTheRail = false;
            MoveFromRail();
        }

        SkateboardAnim.SetTrigger("Bail");
        AudioManager.instance.Play("Bail");

        int multipliedRespect = (int)respectForTrick * performedTricks;
        TrickPoints.text = multipliedRespect.ToString("N0");
        TrickPoints.text = "<color=red>" + TrickPoints.text + "</color>";
        TrickName.text = "<color=red>" + TrickName.text + "</color>";

        GainRespect = 0;
        performedTricks = 0;
        allTricksNamesArr.Clear();
        trickCombinationsArr.Clear();

        // Instantly erase trick names when player bails
        // StartCoroutine(EraseTrickName(eraseTrickNameTime));
        // eraseTrickNameTime = eraseTrickNameTimeDef;
        // erasedTrickTexts = false;
    }


    private void BailDone() {
        IsBailing = false;
        SkateboardControllerScript.RespawnAfterBail();
    }


    private void AwardRespect() {
        BalancingGO.SetActive(false);

        if (GainRespect > 0) {
            int multipliedRespect = GainRespect * performedTricks;

            SkateboardControllerScript.CharacterSheetScript.NewRespectValue += multipliedRespect;
            SkateboardControllerScript.CharacterSheetScript.IncreasingRespect = true;

            AudioManager.instance.Play("Combo Successful");

            TrickPoints.text = multipliedRespect.ToString("N0");

            if (!switchTextColor) {
                // TrickPoints.colorGradient = new VertexGradient(ColorManager.TricksYellowTop, ColorManager.TricksYellowTop, ColorManager.TricksYellowBottom, ColorManager.TricksYellowBottom);
                TrickPoints.color = ColorManager.KeyGreen;
                switchTextColor = true;
            }

            GainRespect = 0;
            performedTricks = 0;
            allTricksNamesArr.Clear();
            trickCombinationsArr.Clear();

            // Erase trick names after a few seconds
            // StartCoroutine(EraseTrickName(eraseTrickNameTime));
            // eraseTrickNameTime = eraseTrickNameTimeDef;
            // erasedTrickTexts = false;
        }
    }


    private void EraseTrickTexts() {
        if (trickCombinationsArr.Count == 0) {

            if (eraseTrickNameTime > 0) {
                eraseTrickNameTime -= Time.deltaTime;
            } else {
                if (!erasedTrickTexts) {
                    erasedTrickTexts = true;

                    TrickName.text = "";
                    TrickPoints.text = "";
                }
            }
            
        } else {
            if (eraseTrickNameTime < eraseTrickNameTimeDef) {
                eraseTrickNameTime = eraseTrickNameTimeDef;
                erasedTrickTexts = false;
            }
        }
    }


    // private IEnumerator EraseTrickName(float eraseTrickTime) {
    //     yield return new WaitForSeconds(eraseTrickTime);
        
    //     if (!PerformsTrick) {
    //         TrickName.text = "";
    //         TrickPoints.text = "";
    //     }
    // }


    public void AddComboElement(string comboName, int points) {
        AudioManager.instance.Play("Gap");

        addGap = true;
        respectForTrick += points;

        DisplayTrickName(comboName);
        DisplayTrickPoints();

        if (!PerformsTrick) TrickDone();
    }


    private void MoveToRail() {
        if (InsideRailTrigger) {
            if (trickCombinationsArr.Count > 0) {
                if (trickCombinationsArr[0].trickCondition == 1) {
                    if (!AtTheRail) {
                        AtTheRail = true;
                        SkateboardControllerScript.gameObject.transform.position = new Vector3(
                            SkateboardControllerScript.gameObject.transform.position.x,
                            SkateboardControllerScript.gameObject.transform.position.y,
                            0.25f
                        );
                    }
                }
            }
        }
    }


    private void CheckIfNeedToRemove() {
        if (InsideRailTrigger) {
            if (SkateboardControllerScript.IsGrinding) {
                if (trickCombinationsArr.Count > 0) {
                    if (trickCombinationsArr[0].trickCondition != 1) {
                        if (AtTheRail) {
                            AtTheRail = false;
                            MoveFromRail();
                        }
                    }
                } else {
                    if (AtTheRail) {
                        AtTheRail = false;
                        MoveFromRail();
                    }
                }
            }
        } else {
            if (AtTheRail) {
                AtTheRail = false;
                MoveFromRail();
            }
        }
    }


    private void MoveFromRail() {
        SkateboardControllerScript.gameObject.transform.position = new Vector3(
            SkateboardControllerScript.gameObject.transform.position.x,
            SkateboardControllerScript.gameObject.transform.position.y,
            0
        );
    }
    
}
