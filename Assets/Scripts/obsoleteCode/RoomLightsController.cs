using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class RoomLightsController : MonoBehaviour, Powerable, PowerProvider {

    Powerable ConnectedObject;
	public MonoBehaviour powerProviderMonoBehaviour;
	PowerProvider powerProvider;
    LightStripController[] lightStripObjects;
    [SerializeField]
    FadeEmissionLightGroup[] lightGroups;
    public bool powered;
    public float animationLength = 2;
    public LightColorSource colorSource;
	public string roomName;
	public static List<RoomLightsController> AllRooms = new List<RoomLightsController>();
	Coroutine currentCoroutine;
	float coroutineCurrentPoint = 0;

	// Use this for initialization
	void Start () {
		AllRooms.Add (this);
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

        if (powerProviderMonoBehaviour != null)
        {
			powerProvider = (PowerProvider) powerProviderMonoBehaviour;
            powerProvider.sendReference(this);
        }
	}
	
	// Update is called once per frame
//	void Update () {
//	    if (powered && !(lightStripObjects[0].powered))
//        {
//            powerOn();
//        } else if (!powered && lightStripObjects[0].powered)
//        {
//            powerOff();
//        }
//	}

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
		
	public void changePower(float[] powerArgs) {
		bool powered;
		float num = powerArgs [1];
		float dif = num - coroutineCurrentPoint;
		if (dif >= 0) {
			powered = true;
		} else {
			powered = false;
		}
		if (currentCoroutine != null) {
			LerpCoroutine.stopCoroutine (currentCoroutine);
		}
		//print (animationLength*Mathf.Abs(dif));
		currentCoroutine = LerpCoroutine.LerpMinToMax(animationLength*Mathf.Abs(dif),coroutineCurrentPoint,num,coroutineCurrentPoint,changeAllLights,false);

		//powered = false;
		if (ConnectedObject != null) {
			powerArgs [0] = this.GetInstanceID ();
			ConnectedObject.changePower (powerArgs);
		}
	}

	public void sendReference (Powerable reference) {
		ConnectedObject = reference;
	}

	public void changeAllLights(float t) {
		for (int i = 0; i < lightStripObjects.Length; i++) {
			LightStripController lightStrip = lightStripObjects [i];
			lightStrip.setColorAndIntensity (Color.Lerp(lightStrip.color,Color.white,t),
				Mathf.Lerp(lightStrip.defaltIntensity,lightStrip.maxIntensity,t));
		}
		coroutineCurrentPoint = t;
	}

	public GameObject getGameObject() {
		return gameObject;
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
