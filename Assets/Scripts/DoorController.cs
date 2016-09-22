using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour , Powerable {

	Animator animator;
	public GameObject powerProviderGameObject;
	PowerProvider powerProvider;
	bool open;
	[SerializeField] bool doorOpened = false;

	// Use this for initialization
	public virtual void Start () {
		animator = GetComponent<Animator> ();
		animator.SetBool ("DoorUnlocked",true);
		if (powerProviderGameObject != null) {
			powerProvider = powerProviderGameObject.GetComponent<PowerProvider> ();
			if (powerProvider != null) {
				powerProvider.sendReference (this);
				animator.SetBool ("DoorUnlocked",false);
			}
		}
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (doorOpened != open) {
			if (doorOpened) {
				animateDoor (true);
			} else {
				animateDoor (false);
			}
		}
	}

	public virtual void OnTriggerEnter (Collider collider) {
		animateDoor (true);
	}

	public virtual void OnTriggerExit (Collider collider) {
		animateDoor (false);
	}

	public void powerOn () {
		animator.SetBool ("DoorUnlocked",true);
	}

	public virtual void powerOn (PowerProvider reference) {
		animator.SetBool ("DoorUnlocked",true);
	}

	public void powerOff () {
		animator.SetBool ("DoorUnlocked",false);
	}

	public virtual void powerOff (PowerProvider reference) {
		animator.SetBool ("DoorUnlocked",false);
	}

	public virtual void changePower(float[] powerArgs) {
		bool doorUnlocked;
		if (powerArgs.Length >= 2 && powerArgs [1] >= 1) {
			doorUnlocked = true;
		} else {
			doorUnlocked = false;
		}
		animator.SetBool ("DoorUnlocked", doorUnlocked);
	}

	void animateDoor (bool state) {
		animator.SetBool ("DoorOpen", state);
		animator.SetBool ("DoorClosed", !state);
		doorOpened = state;
		open = state;
	}
}
