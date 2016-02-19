using UnityEngine;
using System.Collections;

public class WireButtonController : MonoBehaviour {

	Collider currentCollider;
	Renderer objectRenderer;
	Material mat;
	public Color baseColor = Color.yellow;
	public float duration = 1;
	float startEmission;
	float endEmission;
	float Emission = 0;
	public float maxEmission = 1;
	// Use this for initialization
	void Start () {
		objectRenderer = GetComponent<Renderer> ();
		mat = objectRenderer.material;
		mat.SetColor ("_EmissionColor", baseColor * Mathf.LinearToGammaSpace (0));
		DynamicGI.UpdateMaterials (objectRenderer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider collider) {
		Debug.Log ("object entered trigger");
		StopCoroutine (changeValue ());
		startEmission = Emission;
		endEmission = maxEmission;
		StartCoroutine (changeValue ());
	}

	void OnTriggerExit (Collider collider) {
		Debug.Log ("object exited trigger");
		StopCoroutine (changeValue ());
		startEmission = Emission;
		endEmission = 0;
		StartCoroutine (changeValue ());
	}

	IEnumerator changeValue() {
		float pointInTime = 0;
		while (pointInTime <= duration) {
			Debug.Log ("updating wire");
			Emission = Mathf.Lerp (startEmission, endEmission, pointInTime / duration);
			Color finalColor = baseColor * Mathf.LinearToGammaSpace (Emission);
			mat.SetColor ("_EmissionColor", finalColor);
			DynamicGI.UpdateMaterials (objectRenderer);
			pointInTime += Time.deltaTime;
			yield return new WaitForSeconds (0.001f);
		}
		mat.SetColor ("_EmissionColor", baseColor * Mathf.LinearToGammaSpace (endEmission));
		DynamicGI.UpdateMaterials (objectRenderer);
	}
}
