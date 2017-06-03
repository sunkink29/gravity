using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gravity))]
public class ChangeGravityToNormals : MonoBehaviour {

    public Gravity objectGravity;
    Collider objectCollider;
    Vector3 lasthitDirection;
    public bool rotateObject = true;
    private bool resetRotation;
    private float pointInRotation;
    public float angleSpeed = 0.05f;
    private float angleDistance;
    Quaternion lastRotation;
    Quaternion targetQuaternion;

    private void Awake() {
        objectCollider = GetComponent<Collider>();
        objectGravity = GetComponent<Gravity>();
    }

    void FixedUpdate () {
        rayCastGround();
        if (rotateObject) {
            rotate();
        }
	}

    void rayCastGround() {
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(transform.position, objectGravity.currentDirection, out hitInfo);
        
        if (hit) {
            Bounds objectBounds = objectCollider.bounds;
            if (hitInfo.distance < objectBounds.extents.y + 5) {
                lasthitDirection = objectGravity.currentDirection;
                objectGravity.currentDirection = -hitInfo.normal;
            }

            if (lasthitDirection != objectGravity.currentDirection) {
                resetRotation = true;
            }
        }

    }

    void rotate() {
        if (-transform.up != objectGravity.currentDirection) {
            if (resetRotation) {
                pointInRotation = 0;
                angleDistance = Vector3.Angle(-transform.up, objectGravity.currentDirection);
                targetQuaternion = Quaternion.FromToRotation(-transform.up, objectGravity.currentDirection) * transform.rotation;
                lastRotation = transform.rotation;
                resetRotation = false;
            }

            pointInRotation += angleSpeed * Time.fixedDeltaTime / angleDistance;

            transform.rotation = Quaternion.Slerp(lastRotation, targetQuaternion, pointInRotation);
        }
    }

    public void resetObjectRotation() {
        resetRotation = true;
    }
}
