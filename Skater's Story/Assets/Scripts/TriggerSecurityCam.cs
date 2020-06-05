using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSecurityCam : MonoBehaviour {

    public Camera SecurityCameraGO;
    private Camera PlayerCameraGO;
    private GameObject PlayerUI;
    private SkateboardController skateboardControllerScript;


    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            // Get the players camera once
            if (skateboardControllerScript == null) {
                skateboardControllerScript = other.GetComponent<SkateboardController>();

                PlayerCameraGO = skateboardControllerScript.PlayerCamera;
                PlayerUI = skateboardControllerScript.PlayerUI;
            }

            SecurityCameraGO.enabled = true;
            PlayerUI.GetComponent<Canvas>().worldCamera = SecurityCameraGO;
            PlayerCameraGO.enabled = false;
        }
    }


    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            PlayerCameraGO.enabled = true;
            PlayerUI.GetComponent<Canvas>().worldCamera = PlayerCameraGO;
            SecurityCameraGO.enabled = false;
        }
    }

}
