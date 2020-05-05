using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevelRespawn : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            if (other.GetComponent<BoardController>()) {
                other.GetComponent<BoardController>().CheckpointRespawn();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (other.GetComponent<BoardController>()) {
                other.GetComponent<BoardController>().CheckpointRespawn();
            }
        }
    }

}
