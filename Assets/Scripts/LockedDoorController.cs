using UnityEngine;
using System.Collections;

public class LockedDoorController : DoorController {


	public GameObject[] references;
	LockedDoorPart[] doorLocks;
	public bool printLockStates = false;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		doorLocks = new LockedDoorPart[references.Length];
		for (int i = 0; i < references.Length; i++) {
			if (references [i] != null) {
				doorLocks = GetComponentsInChildren<LockedDoorPart> ();
				doorLocks [i].UseLockPart ();
				doorLocks [i].reference = references [i].GetComponent<PowerProvider> ();
				doorLocks [i].reference.sendReference (this);
			}
		}
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
		string lockStates = "";
		int locksPowered = 0;
		for (int i = 0;i < doorLocks.Length;i++){
			if (printLockStates) {
				lockStates += doorLocks [i].isPowered + " | ";
			}
		}
		if (printLockStates) {
			Debug.Log (lockStates + locksPowered);
		}
	}

	public override void OnTriggerEnter(Collider collider) {
		if (checkIfUnlocked()) {
			base.OnTriggerEnter (collider);
		}
	}
	public override void OnTriggerExit(Collider collider) {
		if (checkIfUnlocked()) {
			base.OnTriggerExit (collider);
		}
	}

	public override void powerOn(PowerProvider reference) {
		for (int i = 0; i < doorLocks.Length; i++) {
			if (doorLocks [i].reference == reference) {
				changeLockPowerState (i, true);
				break;
			}
		}
	}

	public override void powerOff(PowerProvider reference) {
		for (int i = 0; i < doorLocks.Length; i++) {
			if (doorLocks [i].reference == reference) {
				changeLockPowerState (i, false);
				break;
			}
		}
	}

	public void changeLockPowerState(int index, bool state) {
		if (state) {
			doorLocks [index].turnOn ();
		} else {
			doorLocks [index].turnOff ();
		}
	}

	bool checkIfUnlocked() {
		int locksPowered = 0;
		bool unlocked = false;
		for (int i = 0; i < doorLocks.Length; i++) {
			if (doorLocks [i].isPowered) {
				locksPowered++;
			}
		}
		if (locksPowered == doorLocks.Length	) {
			unlocked = true;
		}
		return unlocked;
	}
}
