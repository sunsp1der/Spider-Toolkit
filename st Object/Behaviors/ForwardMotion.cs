using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Behaviors/Forward Motion")]
[RequireComponent(typeof(Rigidbody2D))]

public class ForwardMotion : MonoBehaviour {
	// Maintains a constant state of forward motion

	public float speed = 2;
	public bool accelerate = false; // if true, speed indicated acceleration, not velocity

	float lastRotation = 0;
	Vector2 move;

	// Use this for initialization
	void Start () {
		SetMotion();
	}

	// Update is called once per frame
	void Update () {
		if (accelerate || transform.eulerAngles.z != lastRotation) {
			SetMotion();
		}
		if (accelerate) {
			rigidbody2D.AddForce( move);
		}
		else {
			rigidbody2D.velocity = move;
		}
	}

	void SetMotion() {
		float angle = (transform.eulerAngles.z + 90) * Mathf.Deg2Rad;
		move = new Vector2(Mathf.Cos(angle) * speed, Mathf.Sin(angle) * speed);
		lastRotation = transform.eulerAngles.z;
	}
}
