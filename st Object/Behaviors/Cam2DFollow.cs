using UnityEngine;
using System.Collections;

public class Cam2DFollow : MonoBehaviour {
	
	[Tooltip("Camera that should follow this object")]
	public Transform followingCamera;
	[Tooltip("Damping of follow movement")]
	public float damping = 1;
	[Tooltip("How much to look ahead")]
	public float lookAhead = 3;
	[Tooltip("How fast to bounce back from look ahead")]
	public float lookAheadReturnSpeed = 0.5f;
	[Tooltip("How far object can move before follow starts")]
	public float lookAheadMoveThreshold = 0.1f;
	[Tooltip("Follow on X axis")]
	public bool followX = true;
	[Tooltip("Follow on Y axis")]
	public bool followY = false;
	[Tooltip("Follow on Z axis (maintaining original offset)")]
	public bool followZ = false;

	Vector3 offsetZ; // camera's original z distance from object
	float positionZ; // camera's original z location
	Vector3 lastPosition;
	[HideInInspector]
	public Vector3 currentVelocity;
	Vector3 lookAheadOffset; // target 
	
	void Start () {
		lastPosition = transform.position;
		offsetZ = new Vector3(0,0,(followingCamera.position - transform.position).z);
		positionZ = followingCamera.position.z;
		lookAheadOffset = Vector3.zero;
	} 
	
	void Update () {
		Vector3 moveDelta = transform.position - lastPosition; // amount moved

 		// only update look ahead offset if we moved enough
		bool updateOffset = moveDelta.magnitude > lookAheadMoveThreshold;

		float xOffset = 0;
		float yOffset = 0;
		if (updateOffset) {
			if (followX){
				xOffset = lookAhead * Mathf.Sign (moveDelta.x);
			}
			if (followY) {
				yOffset = lookAhead * Mathf.Sign (moveDelta.y);
			}
			lookAheadOffset = new Vector3( xOffset, yOffset, 0);
		}
		else { 
			// if we're not moving, start resetting the offset to zero
			lookAheadOffset = Vector3.MoveTowards(lookAheadOffset, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);	
		}		
		Vector3 targetCameraPos = transform.position + lookAheadOffset;
		if (followZ) {
			targetCameraPos += offsetZ;
		}
		else {
			targetCameraPos = new Vector3 (targetCameraPos.x, targetCameraPos.y, positionZ);
		}
		followingCamera.position = Vector3.SmoothDamp(followingCamera.position, targetCameraPos, ref currentVelocity, damping);
		lastPosition = transform.position;		
	}
	
}
