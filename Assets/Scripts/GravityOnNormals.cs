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

	// Use this for initialization
	void Awake () {
		playerRigidbody = GetComponent<Rigidbody>();
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
	}

	// Update is called once per frame
	public void Update () 
	{
		rotate ();

	}

	public void toggleXRotation (bool status) {
		disableXRotation = status;
		if (status == true)
			lastRotation = attachedPlayer.transform.rotation.eulerAngles;
	}

	void rotate (){
		if (attachedPlayer.transform.up != currentDirection) {
			Quaternion targetQuaternion = Quaternion.FromToRotation ( attachedPlayer.transform.up, currentDirection) * attachedPlayer.transform.rotation;
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

			attachedPlayer.transform.rotation = Quaternion.Slerp (lastrotation, targetQuaternion, pointInRotation);
		}
	}

	void useGravity () {
			playerRigidbody.AddForce (-currentDirection * gravity, ForceMode.Impulse);
	}

	public void rayCastGround() {
		RaycastHit hitInfo;
		bool hit = Physics.Raycast (attachedPlayer.transform.position, attachedPlayer.transform.up * -1, out hitInfo);

		attachedPlayer.rotatePlayer = true;
		if (hit) {
			if (hitInfo.distance < playerCollider.height / 2 + playerCollider.radius + 0.5f || currentDirection == new Vector3()) {
				lasthitDirection = currentDirection;
				currentDirection = hitInfo.normal;

				if (lasthitDirection != currentDirection) {
					attachedPlayer.rotatePlayer = false;
					resetPointInRotation = true;
					lastrotation = attachedPlayer.transform.localRotation;
					Debug.DrawLine (raycastHit.point, raycastHit.point + raycastHit.normal, Color.green, 2, false);
				}
			}
		} else {
			currentDirection = new Vector3 ();
		}
	}

}
