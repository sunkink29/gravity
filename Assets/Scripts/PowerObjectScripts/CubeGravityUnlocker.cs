using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGravityUnlocker : PowerObject {

    [SerializeField]
    Transform cubeTargetPosition;
    Collider objectCollider;
    public CubeObject currentCube;

	// Use this for initialization
	new void Start () {
        objectCollider = GetComponent<Collider>();
        if (cubeTargetPosition == null) { cubeTargetPosition = transform; }
        base.Start();
	}

    private void OnCollisionEnter(Collision collision) {
        CubeObject cube = collision.gameObject.GetComponent<CubeObject>();

        if (cube != null && currentCube == null &&
            transform.InverseTransformPoint(collision.contacts[0].point).y > objectCollider.bounds.extents.y) {
            currentCube = cube;
            if (!FirstPersonScript.player.objectPickedUp) {
                teleportCubeToPoint();
            }  else {
                cube.callWhenDropped(teleportCubeToPoint);
            }
        }
    }

    private void OnCollisionExit(Collision collision) {
        CubeObject cube = collision.gameObject.GetComponent<CubeObject>();

        if (cube != null && cube == currentCube) {
            currentCube = null;
        }
    }

    public void teleportCubeToPoint() {
        if (currentCube != null) {
            currentCube.transform.position = cubeTargetPosition.position;
            //currentCube.rb.isKinematic = true;
        }
    }

    public override void changePower(float[] powerArgs) {
        if (powerArgs.Length >= 2 && powerArgs[1] >= 1 && currentCube != null) {
            currentCube.ToggleCubeGravityLock();
        }
    }
}
