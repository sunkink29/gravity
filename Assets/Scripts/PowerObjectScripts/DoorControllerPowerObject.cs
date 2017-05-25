using UnityEngine;
using System.Collections;

public class DoorControllerPowerObject : PowerObject, FindPropertys {

	Animator animator;
	bool open;
	[SerializeField] bool doorOpened = false;
	string[] propertys = {"power", "lockState", "doorState"};

	// Use this for initialization
	public override void Start () {
		animator = GetComponent<Animator> ();
		animator.SetBool ("DoorUnlocked",true);
		base.Start();
        if (powerProvider != null) {
            animator.SetBool ("DoorUnlocked",false);
        }
	}

	public virtual void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.GetComponent<CubeController> () == null) {
			animateDoor (true);
		} else {
			animator.SetBool ("DoorAcessDenied", true);
		}
	}

	public virtual void OnTriggerExit (Collider collider) {
		if (collider.gameObject.GetComponent<CubeController> () == null) {
			animateDoor (false);
		} else {
			animator.SetBool ("DoorAcessDenied", false);
		}
	}

	public override void changePower(float[] powerArgs) {
		bool doorUnlocked;
		if (powerArgs.Length >= 2 && powerArgs [1] >= 1) {
			doorUnlocked = true;
		} else {
			doorUnlocked = false;
		}
		animator.SetBool ("DoorUnlocked", doorUnlocked);
	}

	void animateDoor (bool state) {
		animator.SetBool ("DoorOpen", state);
		animator.SetBool ("DoorClosed", !state);
		doorOpened = state;
		open = state;
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
				float[] propertyValuefloat = ConsoleCommandRouter.convertStringArrayToFloat(propertyValue);
                changePower(propertyValuefloat);
                break;
			case 1:
				switch (propertyValue[0]) {
					case "unlock":
						animator.SetBool ("DoorUnlocked",true);
						break;
					case "lock":
						animator.SetBool ("DoorUnlocked",false);
						break;
				}
				break;
			case 2:
				switch (propertyValue[0]) {
					case "open":
						animateDoor(true);
						break;
					case "close":
						animateDoor(false);
						break;
				}
				break;	
        }
	}

	public string getName() {
        return name;
    }
}
