using UnityEngine;
using System.Collections;

public class FadeEmission : MonoBehaviour {

	Renderer objectRenderer;
	Material mat;
	Material[] materials;
	public string materialName;
	Color baseColor;
	public float speed = 0.1f;
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
        float endAmount;
        if (usingWireShader) {
            endAmount = Vector3.Distance(wireStart.position, wireEnd.position);
        } else {
            endAmount = maxEmission;
        }
        for (float i = 0; i /*+ speed*0.1*/ <= endAmount; i += speed * 0.1f) {
			if (usingWireShader) {
                emission = Mathf.Lerp(startEmission, endEmission, i / endAmount);
                mat.SetFloat ("_Distance", emission);
                mat.SetColor("_EmissionColor", baseColor * Mathf.LinearToGammaSpace(maxEmission));
            }
            else {
                emission = Mathf.Lerp(startEmission, endEmission, i / endAmount);
                Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);
				mat.SetColor ("_EmissionColor", finalColor);
			}
			DynamicGI.UpdateMaterials (objectRenderer);
			yield return new WaitForSeconds (0.01f);
		}
		emission = endEmission;
		if (!usingWireShader) {
			mat.SetColor ("_EmissionColor", baseColor * Mathf.LinearToGammaSpace (endEmission));
			DynamicGI.UpdateMaterials (objectRenderer);
		} else {
            mat.SetFloat("_Distance", endAmount);
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
