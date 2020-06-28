using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MenuManager : MonoBehaviour {

    public SkateboardController SkateboardControllerScript;

    private Player player;

    private int menuIndex = 1;
    private int maxMenuItems = 0;

    public List<GameObject> MenusGOArr = new List<GameObject>();

    // REWIRED

    private bool LeftShoulder = false;
    private bool RightShoulder = false;
    
    
    private void Awake () {
        player = ReInput.players.GetPlayer(SkateboardControllerScript.PlayerID);

        maxMenuItems = this.gameObject.transform.childCount;

        for (int i = 0; i < maxMenuItems; i++) {
            GameObject childMenu = this.gameObject.transform.GetChild(i).gameObject;
            MenusGOArr.Add(childMenu);
        }

        ActivateMenus();
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
            }
        }

        if (RightShoulder) {
            if (menuIndex < maxMenuItems - 1) {
                menuIndex++;
                ActivateMenus();
            }
        }
    }


    private void ActivateMenus() {
        for (int i = 0; i < maxMenuItems; i++) {
            if (menuIndex == i) MenusGOArr[menuIndex].SetActive(true);
            else MenusGOArr[i].SetActive(false);
        }
    }

}
