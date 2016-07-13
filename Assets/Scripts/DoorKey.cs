using UnityEngine;
using System.Collections;

public class DoorKey : MonoBehaviour, Powerable {

    public GameObject[] references;
    LockedDoorPart[] doorLocks;
    LockedDoorController door;
    public bool unlocked = false;
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
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void powerOn () {
        powerOn(null);
    }

    public void powerOn(PowerProvider reference) {
        if (reference != null)
        {
            for (int i = 0; i < doorLocks.Length; i++)
            {
                if (doorLocks[i].reference == reference)
                {
                    changeLockPowerState(i, true);
                    break;
                }
            }
        }
    } 

    public void powerOff () {

    }

    public void powerOff(PowerProvider reference) {
        for (int i = 0; i < doorLocks.Length; i++)
        {
            if (doorLocks[i].reference == reference)
            {
                changeLockPowerState(i, false);
                break;
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
        door.changeLockPowerState(index, state);
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
        unlocked = false;
        if (locksPowered == doorLocks.Length)
        {
            unlocked = true;
        }
    }

    public void sendReference(LockedDoorController reference) {
        door = reference;
        print(reference);
    }
}
