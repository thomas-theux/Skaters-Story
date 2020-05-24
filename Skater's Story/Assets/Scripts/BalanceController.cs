using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class BalanceController : MonoBehaviour {

    public TricksHandler TricksHandlerScript;
    public SkateboardController SkateboardControllerScript;

    private Player player;

    private Slider BalanceSlider;

    private float balanceStat;

    public float currentValue = 50;
    public float valueIncreaser = 1f;
    public float increaserIncreaser = 10f;

    private int dir;

    // REWIRED
    private bool DPadLeft = false;
    private bool DPadUp = false;
    private bool DPadRight = false;
    private bool DPadDown = false;


    private void Awake() {
        player = ReInput.players.GetPlayer(SkateboardControllerScript.PlayerID);
        BalanceSlider = GetComponent<Slider>();
    }


    private void OnEnable() {
        balanceStat = SkateboardControllerScript.CharacterSheetScript.StatBalance;
        increaserIncreaser = 50f / balanceStat;

        dir = Random.Range(0, 2) == 0 ? -1 : 1;
    }


    private void Update() {
        GetInput();
        ProcessInput();

        CalculateSliderValue();
        MoveSlider();

        CheckForBail();
    }


    private void GetInput() {
        DPadLeft = player.GetButton("DPad Left");
        DPadUp = player.GetButton("DPad Up");
        DPadRight = player.GetButton("DPad Right");
        DPadDown = player.GetButton("DPad Down");
    }


    private void ProcessInput() {
        if (TricksHandlerScript.PerformsGrind) {
            if (DPadLeft) {
                if (dir != -1) {
                    dir = -1;
                }
            }

            if (DPadRight) {
                if (dir != 1) {
                    dir = 1;
                }
            }
        } else if (TricksHandlerScript.PerformsManual) {
            if (DPadDown) {
                if (dir != -1) {
                    dir = -1;
                }
            }

            if (DPadUp) {
                if (dir != 1) {
                    dir = 1;
                }
            }
        }
    }


    private void CalculateSliderValue() {
        valueIncreaser += increaserIncreaser * Time.deltaTime;
        currentValue += (valueIncreaser * Time.deltaTime) * dir;
    }


    private void MoveSlider() {
        BalanceSlider.value = currentValue;
    }


    private void CheckForBail() {
        if (BalanceSlider.value <= 0 || BalanceSlider.value >= 100) {
            TricksHandlerScript.Bail();
        }
    }

}
