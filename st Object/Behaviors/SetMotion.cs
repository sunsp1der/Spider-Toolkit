using UnityEngine;
using System.Collections;
[AddComponentMenu("st Object/Behaviors/Set Motion")]
[RequireComponent(typeof(Rigidbody2D))]

public class SetMotion : MonoBehaviour {
	// Set object's velocity and/or angular velocity when spawned

	[Tooltip("Speed of linear velocity")]
	public float speed = 1; 
	[Tooltip("Angle of linear velocity")]
	public float angle = 0; 
	public float angularVelocity = 0;
	[Tooltip("Set motion relative to object rotation")]
	public bool rotated = true; // If owner is already rotated when motion is set,
								// velocity and acceleration will be relative to 
								// that rotation
	[Tooltip("Add to current values")]
	public bool additive = true; // Add to owner's values instead of setting them

	void Start () {
		DoSetMotion();
	}
	
	public void DoSetMotion () {
		float moveangle = -angle + 90;
		if (rotated) {
			moveangle += gameObject.transform.eulerAngles.z;
		}
		moveangle *= Mathf.Deg2Rad;
		Vector2 velocityVector = new Vector2(Mathf.Cos(moveangle) * speed,
		                                     Mathf.Sin(moveangle) * speed);
		//velocityVector = Vector2.
		if (additive) {
			gameObject.rigidbody2D.velocity += velocityVector;
			gameObject.rigidbody2D.angularVelocity += angularVelocity;
		}
		else {
			gameObject.rigidbody2D.velocity = velocityVector;
			gameObject.rigidbody2D.angularVelocity = angularVelocity;
		}
	}
}
