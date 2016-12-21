using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour , Powerable {

	Animator animator;
	public MonoBehaviour powerProviderMonoBehaviour;
	PowerProvider powerProvider;
	bool open;
	[SerializeField] bool doorOpened = false;

	// Use this for initialization
	public virtual void Start () {
		animator = GetComponent<Animator> ();
		animator.SetBool ("DoorUnlocked",true);
		if (powerProviderMonoBehaviour != null) {
			powerProvider =  (PowerProvider) powerProviderMonoBehaviour;
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
		if (collider.gameObject.GetComponent<CubeController> () == null) {
			animateDoor (true);
		} else {
			animator.SetBool ("DoorAcessDenied", true);
		}
	}

	public virtual void OnTriggerExit (Collider collider) {
		if (collider.gameObject.GetComponent<CubeController> () == null) {
			animateDoor (false);
		} else {
			animator.SetBool ("DoorAcessDenied", false);
		}
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

	public GameObject getGameObject(){
		return gameObject;
	}
}
