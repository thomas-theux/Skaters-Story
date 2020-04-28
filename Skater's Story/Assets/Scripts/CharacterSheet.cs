using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSheet : MonoBehaviour {

    public Slider ExpBar;
    public TMP_Text SkaterLevelText;
    public TMP_Text SkaterExpText;

    public float StatOllie = 200.0f;        // Max ollie probably shouldn't be over 220
    public float StatSpeed = 6.0f;          // Max speed probably shouldn't be over 8
    public float StatFlip = 6.0f;
    public float StatGrind = 6.0f;
    public float StatManual = 6.0f;

    public int SkaterLevel = 1;

    public float CurrentSkaterExp = 0;
    private float ExpNeededToLevelUp = 100.0f;
    private float ExpNeededLastLevel = 0.0f;

    private float exponent = 1.5f;
    private float baseExp = 100.0f;


    private void Awake() {
        DisplayExpBar();
        SkaterLevelText.text = SkaterLevel + "";
    }


    public void DisplayExpBar() {
        // Show current exp
        SkaterExpText.text = CurrentSkaterExp + " exp";

        // Check for level up
        if (CurrentSkaterExp > ExpNeededToLevelUp) {
            LevelUp();
            CalculateNeededExp();
        }

        // Calculate and display value on exp bar
        float calculatedCurrentExp = CurrentSkaterExp - ExpNeededLastLevel;
        float calculatedNextLevelExp = ExpNeededToLevelUp - ExpNeededLastLevel;

        float currentExpValue = calculatedCurrentExp / calculatedNextLevelExp;

        ExpBar.value = currentExpValue;
    }


    public void LevelUp() {
        SkaterLevel++;
        SkaterLevelText.text = SkaterLevel + "";
    }


    public void CalculateNeededExp() {
        ExpNeededLastLevel = ExpNeededToLevelUp;

        float calculatedExp = Mathf.Pow(SkaterLevel, exponent);
        float neededExp = Mathf.Round(baseExp * calculatedExp);

        ExpNeededToLevelUp = neededExp;
    }

}
