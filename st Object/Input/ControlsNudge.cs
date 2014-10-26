using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Input/Controls Nudge")]

/// <summary>
/// Input controls for nudging an object by a set amount
/// </summary>
public class ControlsNudge : MonoBehaviour {
	[Tooltip("Distance to nudge object.")]
	public Vector2 nudgeDistance = new Vector2(3,3);
	[Tooltip("Seconds between nudges when key is held. Zero = one nudge per keypress.")]
	public float interval;
	[Tooltip("Ignore interval when key is pressed repeatedly")]
	public bool intervalOnHoldOnly = true;
	[Tooltip("Keep object within boundaries below")]
	public bool useBoundaries;
	[Tooltip("Nudge boundaries. Will not nudge if it would cross these")]
	public RectOffset boundaries;
	[Tooltip("Change rotation of object to direction of motion")]
	public bool faceMotion = true; 
	[Tooltip("Mirror character when moving left. Good for side views.")]
	public bool flipLeft = false; 
	[Tooltip("See project settings/input")]
	public string horizontalInput = "Horizontal"; 
	[Tooltip("See project settings/input")]
	public string verticalInput = "Vertical"; 

	bool facingRight = true;
	float lastNudge;

	// Update is called once per frame
	void Update () { 

		float x = stTools.GetAxisNorm(horizontalInput);
		float y = stTools.GetAxisNorm(verticalInput);

		// both inputs are 0
		if (x == 0 && y == 0) {
			if (intervalOnHoldOnly) {
				// allow nudging
				lastNudge = 0;
			}
			// do nothing else.
			return;
		}

		// make sure the inputs have been released if interval = 0
		if (interval == 0 && lastNudge != 0) {
			return;
		}
		// only move if nudgeSecs have passed
		else if (interval > 0 && Time.time - lastNudge < interval) {
			return;
		}

		// move nudgeDistance multiplied by inputs
		Vector3 nudge =  new Vector3(x * nudgeDistance.x, y * nudgeDistance.y, 0);
		if (useBoundaries) {
			Vector3 pos = gameObject.transform.position;
			float newx = pos.x + nudge.x;
			if (newx > boundaries.right || newx < boundaries.left) {
				nudge = new Vector3 (0, nudge.y, 0);
			}
			float newy = pos.y + nudge.y;
			if (newy > boundaries.top || newy < boundaries.bottom) {
				nudge = new Vector3 (nudge.x, 0, 0);
			}		
			if (nudge == Vector3.zero) {
				return;
			}
		}

		lastNudge = Time.time;
		gameObject.transform.position += nudge;

		if (faceMotion && nudge.magnitude > 0) {
			transform.rotation = Quaternion.AngleAxis (Mathf.Atan2(nudge.y,nudge.x) * Mathf.Rad2Deg - 90, 
			                                           Vector3.forward);
		}

		if (flipLeft) {
			if (nudge.x < 0 && facingRight) {
				facingRight = false;
				Vector3 s = gameObject.transform.localScale;
				s.x *= -1;
				transform.localScale = s;
			}
			else if (nudge.x > 0 && !facingRight) {
				facingRight = true;
				Vector3 s = gameObject.transform.localScale;
				s.x *= -1;
				transform.localScale = s;
			}
		}
	}
}
