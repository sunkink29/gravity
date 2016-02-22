using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour , Powerable {

	Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void powerOn () {
			animator.SetBool ("DoorOpen", true);
			animator.SetBool ("DoorClosed", false);
	}

	public void powerOff () {
			animator.SetBool ("DoorOpen", false);
			animator.SetBool ("DoorClosed", true);
	}
}
