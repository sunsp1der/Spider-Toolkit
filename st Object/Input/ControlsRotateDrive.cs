// add controls to turn right/left and move forward/back

using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Input/Controls Rotate Drive")]
[RequireComponent(typeof(Rigidbody2D))]


public class ControlsRotateDrive : MonoBehaviour {

	[Tooltip("how fast to turn")]
	public float angularVelocity = 50;  
	public float speed = 5;
	public float backwardSpeed = 5;
	[Tooltip("If true, speed is acceleration not velocity. Rigidbody2D's isKinematic must be false if you want it to slow automatically!")]
	public bool accelerate = false; 
	[Tooltip("input increases/decreases gradually")]
	public bool softenInput = false; 
	[Tooltip("see project settings/input")]
	public string horizontalInput = "Horizontal"; 
	[Tooltip("see project settings/input")]
	public string verticalInput = "Vertical"; 
	
	// Update is called once per frame
	void Update () { 
		float r;
		float y;
		if (softenInput) {
			r = Input.GetAxis(horizontalInput);
			y = Input.GetAxis(verticalInput);
		}
		else {
			r = stTools.GetAxisBool(horizontalInput);
			y = stTools.GetAxisBool(verticalInput);
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
			rigidbody2D.velocity += move * 0.01f;
		}
		else {
			rigidbody2D.velocity = move;
		}
	}
}
