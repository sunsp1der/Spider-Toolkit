using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Behaviors/Face Object")]

public class FaceObject : MonoBehaviour {

	public string targetName = "Player"; // name of the object to be faced
	public float rotationSpeed = 50; // negative = always face object exactly
	public float offset = 0; // offset rotation by this much

	GameObject target = null;

	void FindTarget(){
		target = GameObject.Find (targetName);
	}

	void Awake () {
		FindTarget ();
	}

	public void Update () {
		if (!target) {
			FindTarget ();
		}
		if (target.activeSelf) { // if target is active...
			// figure out target rotation
			Vector3 dir = target.transform.position - transform.position;
			float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
			Quaternion targetRotation = Quaternion.AngleAxis(angle - 90 - offset, Vector3.forward);

			if (rotationSpeed < 0) {
				// negative speed means lock to target rotation
				transform.rotation = targetRotation;
			}
			else {
				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
			}
		}
	}
}
