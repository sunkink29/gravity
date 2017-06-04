using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class FirstPersonScript : MonoBehaviour, FindPropertys
{

     // the gameobject that holds the camera and any other object to rotate when the camera goes up or down
    public GameObject playerCamera;
    [SerializeField]
    public CameraController cameraRotator = new CameraController();

    // the speed that the player moves at in units per second
    public float speed = 15;

    // the settings for the cube
    [SerializeField]
    public LiftableObjectSettings liftableObjectSettings = new LiftableObjectSettings();

    // A variable to keep track of whether an object is picked up or not
    public bool objectPickedUp = false;

    // the object the is currently picked up or the last object that was picked up
    public LiftableObject liftableObject;

    // the script for the script that controls the gravity for the player
    [HideInInspector]
    public ChangeGravityToNormals changeGravity;

    // A light on the player to light up the area
    Light spotlight;

    List<Action> fuctionCallWhenFloorCollisionChange = new List<Action>();
    public bool isCollidingFloor = false;
    public bool rotatePlayer = true;
    public bool debug = false;
    public bool cheatsEnabled = false;
    bool cursorHidden = false;
    public bool gamePaused = false;
    Rigidbody playerRigidbody;
    public bool disableMovement = false;
    public static FirstPersonScript player;
    public bool noClipEnabled = false;
    public float maxInteractDistance = 3;
    public KeyCode noClipKey = KeyCode.Backslash;
    Collider playerCollider;
    public GameObject reticle;
    bool debugInteractEnabled = false;
    DebugType debugType;
    object[] debugArgs;
    string[] propertys = new string[] { "gravityStrength", "speed"};

    void Awake()
    {
        //Application.targetFrameRate = 20;
        // get gravity script
        changeGravity = GetComponent<ChangeGravityToNormals>();

	    // set up the camera rotator
	    playerCamera = GameObject.FindWithTag("MainCamera");
	    cameraRotator.Setup(playerCamera.transform, transform);

	    if (Application.isEditor)
	    {
	        cheatsEnabled = true;
	    }

	    // get the spot light
	    spotlight = GameObject.FindGameObjectWithTag("spot light").GetComponent<Light>();
	    spotlight.gameObject.SetActive(false);

	    playerRigidbody = GetComponent<Rigidbody>();

	    player = this;
	    playerCollider = GetComponent<Collider>();

	    // lock and hid the mouse
	    if (!cursorHidden)
	    {
	        lockCursor(true);
	    }
	}

    // Update is called once per frame
    void Update()
    {
        // call the move and rotate helper fuctions
        //		move ();

        checkInteraction();

        if (rotatePlayer && !disableMovement)
        {
            cameraRotator.rotate();
        }

        // call or set anything that needs to be done while holding an object
//        if (objectPickedUp)
//        {
//            liftObject.transform.rotation = transform.rotation;
//        }

        // set the cursor lockstate to locked if it is not locked
        if (!(Cursor.lockState == CursorLockMode.Locked))
        {
            lockCursor(cursorHidden);
        }

        if (Input.GetButtonDown("pause game"))
        {
            pauseGame();
        }

        if (Input.GetButtonDown("Toggle Light"))
        {
            spotlight.gameObject.SetActive(!spotlight.gameObject.activeInHierarchy);
        }

        if (Input.GetKeyDown(KeyCode.Mouse3))
        {
            toggleCheats(); 
        }

        if (cheatsEnabled && Input.GetKeyDown(noClipKey) || (!cheatsEnabled && noClipEnabled))
        {
            toggleNoClip();
        }
			
        if (noClipEnabled && Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            bool hit = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitInfo);

            if (hit)
            {
                changeGravity.objectGravity.currentDirection = -hitInfo.normal;
                changeGravity.resetObjectRotation();
            }
        }
    }

    void FixedUpdate()
    {
        if (!disableMovement)
        {
            move();
        }
    }

    void OnCollisionEnter(Collision collisionInfo) {
        CapsuleCollider playerCollider = GetComponent<CapsuleCollider>();
        if (Physics.Raycast (transform.position, transform.up * -1, playerCollider.height / 2 + playerCollider.radius + 0.5f)) {
            if (!isCollidingFloor) {
                for (int i = 0; i < fuctionCallWhenFloorCollisionChange.Count; i++) {
                    fuctionCallWhenFloorCollisionChange[i](!isCollidingFloor);
                }
            }
            isCollidingFloor = true;
        }
    }

    void OnCollisionExit(Collision collisionInfo) {
        CapsuleCollider playerCollider = GetComponent<CapsuleCollider>();
        if (!Physics.Raycast (transform.position, transform.up * -1, playerCollider.height / 2 + playerCollider.radius + 0.5f)) {
            isCollidingFloor = false;
            for (int i = 0; i < fuctionCallWhenFloorCollisionChange.Count; i++) {
                fuctionCallWhenFloorCollisionChange[i](isCollidingFloor);
            }
        }
    }

    public void CallWhenCollisionChange(Action action, bool addDelegete) {
        if (addDelegete) {
            fuctionCallWhenFloorCollisionChange.Add(action);
        } else {
            fuctionCallWhenFloorCollisionChange.Remove(action);
        }
    }

    // fuction to move
    void move()
    {
        // get the horizontal and vertical axis inputs
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");
        float yAxis = 0;
        float movementModifier = 1;
        if (noClipEnabled)
        {
            yAxis = Input.GetAxis("yAxis");
            movementModifier = 2;
        }
        Vector3 movement = new Vector3(horizontal, yAxis, vertical);

        if (movementModifier != 1 && Input.GetButtonDown("speedModifer"))
        {
            movement = movement * movementModifier;
        }

        // translate the player acording to the horizontal and vertical axis
        movement = transform.TransformVector(movement);
        //		playerRigidbody.MovePosition (gameObject.transform.position + movement * speed * Time.fixedDeltaTime);
        playerRigidbody.AddForce(movement * speed, ForceMode.Impulse);
    }

    void checkInteraction()
    {
        RaycastHit raycastHit;
        bool hit = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out raycastHit, maxInteractDistance);

		Interactible interactScript = null;
		if (hit) {
			interactScript = raycastHit.transform.gameObject.GetComponent<Interactible>();
		}
		if ( interactScript != null) {
			Color color = new Color (1, 1, 1, 1);
			reticle.GetComponent<Image> ().color = color;
		} else {
			Color color = new Color (1, 1, 1, 0.4f);
			reticle.GetComponent<Image> ().color = color;
		}
		if (Input.GetButtonDown ("Interact")) {
			if (objectPickedUp) {
				liftableObject.interact ();
			} else if (hit) {
				if (interactScript != null) {
					interactScript.interact ();
				}
			}
		}
        
        if (cheatsEnabled && hit && Input.GetKeyDown(KeyCode.F) && debugInteractEnabled)
        {
            if (debugType == DebugType.Power) {
                float[] args = new float[debugArgs.Length];
                for (int i = 0; i < args.Length; i++) {
                    args[i] = float.Parse((String)debugArgs[i]);
                }
                PowerObject powerScript = raycastHit.transform.gameObject.GetComponent<PowerObject>();

                if (powerScript != null) {
                    powerScript.changePower(args);
                }
            }
        }
    }

    public void enableDisableDebugingInteract(bool enable, DebugType debugType, params System.Object[] args) {
        debugInteractEnabled = enable;
        this.debugType = debugType;
        debugArgs = args;
    }

    public void pauseGame() {
        gamePaused = !gamePaused;
        disableMovement = gamePaused;
        rotatePlayer = !gamePaused;
        lockCursor(!cursorHidden);
        reticle.SetActive(!gamePaused);
    }

    public void toggleNoClip() {
        toggleNoClip(!noClipEnabled,false);
    }

    public void toggleNoClip(bool enable,bool disableAutoRotate) {
        noClipEnabled = enable;
        changeGravity.objectGravity.enabled = !noClipEnabled;
        playerCollider.enabled = !noClipEnabled;
        if (disableAutoRotate) {
            //gravityOnNormals.disableAutoRotate = false;
        }
        else {
            //gravityOnNormals.disableAutoRotate = noClipEnabled;
        }
    }

    public void lockCursor(bool hidden) {
        cursorHidden = hidden;
        Cursor.visible = !cursorHidden;
        Cursor.lockState = cursorHidden ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void toggleCheats() {
        toggleCheats(!cheatsEnabled);
    }

    public void toggleCheats(bool enable) {
        cheatsEnabled = enable;
    }

    public int findPropertyIndex(string property) {
        for (int i = 0; i < propertys.Length; i++) {
            if (propertys[i].Equals(property)) {
                return i;
            }
        }
        return -1;
    }

    public bool hasProperty(string property) {
        int propertyIndex = findPropertyIndex(property);
        if (propertyIndex == -1) {
            return false;
        }
        return true;
    }

    public void changeProperty(string property, string[] propertyValue) {
        int propertyIndex = findPropertyIndex(property);
        switch (propertyIndex) {
            case 0:
                changeGravity.objectGravity.gravityStrength = float.Parse(propertyValue[0]);
                break;
            case 1:
                speed = float.Parse(propertyValue[0]);
                break;
        }

    }

    public string getName() {
        return name;
    }

}
public enum DebugType { Power }

public delegate void Action(bool isColliding);
