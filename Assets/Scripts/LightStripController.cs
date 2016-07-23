using UnityEngine;
using System;

public enum LightType { pointLight, emissiveLight };
public enum LightDefaltIntensity { fullStrenght, randomHighIntensity, randomLowIntensity, off }

public class LightStripController : MonoBehaviour, Powerable, Debugable {

    public LightType lightType = LightType.emissiveLight;
    public LightDefaltIntensity lightDefaltIntensity = LightDefaltIntensity.randomHighIntensity;
    public Light pointLight;
    public Renderer emission;
    float defaltIntensity;
    //Color defaltEmissionColor;
    //Color maxEmissionColor;
    public Color color = Color.white;
    public float maxIntensity = 1;
    System.Random randomGen = new System.Random();
    public bool powered = false;

	// Use this for initialization
	void Start () {
        if (lightDefaltIntensity == LightDefaltIntensity.fullStrenght)
        {
            defaltIntensity = maxIntensity;
        }else if (lightDefaltIntensity == LightDefaltIntensity.randomHighIntensity)
        {
            defaltIntensity = (float)((randomGen.Next(100) * 0.01) * maxIntensity * .5 + .5);
        } else if (lightDefaltIntensity == LightDefaltIntensity.randomLowIntensity)
        {
            defaltIntensity = (float)((randomGen.Next(100) * 0.01) * maxIntensity * .5);
        } else if (lightDefaltIntensity == LightDefaltIntensity.off)
        {
            defaltIntensity = 0;
        }

        if (lightType == LightType.emissiveLight && pointLight != null)
        {
            pointLight.intensity = 0;
        }
        setColorAndIntensity(color, defaltIntensity);
    }

    public void powerOn()
    {
        powerOn(null);
    }

    public void powerOn(PowerProvider powerProvider)
    {

        powered = true;
        setColorAndIntensity(Color.white, maxIntensity);
    }  

    public void powerOff()
    {
        powerOff(null);
    }

    public void powerOff(PowerProvider powerProvider)
    {
        powered = false;
        setColorAndIntensity(color, defaltIntensity);
    }

    public void debug()
    {
        if (powered)
        {
            powerOff();
        }
        else
        {
            powerOn();
        }
    }

    public void setColorAndIntensity(Color color, float intensity)
    {
        Color emissionColor = color * Mathf.LinearToGammaSpace(intensity);
        emission.material.SetColor("_EmissionColor", emissionColor);
        DynamicGI.SetEmissive(emission,emissionColor);
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

[Serializable]
public class LightInfo
{
    public float defaltIntensity;
    public float maxIntensity;
    public Color color;
}
