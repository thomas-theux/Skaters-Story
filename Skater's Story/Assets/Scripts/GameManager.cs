using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public bool THPSControls;

    [Range(0, 2)]
    public int SlowMotion;


    private void Update() {
        // Adjust control scheme
        if (GameSettings.ClassicControls != THPSControls) {
            GameSettings.ClassicControls = THPSControls;
        }

        // Adjust slow motion settings
        if (GameSettings.SlowMotionTricks != SlowMotion) {
            GameSettings.SlowMotionTricks = SlowMotion;
        }
    }

}
