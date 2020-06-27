using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rewired;

public class BuildingManager : MonoBehaviour {

    private Player player;

    public CharacterSheet CharacterSheetScript;
    public SkateboardController SkateboardControllerScript;

    public GameObject ElementsContainer;
    public GameObject ObjectsContainer;
    public GameObject BuildingElementGO;
    public GameObject BuildingCursorGO;

    public TMP_Text PercentageText;

    public static List<BuildingObjects> BuildingObjectsClassArr = new List<BuildingObjects>();
    public static List<BuildingElement> BuildingElementsScriptArr = new List<BuildingElement>();

    public List<GameObject> BuildingObjectsArr = new List<GameObject>();

    public List<bool> RebuiltElementsArr = new List<bool>();
    private List<GameObject> BuildingElementsArr = new List<GameObject>();

    public int RebuiltElementsCount = 0;
    // private int MaxBuildingElements = 24;
    private int MaxBuildingElements = 9;

    private float itemsDistance = 248f;
    private float cursorMarginX = 3f;
    private float cursorMarginY = 0f;

    private float elementsContainerStartPosX = 0;

    public int rebuildIndex = 0;
    public int scrollIndex = 0;

    public int minScrollIndex = 1;
    public int maxScrollIndex = 4;

    public float RebuiltPercentage = 0.0f;

    private float axisThreshold = 0.5f;
    private bool hasNavigated = false;

    // REWIRED
    private bool dPadLeft = false;
    private bool dPadRight = false;
    private float horizontalAxis;

    private bool XButton = false;


    private void Awake() {
        BuildingObjectsClassArr.Add(new Object00());
        BuildingObjectsClassArr.Add(new Object01());
        BuildingObjectsClassArr.Add(new Object02());
        BuildingObjectsClassArr.Add(new Object03());
        BuildingObjectsClassArr.Add(new Object04());
        BuildingObjectsClassArr.Add(new Object05());
        BuildingObjectsClassArr.Add(new Object06());
        BuildingObjectsClassArr.Add(new Object07());
        BuildingObjectsClassArr.Add(new Object08());
        BuildingObjectsClassArr.Add(new Object09());
        BuildingObjectsClassArr.Add(new Object10());
        BuildingObjectsClassArr.Add(new Object11());
        BuildingObjectsClassArr.Add(new Object12());
        BuildingObjectsClassArr.Add(new Object13());
        BuildingObjectsClassArr.Add(new Object14());
        BuildingObjectsClassArr.Add(new Object15());
        BuildingObjectsClassArr.Add(new Object16());
        BuildingObjectsClassArr.Add(new Object17());
        BuildingObjectsClassArr.Add(new Object18());
        BuildingObjectsClassArr.Add(new Object19());
        BuildingObjectsClassArr.Add(new Object20());
        BuildingObjectsClassArr.Add(new Object21());
        BuildingObjectsClassArr.Add(new Object22());
        BuildingObjectsClassArr.Add(new Object23());

        //////////////////////////////////////////////////////////////////////////////////////

        for (int i = 0; i < MaxBuildingElements; i++) {
            ///////////////////////////////////////////
            // Instantiate navigation elements

            GameObject newBuildingElement = Instantiate(BuildingElementGO);

            BuildingElement BuildingElementScript = newBuildingElement.GetComponent<BuildingElement>();

            BuildingElementScript.ItemID = i;

            newBuildingElement.transform.SetParent(ElementsContainer.transform);
            BuildingElementScript.BuildingObjectGO.transform.SetParent(ObjectsContainer.transform);

            if (i < 10) {
                newBuildingElement.name = "BuildingElement" + "0" + i;
                BuildingElementScript.BuildingObjectGO.name = "BuildingObject" + "0" + i;
            } else {
                newBuildingElement.name = "BuildingElement" + i;
                BuildingElementScript.BuildingObjectGO.name = "BuildingObject" + i;
            }

            newBuildingElement.transform.localScale = new Vector3(1, 1, 1);

            RectTransform newRectTransform = newBuildingElement.GetComponent<RectTransform>();

            newRectTransform.offsetMin = new Vector2(newRectTransform.offsetMin.x, 0);
            newRectTransform.offsetMax = new Vector2(newRectTransform.offsetMax.x, 0);

            float newItemPos = itemsDistance * i;
            newRectTransform.anchoredPosition = new Vector2(newItemPos, 0);

            BuildingElementScript.CharacterSheetScript = CharacterSheetScript;

            BuildingElementsArr.Add(newBuildingElement);
            BuildingElementsScriptArr.Add(BuildingElementScript);
        }

        //////////////////////////////////////////////////////////////////////////////////////

        player = ReInput.players.GetPlayer(SkateboardControllerScript.PlayerID);

        // Instantiate elements array
        for (int i = 0; i < MaxBuildingElements; i++) {
            RebuiltElementsArr.Add(false);
        }

        elementsContainerStartPosX = ElementsContainer.transform.position.x;

        DisplayBuildPercentage();
        DisplayBuildingCursor();
    }


    private void Update() {
        GetInput();

        NavigateBuildingMode();
        BuildElement();
    }


    private void GetInput() {
        // Only be able to navigate and build when the menu is open
        if (CharacterSheetScript.MenuOpen) {
            dPadLeft = player.GetButtonDown("DPad Left");
            dPadRight = player.GetButtonDown("DPad Right");
            horizontalAxis = player.GetAxis("Horizontal Stick");

            XButton = player.GetButtonDown("X");
        }
    }


    private void NavigateBuildingMode() {
        if (horizontalAxis < -axisThreshold && !hasNavigated || dPadLeft && !hasNavigated) {
            if (rebuildIndex > 0) {
                hasNavigated = true;
            
                rebuildIndex--;
                AudioManager.instance.Play("UI Navigate");

                // if (rebuildIndex < 0) {
                //     rebuildIndex = MaxBuildingElements - 1;
                // }

                if (scrollIndex > minScrollIndex) {
                    scrollIndex--;
                } else {
                    MoveElementsContainer();
                }

                DisplayBuildingCursor();
            }
        }

        if (horizontalAxis > axisThreshold && !hasNavigated || dPadRight && !hasNavigated) {
            if (rebuildIndex < MaxBuildingElements - 1) {
                hasNavigated = true;
            
                rebuildIndex++;
                AudioManager.instance.Play("UI Navigate");

                // if (rebuildIndex > MaxBuildingElements - 1) {
                //     rebuildIndex = 0;
                // }

                if (scrollIndex < maxScrollIndex) {
                    scrollIndex++;
                } else {
                    MoveElementsContainer();
                }

                DisplayBuildingCursor();
            }
        }


        if (horizontalAxis >= -axisThreshold && horizontalAxis <= axisThreshold && !dPadRight && !dPadLeft && hasNavigated) {
            hasNavigated = false;
        }
    }


    private void MoveElementsContainer() {
        if (rebuildIndex > 0 && rebuildIndex < MaxBuildingElements - 2) {
            float moveDistance = (rebuildIndex - scrollIndex) * itemsDistance;

            ElementsContainer.transform.localPosition = new Vector2(
                elementsContainerStartPosX - moveDistance,
                ElementsContainer.transform.localPosition.y
            );
        } else if (rebuildIndex >= MaxBuildingElements - 2) {
            float moveDistance = (MaxBuildingElements * itemsDistance) - 1536;

            ElementsContainer.transform.localPosition = new Vector2(
                elementsContainerStartPosX - moveDistance,
                ElementsContainer.transform.localPosition.y
            );
        }
    }


    private void DisplayBuildingCursor() {
        BuildingCursorGO.transform.localPosition = new Vector2(
            BuildingElementsArr[rebuildIndex].transform.localPosition.x + cursorMarginX,
            BuildingElementsArr[rebuildIndex].transform.localPosition.y + cursorMarginY
        );

        // Tell the element if it's selected or not
        for (int i = 0; i < BuildingElementsScriptArr.Count; i++) {
            if (rebuildIndex == i) BuildingElementsScriptArr[rebuildIndex].IsSelected = true;
            else BuildingElementsScriptArr[i].IsSelected = false;

            BuildingElementsScriptArr[i].DisplayObjectStatus();
        }
    }


    private void BuildElement() {
        if (XButton) {
            if (BuildingElementsScriptArr[rebuildIndex].ElementStatus == 1) {
                    // Build element
                    AudioManager.instance.Play("UI Build");

                    CharacterSheetScript.MoneyCount -= BuildingObjectsClassArr[rebuildIndex].itemCosts;
                    CharacterSheetScript.UpdateMoneyCount();

                    BuildingElementsScriptArr[rebuildIndex].ElementStatus = 2;
                    BuildingElementsScriptArr[rebuildIndex].DisplayElementStatus();

                    StartCoroutine(BuildingInProgress(rebuildIndex, BuildingObjectsClassArr[rebuildIndex].buildingTime));
            } else if (BuildingElementsScriptArr[rebuildIndex].ElementStatus == 0) {
                // Missing resources
                print("Missing resources!");
                AudioManager.instance.Play("UI Error");
            } else if (BuildingElementsScriptArr[rebuildIndex].ElementStatus == 2) {
                // In progress
                print("in progress");
                AudioManager.instance.Play("UI Error");
            } else if (BuildingElementsScriptArr[rebuildIndex].ElementStatus == 3) {
                // Already built
                print("Already built!");
                AudioManager.instance.Play("UI Error");
            }
        }
    }


    private IEnumerator BuildingInProgress(int elementIndex, float buildingTime) {
        BuildingElementsScriptArr[elementIndex].IsBuilding = true;

        yield return new WaitForSecondsRealtime(buildingTime);

        // print("Built!");

        RebuiltElementsArr[elementIndex] = true;
        RebuiltElementsCount++;
        
        // BuildingElementsScriptArr[elementIndex].ElementStatus = 3;
        // BuildingElementsScriptArr[elementIndex].DisplayElementStatus();

        DisplayBuildPercentage();
    }


    private void DisplayBuildPercentage() {
        // RebuiltPercentage = (Mathf.Round(RebuiltElementsCount) / Mathf.Round(MaxBuildingElements)) * 100;
        PercentageText.text = RebuiltElementsCount + "/" + MaxBuildingElements;
    }


    private void DisplayObjectsStatus() {
        for (int i = 0; i < BuildingObjectsArr.Count; i++) {}
    }

}
