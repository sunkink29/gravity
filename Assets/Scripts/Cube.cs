using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {

	public bool useGravity = true;
	public Vector3 gravityDirection;
	[HideInInspector]public Rigidbody objectRigidbody;
	public float gravityStrenth = 5f;
	public static float minSpringDistance = 5;
	public static float maxSpringDistance = 5;
	public static float spring = 200;
	public static float damper = 5;
	public static float rigidbodyDrag = 15;
	public static float rigidbodyAnglarDrag = 15;
	SpringJoint springJoint;
	bool objectPickedUp = false;
	BoxCollider triggerCollider;
	Vector3 lastPosition;
	FirstPersonScript playerScript;

	// Use this for initialization
	void Start () {
		gravityDirection = Vector3.up;
		if (GetComponent<Rigidbody> () == null) {
			gameObject.AddComponent<Rigidbody> ();
		}
		objectRigidbody = GetComponent<Rigidbody> ();
		gameObject.tag = "liftable";
		triggerCollider = gameObject.AddComponent<BoxCollider> ();
		objectRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		triggerCollider.isTrigger = true;
		triggerCollider.center = Vector3.Scale(transform.InverseTransformDirection(Vector3.down),new Vector3(1,.5f/transform.lossyScale.y,1));
		triggerCollider.size = new Vector3 (.5f, .5f, .5f);
	}

	void FixedUpdate () {
		if (useGravity) {
			gravity ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if ((objectRigidbody.velocity.x + 	objectRigidbody.velocity.y + objectRigidbody.velocity.z / 3) == 30) {
			objectRigidbody.velocity = new Vector3 (0, 0, 0);
		}
		if (objectPickedUp) {
			if (objectRigidbody.isKinematic == true) {
				objectRigidbody.isKinematic = false;
			}
			
		}
	}

	void gravity () {
		objectRigidbody.AddForce (gravityStrenth * gravityDirection * -1 * Time.fixedTime,ForceMode.Acceleration);
		lastPosition = transform.position;
	}

	public void pickUpObject (FirstPersonScript player) {
		if (player != playerScript) {
			playerScript = player;
		}
		springJoint = gameObject.AddComponent<SpringJoint> ();
//		transform.position = player.springJoint.transform.position;
		springJoint.connectedBody = player.GetComponent<Rigidbody> ();
		springJoint.maxDistance = maxSpringDistance;
		springJoint.minDistance = player.distanceFromObject;
		springJoint.spring = spring;
		springJoint.damper = damper;
		objectRigidbody.drag = rigidbodyDrag;
		objectRigidbody.angularDrag = rigidbodyAnglarDrag;

		objectPickedUp = true;
		gameObject.layer = 9;
	}

	public void dropObject () {
		objectPickedUp = false;
		Destroy (springJoint);
		objectRigidbody.isKinematic = false;
		gameObject.layer = 1;
	}

	void OnTriggerEnter () {
		if (!objectPickedUp) {
			RaycastHit hit;
			Vector3 heading = lastPosition - transform.position;
			float distance = heading.magnitude;
			Vector3 direction = heading / distance;
			if (objectRigidbody.SweepTest (direction, out hit, distance)) {
				transform.position = lastPosition;
				transform.position += direction * (distance - hit.distance);
			}
			objectRigidbody.isKinematic = true;
		}


	}
}
