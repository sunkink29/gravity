using UnityEngine;

public class GravityPad : PowerObject, FindPropertys {

    FirstPersonScript player;
    public LightStripControllerPowerObject powerLight;
    public Vector3 gravityDirection;
    public bool powered = false;
    public bool playerOnGround = false;
    public bool playerInTrigger = false;
    float defaultGravity = 0;
    float gravity = 0;
    Collider playerCollider;

    string[] propertys = {"power"};

    public override void Start() {
        base.Start();
        player = FirstPersonScript.player;
        defaultGravity = player.gravityOnNormals.gravity;
        gravity = defaultGravity;
        playerCollider = player.GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider collider) {
        if (collider == playerCollider) {
            playerInTrigger = true;
            playerOnGround = player.isCollidingFloor;
            checkIfPlayerIsInTriggerAndOnGround();
            player.CallWhenCollisionChange(collisionChange,true);
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider == playerCollider) {
            playerInTrigger = false;
            player.CallWhenCollisionChange(collisionChange,false);
        }
    }

    public override void changePower(float[] powerArgs) {
        powerArgs[0] = GetInstanceID();
		powerLight.changePower(powerArgs);
        if (powerArgs.Length >= 2 && powerArgs[1] > 0) {
            powered = true;
            defaultGravity = player.gravityOnNormals.gravity;
            gravity = powerArgs[1] * defaultGravity;
        } else {
            powered = false;
        }
	}

    public void collisionChange(bool isColliding) {
        playerOnGround = isColliding;
        checkIfPlayerIsInTriggerAndOnGround();
    }

    void checkIfPlayerIsInTriggerAndOnGround() {
        if (!playerOnGround && playerInTrigger && powered) {
            player.gravityOnNormals.turnToVector(gravityDirection);
        }
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
				float[] propertyValueFloat = ConsoleCommandRouter.convertStringArrayToFloat(propertyValue);
				changePower(propertyValueFloat);
				break;
        }
	}

	public string getName() {
        return name;
    }
}