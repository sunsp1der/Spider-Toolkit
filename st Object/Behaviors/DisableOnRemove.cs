using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Behaviors/Disable On Remove")]
[RequireComponent(typeof(stObject))]
public class DisableOnRemove : MonoBehaviour {
	// When object is removed, stop it and disable it
	// Useful for OnRemove aftereffects, like fade.

	public bool zeroVelocity = true;
	public bool zeroAngularVelocity = true;
	public bool disableCollisions = true;
	public bool disablePhysics = true;
	public MonoBehaviour component = null;

	void OnRemove() {
		if (rigidbody2D != null) {
			if (zeroVelocity ) {
				rigidbody2D.velocity = Vector2.zero;
			}
			if (zeroAngularVelocity) {
				rigidbody2D.angularVelocity = 0;
			}
			if (disablePhysics) {
				rigidbody2D.isKinematic = true;
			}
		}
		if (collider2D != null) {
			if (disableCollisions) {
				collider2D.enabled = false;
			}
		}
		if (component != null) {
			component.enabled = false;
		}
	}
}
