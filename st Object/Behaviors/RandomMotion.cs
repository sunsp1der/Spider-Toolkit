using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Behaviors/Random Motion")]
[RequireComponent(typeof(Rigidbody2D))]

public class RandomMotion : MonoBehaviour {

	public float angle = 0; // Base angle of movement
	public float angleVariance = 180; // Amount angle varies randomly in either direction
	public float velocity = 3; // Base velocity of movement
	public float velocityVariance = 2; // Amount velocity can vary in either direction
	public float angularVelocity = 0; // Base rotation speed 
	public float angularVelocityVariance = 0; // Amount rotation speed can vary in either direction
	public bool rotated = true; // If object is rotated, motion angle will be relative to that rotation
	public bool additive = true; // Add to owner's current velocity and rotate speed values instead of setting them
	public bool alignRotation = true; // Set owner's rotation to its direction of movement

	// Use this for initialization
	void Start () {
		SetMotion();	
	}

	void SetMotion () {
		Vector2 motionVector = Vector2.zero;
		// calculate angle
		float motionAngle = -angle + 90 + Random.Range(-angleVariance,angleVariance);
		if (rotated) {
			motionAngle += gameObject.transform.eulerAngles.z;
		}
		// calculate velocity
		float motionVelocity = velocity + Random.Range(-velocityVariance,velocityVariance);
		motionVector.Set(Mathf.Cos (motionAngle * Mathf.Deg2Rad) * motionVelocity, 
		           Mathf.Sin (motionAngle * Mathf.Deg2Rad) * motionVelocity);
		// calculate rotation speed
		float motionRotation = angularVelocity + Random.Range(-angularVelocityVariance, angularVelocityVariance);
		// set velocity and rotation
		if (additive) {
			rigidbody2D.velocity += motionVector;
			rigidbody2D.angularVelocity -= motionRotation;
		}
		else {
			rigidbody2D.velocity = motionVector;
			rigidbody2D.angularVelocity = -motionRotation;
		}
		if (alignRotation) {
			float r = Mathf.Rad2Deg * Mathf.Atan2(gameObject.rigidbody2D.velocity.y,
			                                gameObject.rigidbody2D.velocity.x);
			transform.rotation = Quaternion.AngleAxis(r - 90, Vector3.forward);
		}
	}
}
