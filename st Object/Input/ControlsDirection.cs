using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Input/Controls Direction")]
[RequireComponent(typeof(Rigidbody2D))]

public class ControlsDirection : MonoBehaviour {

	public Vector2 moveVelocity = new Vector2(3,3);
	public bool faceMotion = true; // change rotation to direction of motion
	public bool flipLeft = false; // mirror character when moving left 
	public bool softenInput = false; // input increases/decreases gradually							       // good for side view
	public string horizontalInput = "Horizontal"; // see project settings/input
	public string verticalInput = "Vertical"; // see project settings/input

	bool facingRight = true;

	// Update is called once per frame
	void Update () { 
		float x = Input.GetAxis(horizontalInput);
		float y = Input.GetAxis(verticalInput);

		if (!softenInput) {
			if (!Input.GetButton(horizontalInput)) {
				x = 0;
			}
			else if (x > 0) {
				x = 1;
			}
			else if (x < 0) {
				x = -1;
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

		Vector2 v =  new Vector2( x * moveVelocity.x, y * moveVelocity.y);
		gameObject.rigidbody2D.velocity = v;

		gameObject.rigidbody2D.angularVelocity = 0;

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
