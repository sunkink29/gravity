using UnityEngine;
using System.Collections;

public class FadeEmission : MonoBehaviour {

	Renderer objectRenderer;
	Material mat;
	Material[] materials;
	public string materialName;
	Color baseColor;
	public float duration = 1;
	float startEmission;
	float endEmission;
	[HideInInspector] public float emission = 0;
	public float maxEmission = 1;
	public Transform wireStart;
	public Transform wireEnd;
	Coroutine coroutine;
	public bool printMaterialNames;
	bool usingWireShader;
	[HideInInspector] public bool atMaxEmission;

	// Use this for initialization
	void Start () {
		objectRenderer = GetComponent<Renderer> ();
		materials = objectRenderer.materials;
		for (int i = 0; i < materials.Length; i++) {
			if (printMaterialNames) {
				Debug.Log (materials [i].name);
			}
			if (materials [i].name == materialName) {
				
				mat = materials [i];
				break;
			}
		}
		if (mat == null) {
			mat = objectRenderer.material;
		}
		baseColor = mat.GetColor ("_EmissionColor");
		if (mat.shader.name == "Custom/WireShader") {
			usingWireShader = true;
			mat.SetVector ("_WireStart", wireStart.position);
			mat.SetFloat ("_Distance", 0);
		} else {
			mat.SetColor ("_EmissionColor", baseColor * Mathf.LinearToGammaSpace (0));
		}
		DynamicGI.UpdateMaterials (objectRenderer);
	}

	IEnumerator changeValue () {
		float pointInTime = 0;
		while (pointInTime <= duration) {
			emission = Mathf.Lerp (startEmission, endEmission, pointInTime / duration);
			if (usingWireShader) {
				mat.SetFloat ("_Distance", emission);
                mat.SetColor("_EmissionColor", baseColor * Mathf.LinearToGammaSpace(maxEmission));
            }
            else {
				Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);
				mat.SetColor ("_EmissionColor", finalColor);
			}
			DynamicGI.UpdateMaterials (objectRenderer);
			pointInTime += 0.01f;
			yield return new WaitForSeconds (0.01f);
		}
		emission = endEmission;
		if (!usingWireShader) {
			mat.SetColor ("_EmissionColor", baseColor * Mathf.LinearToGammaSpace (endEmission));
			DynamicGI.UpdateMaterials (objectRenderer);
		}
		if (emission == maxEmission || usingWireShader) {
			atMaxEmission = true;
		}
	}

	public void turnOn () {
		if (coroutine != null) {
			StopCoroutine (coroutine);
		}
		startEmission = emission;
		endEmission = maxEmission;
		if (usingWireShader) {
			endEmission = Vector3.Distance (wireStart.position, wireEnd.position);
		} 
			coroutine = StartCoroutine (changeValue ());
	}

	public void turnOff () {
		if (coroutine != null) {
			StopCoroutine (coroutine);
		}
		startEmission = emission;
		endEmission = 0;
		atMaxEmission = false;
        if (usingWireShader) {
            emission = 0;
            mat.SetFloat("_Distance", 0);
            mat.SetColor("_EmissionColor", baseColor * Mathf.LinearToGammaSpace(endEmission));
            DynamicGI.UpdateMaterials(objectRenderer);
        }
        else {
            coroutine = StartCoroutine(changeValue());
        }
	}
}
