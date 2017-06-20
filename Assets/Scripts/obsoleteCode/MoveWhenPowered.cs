using UnityEngine;
using System.Collections;

public class MoveWhenPowered : MonoBehaviour, Powerable {

	public MonoBehaviour powerProviderMonoBehaviour;
	public Transform startingPoint;
	public Transform endingPoint;
	public float speed = 1;
	float amountOfTime = 1;
	Vector3 startPoint;
	Vector3 endPoint;
	PowerProvider powerProvider;
	Coroutine coroutine;
	float currentPoint = 0;

	// Use this for initialization
	void Start () {
		powerProvider = (PowerProvider) powerProviderMonoBehaviour;
		powerProvider.sendReference (this);
		startPoint = startingPoint.position;
		if (endingPoint != null) {
			endPoint = endingPoint.position;
		} else {
			endPoint = transform.position;
		}
		if (LerpCoroutine.currentInstance == null)
		{
			LerpCoroutine lerpCoroutine = gameObject.AddComponent<LerpCoroutine>();
			lerpCoroutine.Awake();
		}
		changePosition (0);
	}

	public void changePower(float[] powerArgs) {
		if (coroutine != null) {
			LerpCoroutine.stopCoroutine (coroutine);
		}
		coroutine = LerpCoroutine.LerpMinToMax(amountOfTime/speed,currentPoint,powerArgs[1],currentPoint,changePosition,false);

	}

	public void changePosition (float positionFloat) {
		currentPoint = positionFloat;
		Vector3 position = Vector3.Lerp (startPoint, endPoint, positionFloat);
		gameObject.transform.position = position;
	}

	public GameObject getGameObject() {
		return gameObject;
	}
}
