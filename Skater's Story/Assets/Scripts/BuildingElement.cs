using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingElement : MonoBehaviour {

    public CharacterSheet CharacterSheetScript;

    public int ItemID = 0;

    public string ItemName;
    public int ItemCosts;

    public TMP_Text ItemNameText;
    public TMP_Text ItemCostsText;

    // 0 = not enough money; 1 = can be built; 2 = in progress; 3 = done building
    public int ElementStatus = 0;


    private void Start() {
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
                ItemNameText.color = ColorManager.KeyWhite;
                ItemCostsText.color = ColorManager.KeyWhite;
                break;
            case 2:
                ItemNameText.color = ColorManager.KeyWhite;
                ItemCostsText.color = ColorManager.KeyWhite;
                ItemCostsText.text = "in progress";
                break;
            case 3:
                ItemNameText.color = ColorManager.KeyWhite;
                ItemCostsText.color = ColorManager.KeyWhite;
                ItemCostsText.text = "done";
                break;
        }
    }

}
