using UnityEngine;
using System.Collections;
using System;

public class StandingButtonPowerObject : PowerObject, Interactible {

	bool supplyPower = false;
	public bool toggleButton = true;
	public Animator animator;

	public override void Start ()
	{
		powerType = PowerType.PowerProvider;
		base.Start ();
	}

	public void interact () {
		supplyPower = !supplyPower;
		base.changePower(new float[]{this.GetInstanceID(),Convert.ToInt32(supplyPower)});
		animator.SetBool("ButtonPressed", supplyPower);
	}
}
