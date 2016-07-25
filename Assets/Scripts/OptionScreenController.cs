using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Globalization;

public class OptionScreenController : MonoBehaviour {

    public ResolutionObjects resolutionObjects = new ResolutionObjects();
    public SliderAndInputFieldObjects fovObjects = new SliderAndInputFieldObjects();
    public SliderAndInputFieldObjects xAxisSensitivityObjects = new SliderAndInputFieldObjects();
    public SliderAndInputFieldObjects yAxisSensitivityObjects = new SliderAndInputFieldObjects();
    public FirstPersonScript playerScript;
    public Camera playerCamera;
    static Resolution[] resolutions;
    static Resolution CurrentResolution;
    static bool fullScreen;
    static float fieldOfView;
    static float xSensitivity;
    static float ySensitivity;

    void Awake()
    {
        if (resolutions == null)
        {
            resolutions = Screen.resolutions;
            CurrentResolution = getScreenResolution();
            fullScreen = Screen.fullScreen;
        }
        List<Dropdown.OptionData> listResolutions = new List<Dropdown.OptionData>();
        foreach (Resolution res in resolutions)
        {
            Dropdown.OptionData data = new Dropdown.OptionData(res.ToString());
            listResolutions.Add(data);
        }
        resolutionObjects.resolutionDropdown.ClearOptions();
        resolutionObjects.resolutionDropdown.AddOptions(listResolutions);
        setFullScreen(fullScreen);
        setFieldOfView(playerCamera.fieldOfView);
        setSensitivity(playerScript.cameraRotator.horizontalSensitivity, playerScript.cameraRotator.verticalSensitivity);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        resolutionObjects.resolutionDropdown.value = Array.IndexOf(resolutions, Array.Find(resolutions, resolutionEqual));
        setFullScreen(fullScreen);
        setFieldOfView(fieldOfView);
        setSensitivity(xSensitivity, ySensitivity);
    }

    void OnDisable()
    {
        Resolution screenResolution = getScreenResolution();
        if (!resolutionEqual(screenResolution))
        {
            CurrentResolution = screenResolution;
        }
        if (Screen.fullScreen != fullScreen)
        {
            fullScreen = Screen.fullScreen;
        }
    }

    Resolution getScreenResolution()
    {
        Resolution resolution = new Resolution();
        resolution.height = playerCamera.pixelHeight;
        resolution.width = playerCamera.pixelWidth;
        return resolution;
    }

    static bool resolutionEqual(Resolution res)
    {
        return CurrentResolution.width == res.width && CurrentResolution.height == res.height;
    }
    
    public void setResolution()
    {
        CurrentResolution = resolutions[resolutionObjects.resolutionDropdown.value];
    }

    public void setFullScreen(bool value)
    {
        fullScreen = value;
        resolutionObjects.fullScreenToggle.isOn = value;
    }

    public void applyResolution()
    {
        Screen.SetResolution(CurrentResolution.width, CurrentResolution.height, fullScreen);
        print("resolution changed");
    }

    public void setFieldOfView(float value)
    {
        fieldOfView = value;
        playerCamera.fieldOfView = value;
        fovObjects.slider.value = value;
        fovObjects.inputField.text = value.ToString();
    }

    public void setFieldOfView(string value)
    {
        setFieldOfView(float.Parse(value, CultureInfo.InvariantCulture.NumberFormat));
    }

    public void setSensitivity(float x, float y)
    {
        xSensitivity = x;
        playerScript.cameraRotator.horizontalSensitivity = x;
        xAxisSensitivityObjects.slider.value = x;
        xAxisSensitivityObjects.inputField.text = x.ToString();
        ySensitivity = y;
        playerScript.cameraRotator.verticalSensitivity = y;
        yAxisSensitivityObjects.slider.value = y;
        yAxisSensitivityObjects.inputField.text = y.ToString();
    }

    public void setXSensitivity(float x)
    {
        setSensitivity(x, ySensitivity);
    }

    public void setXSensitivity(string x)
    {
        setXSensitivity(float.Parse(x, CultureInfo.InvariantCulture.NumberFormat));
    }

    public void setYSensitivity(float y)
    {
        setSensitivity(xSensitivity, y);
    }

    public void setYSensitivity(string y)
    {
        setYSensitivity(float.Parse(y, CultureInfo.InvariantCulture.NumberFormat));
    }
}

[Serializable]
public class ResolutionObjects
{
    public Dropdown resolutionDropdown;
    public Toggle fullScreenToggle;
}

[Serializable]
public class SliderAndInputFieldObjects
{
    public Slider slider;
    public InputField inputField;
}
