using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gravity))]
public class LiftableObject : MonoBehaviour, Interactible {

    GameObject playerCamera;
    protected Gravity objectGravity;
    protected Rigidbody rb;
    bool held;
    bool outsideBoundery = true;
    LiftableObjectSettings settings;

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
    } 


}
