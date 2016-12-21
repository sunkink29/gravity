using UnityEngine;
using System.Collections;

public class PowerObject : MonoBehaviour {

	[HideInInspector] public PowerType powerType;
	public PowerObject powerProvider;
	PowerObject connectedObject;

	public virtual void Start () {
		if (powerType == PowerType.Powerable || powerType == PowerType.both && powerProvider != null) {
			powerProvider.sendReference (this);
		}
	}

	public virtual void changePower(float[] powerArgs) {
		powerArgs [0] = gameObject.GetInstanceID ();
		if (connectedObject != null) {
			connectedObject.changePower (powerArgs);
		}
	}

	public virtual void sendReference(PowerObject reference) {
		connectedObject = reference;
	}
}

public enum PowerType { PowerProvider, Powerable, both };
