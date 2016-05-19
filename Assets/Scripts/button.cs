using UnityEngine;
using System.Collections;

public class button : MonoBehaviour , Interactible, PowerProvider {

	Powerable connectedObject;
	bool supplyPower = false;
	public bool toggleButton = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void interact () {
		supplyPower = !supplyPower;
		if (supplyPower) {
			connectedObject.powerOn (this);
		} else if (toggleButton) {
			connectedObject.powerOff (this);
		}
	}

	public void sendReference(Powerable reference) {
		connectedObject = reference;
	}
}
