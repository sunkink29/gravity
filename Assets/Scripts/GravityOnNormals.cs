using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GravityOnNormals : MonoBehaviour {

	Rigidbody playerRigidbody;
//	RaycastHit lasthit;
	public Vector3 currentDirection;
	Vector3 lasthitDirection;
	public float gravity = 5f;
	public float angleSpeed = 0.5f;
	bool disableXRotation = false;
	Vector3 lastRotation;
	public bool disableAutoRotate = false;
	RaycastHit raycastHit;
	FirstPersonScript attachedPlayer;
	Quaternion lastrotation;
	float pointInRotation = 0;
	bool resetPointInRotation = false;
	public bool enableGravity = true;
	CapsuleCollider playerCollider;

	// Use this for initialization
	void Start () {
		playerRigidbody = GetComponent<Rigidbody>();
//		currentDirection = Vector3.up;
		attachedPlayer = GetComponent<FirstPersonScript> ();
		playerCollider = GetComponent<CapsuleCollider> ();
	}

	void FixedUpdate ()
	{
		if (enableGravity) {
			useGravity ();
		}
		if (!disableAutoRotate) {
			rayCastGround ();
		}
	}

	// Update is called once per frame
	void Update () 
	{
		rotate ();

	}

	public void toggleXRotation (bool status) {
		disableXRotation = status;
		if (status == true)
			lastRotation = transform.rotation.eulerAngles;
	}

	void rotate (){
		if (transform.up != currentDirection) {
			Quaternion targetQuaternion = Quaternion.FromToRotation (transform.up, currentDirection) * transform.rotation;
			if (resetPointInRotation)
				pointInRotation = 0;
		
			resetPointInRotation = false;

			if (disableXRotation)
				targetQuaternion.eulerAngles = new Vector3 (lastRotation.x, lastRotation.y, targetQuaternion.eulerAngles.z);
		
			if (pointInRotation + angleSpeed * Time.deltaTime <= 1) {
				pointInRotation += angleSpeed * Time.deltaTime;

			} else if (pointInRotation + angleSpeed * Time.deltaTime >= 1) {
				pointInRotation = 1;
			}

			transform.rotation = Quaternion.Slerp (lastrotation, targetQuaternion, pointInRotation);
		}
	}

	void useGravity () {
			playerRigidbody.AddForce (-currentDirection * gravity, ForceMode.Impulse);
	}

	void rayCastGround() {
		RaycastHit hitInfo;
		bool hit = Physics.Raycast (transform.position, transform.up * -1, out hitInfo);

		attachedPlayer.rotatePlayer = true;
		if (hit) {
			if (hitInfo.distance < playerCollider.height / 2 + playerCollider.radius + 0.5f || currentDirection == new Vector3()) {
				lasthitDirection = currentDirection;
				currentDirection = hitInfo.normal;

				if (lasthitDirection != currentDirection) {
					attachedPlayer.rotatePlayer = false;
					resetPointInRotation = true;
					lastrotation = transform.localRotation;
					Debug.DrawLine (raycastHit.point, raycastHit.point + raycastHit.normal, Color.green, 2, false);
				}
			}
		} else {
			currentDirection = new Vector3 ();
		}
	}

}
