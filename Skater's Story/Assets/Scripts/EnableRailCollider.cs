using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableRailCollider : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Board Collider") {
            if (other.transform.root.GetComponent<SkateboardController>().TricksControllerScript.trianglePressed) {
                if (!this.GetComponent<CapsuleCollider>().enabled) {
                    this.GetComponent<CapsuleCollider>().enabled = true;
                }
            }
        }
    }


    private void OnTriggerExit(Collider other) {
        if (other.tag == "Board Collider") {
            if (!other.transform.root.GetComponent<SkateboardController>().TricksControllerScript.trianglePressed) {
                if (this.GetComponent<CapsuleCollider>().enabled) {
                    this.GetComponent<CapsuleCollider>().enabled = false;
                }
            }
        }
    }

}
