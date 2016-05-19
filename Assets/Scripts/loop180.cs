using UnityEngine;
using System.Collections;

public class loop180 : MonoBehaviour {

	GravityOnNormals gravity;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider){
		gravity = collider.gameObject.GetComponent<GravityOnNormals> ();
		gravity.toggleXRotation (true);
	}

	void OnTriggerExit(Collider collider){
		gravity.toggleXRotation (false);
	}
}
