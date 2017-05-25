using UnityEngine;
using System.Collections;
using System;

public class StandingButtonPowerObject : PowerProviderPowerObject, Interactible {

	bool supplyPower = false;
	public bool toggleButton = true;
	public Animator animator;

	public override void Start ()
	{
		base.Start ();
	}

	public void interact () {
		supplyPower = !supplyPower;
		base.changePower(new float[]{this.GetInstanceID(),Convert.ToInt32(supplyPower)});
		if (animator != null) {
			animator.SetBool("ButtonPressed", supplyPower);
		}
	}
}
