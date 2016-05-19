using UnityEngine;
using System.Collections;

public class WireButtonController : MonoBehaviour , Powerable, PowerProvider {

	Collider currentCollider;
	[SerializeField] bool isPowered;
	public GameObject connectedObject;
	public GameObject powerProviderObject;
	Powerable connectedObjectScript;
	PowerProvider powerProvider;
	FadeEmission fadeEmission;
	Coroutine coroutine;

	// Use this for initialization
	void Start () {
		fadeEmission = GetComponent<FadeEmission> ();
		if (fadeEmission == null) {
			fadeEmission = gameObject.AddComponent<FadeEmission> ();
		}
		if (Application.isEditor) {
			StartCoroutine (checkIfPowered ());
		}
//		connectedObjectScript = connectedObject.GetComponent<Powerable> ();
		powerProvider = powerProviderObject.GetComponent<PowerProvider> ();
		powerProvider.sendReference (this);
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.GetComponent<CubeController> () != null) {
			powerOn ();
		}
	}

	void OnTriggerExit (Collider collider) {
		if (collider.gameObject.GetComponent<CubeController> () != null) {
			powerOff ();
		}
	}

	IEnumerator checkIfPowered () {
		while (Application.isPlaying) {
			if (fadeEmission.emission == 0 && isPowered) {
				powerOn ();
			} else if (fadeEmission.emission == 1 && !isPowered) {
				powerOff ();
			}
			yield return new WaitForSeconds (1);
		}
	}

    IEnumerator waitForFullEmission() {
		while (!fadeEmission.atMaxEmission) {
			yield return null;
		}
		if (connectedObjectScript != null) {
			connectedObjectScript.powerOn (this);
		}
    }

	public void powerOn () {
		powerOn (null);
	}

	public void powerOn (PowerProvider reference){
		if (!isPowered) {
			isPowered = true;
			fadeEmission.turnOn ();
			coroutine = StartCoroutine(waitForFullEmission());
		}
	}

	public void powerOff () {
		powerOff (null);
	}

	public void powerOff (PowerProvider reference){
		if (isPowered) {
			StopCoroutine (coroutine);
			isPowered = false;
			fadeEmission.turnOff ();
			if (connectedObjectScript != null) {
				connectedObjectScript.powerOff (this);
			}
		}
	}

	public void sendReference(Powerable reference) {
		connectedObjectScript = reference;
	}

}
