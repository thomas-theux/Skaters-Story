using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BalanceController : MonoBehaviour {

    public TricksHandler TricksHandlerScript;

    public GameObject GrindingGO;
    private Slider GrindSlider;


    private void Awake() {
        GrindSlider = GrindingGO.GetComponent<Slider>();
    }


    private void Update() {
    }

}
