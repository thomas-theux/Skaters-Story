using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Board Collider") {
            other.transform.root.GetComponent<SkateboardController>().TricksHandlerScript.InsideRailTrigger = true;
        }
    }


    private void OnTriggerExit(Collider other) {
        if (other.tag == "Board Collider") {
            other.transform.root.GetComponent<SkateboardController>().TricksHandlerScript.InsideRailTrigger = false;
        }
    }

}
