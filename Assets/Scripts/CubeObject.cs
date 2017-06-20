using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeObject : LiftablePowerObject {

    Animator animator;
    bool cubeLocked { get { return objectGravity.fixedGravity; } set { objectGravity.fixedGravity = value; } }

	// Use this for initialization
	new void Start () {
        base.Start();
        animator = GetComponent<Animator>();
        if (animator != null)
            animator.SetBool("gravityLock", cubeLocked);
    }
	
	// Update is called once per frame
	new void Update () {
        base.Update();
	}

    public void ToggleCubeGravityLock() {
        cubeLocked = !cubeLocked;
        if (animator != null)
            animator.SetBool("gravityLock", cubeLocked);
    }
}
