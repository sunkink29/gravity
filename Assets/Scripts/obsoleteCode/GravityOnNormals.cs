using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[Serializable]
public class GravityOnNormals /*: MonoBehaviour*/ {

	Rigidbody playerRigidbody;
//	RaycastHit lasthit;
	public Vector3 currentDirection;
	Vector3 lasthitDirection;
	public float gravity = 20f;
	public float angleSpeed = 5f;
	bool disableXRotation = false;
	Vector3 lastRotation;
	public bool disableAutoRotate = false;
	RaycastHit raycastHit;
	public FirstPersonScript attachedPlayer;
	Quaternion lastrotation;
	float pointInRotation = 0;
	bool resetPointInRotation = false;
	public bool enableGravity = true;
	CapsuleCollider playerCollider;
	public float angleDistance = 0;
	 bool rotatePlayer = false;

	// Use this for initialization
	public void Awake () {
		playerRigidbody = attachedPlayer.GetComponent<Rigidbody>();
//		currentDirection = Vector3.up;
		playerCollider = attachedPlayer.GetComponent<CapsuleCollider> ();
	}

	public void FixedUpdate ()
	{
		if (enableGravity) {
			useGravity ();
		}
		if (!disableAutoRotate) {
			rayCastGround ();
		}
		rotate ();
	}

	// Update is called once per frame
	public void Update () 
	{
		

	}

	public void toggleXRotation (bool status) {
		disableXRotation = status;
		if (status == true)
			lastRotation = attachedPlayer.transform.rotation.eulerAngles;
	}

	void rotate (){
		if (attachedPlayer.transform.up != currentDirection) {
			Quaternion targetQuaternion = Quaternion.FromToRotation ( attachedPlayer.transform.up, currentDirection) * attachedPlayer.transform.rotation;
			if (resetPointInRotation) {
				pointInRotation = 0;
				angleDistance = Vector3.Angle( attachedPlayer.transform.up, currentDirection);
				resetPointInRotation = false;
			}

			if (disableXRotation)
				targetQuaternion.eulerAngles = new Vector3 (lastRotation.x, lastRotation.y, targetQuaternion.eulerAngles.z);
		
			if (pointInRotation + angleSpeed * Time.deltaTime <= angleDistance) {
				pointInRotation += angleSpeed * Time.deltaTime;

			} else if (pointInRotation + angleSpeed * Time.deltaTime >= angleDistance) {
				pointInRotation = angleDistance;
			}

			attachedPlayer.transform.rotation = Quaternion.Slerp (lastrotation, targetQuaternion, pointInRotation);
		}
	}

	void useGravity () {
			playerRigidbody.AddForce (-currentDirection * gravity, ForceMode.Impulse);
	}

	public void turnToVector(Vector3 rotation) {
		rotatePlayer = true;
		currentDirection = rotation;
		resetPointInRotation = true;
	}

	public void rayCastGround() {
		RaycastHit hitInfo;
		bool hit = Physics.Raycast (attachedPlayer.transform.position, attachedPlayer.transform.up * -1, out hitInfo);

		attachedPlayer.rotatePlayer = true;
		if (hit) {
			if (hitInfo.distance < playerCollider.height / 2 + playerCollider.radius + 0.5f || currentDirection == new Vector3()) {
				lasthitDirection = currentDirection;
				if (!hitInfo.normal.Equals(Vector3.zero)) {
					currentDirection = hitInfo.normal;
				}

				if (lasthitDirection != currentDirection) {
					attachedPlayer.rotatePlayer = false;
					resetPointInRotation = true;
					lastrotation = attachedPlayer.transform.localRotation;
					Debug.DrawLine (raycastHit.point, raycastHit.point + raycastHit.normal, Color.green, 2, false);
				} else {
					rotatePlayer = false;
				}
			}
		} else {
			if (!rotatePlayer) {
				currentDirection = new Vector3 ();
			}
		}
	}

}
