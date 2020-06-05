using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour {

    public Transform Target;
    public Vector3 Offset;


    private void Update() {
        transform.LookAt(Target.position + Offset);
    }

}
