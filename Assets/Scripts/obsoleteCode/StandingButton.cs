using UnityEngine;
using System.Collections;
using System;

public class StandingButton : MonoBehaviour , Interactible, PowerProvider {

	Powerable connectedObject;
	bool supplyPower = false;
	public bool toggleButton = true;
    public Animator animator;

	public void interact () {
		supplyPower = !supplyPower;
		connectedObject.changePower(new float[]{this.GetInstanceID(),Convert.ToInt32(supplyPower)});
        animator.SetBool("ButtonPressed", supplyPower);
	}

	public void sendReference(Powerable reference) {
		connectedObject = reference;
	}
}
