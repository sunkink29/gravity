using UnityEngine;
using System.Collections;

public class WireButtonController : MonoBehaviour , Powerable {

	Collider currentCollider;
	Renderer objectRenderer;
	Material mat;
	public Color baseColor = Color.yellow;
	public float duration = 1;
	float startEmission;
	float endEmission;
	float emission = 0;
	public float maxEmission = 1;
	[SerializeField] bool isPowered;
	public GameObject connectedObject;
	Powerable connectedObjectScript;

	// Use this for initialization
	void Start () {
		objectRenderer = GetComponent<Renderer> ();
		mat = objectRenderer.material;
		mat.SetColor ("_EmissionColor", baseColor * Mathf.LinearToGammaSpace (0));
		DynamicGI.UpdateMaterials (objectRenderer);
		if (Application.isEditor) {
			StartCoroutine (checkIfPowered ());
		}
		connectedObjectScript = connectedObject.GetComponent<Powerable> ();
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

	IEnumerator changeValue () {
		float pointInTime = 0;
		while (pointInTime <= (duration - emission * duration)) {
			emission = Mathf.Lerp (startEmission, endEmission, pointInTime / duration);
			Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);
			mat.SetColor ("_EmissionColor", finalColor);
			DynamicGI.UpdateMaterials (objectRenderer);
			pointInTime += Time.deltaTime;
			yield return new WaitForSeconds (0.001f);
		}
		emission = endEmission;
		mat.SetColor ("_EmissionColor", baseColor * Mathf.LinearToGammaSpace (endEmission));
		DynamicGI.UpdateMaterials (objectRenderer);
	}

	IEnumerator checkIfPowered () {
		while (Application.isPlaying) {
			if (emission == 0 && isPowered) {
				powerOn ();
			} else if (emission == 1 && !isPowered) {
				powerOff ();
			}
			yield return new WaitForSeconds (1);
		}
	}

	public void powerOn (){
		if (!isPowered) {
			isPowered = true;
			StopCoroutine (changeValue ());
			startEmission = emission;
			endEmission = maxEmission;
			StartCoroutine (changeValue ());
			if (connectedObjectScript != null) {
				connectedObjectScript.powerOn ();
			}
		}
	}

	public void powerOff (){
		if (isPowered) {
			isPowered = false;
			StopCoroutine (changeValue ());
			startEmission = emission;
			endEmission = 0;
			StartCoroutine (changeValue ());
			if (connectedObjectScript != null) {
				connectedObjectScript.powerOff ();
			}
		}
	}

}
