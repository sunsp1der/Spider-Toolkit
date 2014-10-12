// remove this object when something collides with it

using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Behaviors/Remove On Collision")]

public class RemoveOnCollision : MonoBehaviour {

	[Tooltip("Minimum difference in velocity to count as hit")]
	public float minMagnitude = 0;
	public bool doRemoveEffects = true;
	
	void Start () {
		// only included to show enabled checkbox in editor
	}
	
	void OnCollisionEnter2D (Collision2D info) {
		if (enabled) {
			if (info.relativeVelocity.magnitude >= minMagnitude) {
				Invoke ("Remove",0); // we delay this call so that the other object will get collision callback
			}
		}
	}

	void Remove(){
		if (doRemoveEffects) {
			stTools.Remove(gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}
}
