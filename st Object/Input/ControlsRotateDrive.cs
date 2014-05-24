using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Input/Controls Rotate Drive")]
[RequireComponent(typeof(Rigidbody2D))]


public class ControlsRotateDrive : MonoBehaviour {
	
	public float angularVelocity = 50; // how fast to turn
	public float speed = 5;
	public float backwardSpeed = 5;
	public bool accelerate = false; // if true, speed is acceleration not velocity
	public bool softenInput = false; // input increases/decreases gradually
	public string horizontalInput = "Horizontal"; // see project settings/input
	public string verticalInput = "Vertical"; // see project settings/input
	
	// Update is called once per frame
	void Update () { 
		float r = Input.GetAxis(horizontalInput);
		float y = Input.GetAxis(verticalInput);

		if (!softenInput) {
			if (!Input.GetButton(horizontalInput)) {
				r = 0;
			}
			else if (r > 0) {
				r = 1;
			}
			else if (r < 0) {
				r = -1;
			}
			if (!Input.GetButton(verticalInput)) {
				y = 0;
			}
			if (y > 0) {
				y = 1;
			}
			else if (y < 0) {
				y = -1;
			}
		}

		gameObject.rigidbody2D.angularVelocity = -r * angularVelocity;

		float a = 0.0f;
		//gameObject.transform.rotation.ToAngleAxis(out a, out axis);
		a = (transform.eulerAngles.z + 90) * Mathf.Deg2Rad;
		Vector2 move = Vector2.zero;
		if (y > 0){
			move = new Vector2(Mathf.Cos(a) * speed, Mathf.Sin(a) * speed);
		}
		else if (y < 0) {
			move = new Vector2(Mathf.Cos(a) * -backwardSpeed, Mathf.Sin(a) * -backwardSpeed);
		}
		if (accelerate) {
			rigidbody2D.AddForce( move);
		}
		else {
			rigidbody2D.velocity = move;
		}
	}
}
