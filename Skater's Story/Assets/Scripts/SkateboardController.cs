using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rewired;

public class SkateboardController : MonoBehaviour {

    private Rigidbody rb;

    public float speed = 90f;
    private float powerInput;


    void Awake () {
        rb = GetComponent <Rigidbody>();
    }


    void Update () {
        powerInput = Input.GetAxis ("Horizontal");
    }


    void FixedUpdate() {
        rb.AddRelativeForce(powerInput * speed, 0, 0);
    }

}