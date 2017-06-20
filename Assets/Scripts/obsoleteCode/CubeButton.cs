using UnityEngine;
using System.Collections;

public class CubeButton : MonoBehaviour, PowerProvider {

	Powerable connectedObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void sendReference(Powerable reference) {
		connectedObject = reference;
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.GetComponent<CubeController> () != null) {
			connectedObject.changePower (new float[2] { GetInstanceID (), 1 });
		}
	}

	void OnTriggerExit (Collider collider) {
		if (collider.gameObject.GetComponent<CubeController> () != null) {
			connectedObject.changePower (new float[2] { GetInstanceID (), 0 });
		}
	}
}
