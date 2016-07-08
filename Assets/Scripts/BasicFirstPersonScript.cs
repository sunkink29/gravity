using UnityEngine;
using System;

[Serializable]
public class InputKeyCodes
{
    public KeyCode pauseGame = KeyCode.P;
    public KeyCode noClipKey = KeyCode.Backslash;
    public KeyCode noClipUp = KeyCode.E;
    public KeyCode noClipDown = KeyCode.Q;
}

public class BasicFirstPersonScript : MonoBehaviour {

    // mass = 10
    // drag = 10
    // gavincarr54@gmail.com

	// the gameobject that holds the camera and any other object to rotate when the camera goes up or down
	public GameObject playerCamera;
	[SerializeField] private CameraController cameraRotator = new CameraController();

	// the speed that the player moves at in units per second
	public float speed = 15;

	public bool disableMovement = false;
    public bool rotatePlayer = true;
    public bool cursorHidden = true;
    bool cursorHiddenInternal = true;
	public bool noClipEnabled = false;
    bool noClipEnabledInternal = false;
    public bool gamePaused = false;
    bool gamePausedInternal = false;
    public static BasicFirstPersonScript player;
    Collider playerCollider;
    Rigidbody playerRigidbody;
    public InputKeyCodes keys = new InputKeyCodes();

	// Use this for initialization
	void Start () {
		// set up the camera rotator
		playerCamera = GameObject.FindWithTag ("MainCamera");
		cameraRotator.Setup (playerCamera.transform,transform);

		// lock and hid the mouse
		if (cursorHidden) {
            lockCursor(true);
		}

		playerRigidbody = GetComponent<Rigidbody> ();
		player = this;
		playerCollider = GetComponent<Collider> ();
	}


	// Update is called once per frame
	void Update () {

		if (rotatePlayer) {
			cameraRotator.rotate ();
		}

		// set the cursor lockstate to locked if it is not locked
		if (Cursor.lockState != CursorLockMode.Locked && cursorHidden) {
            lockCursor(cursorHidden);
		}

        if (cursorHidden != cursorHiddenInternal)
        {
            cursorHidden = cursorHiddenInternal;
            lockCursor(!cursorHidden);
        }

		if (Input.GetKeyDown (keys.pauseGame)) {
            pauseGame();
		}

        if (gamePaused != gamePausedInternal)
        {
            gamePaused = gamePausedInternal;
            pauseGame();
        }

		if (Input.GetKeyDown (keys.noClipKey)) {
			toggleNoClip ();
		}

        if (noClipEnabled != noClipEnabledInternal)
        {
            noClipEnabled = noClipEnabledInternal;
            toggleNoClip();
        }
        
	}

	void FixedUpdate() {
		if (!disableMovement) {
			move ();
		}
	}


	// fuction to move
	void move()
	{
		// get the horizontal and vertical axis inputs
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		float yAxis = 0;
		if (noClipEnabled) {
			yAxis = Input.GetKey(keys.noClipUp) ? 1 : 0;
            yAxis += Input.GetKey(keys.noClipDown) ? -1 : 0;
		}
		Vector3 movement = new Vector3 (horizontal, yAxis, vertical);

		// translate the player acording to the horizontal and vertical axis
		movement = transform.TransformVector(movement);
		playerRigidbody.AddForce (movement * speed, ForceMode.Impulse);
	}

	public void toggleNoClip() {
		noClipEnabled = !noClipEnabled;
        noClipEnabledInternal = noClipEnabled;
		playerRigidbody.useGravity = !noClipEnabled;
		playerCollider.enabled = !noClipEnabled;
	}

    public void pauseGame()
    {
        gamePaused = !gamePaused;
        gamePausedInternal = gamePaused;
        disableMovement = gamePaused;
        rotatePlayer = !gamePaused;
        lockCursor(!cursorHidden);
    }

    public void lockCursor(bool hidden)
    {
        cursorHidden = hidden;
        cursorHiddenInternal = cursorHidden;
        Cursor.visible = !cursorHidden;
        Cursor.lockState = cursorHidden ? CursorLockMode.Locked : CursorLockMode.None;
    }

}
