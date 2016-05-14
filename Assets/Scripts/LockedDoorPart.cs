using UnityEngine;
using System.Collections;

public class LockedDoorPart : MonoBehaviour {

	public PowerProvider reference;
	bool powered;
	public bool isPowered;
	FadeEmission fadeEmission;
	Coroutine coroutine;

	void Start() {
		fadeEmission = GetComponent<FadeEmission> ();
		if (fadeEmission == null) {
			fadeEmission = gameObject.AddComponent<FadeEmission> ();
		}
		coroutine = StartCoroutine (WaitForDoor ());
	}

	void Update() {
		if (isPowered != powered) {
			if (isPowered) {
				turnOn ();
			} else {
				turnOff ();
			}
		}
	}

	public void turnOn () {
		isPowered = true;
		powered = true;
		fadeEmission.turnOn ();
	}

	public void turnOff () {
		isPowered = false;
		powered = false;
		fadeEmission.turnOff ();
	}

	IEnumerator WaitForDoor () {
		yield return null;
		yield return null;
		turnOn ();
	}

	IEnumerator CheckForVariableChange() {
		while (Application.isPlaying) {
			if (isPowered != powered) {
				if (isPowered) {
					turnOn ();
				} else {
					turnOff ();
				}
			}
			yield return new WaitForSeconds (0.5f);
		}
	}

	public void UseLockPart(){
		StopCoroutine (coroutine);
		if (Application.isEditor) {
			StartCoroutine (CheckForVariableChange ());
		}
	}
}
