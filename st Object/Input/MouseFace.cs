using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Input/Mouse Face")]

public class MouseFace : MonoBehaviour{

	[Tooltip("negative = always face object exactly")]
	public float angularVelocity = 50;
	[Tooltip("Add this to rotation angle")]
	public float offset = 0; 
	[Tooltip("Follow when mouse is not moving")]
	public bool followWhenStill = false;

	Vector3 oldLocation;

	void Start() {
		oldLocation = new Vector3();
	}

	void Update () {
		if (!Input.mousePresent) return;
		// figure out target rotation
		Vector3 mouseLocation = Input.mousePosition;
		if (!followWhenStill && mouseLocation == oldLocation) {
			return;
		}
		oldLocation = mouseLocation;
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