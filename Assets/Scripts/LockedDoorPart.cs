using UnityEngine;
using System.Collections;
using System;

public class LockedDoorPart : MonoBehaviour, Debugable {

	public PowerProvider reference;
	bool powered;
	public bool isPowered;
	FadeEmission fadeEmission;
	Coroutine coroutine;
    LockedDoorController doorController;
    [SerializeField]
    public FadeDoorPartEmission fadePartEmission = new FadeDoorPartEmission();
    public Renderer emission;
    public Light pointLight;

	void Awake() {
		coroutine = StartCoroutine (WaitForDoor ());
        fadePartEmission.doorPart = this;
        fadePartEmission.color = emission.material.GetColor("_EmissionColor");
        setColorAndIntensity(fadePartEmission.color, 0);
	}

	void Start() {
		fadeEmission = GetComponent<FadeEmission> ();
		if (fadeEmission == null) {
			fadeEmission = gameObject.AddComponent<FadeEmission> ();
		}
        setColorAndIntensity(fadePartEmission.color, 0);
    }

	void Update() {
		if (isPowered != powered) {
			if (isPowered) {
				turnOn ();
			} else {
				turnOff ();
			}
		}
	}

	public void turnOn () {
		isPowered = true;
		powered = true;
		fadePartEmission.turnOn ();
	}

	public void turnOff () {
		isPowered = false;
		powered = false;
		fadePartEmission.turnOff ();
	}

	IEnumerator WaitForDoor () {
		yield return null;
		yield return null;
		turnOn ();
	}

	IEnumerator CheckForVariableChange() {
		while (Application.isPlaying) {
			if (isPowered != powered) {
				if (isPowered) {
					turnOn ();
				} else {
					turnOff ();
				}
			}
			yield return new WaitForSeconds (0.5f);
		}
	}

	public void UseLockPart(LockedDoorController door){
        doorController = door;
		StopCoroutine (coroutine);
		if (Application.isEditor) {
			StartCoroutine (CheckForVariableChange ());
		}
	}

    public void debug()
    {
        doorController.toggleDoorLockState();
    }

    public void setColorAndIntensity(Color color, float intensity)
    {
        Color emissionColor = color * intensity; // Mathf.LinearToGammaSpace(intensity);
        emission.material.SetColor("_EmissionColor", emissionColor);
        DynamicGI.SetEmissive(emission, emissionColor);
        if (pointLight != null)
        {
            pointLight.intensity = intensity;
            pointLight.color = color;
        }
        
    }
}

[Serializable]
public class FadeDoorPartEmission
{
    public LockedDoorPart doorPart;
    public Color color = Color.white;
    Color currentColor = Color.white;
    float currentIntensity = 0;
    float minIntensity = 0;
    public float maxIntensity = 1;
    public float animationLength = 1;
    Coroutine currentCoroutine;
    public bool debug = false;


    public void changeColorAndIntensity(float intensity, Color color)
    {
        currentIntensity = intensity;
        currentColor = color;
        if (debug)
        {
            Debug.Log(intensity);
        }
        doorPart.setColorAndIntensity(color, intensity);
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
        if (currentColor == Color.white)
        {
            currentColor = color;
        }

        if (currentCoroutine != null)
        {
            LerpCoroutine.stopCoroutine(currentCoroutine);
        }
        currentCoroutine = LerpCoroutine.LerpMinToMax(animationLength, minIntensity, maxIntensity, currentIntensity, changeIntensity, false);
    }

    public void turnOff()
    {
        if (currentCoroutine != null)
        {
            LerpCoroutine.stopCoroutine(currentCoroutine);
        }
        currentCoroutine = LerpCoroutine.LerpMinToMax(animationLength, minIntensity, maxIntensity, currentIntensity, changeIntensity, true);
    }
}
