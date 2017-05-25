using UnityEngine;
using System.Collections;

public class MoveWhenPoweredPowerObject : PowerObject{

	public Transform startingPoint;
	public Transform endingPoint;
	public float speed = 1;
	float amountOfTime = 1;
	Vector3 startPoint;
	Vector3 endPoint;
	Coroutine coroutine;
	float currentPoint = 0;

	// Use this for initialization
	public override void Start () {
		base.Start ();
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

	public override void changePower(float[] powerArgs) {
		if (coroutine != null) {
			LerpCoroutine.stopCoroutine (coroutine);
		}
		coroutine = LerpCoroutine.LerpMinToMax(amountOfTime/speed,currentPoint,powerArgs[1],currentPoint,changePosition,false);
		powerArgs [0] = GetInstanceID ();
		base.changePower (powerArgs);

	}

	public void changePosition (float positionFloat) {
		currentPoint = positionFloat;
		Vector3 position = Vector3.Lerp (startPoint, endPoint, positionFloat);
		gameObject.transform.position = position;
	}
}
