using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerProviderPowerObject :PowerObject {

	PowerObject connectedObject;

	public override void changePower(float[] powerArgs) {
		powerArgs [0] = gameObject.GetInstanceID ();
		if (connectedObject != null) {
			connectedObject.changePower (powerArgs);
		}
	}

	public virtual void sendReference(PowerObject reference) {
		connectedObject = reference;
	}
}
