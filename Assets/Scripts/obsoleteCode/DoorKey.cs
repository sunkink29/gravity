﻿using UnityEngine;
using System.Collections;

public class DoorKey : MonoBehaviour, Powerable, PowerProvider {

	public MonoBehaviour[] references;
    LockedDoorPart[] doorLocks;
	int[] doorLocksRefsIDs;
    LockedDoorController door;
	Powerable powerable;
    public bool unlocked = false;
    public RoomLightsController roomLights;
    public GameObject roomLightGameObject;
    void Start () {
        doorLocks = new LockedDoorPart[references.Length];
		doorLocksRefsIDs = new int[references.Length];
        for (int i = 0; i < references.Length; i++)
        {
            if (references[i] != null)
            {
                doorLocks = GetComponentsInChildren<LockedDoorPart>();
                doorLocks[i].UseLockPart(door);
				doorLocks[i].reference = (PowerProvider) references[i];
				MonoBehaviour reference = references[i];
				if (reference != null) {
					doorLocksRefsIDs [i] = reference.GetInstanceID ();
				} else {
					doorLocksRefsIDs [i] = -1;
				}
                doorLocks[i].reference.sendReference(this);
            }
        }
        if (roomLightGameObject != null)
        {
        roomLights = roomLightGameObject.GetComponent<RoomLightsController>();

        }
    }
	
	// Update is called once per frame
//	void Update () {
//        if (roomLights != null)
//        {
//            if (unlocked && !roomLights.powered)
//            {
//                roomLights.powerOn();
//            } else if (!unlocked && roomLights.powered)
//            {
//                roomLights.powerOff();
//            }
//        }
//	}

	public void changePower(float[] powerArgs) {
		if (powerArgs.Length >= 2 && powerArgs [1] != -1) {
			int numOn = 0;
			bool powered;
			if (powerArgs [1] >= 1) {
				powered = true;
			} else {
				powered = false;
			}
			for (int i = 0; i < doorLocks.Length; i++) {
				if (powerArgs.Length >= 2 && doorLocksRefsIDs[i] == powerArgs[0]) {
					changeLockPowerState (i, powered);
				}
				if (doorLocks [i].isPowered) {
					numOn++;
				}
			}
			if (roomLights != null) {
				roomLights.changePower (new float[]{ this.GetInstanceID (), (float)numOn / doorLocks.Length });
			}
//			if (roomLights != null) {
//				roomLights.powerOff (numOn / doorLocks.Length);
//			}
		}
	}

    public void changeLockPowerState(int index, bool state)
    {
        if (state)
        {
            doorLocks[index].turnOn();
        }
        else
        {
            doorLocks[index].turnOff();
        }
        checkIfUnlocked();
		if (door != null) {
			door.changeLockPowerState (index, state);
		}
    }

    void checkIfUnlocked()
    {
        int locksPowered = 0;
        for (int i = 0; i < doorLocks.Length; i++)
        {
            if (doorLocks[i].isPowered)
            {
                locksPowered++;
            }
        }
		if (locksPowered == doorLocks.Length && !unlocked) {
			unlocked = true;
			if (powerable != null) {
				powerable.changePower (new float[]{ GetInstanceID (), 1 });
			}
		} else if (locksPowered != doorLocks.Length && unlocked){
			unlocked = false;
			if (powerable != null) {
				powerable.changePower (new float[]{ GetInstanceID (), 0 });
			}
		}
    }

	public void sendReference(LockedDoorController reference) {
        door = reference;
        //print(reference);
    }

	public void sendReference(Powerable reference) {
		powerable = reference;
		//print(reference);
	}

	public GameObject getGameObject (){
		return gameObject;
	}
}
