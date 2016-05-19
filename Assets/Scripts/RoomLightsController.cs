using UnityEngine;
using System.Collections;

public class RoomLightsController : MonoBehaviour, Powerable, PowerProvider {

	FadeEmission fadeEmission;
	Powerable ConnectedObject;
	public GameObject powerProviderObject;
	PowerProvider powerProvider;


	// Use this for initialization
	void Start () {
		fadeEmission = GetComponent<FadeEmission> ();
		if (fadeEmission == null) {
			fadeEmission = gameObject.AddComponent<FadeEmission> ();
		}
		powerProvider = powerProviderObject.GetComponent<PowerProvider> ();
		powerProvider.sendReference (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void powerOn () {
		powerOn (null);
	}

	public void powerOn (PowerProvider reference) {
		fadeEmission.turnOn ();
		ConnectedObject.powerOn (this);
	}

	public void powerOff () {
		powerOff (null);
	}

	public void powerOff (PowerProvider reference) {
		fadeEmission.turnOff ();
		ConnectedObject.powerOff (this);
	}

	public void sendReference (Powerable reference) {
		ConnectedObject = reference;
	}
}
