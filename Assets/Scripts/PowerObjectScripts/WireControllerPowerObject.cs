using UnityEngine;
using System.Collections;

public class WireControllerPowerObject : PowerProviderPowerObject, FindPropertys {

	Collider currentCollider;
	[SerializeField] bool isPowered;
	Renderer objectRenderer;
	public Transform startPoint;
	public Transform endPoint;
	public float speed = .1f;
	Color baseColor;
	Material mat;
	float currentPoint;
	Coroutine coroutine;
	float[] currentPowerArgs;

	string[] propertys = {"speed", "wireDistance", "power"};


	// Use this for initialization
	public override void Start () {
		if (LerpCoroutine.currentInstance == null) {
			LerpCoroutine lerpCoroutine = gameObject.AddComponent<LerpCoroutine>();
			lerpCoroutine.Awake();
		}
		objectRenderer = GetComponent<Renderer>();
		Material[] materials = objectRenderer.materials;
		if (materials.Length >= 2) {
			mat = materials [1];
		} else {
			mat = objectRenderer.material;
		}
		baseColor = mat.GetColor ("_EmissionColor");
		mat.SetVector ("_WireStart", startPoint.position);
		mat.SetFloat ("_Distance", 0);
		DynamicGI.UpdateMaterials (objectRenderer);
		base.Start();
	}

	public override void changePower(float[] powerArgs) {
		currentPowerArgs = powerArgs;
		if (powerArgs.Length >= 2 && powerArgs [1] >= 1) {
			if (isPowered == false) {
				coroutine = LerpCoroutine.LerpMinToMax (Vector3.Distance (startPoint.position, endPoint.position) * speed, 0,
					Vector3.Distance (startPoint.position, endPoint.position), currentPoint, changeWireDistance, false);
			}
			isPowered = true;
		} else {
			isPowered = false;
			changeWireDistance(0);
			powerArgs = new float[] {GetInstanceID(), 0};
			base.changePower(powerArgs);
		}
	}

	public void changeWireDistance (float distance) {
		mat.SetFloat ("_Distance", distance);
		DynamicGI.UpdateMaterials (objectRenderer);
		currentPoint = distance;
		if (distance / Vector3.Distance (startPoint.position, endPoint.position) == 1) {
			base.changePower(currentPowerArgs);
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
                speed = float.Parse(propertyValue[0]);
                break;
			case 1:
				changeWireDistance(float.Parse(propertyValue[0]));
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