using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PointInChain { Begining, Middle, End };

public class LightColorSource : MonoBehaviour {

    public PointInChain pointInChain = PointInChain.End;
    public Color color = Color.white;
    public LightColorSource nextColorSource;
    public float distanceFromEnd = 0;
    public float totalChainDistance = 0;
    public LightColorSource[] colorSources;
    void Awake ()
    {
        if (pointInChain == PointInChain.Begining)
        {
            //nextColorSource.sendReference(this);
            LightColorSource currentSource = this;
            List<LightColorSource> colorSources = new List<LightColorSource>();
            while(currentSource != null)
            {
                colorSources.Add(currentSource);
                currentSource = currentSource.nextColorSource;
            }
            this.colorSources = colorSources.ToArray();
            colorSources[colorSources.Count-1].setupChain(this.colorSources);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setupChain(LightColorSource[] allSources)
    {
        colorSources = allSources;
        LightColorSource lastSource = this;
        float totalDistance = 0;
        for(int i = colorSources.Length-1; i >= 0; i--)
        {
            totalDistance += Vector3.Distance(lastSource.transform.position, colorSources[i].transform.position);
            colorSources[i].distanceFromEnd = totalDistance;
            lastSource = colorSources[i];
        }
        for(int i = 0; i < colorSources.Length; i++)
        {
            float pointInChain = colorSources[i].distanceFromEnd / totalDistance;
            Color color = Color.Lerp(this.color, Color.white, pointInChain);
            colorSources[i].setup(color, totalDistance, colorSources);
        }
    }

    public void setup(Color color, float totalDistance, LightColorSource[] allSources)
    {
        this.color = color;
        totalChainDistance = totalDistance;
        colorSources = allSources;
    }
}
