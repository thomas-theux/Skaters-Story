using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gap : MonoBehaviour {

    public string GapName = "";


    private void Awake() {
        GapName = gameObject.name;
    }


    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            other.GetComponent<SkateboardController>().TricksHandlerScript.AddComboElement("<#1BE7FF>" + GapName + "</color>", 50);
        }
    }

}
