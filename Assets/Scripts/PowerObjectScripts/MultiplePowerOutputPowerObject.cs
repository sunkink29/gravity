using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MultiplePowerOutputPowerObject : PowerObject {

	public List<PowerObject> powerables = new List<PowerObject>();

	// Use this for initialization
	public override void Start () {
		powerType = PowerType.both;
		base.Start ();
	}

	public override void changePower(float[] powerArgs) {
		powerArgs [0] = gameObject.GetInstanceID ();
		for (int i = 0; i < powerables.Count; i++) {
			powerables [i].changePower (powerArgs);
		}
	}

	public override void sendReference(PowerObject reference) {
		powerables.Add (reference);
	}
}
