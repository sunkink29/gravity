using UnityEngine;
using System.Collections;

public class pulsePower : MonoBehaviour, PowerProvider {

	Powerable connectedObject;
	public float maxPower = 1;
	public float minPower = 0;
	public float timeForLoop = 1;
	Coroutine loopCoroutine;
	bool isMovingForword = true;

	// Use this for initialization
	void Start () {
		if (LerpCoroutine.currentInstance == null)
		{
			LerpCoroutine lerpCoroutine = gameObject.AddComponent<LerpCoroutine>();
			lerpCoroutine.Awake();
		}
		loopCoroutine = LerpCoroutine.LerpMinToMax (timeForLoop, minPower, maxPower, minPower, changePower, false);
	}

	void changePower(float power) {
		if (connectedObject != null) {
			connectedObject.changePower (new float[]{GetInstanceID(),power});
		}
		if (power >= maxPower && isMovingForword) {
			isMovingForword = false;
			LerpCoroutine.stopCoroutine (loopCoroutine);
			loopCoroutine = LerpCoroutine.LerpMinToMax (timeForLoop, minPower, maxPower, maxPower, changePower, true);
		} else if (power <= minPower && !isMovingForword) {
			isMovingForword = true;
			LerpCoroutine.stopCoroutine (loopCoroutine);
			loopCoroutine = LerpCoroutine.LerpMinToMax (timeForLoop, minPower, maxPower, minPower, changePower, false);
		}
	}

	public void sendReference(Powerable reference) {
		connectedObject = reference;
	}
}
