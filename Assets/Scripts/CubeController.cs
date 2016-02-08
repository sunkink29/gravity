using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour {

	public bool useGravity = true;
	public Vector3 gravityDirection;
	public float gravityStrenth = 5f;
	[HideInInspector]public Rigidbody objectRigidbody;
	CubeSpringSettings cubeControllerSettings;
	SpringJoint springJoint;
	bool objectPickedUp = false;
	BoxCollider triggerCollider;
	Vector3 lastPosition;
	FirstPersonScript playerScript;
	Transform playerCamera;

	// Use this for initialization
	void Start () {
		gravityDirection = Vector3.up;
		if (GetComponent<Rigidbody> () == null) {
			objectRigidbody = gameObject.AddComponent<Rigidbody> ();
		} else {
			objectRigidbody = GetComponent<Rigidbody> ();
		}
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
			Vector3 heading = (playerCamera.position + playerCamera.forward * cubeControllerSettings.distanceFromObject) - transform.position;
			float distance = heading.magnitude;
			Vector3 direction = heading / distance;
			objectRigidbody.AddForce (direction * cubeControllerSettings.force * distance, ForceMode.Acceleration);
			if (springJoint.maxDistance != cubeControllerSettings.distanceFromObject) {
				springJoint.maxDistance = cubeControllerSettings.distanceFromObject;
				springJoint.minDistance = cubeControllerSettings.distanceFromObject;
			}
			if (springJoint.spring != cubeControllerSettings.spring) {
				springJoint.spring = cubeControllerSettings.spring;
			}
			if (springJoint.damper != cubeControllerSettings.damper) {
				springJoint.damper = cubeControllerSettings.damper;
			}
			if (objectRigidbody.drag != cubeControllerSettings.rigidbodyDrag) {
				objectRigidbody.drag = cubeControllerSettings.rigidbodyDrag;
			}
			if (objectRigidbody.angularDrag != cubeControllerSettings.rigidbodyAnglarDrag) {
				objectRigidbody.angularDrag = cubeControllerSettings.rigidbodyAnglarDrag;
			}
		}
	}

	void gravity () {
		objectRigidbody.AddForce ( gravityStrenth * gravityDirection * -1 * Time.fixedTime,ForceMode.Acceleration);
		lastPosition = transform.position;
	}

	public void pickUpObject (FirstPersonScript player) {
		if (player != playerScript) {
			playerScript = player;
			playerCamera = player.playerCamera.transform;
			cubeControllerSettings = player.cubeControllerSetting;
		}
		RaycastHit hit;
		transform.position = playerCamera.transform.position + playerCamera.transform.up * .5f;
		springJoint = gameObject.AddComponent<SpringJoint> ();
		springJoint.connectedBody = player.GetComponent<Rigidbody> ();
		springJoint.maxDistance = cubeControllerSettings.distanceFromObject;
		springJoint.minDistance = cubeControllerSettings.distanceFromObject;
		springJoint.spring = cubeControllerSettings.spring;
		springJoint.damper = cubeControllerSettings.damper;
		objectRigidbody.drag = cubeControllerSettings.rigidbodyDrag;
		objectRigidbody.angularDrag = cubeControllerSettings.rigidbodyAnglarDrag;
		if (objectRigidbody.SweepTest (playerCamera.transform.forward, out hit, cubeControllerSettings.distanceFromObject)) {
			transform.position += playerCamera.transform.forward * hit.distance;
		} else {
			transform.position += playerCamera.transform.forward * cubeControllerSettings.distanceFromObject;
		}

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
