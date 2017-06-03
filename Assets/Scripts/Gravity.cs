using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Gravity : MonoBehaviour {

    Rigidbody objectRigidbody;
    public bool fixedGravity = true;
    public float gravityStrength = 9.8f;
    public Vector3 currentDirection = new Vector3(0, -1, 0);

    private void Awake() {
        objectRigidbody = GetComponent<Rigidbody>();
        objectRigidbody.useGravity = false;
    }

    private void FixedUpdate() {
        objectRigidbody.AddForce(objectRigidbody.mass * gravityStrength * currentDirection, ForceMode.Acceleration);
    }

    public void changeGravity(Vector3 newDirection) {
        if (!fixedGravity) {
            currentDirection = newDirection;
        }
    }
}
