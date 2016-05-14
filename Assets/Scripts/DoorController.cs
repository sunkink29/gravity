using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour , Powerable {

	Animator animator;
	bool open;
	[SerializeField] bool doorOpened = false;

	// Use this for initialization
	public virtual void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (doorOpened != open) {
			if (doorOpened) {
				powerOn ();
			} else {
				powerOff ();
			}
		}
	}

	public virtual void OnTriggerEnter (Collider collider) {
		powerOn ();
	}

	public virtual void OnTriggerExit (Collider collider) {
		powerOff ();
	}

	public void powerOn () {
		animateDoor (true);
	}

	public virtual void powerOn (PowerProvider reference) {
		animateDoor (true);
	}

	public void powerOff () {
		animateDoor (false);
	}

	public virtual void powerOff (PowerProvider reference) {
		animateDoor (false);
	}

	void animateDoor (bool state) {
		animator.SetBool ("DoorOpen", state);
		animator.SetBool ("DoorClosed", !state);
		doorOpened = state;
		open = state;
	}
}
