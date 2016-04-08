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
	[SerializeField] bool disableAutoRotate = false;
	RaycastHit raycastHit;
	Vector3 pickupDirection;
	Vector3 pickupDownDirection;
	FirstPersonScript attachedPlayer;
	Quaternion lastrotation;
	float pointInRotation = 0;
	bool resetPointInRotation = false;

	// Use this for initialization
	void Start () {
		playerRigidbody = GetComponent<Rigidbody>();
		currentDirection = Vector3.up;
		lasthitDirection = currentDirection;
		lastrotation = Quaternion.FromToRotation (transform.up, lasthitDirection);

		Physics.Raycast(gameObject.transform.position,Vector3.down,out raycastHit);
		attachedPlayer = GetComponent<FirstPersonScript> ();
	}

	void FixedUpdate ()
	{
		usePlayerGravity ();
	}

	// Update is called once per frame
	void Update () 
	{
		if (!disableAutoRotate) {
			rotate ();
		}

	}

	public void toggleXRotation (bool status) {
		disableXRotation = status;
		if (status == true)
			lastRotation = transform.rotation.eulerAngles;
	}

	void rotate (){
		Quaternion targetQuaternion = Quaternion.FromToRotation ( transform.up,currentDirection) * transform.rotation;
		if (resetPointInRotation)
			pointInRotation = 0;
		resetPointInRotation = false;
		if (disableXRotation)
			targetQuaternion.eulerAngles = new Vector3(lastRotation.x,lastRotation.y,targetQuaternion.eulerAngles.z);
		if (pointInRotation + angleSpeed * Time.deltaTime <= 1) {
			pointInRotation += angleSpeed * Time.deltaTime;
		} else if (pointInRotation + angleSpeed * Time.deltaTime >= 1) {
			pointInRotation = 1;
		}
//		lastrotation.eulerAngles = new Vector3 (lastrotation.eulerAngles.x,transform.rotation.eulerAngles.y,lastrotation.eulerAngles.z);
		transform.rotation = Quaternion.Slerp (lastrotation, targetQuaternion, pointInRotation);
	}
	
	void usePlayerGravity ()
	{
		RaycastHit[] allHits;
		float minDistance = 1000;
		allHits = Physics.RaycastAll (transform.position, transform.up * -1);
		//raycastHit.distance = 1001;
		for (int i = 0; i < allHits.Length; i++) {
			if (allHits [i].distance < minDistance) {
				minDistance = allHits [i].distance;
				raycastHit = allHits [i];
			}
		}
		if (!(raycastHit.distance == 1001) && raycastHit.transform.gameObject) {
			lasthitDirection = currentDirection;
			currentDirection = raycastHit.normal;
			if (!(lasthitDirection.Equals (currentDirection))) {
				attachedPlayer.rotatePlayer = false;
			} else {
				attachedPlayer.rotatePlayer = true;
			}
			playerRigidbody.MovePosition (transform.position - (currentDirection * Time.deltaTime * gravity));
		}
		if (lasthitDirection != currentDirection) {
			resetPointInRotation = true;
			lastrotation = transform.localRotation;
			//Debug.Log (raycastHitDirection);
			//Debug.Log (raycastHit.distance);
			Debug.DrawLine (raycastHit.point, raycastHit.point + raycastHit.normal, Color.green, 2, false);
//			lasthit = raycastHit;
		}
	}
//	void OnCollisionEnter(Collision collision) {
//	
//	}
}
