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

    public float XPos;
    public float YPos;

    public TMP_Text ItemNameText;
    public TMP_Text ItemCostsText;

    public bool IsSelected = false;

    private Image BuildingObjectOutlineIMG;
    private Image BuildingObjectFillIMG;

    // 0 = not enough money; 1 = can be built; 2 = in progress; 3 = done building
    public int ElementStatus = 0;


    private void Start() {
        BuildingObjectOutlineIMG = BuildingObjectGO.GetComponent<Image>();
        BuildingObjectFillIMG = BuildingObjectGO.transform.GetChild(0).GetComponent<Image>();

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

        // print(BuildingObjectGO.GetComponent<Image>().rectTransform.sizeDelta.x);
        // print(BuildingObjectGO.GetComponent<Image>().rectTransform.sizeDelta.y);

        ItemName = BuildingManager.BuildingObjectsClassArr[ItemID].itemName;
        ItemCosts = BuildingManager.BuildingObjectsClassArr[ItemID].itemCosts;

        ItemNameText.text = "" + ItemName;
        ItemCostsText.text = "$" + ItemCosts.ToString("N0");

        CheckForMoney();
        DisplayElementStatus();
    }


    public void CheckForMoney() {
        if (ElementStatus < 2) {
            if (CharacterSheetScript.MoneyCount >= ItemCosts) {
                ElementStatus = 1;
                DisplayElementStatus();
            } else {
                ElementStatus = 0;
                DisplayElementStatus();
            }
        }
    }


    public void DisplayElementStatus() {
        switch (ElementStatus) {
            case 0:
                ItemNameText.color = ColorManager.KeyWhite40;
                ItemCostsText.color = ColorManager.KeyWhite40;
                break;
            case 1:
                ItemNameText.color = ColorManager.KeyWhite100;
                ItemCostsText.color = ColorManager.KeyWhite100;
                break;
            case 2:
                ItemNameText.color = ColorManager.KeyWhite100;
                ItemCostsText.color = ColorManager.KeyWhite100;
                ItemCostsText.text = "in progress";
                break;
            case 3:
                ItemNameText.color = ColorManager.KeyWhite100;
                ItemCostsText.color = ColorManager.KeyWhite100;
                ItemCostsText.text = "done";
                break;
        }        
    }


    public void DisplayObjectStatus() {
        if (!IsSelected) {
            switch (ElementStatus) {
                case 0:
                    BuildingObjectOutlineIMG.color = ColorManager.KeyWhite40;
                    BuildingObjectFillIMG.color = ColorManager.KeyWhite0;
                    break;
                case 1:
                    BuildingObjectOutlineIMG.color = ColorManager.KeyWhite100;
                    BuildingObjectFillIMG.color = ColorManager.KeyWhite0;
                    break;
                case 2:
                    BuildingObjectOutlineIMG.color = ColorManager.KeyWhite100;
                    BuildingObjectOutlineIMG.color = ColorManager.KeyWhite40;
                    break;
                case 3:
                    BuildingObjectOutlineIMG.color = ColorManager.KeyWhite100;
                    BuildingObjectFillIMG.color = ColorManager.KeyWhite80;
                    break;
            }
        } else {
            switch (ElementStatus) {
                case 0:
                    BuildingObjectOutlineIMG.color = ColorManager.KeyOrange40;
                    BuildingObjectFillIMG.color = ColorManager.KeyOrange16;
                    break;
                case 1:
                    BuildingObjectOutlineIMG.color = ColorManager.KeyOrange100;
                    BuildingObjectFillIMG.color = ColorManager.KeyOrange40;
                    break;
                case 2:
                    BuildingObjectOutlineIMG.color = ColorManager.KeyOrange100;
                    BuildingObjectOutlineIMG.color = ColorManager.KeyOrange40;
                    break;
                case 3:
                    BuildingObjectOutlineIMG.color = ColorManager.KeyOrange100;
                    BuildingObjectFillIMG.color = ColorManager.KeyOrange80;
                    break;
            }
        }
    }

}
