using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public int CameraID = 0;

	public Transform Target;
	public Camera PlayerCam;

	// public bool ZoomOut = false;
	private float FieldOfViewDef = 60.0f;
	public float FieldOfView;

	public float SmoothSpeed = 3.0f;
	public Vector3 Offset;


	private void Start() {
		transform.parent = null;
		PlayerCam = this.GetComponent<Camera>();

		FieldOfViewDef = PlayerCam.fieldOfView;
	}


	private void Update() {
		PlayerCam.fieldOfView = FieldOfViewDef + FieldOfView;
	}


	private void FixedUpdate() {

        Vector3 desiredPos = Target.position + Offset;
        Vector3 smoothedPos = Vector3.Lerp(this.transform.position, desiredPos, SmoothSpeed * Time.fixedDeltaTime);
        
        this.transform.position = smoothedPos;
	}
}
