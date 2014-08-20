// deal damage to object collided with if that object has
// TakesDamage component

using UnityEngine;
using System.Collections;

public class DealsDamage : MonoBehaviour {

	public int amount = 100;
	public bool removeOnDamage = true;
	public bool removeOnCollide = false;
	public bool ondealdamageCallback = false;

	void OnCollisionEnter2D (Collision2D info) {
		if (!enabled) return;
		DealDamage (info.gameObject);
		if (removeOnCollide) {
			stTools.Remove(gameObject,0);
		}
	}

	// just here to create enabled checkbox
	void Start () {
	}

	bool DealDamage (GameObject target, int damage_amount = 0) {
		// if amount is 0, use 
		TakesDamage takesDamage = target.GetComponent<TakesDamage>();
		if (takesDamage == null) {
			return false;
		}
		if (damage_amount == 0) {
			damage_amount = amount;
		}
		if (ondealdamageCallback) {
			SendMessage("OnDealDamage", target, SendMessageOptions.DontRequireReceiver);
		}
		takesDamage.TakeDamage( damage_amount, gameObject);
		if (removeOnDamage) {
			stTools.Remove(gameObject,0);
		}
		return true;
	}
}
