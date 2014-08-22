
using UnityEngine;
using System.Collections;

// always face current direction of movement
public class FaceMotion : MonoBehaviour {
	
	[Tooltip("Only change rotation if velocity is higher than this")]
	public float minVelocity = 0;

	void Update () {
		Vector2 v =  rigidbody2D.velocity;
		if (v.magnitude > minVelocity) {
			transform.rotation = Quaternion.AngleAxis (Mathf.Atan2(v.y,v.x) * Mathf.Rad2Deg - 90, Vector3.forward);
		}
	}
}
