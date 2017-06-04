using UnityEngine;
using System.Collections;

public class CubeButtonPowerObject : PowerProviderPowerObject, FindPropertys {

	string[] propertys = {"power"};

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.GetComponent<LiftablePowerObject> () != null) {
			base.changePower (new float[] { GetInstanceID (), 1 });
		}
	}

	void OnTriggerExit (Collider collider) {
		if (collider.gameObject.GetComponent<LiftablePowerObject> () != null) {
			base.changePower (new float[] { GetInstanceID (), 0 });
		}
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
				float[] propertyValueFloat = ConsoleCommandRouter.convertStringArrayToFloat(propertyValue);
				changePower(propertyValueFloat);
				break;
        }
	}

	public string getName() {
        return name;
    }
}
