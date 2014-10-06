// add up/down/left/right controls

using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Input/Controls Platformer")]
[RequireComponent(typeof(Rigidbody2D))]

public class ControlsPlatformer : MonoBehaviour {

	[Tooltip("Horizontal speed")]
	public float speed = 3;
	[Tooltip("Jump force")]
	public float jumpForce = 400;
	[Tooltip("Input increases/decreases gradually")]
	public bool softenInput = false;
	[Tooltip("Name of layer that ground objects are in")]
	public string groundLayer = "Collide All";
	[Tooltip("See project settings/input")]
	public string horizontalInput = "Horizontal"; 
	[Tooltip("See project settings/input")]
	public string jumpInput = "Jump"; 
	 
	bool facingRight = true;
	Vector3 groundVector; // distance from center to the ground under object
	Vector3 rightVector; // distance from center to just right object
	public float test;
	void Start () {
		// calculate groundVector based on sprite's size
		Vector3 min = gameObject.GetComponent<SpriteRenderer>().bounds.min;
		Vector3 max = gameObject.GetComponent<SpriteRenderer>().bounds.max;
		groundVector = new Vector3 (0, (max.y - min.y) * 0.5f + 0.03f);
		rightVector = new Vector3(0, (max.x - min.x) * 0.5f + test);
		print (rightVector);
	}

	// Update is called once per frame
	void Update () { 
		// jump
		bool jump = Input.GetButtonDown(jumpInput);
		if (jump) {
			Vector2 groundPos = (Vector2) (transform.position - groundVector);
			// Use Linecast to see if there is ground under the player
			if (Physics2D.Linecast(transform.position, groundPos, 1 << LayerMask.NameToLayer(groundLayer))) {
				rigidbody2D.AddForce(  new Vector2(0, jumpForce)); 
			}
		}

		// horizontal move
		float x;
		if (softenInput) {
			x = Input.GetAxis(horizontalInput);
		}
		else {
			x = stTools.GetAxisBool(horizontalInput);
		}
		if (x > 0 && Physics2D.Linecast(transform.position, transform.position - rightVector, 
		                                1 << LayerMask.NameToLayer(groundLayer))) {
			x = 0;
			print ("!");
		}
		if (x < 0 && Physics2D.Linecast(transform.position, transform.position + rightVector, 
		                                1 << LayerMask.NameToLayer(groundLayer))) {
			x = 0;
			print("!2");
		}
		Vector2 v = new Vector2(x * speed, rigidbody2D.velocity.y);
		rigidbody2D.velocity = v;
		rigidbody2D.angularVelocity = 0;

		//facing
		if (v.x < 0) {
			if (facingRight) {
				facingRight = false;
				Vector3 s = gameObject.transform.localScale;
				s.x *= -1;
				transform.localScale = s;
			}
		}
		else if (v.x > 0) {
			if (!facingRight) {
				facingRight = true;
				Vector3 s = gameObject.transform.localScale;
				s.x *= -1;
				transform.localScale = s;
			}
		}
	}
}
