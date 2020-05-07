using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public bool THPSControls;


    private void Update() {
        if (GameSettings.ClassicControls != THPSControls) {
            GameSettings.ClassicControls = THPSControls;
        }
    }

}
