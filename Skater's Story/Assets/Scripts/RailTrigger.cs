using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailTrigger : MonoBehaviour {

    private SkateboardController skateboardControllerScript;
    public Transform GrindStartPos;


    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Board Collider") {
            if (skateboardControllerScript == null) {
                skateboardControllerScript = other.transform.root.GetComponent<SkateboardController>();
            }
            
            skateboardControllerScript.TricksHandlerScript.InsideRailTrigger = true;
        }
    }


    private void OnTriggerExit(Collider other) {
        if (other.tag == "Board Collider") {
            if (skateboardControllerScript == null) {
                skateboardControllerScript = other.transform.root.GetComponent<SkateboardController>();
            }
            
            skateboardControllerScript.TricksHandlerScript.InsideRailTrigger = false;
        }
    }

}
