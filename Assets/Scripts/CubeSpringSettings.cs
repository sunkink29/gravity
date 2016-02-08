using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CubeSpringSettings {
	public float distanceFromObject = 4;
	public float spring = 200;
	public float damper = 5;
	public float rigidbodyDrag = 15;
	public float rigidbodyAnglarDrag = 15;
	public float force = 15;
}
