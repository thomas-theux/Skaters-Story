using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesMagnet : MonoBehaviour {

    public bool PlayerDetected = false;
    private Transform collectingPlayer;
    private float collectSpeed = 3.0f;


    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            collectingPlayer = other.transform;
            PlayerDetected = true;
        }
    }


    private void FixedUpdate() {
        if (PlayerDetected) {
            // Vector3 desiredPos = new Vector3(collectingPlayer.transform.position.x,  collectingPlayer.transform.position.y, collectingPlayer.transform.position.z);
            Vector3 desiredPos = collectingPlayer.transform.position;
            Vector3 smoothedPos = Vector3.Lerp(transform.parent.position, desiredPos, collectSpeed * Time.deltaTime);
            collectSpeed *= 1.2f;

            transform.parent.position = smoothedPos;
        }
    }

}
