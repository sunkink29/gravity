using UnityEngine;
using System.Collections;

public class DoorKey : MonoBehaviour, Powerable, PowerProvider {

    public GameObject[] references;
    LockedDoorPart[] doorLocks;
    LockedDoorController door;
	Powerable powerable;
    public bool unlocked = false;
    public RoomLightsController roomLights;
    public GameObject roomLightGameObject;
    void Start () {
        doorLocks = new LockedDoorPart[references.Length];
        for (int i = 0; i < references.Length; i++)
        {
            if (references[i] != null)
            {
                doorLocks = GetComponentsInChildren<LockedDoorPart>();
                doorLocks[i].UseLockPart(door);
                doorLocks[i].reference = references[i].GetComponent<PowerProvider>();
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

    public void powerOn () {
        powerOn(null);
    }

    public void powerOn(PowerProvider reference) {
        if (reference != null)
        {
			float numOn = 0;
            for (int i = 0; i < doorLocks.Length; i++)
            {
                if (doorLocks[i].reference == reference)
                {
                    changeLockPowerState(i, true);
                }
				if (doorLocks [i].isPowered) {
					numOn++;
				}
            }
			if (roomLights != null) {
				roomLights.powerOn (numOn / doorLocks.Length);
			}        
		}
    } 

    public void powerOff () {

    }

    public void powerOff(PowerProvider reference) {
		if (reference != null) {
			int numOn = 0;
			for (int i = 0; i < doorLocks.Length; i++) {
				if (doorLocks [i].reference == reference) {
					changeLockPowerState (i, false);
				}
				if (doorLocks [i].isPowered) {
					numOn++;
				}
			}
			if (roomLights != null) {
				roomLights.powerOff (numOn / doorLocks.Length);
			}
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
				powerable.powerOn ();
			}
		} else if (locksPowered != doorLocks.Length && unlocked){
			unlocked = false;
			if (powerable != null) {
				powerable.powerOff ();
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
}
