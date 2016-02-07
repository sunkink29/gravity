using UnityEngine;
using System.Collections;

public class FirstPersonScript : MonoBehaviour {

	public GameObject playerCamera;
	[SerializeField] private CameraController cameraRotator = new CameraController();
	[SerializeField] float speed = 15;
	bool objectPickedUp = false;
	GameObject liftObject;
	Cube liftObjectScript;
	public GameObject DynamicObjects;
	GravityOnNormals gravityOnNormals;
	public float distanceFromObject = 5;
	float origialDistance = 0;
	public GameObject springJoint;

	// Use this for initialization
	void Start () {
		gravityOnNormals = GetComponent<GravityOnNormals> ();
		springJoint = GameObject.FindGameObjectWithTag ("spring");
		playerCamera = GameObject.FindWithTag ("MainCamera");
		cameraRotator.Setup (playerCamera.transform,transform);
	}

	void FixedUpdate () {
		if (objectPickedUp) {
//			Vector3 heading = playerCamera.transform.position + playerCamera.transform.forward * distanceFromObject - liftObjectScript.objectRigidbody.position;
//			float distance = heading.magnitude;
//			if (distance > origialDistance) {
//				origialDistance = distance;
//			}
//			Vector3 direction = heading / distance;
//			liftObjectScript.objectRigidbody.AddForce (direction * 5 * (distance/origialDistance), ForceMode.Acceleration);
		}
	}
	
	// Update is called once per frame
	void Update () {
		move ();
		cameraRotator.rotate ();

		if (objectPickedUp) {
			liftObject.transform.rotation = gameObject.transform.rotation;

//			if (liftObject.transform.position == (playerCamera.transform.position + playerCamera.transform.forward * distanceFromObject)) {
//				liftObjectScript.objectRigidbody.velocity = Vector3.zero;
//				liftObjectScript.objectRigidbody.angularVelocity = Vector3.zero;
//				liftObjectScript.objectRigidbody.Sleep ();
//				origialDistance = 0;
//			}

//			RaycastHit hit;
//			liftObject.transform.position = playerCamera.transform.position + playerCamera.transform.up * .5f;
//			if (liftObject.GetComponent<Rigidbody> ().SweepTest (playerCamera.transform.forward, out hit, distanceFromObject)) {
//				liftObject.transform.position += playerCamera.transform.forward * hit.distance;
//			} else {
//				liftObject.transform.position += playerCamera.transform.forward * distanceFromObject;
//			}
		}

		if (Input.GetButtonDown ("Interact")) {
			if (!objectPickedUp) {
				liftObjects ();
			} else {
				dropObject ();
			}
		}
	}

	void move()
	{
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		transform.Translate(new Vector3(horizontal * speed * Time.deltaTime, 0, vertical * speed * Time.deltaTime));

	}

	void liftObjects (){
		RaycastHit raycastHit;
		Physics.Raycast (playerCamera.transform.position, playerCamera.transform.forward, out raycastHit);
		if (raycastHit.transform.gameObject.tag == "liftable") {
			liftObject = raycastHit.transform.gameObject;
			liftObjectScript = liftObject.GetComponent<Cube> ();
			if (liftObjectScript.gravityDirection == gravityOnNormals.currentDirection) {
				liftObjectScript.useGravity = false;
//				liftObject.transform.parent = playerCamera.transform;
				objectPickedUp = true;
				liftObjectScript.pickUpObject (this);
			}
		}
	}

	void dropObject (){
		liftObject.GetComponent<Cube> ().useGravity = true;
//		liftObject.transform.parent = DynamicObjects.transform;
		objectPickedUp = false;
		liftObject.GetComponent<Cube> ().dropObject ();
		liftObject.GetComponent<Cube> ().gravityDirection = gravityOnNormals.currentDirection;
	}

}
