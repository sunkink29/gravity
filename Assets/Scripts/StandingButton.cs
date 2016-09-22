using UnityEngine;
using System.Collections;
using System;

public class StandingButton : MonoBehaviour , Interactible, PowerProvider {

	Powerable connectedObject;
	bool supplyPower = false;
	public bool toggleButton = true;
	public bool useNewInterface = false;
    public Animator animator;

	public void interact () {
		supplyPower = !supplyPower;
		if (useNewInterface) {
			connectedObject.changePower(new float[]{this.GetInstanceID(),Convert.ToInt32(supplyPower)});
		} else {
			if (supplyPower) {
				connectedObject.powerOn (this);
			} else if (toggleButton) {
				connectedObject.powerOff (this);
			}
		}
        animator.SetBool("ButtonPressed", supplyPower);
	}

	public void sendReference(Powerable reference) {
		connectedObject = reference;
	}
}
