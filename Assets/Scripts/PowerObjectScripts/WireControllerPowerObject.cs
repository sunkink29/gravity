using UnityEngine;
using System.Collections;

public class WireControllerPowerObject : PowerProviderPowerObject, FindPropertys {

	Collider currentCollider;
	[SerializeField] bool isPowered;
	Renderer objectRenderer;
	public float speed = .1f;
    float falloutDistance;
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
        falloutDistance = mat.GetFloat("_FalloutDistance");
		mat.SetFloat ("_Distance", 0 - falloutDistance);
		DynamicGI.UpdateMaterials (objectRenderer);
		base.Start();
	}

	public override void changePower(float[] powerArgs) {
		currentPowerArgs = powerArgs;
		if (powerArgs.Length >= 2 && powerArgs [1] >= 1) {
			if (isPowered == false) {
				coroutine = LerpCoroutine.LerpMinToMax (transform.lossyScale.x * speed * (1 + falloutDistance),
                    0 - falloutDistance, 1, currentPoint, changeWireDistance, false);
			}
			isPowered = true;
		} else {
            if (coroutine != null)
                LerpCoroutine.stopCoroutine(coroutine);
			isPowered = false;
			changeWireDistance(0 - falloutDistance);
			powerArgs = new float[] {GetInstanceID(), 0};
			base.changePower(powerArgs);
		}
	}

	public void changeWireDistance (float distance) {
		mat.SetFloat ("_Distance", distance);
		DynamicGI.UpdateMaterials (objectRenderer);
		currentPoint = distance;
		if (distance == 1) {
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