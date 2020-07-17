using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSheet : MonoBehaviour {

    public bool MenuOpen = false;

    public Image RespectBar;
    public TMP_Text CurrentLevelText;
    // public TMP_Text CurrentRespectText;

    public TMP_Text MoneyCountText;
    public int MoneyCount = 100;

    public float StatOllie = 200.0f;            // Max ollie probably shouldn't be over 220
    public float StatSpeed = 6.0f;              // Max speed probably shouldn't be over 8
    public float StatFlip = 6.0f;               // Swiftness: how fast you perform flip tricks
    public float StatBalance = 6.0f;            // Stat for balancing grinds and manuals
    public float StatCharisma = 1.0f;           // The multiplier for respect when performing tricks and for collecting money

    public int SkaterLevel = 1;

    public int[] FlipTricksLevel = {0, 0, 0, 0, 0};
    public int[] GrindTricksLevel = {0, 0, 0, 0, 0};
    public int[] ManualTricksLevel = {0, 0, 0, 0};

    public float CurrentRespectValue = 0;
    public float NewRespectValue = 0;
    private float RespectNeededToLevelUp = 250;
    private float RespectNeededLastLevel = 0;

    private float exponent = 1.75f;
    private float baseRespect = 250.0f;

    private float newRespectValue = 0.0f;
    private float currentVelocity = 0.0f;
    public float smoothTime;

    public bool IncreasingRespect = false;


    private void Awake() {
        // DisplayRespectBar();
        CurrentLevelText.text = SkaterLevel + "";
        UpdateMoneyCount();
    }


    private void Update() {
        if (IncreasingRespect) {
            IncreaseRespect();

            CheckForLevelUp();

            // UpdateRespectText();
            UpdateRespectBar();
        }
    }


    private void IncreaseRespect() {
        float desiredRespectValue = Mathf.SmoothDamp(CurrentRespectValue, NewRespectValue, ref currentVelocity, smoothTime);
        CurrentRespectValue = desiredRespectValue;
    }


    private void CheckForLevelUp() {
        if (CurrentRespectValue >= RespectNeededToLevelUp) {
            LevelUp();
            CalculateNeededRespect();
        }
    }


    // private void UpdateRespectText() {
    //     CurrentRespectText.text = Mathf.Ceil(CurrentRespectValue) + " respect";
    // }


    private void UpdateRespectBar() {
        // Calculate and display value on respect bar
        float calculatedCurrentRespect = CurrentRespectValue - RespectNeededLastLevel;
        float calculatedNextLevelRespect = RespectNeededToLevelUp - RespectNeededLastLevel;

        newRespectValue = calculatedCurrentRespect / calculatedNextLevelRespect;

        RespectBar.fillAmount = newRespectValue;
    }


    public void LevelUp() {
        SkaterLevel++;
        CurrentLevelText.text = SkaterLevel + "";
    }


    public void CalculateNeededRespect() {
        RespectNeededLastLevel = RespectNeededToLevelUp;

        float calculatedRespect = Mathf.Pow(SkaterLevel, exponent);
        float neededRespect = baseRespect * calculatedRespect;

        RespectNeededToLevelUp = neededRespect;
    }


    public void UpdateMoneyCount() {
        MoneyCountText.text = "$" + MoneyCount.ToString("N0");

        for (int i = 0; i < BuildingManager.BuildingElementsScriptArr.Count; i++) {
            BuildingManager.BuildingElementsScriptArr[i].CheckForMoney();
        }
    }

}
