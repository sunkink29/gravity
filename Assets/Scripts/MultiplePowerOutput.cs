using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MultiplePowerOutput : MonoBehaviour , Powerable, PowerProvider {

	List<Powerable> powerables = new List<Powerable>();
	public GameObject powerProviderGameObject;
	PowerProvider powerProvider;

	// Use this for initialization
	void Start () {
		powerProvider = powerProviderGameObject.GetComponent<PowerProvider> ();
		powerProvider.sendReference (this);
	}

	public void powerOn() {
		powerOn (null);
	}

	public void powerOn(PowerProvider provider) {
		changePower(new float[]{GetInstanceID(),1});
	}

	public void powerOff() {
		powerOff (null);
	}

	public void powerOff(PowerProvider provider) {
		changePower(new float[]{GetInstanceID(),1});
	}

	public void changePower(float[] powerArgs) {
		powerArgs [0] = GetInstanceID ();
		for (int i = 0; i < powerables.Count; i++) {
			powerables [i].changePower (powerArgs);
		}
	}

	public void sendReference(Powerable reference) {
		powerables.Add (reference);
	}
}
