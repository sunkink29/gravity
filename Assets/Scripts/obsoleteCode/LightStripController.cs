using UnityEngine;
using System;
using System.Collections.Generic;

public enum LightType { pointLight, emissiveLight };
public enum LightDefaltIntensity { fullStrenght, randomHighIntensity, randomLowIntensity, off }

public class LightStripController : MonoBehaviour, Powerable, PowerProvider, Debugable {

    public LightType lightType = LightType.emissiveLight;
    public LightDefaltIntensity lightDefaltIntensity = LightDefaltIntensity.randomHighIntensity;
    public Light pointLight;
    public Renderer emission;
    public float defaltIntensity;
    //Color defaltEmissionColor;
    //Color maxEmissionColor;
    public bool useCustomColor = false;
    public Color color = Color.white;
    public float maxIntensity = 1;
	public bool changeIntensityToPower = false;
    System.Random randomGen = new System.Random();
    public bool powered = false;
    public bool UpdateGI = false;
    public bool useSlider = false;
    [Range(0, 1)]
    public float colorSlider = 0;
    Powerable connectedObject;
	public MonoBehaviour powerProviderMonoBehaviour;
    PowerProvider powerProvider;

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
        if (powerProviderMonoBehaviour != null)
        {
            powerProvider = powerProviderMonoBehaviour.GetComponent<PowerProvider>();
            powerProvider.sendReference(this);
        }
       
    }

    public void setup(LightColorSource colorSource)
    {
        LightColorSource[] colorChain = colorSource.colorSources;
        List<LightColorSource> colorSources = new List<LightColorSource>();
        for(int i = 0; i < colorChain.Length; i++)
        {
            colorSources.Add(colorChain[i]);
        }
        colorSources.Sort(isCloser);

    }

    public int isCloser(LightColorSource x, LightColorSource y)
    {
        float xDistance = Vector3.Distance(transform.position, x.transform.position);
        float yDistance = Vector3.Distance(transform.position, y.transform.position);

        if (xDistance < yDistance)
        {
            return -1;
        } else if (xDistance == yDistance)
        {
            return 0;
        } else
        {
            return 1;
        }
    }

	// Use this for initialization
	void Start () {
		
        setColorAndIntensity(color, defaltIntensity);
    }

    void Update()
    {
        if (UpdateGI)
        {
            UpdateGI = false;
            if (powered)
            {
				changePower (new float[]{ GetInstanceID(), 1 });
            } else
            {
				changePower (new float[]{ GetInstanceID(), 0 });
            }
        }

        if (useSlider)
        {
            setColorAndIntensity(Color.Lerp(color, Color.white, colorSlider),defaltIntensity);
        }
    }

	public void changePower(float[] powerArgs) {
		Color color;
		float intensity;
		if (powerArgs.Length >= 2 && powerArgs [1] >= 1) {
			powered = true;
			color = Color.white;
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
		if (connectedObject != null && powerArgs.Length >= 2) {
			powerArgs [0] = this.GetInstanceID ();
			connectedObject.changePower(powerArgs);
		}
	}

    public void sendReference(Powerable obj)
    {
        connectedObject = obj;
    }

    public void debug()
    {
        if (powered)
        {
			changePower (new float[]{ GetInstanceID (), 0 });
        }
        else
        {
			changePower (new float[]{ GetInstanceID (), 1 });
        }
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

	public GameObject getGameObject() {
		return gameObject;
	}
}

[Serializable]
public class LightInfo
{
    public float defaltIntensity;
    public float maxIntensity;
    public Color color;
}
