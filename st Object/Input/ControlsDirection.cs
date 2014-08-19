// add up/down/left/right controls

using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Input/Controls Direction")]
[RequireComponent(typeof(Rigidbody2D))]

public class ControlsDirection : MonoBehaviour {

	[Tooltip("Speed in each direction. Use zero to limit motion to one axis.")]
	public Vector2 moveVelocity = new Vector2(3,3);
	[Tooltip("Change rotation of object to direction of motion")]
	public bool faceMotion = true; 
	[Tooltip("Mirror character when moving left. Good for side views.")]
	public bool flipLeft = false; 
	[Tooltip("Input increases/decreases gradually")]
	public bool softenInput = false; 
	[Tooltip("See project settings/input")]
	public string horizontalInput = "Horizontal"; 
	[Tooltip("See project settings/input")]
	public string verticalInput = "Vertical"; 

	bool facingRight = true;

	// Update is called once per frame
	void Update () { 
		float x;
		float y;
		if (softenInput) {
			x = Input.GetAxis(horizontalInput);
			y = Input.GetAxis(verticalInput);
		}
		else {
			x = stTools.GetAxisBool(horizontalInput);
			y = stTools.GetAxisBool(verticalInput);
		}

		Vector2 v =  new Vector2( x * moveVelocity.x, y * moveVelocity.y);
		rigidbody2D.velocity = v;

		rigidbody2D.angularVelocity = 0;

		if (faceMotion && v.magnitude > 0) {
			transform.rotation = Quaternion.AngleAxis (Mathf.Atan2(v.y,v.x) * Mathf.Rad2Deg - 90, Vector3.forward);
		}

		if (flipLeft) {
			if (v.x < 0 && facingRight) {
				facingRight = false;
				Vector3 s = gameObject.transform.localScale;
				s.x *= -1;
				transform.localScale = s;
			}
			else if (v.x > 0 && !facingRight) {
				facingRight = true;
				Vector3 s = gameObject.transform.localScale;
				s.x *= -1;
				transform.localScale = s;
			}
		}
	}
}
