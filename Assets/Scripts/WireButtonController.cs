using UnityEngine;
using System.Collections;

public class WireButtonController : MonoBehaviour , Powerable, PowerProvider, Debugable {

	Collider currentCollider;
	[SerializeField] bool isPowered;
	public GameObject powerProviderObject;
	public Renderer objectRenderer;
	public Transform startPoint;
	public Transform endPoint;
	public float speed = .1f;
	Color baseColor;
	Material mat;
	float currentPoint;
	Powerable connectedObject;
	PowerProvider powerProvider;
	FadeEmission fadeEmission;
	Coroutine coroutine;
	float[] currentPowerArgs;


	// Use this for initialization
	void Start () {
//		fadeEmission = GetComponent<FadeEmission> ();
//		if (fadeEmission == null) {
//			fadeEmission = gameObject.AddComponent<FadeEmission> ();
//		}
		if (LerpCoroutine.currentInstance == null)
		{
			LerpCoroutine lerpCoroutine = gameObject.AddComponent<LerpCoroutine>();
			lerpCoroutine.Awake();
		}
//		if (Application.isEditor) {
//			StartCoroutine (checkIfPowered ());
//		}
		powerProvider = powerProviderObject.GetComponent<PowerProvider> ();
		powerProvider.sendReference (this);
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
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.GetComponent<CubeController> () != null) {
			powerOn ();
		}
	}

	void OnTriggerExit (Collider collider) {
		if (collider.gameObject.GetComponent<CubeController> () != null) {
			powerOff ();
		}
	}

	IEnumerator checkIfPowered () {
		while (Application.isPlaying) {
			if (fadeEmission.emission == 0 && isPowered) {
				powerOn ();
			} else if (fadeEmission.emission == 1 && !isPowered) {
				powerOff ();
			}
			yield return new WaitForSeconds (1);
		}
	}

    IEnumerator waitForFullEmission() {
		while (!fadeEmission.atMaxEmission) {
			yield return null;
		}
		if (connectedObject != null) {
			connectedObject.changePower (new float[]{ this.GetInstanceID (), 1 });
		}
    }

	public void powerOn () {
		powerOn (null);
	}

	public void powerOn (PowerProvider reference){
		if (!isPowered) {
			isPowered = true;
			LerpCoroutine.LerpMinToMax(Vector3.Distance(startPoint.position,endPoint.position)*speed,0,
				Vector3.Distance(startPoint.position,endPoint.position),currentPoint,changeWireDistance,false);
			//fadeEmission.turnOn ();
			//coroutine = StartCoroutine(waitForFullEmission());
		}
	}

	public void powerOff () {
		powerOff (null);
	}

	public void powerOff (PowerProvider reference){
		if (isPowered) {
			StopCoroutine (coroutine);
			isPowered = false;
			changeWireDistance (0);
//			fadeEmission.turnOff ();
//			if (connectedObject != null) {
//				connectedObject.powerOff (this);
//			}
		}
	}

	public void changePower(float[] powerArgs) {
		currentPowerArgs = powerArgs;
		if (powerArgs.Length >= 2 && powerArgs [1] >= 1) {
			if (isPowered == false) {
				coroutine = LerpCoroutine.LerpMinToMax (Vector3.Distance (startPoint.position, endPoint.position) * speed, 0,
					Vector3.Distance (startPoint.position, endPoint.position), currentPoint, changeWireDistance, false);
			}
			isPowered = true;
//			fadeEmission.turnOn ();
//			coroutine = StartCoroutine(waitForFullEmission());
		} else {
			isPowered = false;
			changeWireDistance (0);
			if (coroutine != null) {
				StopCoroutine (coroutine);
			}
//			fadeEmission.turnOff ();
			if (connectedObject != null) {
				powerArgs [0] = this.GetInstanceID ();
				connectedObject.changePower(powerArgs);
			}
		}
	}

	public void changeWireDistance (float distance) {
		mat.SetFloat ("_Distance", distance);
		DynamicGI.UpdateMaterials (objectRenderer);
		currentPoint = distance;
		if (distance / Vector3.Distance (startPoint.position, endPoint.position) == 1) {
			currentPowerArgs [0] = GetInstanceID ();
			connectedObject.changePower (currentPowerArgs);
		}
	}

	public void sendReference(Powerable reference) {
		connectedObject = reference;
	}

    public void debug() {
        if (isPowered)
        {
            powerOff();
        } else
        {
            powerOn();
        }
    }

}
