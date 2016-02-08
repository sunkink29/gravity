using UnityEngine;
using System.Collections;

public class FirstPersonScript : MonoBehaviour {

	public GameObject playerCamera;
	[SerializeField] private CameraController cameraRotator = new CameraController();
	[SerializeField] float speed = 15;
	[SerializeField] public CubeSpringSettings cubeControllerSetting = new CubeSpringSettings();
	bool objectPickedUp = false;
	GameObject liftObject;
	CubeController liftObjectScript;
	public GameObject DynamicObjects;
	GravityOnNormals gravityOnNormals;


	// Use this for initialization
	void Start () {
		gravityOnNormals = GetComponent<GravityOnNormals> ();
		playerCamera = GameObject.FindWithTag ("MainCamera");
		cameraRotator.Setup (playerCamera.transform,transform);
	}
	
	// Update is called once per frame
	void Update () {
		move ();
		cameraRotator.rotate ();

		if (objectPickedUp) {
			liftObject.transform.rotation = gameObject.transform.rotation;
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
			liftObjectScript = liftObject.GetComponent<CubeController> ();
			if (liftObjectScript.gravityDirection == gravityOnNormals.currentDirection) {
				liftObjectScript.useGravity = false;
				objectPickedUp = true;
				liftObjectScript.pickUpObject (this);
			}
		}
	}

	void dropObject (){
		liftObject.GetComponent<CubeController> ().useGravity = true;
		objectPickedUp = false;
		liftObject.GetComponent<CubeController> ().dropObject ();
		liftObject.GetComponent<CubeController> ().gravityDirection = gravityOnNormals.currentDirection;
	}

}
