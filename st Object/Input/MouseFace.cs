using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Input/Mouse Face")]

public class MouseFace : MonoBehaviour{

	public float angularVelocity = 50; // negative = always face object exactly
	public float offset = 0; // offset rotation by this much
	public bool followOffscreen = true; // if true, point at last known mouse position

	public void Update () {
		if (!followOffscreen && !Input.mousePresent) return;
		// figure out target rotation
		Vector3 mouseLocation = Input.mousePosition;
		mouseLocation.z = -Camera.main.transform.position.z;
		mouseLocation = Camera.main.ScreenToWorldPoint(mouseLocation);
		Vector3 difference = mouseLocation - transform.position;
		float angle = Mathf.Atan2(difference.y,difference.x) * Mathf.Rad2Deg;
		Quaternion targetRotation = Quaternion.AngleAxis(angle - 90 - offset, Vector3.forward);

		if (angularVelocity < 0) {
			// negative speed means lock to target rotation
			transform.rotation = targetRotation;
		}
		else {
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angularVelocity * Time.deltaTime);
		}
	}
}