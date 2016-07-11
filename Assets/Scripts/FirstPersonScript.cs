using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

    public class FirstPersonScript : MonoBehaviour
    {

         // the gameobject that holds the camera and any other object to rotate when the camera goes up or down
        public GameObject playerCamera;
        [SerializeField]
        private CameraController cameraRotator = new CameraController();

        // the speed that the player moves at in units per second
        public float speed = 15;

        // the settings for the cube's spring
        [SerializeField]
        public CubeSpringSettings cubeControllerSetting = new CubeSpringSettings();

        // A variable to keep track of whether an object is picked up or not
        bool objectPickedUp = false;

        // the object the is currently picked up or the last object that was picked up
        GameObject liftObject;
        CubeController liftObjectScript;

        // the script for the script that controls the gravity for the player
        public GravityOnNormals gravityOnNormals;

        // A light on the player to light up the area
        Light spotlight;

        public bool rotatePlayer = true;
        public bool debug = false;
        public bool cheatsEnabled = false;
        bool cursorHidden = false;
        bool gamePaused = false;
        Rigidbody playerRigidbody;
        public bool disableMovement = false;
        public static FirstPersonScript player;
        public bool noClipEnabled = false;
        public float maxInteractDistance = 3;
        public KeyCode noClipKey = KeyCode.Backslash;
        Collider playerCollider;
        
        void Awake()
        {
        Application.targetFrameRate = 20;
        // get gravity script
        gravityOnNormals = GetComponent<GravityOnNormals>();

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
    }
        
        // Use this for initialization
        void Start()
        {
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
            if (objectPickedUp)
            {
                liftObject.transform.rotation = transform.rotation;
            }

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

            // check if the player has pressed a button to interact
            //		if (Input.GetButtonDown ("Interact")) {
            //			if (!objectPickedUp) {
            //
            //				// if an object is not pick up call the fuction to pick it up
            //				liftObjects ();
            //			} else {
            //
            //				// if an object is picked up call the fuction to drop it
            //				dropObject ();
            //			}
            //		}

            if (noClipEnabled && Input.GetMouseButtonDown(0))
            {
                RaycastHit hitInfo;
                bool hit = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitInfo);

                if (hit)
                {
                    gravityOnNormals.currentDirection = hitInfo.normal;
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
            if (hit && Input.GetButtonDown("Interact"))
            {

                Interactible interactScript = raycastHit.transform.gameObject.GetComponent<Interactible>();
                Vector3 objectRotation;

                if (hit && raycastHit.transform.gameObject.tag == "liftable")
                {
                    if (!objectPickedUp)
                    {

                        // if an object is not pick up call the fuction to pick it up
                        liftObjects();
                    }
                    else
                    {

                        // if an object is picked up call the fuction to drop it
                        dropObject();
                    }
                }
                else if (interactScript != null)
                {
                    interactScript.interact();
                }
            }
            
            if (cheatsEnabled && hit && Input.GetMouseButtonDown(4))
            {
                Debugable debugScript = raycastHit.transform.gameObject.GetComponent<Debugable>();

                if (debugScript != null) {
                    debugScript.debug();
                }
            }
        }


        // function to lift object
        void liftObjects()
        {

            // raycast to get the object in front of the player
            RaycastHit raycastHit;
            bool hit = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out raycastHit);

            // check if the object is liftable
            if (hit && raycastHit.transform.gameObject.tag == "liftable")
            {

                // set the racast gameObject to a variable and get the cubeController
                liftObject = raycastHit.transform.gameObject;
                liftObjectScript = liftObject.GetComponent<CubeController>();

                // check if the current gravity of the player is the same as the direction
                if (liftObjectScript.gravityDirection == gravityOnNormals.currentDirection)
                {

                    // set the script to not use gravity
                    liftObjectScript.useGravity = false;

                    // set objectPickedUp to true so the player script knows that an object is picked up 
                    objectPickedUp = true;

                    // call a fuction in the object's script to be picked up
                    liftObjectScript.pickUpObject(this);
                }
            }
        }


        // fuction to drop object
        void dropObject()
        {

            // tell the object to use gravity
            liftObjectScript.useGravity = true;

            // set the objects gravity to the players current gravity
            liftObjectScript.gravityDirection = gravityOnNormals.currentDirection;

            // call the fuction in the object's script to drop the object
            liftObjectScript.dropObject();

            // set objectPickedUp to false so the player script knows that an object is not picked up 
            objectPickedUp = false;
        }

        public void pauseGame()
        {
            gamePaused = !gamePaused;
            disableMovement = gamePaused;
            rotatePlayer = !gamePaused;
            lockCursor(!cursorHidden);
        }

        public void toggleNoClip()
        {
            toggleNoClip(false);
        }

        public void toggleNoClip(bool disableAutoRotate)
        {
            noClipEnabled = !noClipEnabled;
            gravityOnNormals.enableGravity = !noClipEnabled;
            playerCollider.enabled = !noClipEnabled;
            if (disableAutoRotate)
            {
                gravityOnNormals.disableAutoRotate = false;
            }
            else
            {
                gravityOnNormals.disableAutoRotate = noClipEnabled;
            }
        }

        public void lockCursor(bool hidden)
        {
            cursorHidden = hidden;
            Cursor.visible = !cursorHidden;
            Cursor.lockState = cursorHidden ? CursorLockMode.Locked : CursorLockMode.None;
        }

        public void toggleCheats()
        {
            cheatsEnabled = !cheatsEnabled;
        }

}
