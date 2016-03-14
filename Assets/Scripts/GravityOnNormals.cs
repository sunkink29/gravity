using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GravityOnNormals : MonoBehaviour {

	Rigidbody playerRigidbody;
//	RaycastHit lasthit;
	public Vector3 currentDirection;
	Vector3 lasthitDirection;
	public float gravity = 5f;
	public float angleSpeed = 10;
	bool disableXRotation = false;
	Vector3 lastRotation;
	[SerializeField] bool disableAutoRotate = false;
	RaycastHit raycastHit;
	Vector3 pickupDirection;
	Vector3 pickupDownDirection;
	FirstPersonScript attachedPlayer;

	// Use this for initialization
	void Start () {
		playerRigidbody = GetComponent<Rigidbody>();
		currentDirection = Vector3.up;
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
		Quaternion newQuaternion = Quaternion.FromToRotation ( transform.up,currentDirection) * transform.rotation;
		if (disableXRotation)
			newQuaternion.eulerAngles = new Vector3(lastRotation.x,lastRotation.y,newQuaternion.eulerAngles.z);
		transform.rotation = Quaternion.Slerp (transform.rotation, newQuaternion, angleSpeed * Time.deltaTime);
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
