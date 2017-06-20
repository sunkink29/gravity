using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gravity))]
public class LiftableObject : MonoBehaviour, Interactible {

    GameObject playerCamera;
    protected Gravity objectGravity;
    public Rigidbody rb;
    bool held;
    bool outsideBoundery = true;
    LiftableObjectSettings settings;
    List<Method> callWhenDroppedMethods = new List<Method>();

	// Use this for initialization
	protected void Start () {
        rb = GetComponent<Rigidbody>();
        objectGravity = GetComponent<Gravity>();
        playerCamera = FirstPersonScript.player.playerCamera;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
	}
	
	// Update is called once per frame
	protected void Update () {
		if (held) {
            Vector3 targetPosition = playerCamera.transform.position + playerCamera.transform.rotation * Vector3.forward * settings.distanceFromCamera;
            Vector3 forceDirection = targetPosition - transform.position;

            float forceDistance = Vector3.Distance(transform.position, targetPosition);
            float power = forceDistance * settings.power;

            rb.AddForce(forceDirection.normalized * power * Time.deltaTime);

            rb.drag = (1 - forceDistance / settings.distanceToTarget) * 5; ;
            rb.drag = Mathf.Pow(2, rb.drag);
        }
	}

    public void interact() {
        settings = FirstPersonScript.player.liftableObjectSettings;
        rb.drag = settings.defaltDrag;
        held = !held;
        objectGravity.enabled = !held;
        FirstPersonScript.player.objectPickedUp = held;
        FirstPersonScript.player.liftableObject = this;
        if (held == false) {
            for (int i = 0; i < callWhenDroppedMethods.Count; i++) {
                callWhenDroppedMethods[i]();
            }
            callWhenDroppedMethods.Clear();
        } else {
            rb.mass = settings.mass;
        }
    } 

    public void changeGravity(Vector3 newDirection) {
        objectGravity.changeGravity(newDirection);
    }

    public void callWhenDropped(Method method) {
        callWhenDroppedMethods.Add(method);
    }

    private void OnCollisionEnter(Collision collision) {
        if (-collision.contacts[0].normal == objectGravity.currentDirection && rb != null && settings != null)
            rb.mass = settings.defaltMass;
    }

}

public delegate void Method();

[Serializable]
public class LiftableObjectSettings {
    public float distanceToTarget = 1.5f;
    public float power = 1600;
    public float distanceFromCamera = 3;
    public float defaltDrag = 1;
    public float defaltMass = 40;
    public float mass = 10;
}