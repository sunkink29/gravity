using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour , Interactible {

	public bool useGravity = true;
	public Vector3 gravityDirection;
	public float gravityStrenth = 5f;
	public float cubeMass = 10;
	[HideInInspector]public Rigidbody objectRigidbody;
	LiftableObjectSettings cubeControllerSettings;
	SpringJoint springJoint;
	public bool objectPickedUp = false;
	FirstPersonScript playerScript;
	Transform playerCamera;
	Collider boxCollider;
	public bool interactible = true;

	// Use this for initialization
	void Start () {
		//gravityDirection = Vector3.up;
		if (GetComponent<Rigidbody> () == null) {
			objectRigidbody = gameObject.AddComponent<Rigidbody> ();
		} else {
			objectRigidbody = GetComponent<Rigidbody> ();
		}
		boxCollider = GetComponent<Collider> ();
//		gameObject.tag = "liftable";
//		objectRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		objectRigidbody.useGravity = false;
		objectRigidbody.mass = cubeMass;

	}

	void FixedUpdate () {
		if (useGravity) {
			gravity ();
		}
	}
	
	// Update is called once per frame
	void Update () {
//		if ((objectRigidbody.velocity.x + 	objectRigidbody.velocity.y + objectRigidbody.velocity.z / 3) == 30) {
//			objectRigidbody.velocity = new Vector3 (0, 0, 0);
//		}
		if (objectPickedUp) {
			if (objectRigidbody.isKinematic == true) {
				objectRigidbody.isKinematic = false;
			}
			//Vector3 heading = (playerCamera.position + playerCamera.forward * cubeControllerSettings.distanceFromObject) - transform.position;
			//float distance = heading.magnitude;
			//Vector3 direction = heading / distance;
			//objectRigidbody.AddForce (direction * cubeControllerSettings.force * distance, ForceMode.Acceleration);

			//if (springJoint.maxDistance != cubeControllerSettings.distanceFromObject) {
				//springJoint.maxDistance = cubeControllerSettings.distanceFromObject;
				//springJoint.minDistance = cubeControllerSettings.distanceFromObject;
			//}
			//if (springJoint.spring != cubeControllerSettings.spring) {
				//springJoint.spring = cubeControllerSettings.spring;
			//}
			//if (springJoint.damper != cubeControllerSettings.damper) {
				//springJoint.damper = cubeControllerSettings.damper;
			//}
			//if (objectRigidbody.drag != cubeControllerSettings.rigidbodyDrag) {
				//objectRigidbody.drag = cubeControllerSettings.rigidbodyDrag;
			//}
			//if (objectRigidbody.angularDrag != cubeControllerSettings.rigidbodyAnglarDrag) {
				//objectRigidbody.angularDrag = cubeControllerSettings.rigidbodyAnglarDrag;
			//}
			//transform.rotation = playerScript.transform.rotation;
		}
	}

	void gravity () {
		RaycastHit hitInfo;
		bool hit = Physics.Raycast (transform.position, gravityDirection, out hitInfo);
		if (objectRigidbody.velocity == Vector3.zero) {
			objectRigidbody.isKinematic = true;
		}
		if (hitInfo.distance <= 0.1 + 0.5 && hit && !objectRigidbody.isKinematic) {
			objectRigidbody.position += (hitInfo.distance - 0.5f) * gravityDirection;
			objectRigidbody.isKinematic = true;
//		} else if (Physics.Raycast (transform.position + new Vector3 (0.5f, 0, 0.5f), gravityDirection, out hitInfo, .1f + .5f)&& !objectRigidbody.isKinematic) {
//			objectRigidbody.position += (hitInfo.distance - 0.5f) * gravityDirection;
//			objectRigidbody.isKinematic = true;
//		} else if (Physics.Raycast (transform.position + new Vector3 (-0.5f, 0, -0.5f), gravityDirection, out hitInfo, .1f + .5f)&& !objectRigidbody.isKinematic) {
//			objectRigidbody.position += (hitInfo.distance - 0.5f) * gravityDirection;
//			objectRigidbody.isKinematic = true;
//		} else if (Physics.Raycast (transform.position + new Vector3 (0.5f, 0, -0.5f), gravityDirection, out hitInfo, .1f + .5f)&& !objectRigidbody.isKinematic) {
//			objectRigidbody.position += (hitInfo.distance - 0.5f) * gravityDirection;
//			objectRigidbody.isKinematic = true;
//		} else if (Physics.Raycast (transform.position + new Vector3 (-0.5f, 0, 0.5f), gravityDirection, out hitInfo, .1f + .5f) && !objectRigidbody.isKinematic) {
//			objectRigidbody.position += (hitInfo.distance - 0.5f) * gravityDirection;
//			objectRigidbody.isKinematic = true;
		} else if (hitInfo.distance > 0.1 + 0.5) {
			objectRigidbody.isKinematic = false;
			objectRigidbody.velocity = Vector3.zero;
			transform.eulerAngles = hitInfo.normal;
		}
		objectRigidbody.velocity += gravityStrenth * gravityDirection * Time.fixedDeltaTime;
//		print (objectRigidbody.isKinematic + gameObject.name);
//		objectRigidbody.AddForce (gravityStrenth * gravityDirection * Time.fixedDeltaTime);
	}

	public void pickUpObject (FirstPersonScript player) {
		if (player != playerScript) {
			playerScript = player;
			playerCamera = player.playerCamera.transform;
			cubeControllerSettings = player.liftableObjectSettings;
		}
		RaycastHit hit;
		transform.position = playerCamera.transform.position + playerCamera.transform.up * .5f;
		springJoint = gameObject.AddComponent<SpringJoint> ();
		springJoint.connectedBody = player.GetComponent<Rigidbody> ();
		//springJoint.maxDistance = cubeControllerSettings.distanceFromObject;
		//springJoint.minDistance = cubeControllerSettings.distanceFromObject;
		//springJoint.spring = cubeControllerSettings.spring;
		//springJoint.damper = cubeControllerSettings.damper;
		//objectRigidbody.drag = cubeControllerSettings.rigidbodyDrag;
		//objectRigidbody.angularDrag = cubeControllerSettings.rigidbodyAnglarDrag;
		//if (objectRigidbody.SweepTest (playerCamera.transform.forward, out hit, cubeControllerSettings.distanceFromObject)) {
			//transform.position += playerCamera.transform.forward * hit.distance;
		//} else {
			//transform.position += playerCamera.transform.forward * cubeControllerSettings.distanceFromObject;
		//}

		objectPickedUp = true;
		useGravity = false;
		gameObject.layer = 9;
		player.objectPickedUp = true;
		//player.liftableObject = this;
	}

	public void dropObject () {
		objectPickedUp = false;
		Destroy (springJoint);
		objectRigidbody.isKinematic = false;
		gameObject.layer = 1;
		useGravity = true;
		playerScript.objectPickedUp = false;
	}

	public void interact() {
        print("replace cubeController with new liftableObject script");
		if (interactible && false) {
			if (!objectPickedUp) {
				pickUpObject (FirstPersonScript.player);
			} else {
				dropObject ();
			}
		}
	}

}
