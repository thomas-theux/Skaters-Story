using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour {

    public CollectiblesMagnet CollectiblesMagnetScript;

    private int minAmount = 100;
    private int maxAmount = 501;

    private float rotateSpeed = 150f;
    private float moveSpeed = 3f;

    private float moveHeight = 10f;      // 1 / x

    private Vector3 startPosition;


    private void Awake() {
        startPosition = this.transform.position;
    }


    private void Update() {
        Rotating();
        Floating();
    }


    private void Rotating() {
        transform.Rotate(new Vector3(0, Time.deltaTime *rotateSpeed, 0));
    }


    private void Floating() {
        if (!CollectiblesMagnetScript.PlayerDetected) {
            float yPos = Mathf.Sin(Time.time * moveSpeed);
            transform.position = startPosition + new Vector3(0, yPos / moveHeight, 0);
        }
    }


    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            AudioManager.instance.Play("Collect Money");

            CharacterSheet CharacterSheetScript = other.GetComponent<CharacterSheet>();

            float moneyMultiplier = CharacterSheetScript.StatCharisma;
            int rndAmount = Random.Range(minAmount, maxAmount);
            int addResource = Mathf.RoundToInt(rndAmount * moneyMultiplier);

            CharacterSheetScript.MoneyCount += addResource;
            CharacterSheetScript.UpdateMoneyCount();

            Destroy(this.gameObject);
        }
    }

}
