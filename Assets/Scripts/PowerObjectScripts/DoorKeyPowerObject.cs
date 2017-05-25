using UnityEngine;
using System.Collections;

public class DoorKeyPowerObject : PowerProviderPowerObject, FindPropertys {

    public PowerProviderPowerObject[] references;
    LockedDoorPartPowerObject[] doorLocks;
    int[] doorLocksRefsIDs;
    LockedDoorController door;
    public bool unlocked = false;

    string[] propertys = {"lockPowerState", "unlocked", "power"};
    
    public override void Start()
    {
        doorLocks = new LockedDoorPartPowerObject[references.Length];
        doorLocksRefsIDs = new int[references.Length];
        doorLocks = GetComponentsInChildren<LockedDoorPartPowerObject>();
        for (int i = 0; i < references.Length; i++)
        {
            if (references[i] != null)
            {
                doorLocks[i].UseLockPart();
                doorLocks[i].reference = references[i];
                PowerProviderPowerObject reference = references[i];
                if (reference != null)
                {
                    doorLocksRefsIDs[i] = reference.GetInstanceID();
                }
                else
                {
                    doorLocksRefsIDs[i] = -1;
                }
                doorLocks[i].reference.sendReference(this);
            }
        }
    }

    public override void changePower(float[] powerArgs)
    {
        if (powerArgs.Length >= 2 && powerArgs[1] != -1)
        {
            int numOn = 0;
            bool powered;
            if (powerArgs[1] >= 1)
            {
                powered = true;
            }
            else
            {
                powered = false;
            }
            for (int i = 0; i < doorLocks.Length; i++)
            {
                if (powerArgs.Length >= 2 && doorLocksRefsIDs[i] == powerArgs[0])
                {
                    changeLockPowerState(i, powered);
                }
                if (doorLocks[i].isPowered)
                {
                    numOn++;
                }
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
        if (door != null)
        {
            door.changeLockPowerState(index, state);
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
        if (locksPowered == doorLocks.Length && !unlocked)
        {
            unlocked = true;
            base.changePower(new float[] { GetInstanceID(), 1 });
        }
        else if (locksPowered != doorLocks.Length && unlocked)
        {
            unlocked = false;
            base.changePower(new float[] { GetInstanceID(), 0 });
        }
    }

    public void sendReference(LockedDoorController reference)
    {
        door = reference;
    }

    public int findPropertyIndex(string property) {
        for (int i = 0; i < propertys.Length; i++) {
            if (propertys[i].Equals(property)) {
                return i;
            }
        }
        return -1;
    }

	public bool hasProperty(string property) {
        int propertyIndex = findPropertyIndex(property);
        if (propertyIndex == -1) {
            return false;
        }
        return true;
    }

	public void changeProperty(string property, string[] propertyValue) {
        int propertyIndex = findPropertyIndex(property);
        switch (propertyIndex) {
            case 0:
                // changeLockPowerState(int.Parse(propertyValue[0],bool.Parse(propertyValue[1])));
                break;
			case 1:
				for(int i = 0; i < doorLocks.Length; i++) {
                    changeLockPowerState(i,bool.Parse(propertyValue[0]));
                }
				break;
			case 2:
				float[] propertyValueFloat = ConsoleCommandRouter.convertStringArrayToFloat(propertyValue);
				changePower(propertyValueFloat);
				break;
        }
	}

	public string getName() {
        return name;
    }

}
