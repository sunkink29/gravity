using UnityEngine;
using System.Collections;

public class emissionBeat : MonoBehaviour {

	GravityOnNormals playerGravity;
	Renderer objectRenderer;
	Material mat;
	int count = 1;

	// Use this for initialization
	void Start () {
		playerGravity = GameObject.FindWithTag ("Player").GetComponent<GravityOnNormals> ();
		objectRenderer = GetComponent<Renderer> ();
		mat = objectRenderer.material;

	}
	
	// Update is called once per frame
	void Update () {
		if (count == 2) {
			float emission =(Mathf.PingPong (Time.time * .5f, 0.5f) * .5f) + 0;
			Vector3 color = playerGravity.currentDirection * 0.5f + new Vector3 (0.5f, 0.5f, 0.5f);
			Color baseColor = //Color.cyan
				new Vector4 (color.x, color.y, color.z, 1); //Replace this with whatever you want for your base color at emission level '1'

			Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);


			mat.SetColor ("_EmissionColor", finalColor);
			DynamicGI.UpdateMaterials (objectRenderer);
			count = 0;
		}
		count++;
	}
}
