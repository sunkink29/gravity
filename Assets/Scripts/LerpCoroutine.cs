using UnityEngine;
using System.Collections;

public class LerpCoroutine : MonoBehaviour {
    public delegate void floatDel(float currentPointFloat);
    public delegate void colorDel(Color currentPoint);
    
    public static LerpCoroutine currentInstance;

    public void Awake()
    {
        currentInstance = this;
    }

    public static Coroutine LerpMinToMax(float lengthInSec, float min, float max, float currentPoint, floatDel callback, bool lerpInverse)
    {
        if (!lerpInverse)
        {
            if (currentPoint == min)
            {
                return currentInstance.StartCoroutine(lerpFloat(lengthInSec, min, max, callback));
            } else if (currentPoint != max)
            {
                return currentInstance.StartCoroutine(lerpFloat(lengthInSec, currentPoint, max, callback));
            }
        }
        else
        {
            if (currentPoint == max)
            {
                return currentInstance.StartCoroutine(lerpFloat(lengthInSec, max, min, callback));
            } else if (currentPoint != min)
            {
                return currentInstance.StartCoroutine(lerpFloat(lengthInSec, currentPoint, min, callback));
            }
        }
        return null;
    }

    public static Coroutine LerpMinToMax(float lengthInSec, Color min, Color max, Color currentPoint, colorDel callback, bool lerpInverse)
    {
        if (!lerpInverse)
        {
            if (currentPoint == min)
            {
                return currentInstance.StartCoroutine(lerpColor(lengthInSec, min, max, callback));
            }
            else if (currentPoint != max)
            {
                return currentInstance.StartCoroutine(lerpColor(lengthInSec, currentPoint, max, callback));
            }
        }
        else
        {
            if (currentPoint == max)
            {
                return currentInstance.StartCoroutine(lerpColor(lengthInSec, max, min, callback));
            }
            else if (currentPoint != min)
            {
                return currentInstance.StartCoroutine(lerpColor(lengthInSec, currentPoint, min, callback));
            }
        }
        return null;
    }

    public static void stopCoroutine(Coroutine routine)
    {
        currentInstance.StopCoroutine(routine);
    }

    public static IEnumerator lerpFloat(float lengthInSec, float start, float end, floatDel callback)
    {
        
        //Debug.Log("coroutine started");
        for (float i = 0; i < lengthInSec; i += Time.deltaTime)
        {
            float currentPoint = Mathf.Lerp(start, end, i / lengthInSec);
            callback(currentPoint);
            yield return null;
        }
        callback(end);
    }

    public static IEnumerator lerpColor(float lengthInSec, Color start, Color end, colorDel callback)
    {
        for (float i = 0; i < lengthInSec; i += Time.deltaTime)
        {
            Color currentPoint = Color.Lerp(start, end, i / lengthInSec);
            callback(currentPoint);
            yield return null;
        }
        callback(end);
    }

}
