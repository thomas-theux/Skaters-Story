using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingElement : MonoBehaviour {

    public CharacterSheet CharacterSheetScript;

    public GameObject BuildingObjectGO;

    public int ItemID = 0;

    public string ItemName;
    public int ItemCosts;
    public float BuildingTime;

    private float buildingTimeRemaining;
    // private float buildTimerDelay = 0.5f;
    private float buildTimerDelay = 0f;

    public float XPos;
    public float YPos;

    public TMP_Text ItemNameTXT;
    public TMP_Text ItemCostsTXT;
    public TMP_Text BuildingTimeTXT;

    public GameObject BuildTimerGO;
    public Image BuildTimerIMG;

    private float timerFloatingHeight = 20f;

    public bool IsBuilding = false;
    public bool IsSelected = false;

    private Image BuildingObjectOutlineIMG;
    private Image BuildingObjectFillIMG;

    // 0 = not enough money; 1 = can be built; 2 = in progress; 3 = done building
    public int ElementStatus = 0;


    private void Awake() {
        BuildingObjectOutlineIMG = BuildingObjectGO.GetComponent<Image>();
        BuildingObjectFillIMG = BuildingObjectGO.transform.GetChild(0).GetComponent<Image>();
    }


    private void Start() {
        // Instantiate sprites for objects
        BuildingObjectOutlineIMG.sprite = Resources.Load<Sprite>("_DEV/Building/Outline/" + "Object0" + ItemID + "-Outline");
        BuildingObjectFillIMG.sprite = Resources.Load<Sprite>("_DEV/Building/Fill/" + "Object0" + ItemID + "-Fill");

        BuildingObjectOutlineIMG.rectTransform.sizeDelta = new Vector2(
            BuildingObjectOutlineIMG.sprite.rect.width,
            BuildingObjectOutlineIMG.sprite.rect.height
        );

        BuildingObjectFillIMG.rectTransform.sizeDelta = new Vector2(
            BuildingObjectFillIMG.sprite.rect.width,
            BuildingObjectFillIMG.sprite.rect.height
        );

        BuildingObjectGO.transform.localScale = new Vector3(1, 1, 1);
        BuildingObjectGO.transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1);

        XPos = BuildingManager.BuildingObjectsClassArr[ItemID].xPos;
        YPos = BuildingManager.BuildingObjectsClassArr[ItemID].yPos;

        BuildingObjectOutlineIMG.rectTransform.anchoredPosition = new Vector2(XPos, YPos);

        BuildTimerGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            0,
            BuildingObjectOutlineIMG.sprite.rect.height + timerFloatingHeight
        );

        // print(BuildingObjectGO.GetComponent<Image>().rectTransform.sizeDelta.x);
        // print(BuildingObjectGO.GetComponent<Image>().rectTransform.sizeDelta.y);

        ItemName = BuildingManager.BuildingObjectsClassArr[ItemID].itemName;
        ItemCosts = BuildingManager.BuildingObjectsClassArr[ItemID].itemCosts;
        BuildingTime = BuildingManager.BuildingObjectsClassArr[ItemID].buildingTime;

        buildingTimeRemaining = BuildingTime;

        ItemNameTXT.text = "" + ItemName;
        ItemCostsTXT.text = "$" + ItemCosts.ToString("N0");

        int minutes = Mathf.FloorToInt(BuildingTime / 60);
        int seconds = Mathf.FloorToInt(BuildingTime % 60);

        if (seconds < 10) BuildingTimeTXT.text = minutes + ":" + "0" + seconds;
        else BuildingTimeTXT.text = minutes + ":" + seconds;

        CheckForMoney();
        DisplayElementStatus();
        DisplayObjectStatus();
    }


    private void Update() {
        BuildingInProgress();
    }


    public void CheckForMoney() {
        if (ElementStatus < 2) {
            if (CharacterSheetScript.MoneyCount >= ItemCosts) {
                ElementStatus = 1;
                DisplayElementStatus();
                DisplayObjectStatus();
            } else {
                ElementStatus = 0;
                DisplayElementStatus();
                DisplayObjectStatus();
            }
        }
    }


    public void DisplayElementStatus() {
        switch (ElementStatus) {
            case 0:
                ItemNameTXT.color = ColorManager.KeyWhite40;
                ItemCostsTXT.color = ColorManager.KeyWhite40;
                break;
            case 1:
                ItemNameTXT.color = ColorManager.KeyWhite100;
                ItemCostsTXT.color = ColorManager.KeyWhite100;
                break;
            case 2:
                ItemNameTXT.color = ColorManager.KeyWhite100;
                ItemCostsTXT.color = ColorManager.KeyWhite100;
                ItemCostsTXT.text = "in progress";
                break;
            case 3:
                ItemNameTXT.color = ColorManager.KeyWhite100;
                ItemCostsTXT.color = ColorManager.KeyWhite100;
                ItemCostsTXT.text = "done";
                break;
        }        
    }


    public void DisplayObjectStatus() {
        if (!IsSelected) {
            switch (ElementStatus) {
                case 0:
                    BuildingObjectOutlineIMG.color = ColorManager.KeyWhite40;
                    // BuildingObjectOutlineIMG.color = ColorManager.KeyWhite0;
                    BuildingObjectFillIMG.color = ColorManager.KeyWhite0;

                    BuildTimerIMG.enabled = false;
                    BuildTimerGO.transform.GetChild(0).gameObject.SetActive(false);
                    break;
                case 1:
                    BuildingObjectOutlineIMG.color = ColorManager.KeyWhite100;
                    // BuildingObjectOutlineIMG.color = ColorManager.KeyWhite0;
                    BuildingObjectFillIMG.color = ColorManager.KeyWhite0;

                    BuildTimerIMG.enabled = false;
                    BuildTimerGO.transform.GetChild(0).gameObject.SetActive(false);
                    break;
                case 2:
                    BuildingObjectOutlineIMG.color = ColorManager.KeyWhite100;
                    BuildingObjectFillIMG.color = ColorManager.KeyWhite40;
                    
                    BuildTimerIMG.enabled = false;
                    BuildTimerGO.transform.GetChild(0).gameObject.SetActive(true);
                    break;
                case 3:
                    BuildingObjectOutlineIMG.color = ColorManager.KeyWhite100;
                    BuildingObjectFillIMG.color = ColorManager.KeyWhite80;
                    
                    BuildTimerIMG.enabled = false;
                    BuildTimerGO.transform.GetChild(0).gameObject.SetActive(false);
                    break;
            }
        } else {
            switch (ElementStatus) {
                case 0:
                    BuildingObjectOutlineIMG.color = ColorManager.KeyOrange40;
                    BuildingObjectFillIMG.color = ColorManager.KeyOrange16;

                    BuildTimerIMG.enabled = true;
                    BuildTimerGO.transform.GetChild(0).gameObject.SetActive(true);
                    break;
                case 1:
                    BuildingObjectOutlineIMG.color = ColorManager.KeyOrange100;
                    BuildingObjectFillIMG.color = ColorManager.KeyOrange40;

                    BuildTimerIMG.enabled = true;
                    BuildTimerGO.transform.GetChild(0).gameObject.SetActive(true);
                    break;
                case 2:
                    BuildingObjectOutlineIMG.color = ColorManager.KeyOrange100;
                    BuildingObjectFillIMG.color = ColorManager.KeyOrange40;

                    BuildTimerIMG.enabled = true;
                    BuildTimerGO.transform.GetChild(0).gameObject.SetActive(true);
                    break;
                case 3:
                    BuildingObjectOutlineIMG.color = ColorManager.KeyOrange100;
                    BuildingObjectFillIMG.color = ColorManager.KeyOrange80;

                    BuildTimerIMG.enabled = false;
                    BuildTimerGO.transform.GetChild(0).gameObject.SetActive(false);
                    break;
            }
        }
    }


    private void BuildingInProgress() {
        if (IsBuilding) {

            // Short delay before the timer starts running to improve the PX
            if (buildTimerDelay > 0) {
                buildTimerDelay -= Time.deltaTime;
            } else {
                buildingTimeRemaining -= Time.deltaTime;

                // int minutes = Mathf.FloorToInt(buildingTimeRemaining / 60);
                // int seconds = Mathf.FloorToInt(buildingTimeRemaining % 60);

                // if (seconds < 10) BuildingTimeTXT.text = minutes + ":" + "0" + seconds;
                // else BuildingTimeTXT.text = minutes + ":" + seconds;

                int minutes = Mathf.FloorToInt(buildingTimeRemaining / 60);
                int seconds = Mathf.CeilToInt(buildingTimeRemaining % 60);

                if (seconds < 10) BuildingTimeTXT.text = minutes + ":" + "0" + seconds;
                else {
                    if (seconds == 60) BuildingTimeTXT.text = (minutes + 1) + ":" + "0" + "0";
                    else BuildingTimeTXT.text = minutes + ":" + seconds;
                }

                float fillProgress = 1 - (buildingTimeRemaining / BuildingTime);
                BuildingObjectFillIMG.fillAmount = fillProgress;

                if (buildingTimeRemaining <= 0) {
                    print("Built!");
                    AudioManager.instance.Play("UI Success");

                    ElementStatus = 3;
                    DisplayElementStatus();
                    DisplayObjectStatus();

                    IsBuilding = false;
                }
            }

        }
    }

}
