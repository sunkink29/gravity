using UnityEngine;
using System;
using System.Collections.Generic;

//public enum LightType { pointLight, emissiveLight };
//public enum LightDefaltIntensity { fullStrenght, randomHighIntensity, randomLowIntensity, off }

public class LightStripControllerPowerObject : PowerObject{

	public LightType lightType = LightType.emissiveLight;
	public LightDefaltIntensity lightDefaltIntensity = LightDefaltIntensity.randomHighIntensity;
	public Light pointLight;
	public Renderer emission;
	public float defaltIntensity;
	public bool useCustomColor = false;
	public Color color = Color.white;
	Color minEmissionColor;
	public Color maxEmissionColor = Color.white;
	public float maxIntensity = 1;
	public bool changeIntensityToPower = false;
	System.Random randomGen = new System.Random();
	public bool powered = false;

	void Awake()
	{
		if (lightDefaltIntensity == LightDefaltIntensity.fullStrenght)
		{
			defaltIntensity = maxIntensity;
		}
		else if (lightDefaltIntensity == LightDefaltIntensity.randomHighIntensity)
		{
			defaltIntensity = (float)((randomGen.Next(100) * 0.01) * maxIntensity * .5 + maxIntensity * .5);
		}
		else if (lightDefaltIntensity == LightDefaltIntensity.randomLowIntensity)
		{
			defaltIntensity = (float)((randomGen.Next(100) * 0.01) * maxIntensity * .5);
		}
		else if (lightDefaltIntensity == LightDefaltIntensity.off)
		{
			defaltIntensity = 0;
		}

		if (lightType == LightType.emissiveLight && pointLight != null)
		{
			pointLight.intensity = 0;
		} else if (lightType == LightType.pointLight && pointLight != null)
		{
			pointLight.enabled = true;
			pointLight.gameObject.SetActive(true);
		}
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();
		setColorAndIntensity(color, defaltIntensity);
	}

	public override void changePower(float[] powerArgs) {
		Color color;
		float intensity;
		if (powerArgs.Length >= 2 && powerArgs [1] >= 1) {
			powered = true;
			color = maxEmissionColor;
			intensity = maxIntensity;
		} else if (changeIntensityToPower && powerArgs.Length >= 2) {
			intensity = powerArgs [1];
			color = this.color;
		} else {
			powered = false;
			color = this.color;
			intensity = defaltIntensity;
		}
		setColorAndIntensity (color, intensity);
		base.changePower (powerArgs);
	}

	public void setColorAndIntensity(Color color, float intensity)
	{
		Color emissionColor = color * intensity; // Mathf.LinearToGammaSpace(intensity);
		emission.material.SetColor("_EmissionColor", emissionColor);
		//		DynamicGI.UpdateMaterials (emission);
		DynamicGI.SetEmissive(emission,emissionColor);
		//print(intensity);
		if (lightType == LightType.pointLight)
		{
			pointLight.intensity = intensity;
			pointLight.color = color;
		}
	}

	public LightInfo getLightInfo()
	{
		LightInfo lightInfo = new LightInfo();
		lightInfo.defaltIntensity = defaltIntensity;
		lightInfo.maxIntensity = maxIntensity;
		lightInfo.color = color;
		return lightInfo;
	}
}
//
//[Serializable]
//public class LightInfo
//{
//	public float defaltIntensity;
//	public float maxIntensity;
//	public Color color;
//}
