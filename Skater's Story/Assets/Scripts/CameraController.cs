using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public int CameraID = 0;

	public Transform Target;

	public float SmoothSpeed = 3.0f;
	public Vector3 Offset;


	// private void Awake() {
	// 	transform.parent = transform.root;
	// }


	private void Start() {
		transform.parent = null;
	}


	private void FixedUpdate() {

        Vector3 desiredPos = Target.position + Offset;
        Vector3 smoothedPos = Vector3.Lerp(this.transform.position, desiredPos, SmoothSpeed * Time.fixedDeltaTime);
        
        this.transform.position = smoothedPos;
	}
}
