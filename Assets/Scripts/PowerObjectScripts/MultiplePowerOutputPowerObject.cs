using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MultiplePowerOutputPowerObject : PowerProviderPowerObject {

	[HideInInspector] 
	public bool showPowerProvider;
	public List<PowerObject> powerables = new List<PowerObject>();

    // Use this for initialization
    public override void Start () {
		base.Start ();
	}

	public override void changePower(float[] powerArgs) {
		powerArgs [0] = GetInstanceID ();
		for (int i = 0; i < powerables.Count; i++) {
			powerables [i].changePower (powerArgs);
		}
	}

	public override void sendReference(PowerObject reference) {
		powerables.Add (reference);
	}
}
