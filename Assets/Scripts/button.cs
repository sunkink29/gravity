﻿using UnityEngine;
using System.Collections;

public class button : MonoBehaviour , Interactible, PowerProvider {

	Powerable connectedObject;
	bool supplyPower = false;
	public bool toggleButton = true;
    public Animator animator;

	public void interact () {
		supplyPower = !supplyPower;
		if (supplyPower) {
			connectedObject.powerOn (this);
		} else if (toggleButton) {
			connectedObject.powerOff (this);
		}
        animator.SetBool("ButtonPressed", supplyPower);
	}

	public void sendReference(Powerable reference) {
		connectedObject = reference;
	}
}
