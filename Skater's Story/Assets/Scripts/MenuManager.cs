using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class MenuManager : MonoBehaviour {

    public SkateboardController SkateboardControllerScript;

    public RectTransform MenuNavRect;

    private Player player;

    private int menuIndex = 1;
    private int maxMenuItems = 0;
    private int lastItem = 0;

    private int navSizeMin = 336;
    private int navSizeMax = 456;

    public List<GameObject> MenusGOArr = new List<GameObject>();
    public List<GameObject> MenuNavGOArr = new List<GameObject>();
    public List<Image> MenuNavIMGArr = new List<Image>();

    // REWIRED
    private bool LeftShoulder = false;
    private bool RightShoulder = false;
    
    
    private void Awake () {
        player = ReInput.players.GetPlayer(SkateboardControllerScript.PlayerID);

        maxMenuItems = this.gameObject.transform.childCount;

        for (int i = 0; i < maxMenuItems; i++) {
            GameObject childMenu = this.gameObject.transform.GetChild(i).gameObject;
            MenusGOArr.Add(childMenu);

            MenuNavGOArr.Add(MenuNavRect.transform.GetChild(i).gameObject);
            MenuNavIMGArr.Add(MenuNavGOArr[i].GetComponent<Image>());
        }

        lastItem = MenuNavGOArr.Count - 1;

        ActivateMenus();
        DisplayActiveMenu();
    }


    private void OnEnable() {
        MenuNavRect.sizeDelta = new Vector2(navSizeMax, MenuNavRect.sizeDelta.y);
        MenuNavGOArr[0].SetActive(true);
        MenuNavGOArr[lastItem].SetActive(true);

        ActivateMenus();
        DisplayActiveMenu();
    }


    private void OnDisable() {
        for (int i = 0; i < MenuNavIMGArr.Count; i++) {
            MenuNavIMGArr[i].color = ColorManager.KeyWhite100;
        }

        MenuNavRect.sizeDelta = new Vector2(navSizeMin, MenuNavRect.sizeDelta.y);
        MenuNavGOArr[0].SetActive(false);
        MenuNavGOArr[lastItem].SetActive(false);
    }


    private void Update() {
        GetInput();

        NavigateMainMenu();
    }


    private void GetInput() {
        LeftShoulder = player.GetButtonDown("Left Shoulder 1");
        RightShoulder = player.GetButtonDown("Right Shoulder 1");
    }


    private void NavigateMainMenu() {
        if (LeftShoulder) {
            if (menuIndex > 0) {
                menuIndex--;
                ActivateMenus();
                DisplayActiveMenu();
            }
        }

        if (RightShoulder) {
            if (menuIndex < maxMenuItems - 1) {
                menuIndex++;
                ActivateMenus();
                DisplayActiveMenu();
            }
        }
    }


    private void ActivateMenus() {
        for (int i = 0; i < maxMenuItems; i++) {
            if (menuIndex == i) MenusGOArr[menuIndex].SetActive(true);
            else MenusGOArr[i].SetActive(false);
        }
    }


    private void DisplayActiveMenu() {
        for (int i = 0; i < maxMenuItems; i++) {
            if (menuIndex == i) MenuNavIMGArr[menuIndex].color = ColorManager.KeyWhite100;
            else MenuNavIMGArr[i].color = ColorManager.KeyWhite40;
        }
    }

}
