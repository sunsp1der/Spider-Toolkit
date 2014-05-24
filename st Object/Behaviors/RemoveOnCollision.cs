using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Behaviors/Remove On Collision")]


public class RemoveOnCollision : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D info) {
		stTools.Remove(gameObject);
	}
}
