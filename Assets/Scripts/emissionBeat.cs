using UnityEngine;
using System.Collections;

public class emissionBeat : MonoBehaviour {

	GravityOnNormals playerGravity;
	Renderer renderer;
	Material mat;
	int count = 1;

	// Use this for initialization
	void Start () {
		playerGravity = GameObject.FindWithTag ("Player").GetComponent<GravityOnNormals> ();
		renderer = GetComponent<Renderer> ();
		mat = renderer.material;

	}
	
	// Update is called once per frame
	void Update () {
		if (count == 3) {
			float emission = /*Mathf.Clamp((Mathf.Cos(Time.time*1.5f)+2f)*1f,0f,3f)*/ (Mathf.PingPong (Time.time * .5f, 1.0f) * 5f) + 0.5f;
			Vector3 color = playerGravity.currentDirection * 0.5f + new Vector3 (0.5f, 0.5f, 0.5f);
			Color baseColor = //Color.cyan
				new Vector4 (color.x, color.y, color.z, 1); //Replace this with whatever you want for your base color at emission level '1'

			Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);


			mat.SetColor ("_EmissionColor", finalColor);
			DynamicGI.UpdateMaterials (renderer);
			count = 1;
		}
		count++;
	}
}
