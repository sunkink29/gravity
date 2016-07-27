using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class RoomLightsController : MonoBehaviour, Powerable, PowerProvider {

    Powerable ConnectedObject;
	public GameObject powerProviderObject;
	PowerProvider powerProvider;
    LightStripController[] lightStripObjects;
    [SerializeField]
    FadeEmissionLightGroup[] lightGroups;
    public bool powered;
    public float animationLength = 2;

	// Use this for initialization
	void Start () {
        lightStripObjects = GetComponentsInChildren<LightStripController>();
        List<List<LightStripController>> lightStripGroups = new List<List<LightStripController>>(20);
        int numberOfGroups = 0;
        for (int i = 0; i < lightStripObjects.Length; i++)
        {
            bool spotFound = false; ;
            LightInfo lightInfo = lightStripObjects[i].getLightInfo();
            for (int j = 0; j < lightStripGroups.Count; j++)
            {
                LightInfo groupLightInfo = lightStripGroups[j][0].getLightInfo();
                if (groupLightInfo.color == lightInfo.color && groupLightInfo.defaltIntensity == lightInfo.defaltIntensity && groupLightInfo.maxIntensity == lightInfo.maxIntensity)
                {
                    lightStripGroups[j].Add(lightStripObjects[i]);
                    spotFound = true;
                    break;
                }
            }
            if (!spotFound)
            {
                lightStripGroups.Add(new List<LightStripController>());
                lightStripGroups[lightStripGroups.Count-1].Add(lightStripObjects[i]);
                numberOfGroups++;
            }
        }

        lightGroups = new FadeEmissionLightGroup[lightStripGroups.Count];
        for (int i = 0; i < lightGroups.Length; i++)
        {
            lightGroups[i] = new FadeEmissionLightGroup();
            LightInfo lightInfo = lightStripGroups[i][0].getLightInfo();
            lightGroups[i].setup(lightStripGroups[i].ToArray(), lightInfo, this, animationLength);
        }

        if (LerpCoroutine.currentInstance == null)
        {
            LerpCoroutine lerpCoroutine = gameObject.AddComponent<LerpCoroutine>();
            lerpCoroutine.Awake();
        }

        if (powerProviderObject != null)
        {
            powerProvider = powerProviderObject.GetComponent<PowerProvider>();
            powerProvider.sendReference(this);
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (powered && !(lightStripObjects[0].powered))
        {
            powerOn();
        } else if (!powered && lightStripObjects[0].powered)
        {
            powerOff();
        }
	}

    void changeLightState(int index, bool state)
    {
        if (state)
        {
            lightGroups[index].turnOn();
        } else
        {
            lightGroups[index].turnOff();
        }
    }

	public void powerOn () {
		powerOn (null);
	}

	public void powerOn (PowerProvider reference) {
        //fadeEmission.turnOn ();
        //ConnectedObject.powerOn (this);

        powered = true;
        for (int i = 0; i < lightGroups.Length; i++)
        {
            changeLightState(i, true);
        }
        ConnectedObject.powerOn(this);

	}

	public void powerOff () {
		powerOff (null);
	}

	public void powerOff (PowerProvider reference) {
        //fadeEmission.turnOff ();
        //ConnectedObject.powerOff (this);

        powered = false;
        for (int i = 0; i < lightGroups.Length; i++)
        {
            changeLightState(i, false);
        }
        ConnectedObject.powerOff(this);
    }

	public void sendReference (Powerable reference) {
		ConnectedObject = reference;
	}

    public Coroutine startCorutine(IEnumerator routine)
    {
        Debug.Log("startCorutine Called " + routine);
        return startCorutine(routine);
    }

    public void stopCorutine(Coroutine corutine)
    {
        stopCorutine(corutine);
    }
}

[Serializable]
public class FadeEmissionLightGroup
{
    public LightStripController[] lights;
    public LightInfo lightInfo;
    float animationLength;
    float currentIntensity;
    Color currentColor;
    Coroutine currentFloatCoroutine;
    Coroutine currentColorCoroutine;

    public void setup(LightStripController[] lightGroup, LightInfo lightInfo,MonoBehaviour monoBehavior, float animationLength)
    {
        lights = lightGroup;
        this.lightInfo = lightInfo;
        currentColor = lightInfo.color;
        currentIntensity = lightInfo.defaltIntensity;
        this.animationLength = animationLength;
    }

    public void changeColorAndIntensity(float intensity, Color color)
    {
        currentIntensity = intensity;
        currentColor = color;
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].setColorAndIntensity(color, intensity);
            //Debug.Log("color set");
        }
    }

    public void changeColor(Color color)
    {
        changeColorAndIntensity(currentIntensity, color);
    }
    
    public void changeIntensity(float intensity)
    {
        changeColorAndIntensity(intensity, currentColor);
    }

    public void turnOn()
    {
        if (!(lights[0].powered))
        {
            if (currentFloatCoroutine != null)
            {
                LerpCoroutine.stopCoroutine(currentFloatCoroutine);
            }
            currentFloatCoroutine = LerpCoroutine.LerpMinToMax(animationLength, lightInfo.defaltIntensity, lightInfo.maxIntensity, currentIntensity, changeIntensity, false);
        }
        if (!(lights[0].powered))
        {
            if (currentColorCoroutine != null)
            {
                LerpCoroutine.stopCoroutine(currentColorCoroutine);
            }
            currentColorCoroutine = LerpCoroutine.LerpMinToMax(animationLength, lightInfo.color, Color.white, currentColor, changeColor, false);
        }
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].powered = true;
        }
    }

    public void turnOff()
    {
        if (currentIntensity != lightInfo.defaltIntensity)
        {
            if (currentFloatCoroutine != null)
            {
                LerpCoroutine.stopCoroutine(currentFloatCoroutine);
            }
            currentFloatCoroutine = LerpCoroutine.LerpMinToMax(animationLength, lightInfo.defaltIntensity, lightInfo.maxIntensity, currentIntensity, changeIntensity, true);
        }
        if (currentColor != lightInfo.color)
        {
            if (currentColorCoroutine != null)
            {
                LerpCoroutine.stopCoroutine(currentColorCoroutine);
            }
            currentColorCoroutine = LerpCoroutine.LerpMinToMax(animationLength, lightInfo.color, Color.white, currentColor, changeColor, true);
        }
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].powered = false;
        }
    }
    
}
