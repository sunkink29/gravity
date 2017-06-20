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
        animator.SetBool("toggleButton", toggleButton);
	}

	public void interact () {
        if (toggleButton)
            supplyPower = !supplyPower;
        else
            supplyPower = true;
        base.changePower(new float[]{this.GetInstanceID(),Convert.ToInt32(supplyPower)});
		if (animator != null) {
			animator.SetBool("ButtonPressed", supplyPower);
            animator.SetTrigger("pressButton");
		}
	}
}
