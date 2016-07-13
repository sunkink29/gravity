using UnityEngine;
using System.Collections;

public class LockedDoorPart : MonoBehaviour, Debugable {

	public PowerProvider reference;
	bool powered;
	public bool isPowered;
	FadeEmission fadeEmission;
	Coroutine coroutine;
    LockedDoorController doorController;

	void Awake() {
		coroutine = StartCoroutine (WaitForDoor ());
	}

	void Start() {
		fadeEmission = GetComponent<FadeEmission> ();
		if (fadeEmission == null) {
			fadeEmission = gameObject.AddComponent<FadeEmission> ();
		}
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

	public void UseLockPart(LockedDoorController door){
        doorController = door;
		StopCoroutine (coroutine);
		if (Application.isEditor) {
			StartCoroutine (CheckForVariableChange ());
		}
	}

    public void debug()
    {
        doorController.toggleDoorLockState();
    }
}
