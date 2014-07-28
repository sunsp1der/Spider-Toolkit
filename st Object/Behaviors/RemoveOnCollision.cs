// remove this object when something collides with it

using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Behaviors/Remove On Collision")]

public class RemoveOnCollision : MonoBehaviour {

	[Tooltip("Minimum difference in velocity to count as hit")]
	public float minMagnitude = 1;
	
	void Start () {
	}
	
	void OnCollisionEnter2D (Collision2D info) {
		if (enabled) {
			if (info.relativeVelocity.magnitude > minMagnitude) {
				stTools.Remove(gameObject);
			}
		}
	}
}
