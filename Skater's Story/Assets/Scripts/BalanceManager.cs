using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalanceManager : MonoBehaviour {

    public TricksHandler TricksHandlerScript;

    public GameObject GrindBalanceGO;
    public GameObject ManualBalanceGO;

    private BalanceController GrindBalanceScript;
    private BalanceController ManualBalanceScript;


    private void Awake() {
        GrindBalanceScript = GrindBalanceGO.GetComponent<BalanceController>();
        ManualBalanceScript = ManualBalanceGO.GetComponent<BalanceController>();
    }


    private void OnDisable() {
        GrindBalanceScript.currentValue = 50;
        GrindBalanceScript.valueIncreaser = 1f;

        ManualBalanceScript.currentValue = 50;
        ManualBalanceScript.valueIncreaser = 1f;
    }

}
