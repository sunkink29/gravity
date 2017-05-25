using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiplePowerInputPowerObject : PowerProviderPowerObject {
	public PowerProviderPowerObject[] powerProviders;
	public float[] powerProviderStates;
	PowerObject powerable;
	public bool sendPowerWhenFullyPowered;

	// Use this for initialization
	public override void Start () {
		for (int i = 0; i < powerProviders.Length; i++) {
			powerProviders[i].sendReference (this);
		}
		powerProviderStates = new float[powerProviders.Length];
	}

	public override void changePower (float[] powerArgs)
	{
		float sum = 0;
		for (int i = 0; i < powerProviders.Length; i++) {
			if (powerArgs.Length >= 1 && powerProviders[i].GetInstanceID() == powerArgs[0]) {
				if (powerArgs.Length >= 2 && powerArgs [1] <= 1) {
					powerProviderStates [i] = powerArgs [1];
				} else {
					powerProviderStates [i] = 1;
				}
				print (powerProviders [i].GetInstanceID () + " " + powerArgs [0]);
			}

			sum += powerProviderStates [i];
		}
		powerArgs = new float[] { GetInstanceID (), sum / powerProviders.Length };
		if (sendPowerWhenFullyPowered && powerArgs.Length >= 2 && powerArgs [1] >= 1) {
			powerable.changePower (powerArgs);
		} else if (!sendPowerWhenFullyPowered) {
			powerable.changePower (powerArgs);
		} else {
			powerArgs [1] = 0;
			powerable.changePower (powerArgs);
		}
	}

	public override void sendReference(PowerObject reference) {
		powerable = reference;
	}
}

