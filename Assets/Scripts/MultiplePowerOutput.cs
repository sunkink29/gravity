using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MultiplePowerOutput : MonoBehaviour , Powerable, PowerProvider {

	List<Powerable> powerables = new List<Powerable>();
	public MonoBehaviour powerProviderMonoBehaviour;
	PowerProvider powerProvider;

	// Use this for initialization
	void Start () {
		powerProvider = (PowerProvider) powerProviderMonoBehaviour;
		powerProvider.sendReference (this);
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

	public GameObject getGameObject() {
		return gameObject;
	}
}
