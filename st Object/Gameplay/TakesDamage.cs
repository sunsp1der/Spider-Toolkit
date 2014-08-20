using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Gameplay/Takes Damage")]
[RequireComponent(typeof(SpriteRenderer))]

public class TakesDamage : MonoBehaviour {

	public int startHealth = 100; // amount of damage the object can take
	public bool removeAt0 = true; // remove at zero health
	public bool doDamageTint = true;
	public Color tintColor = Color.red;
	public float invincibleTime = 0; // after spawning, this object will not
	                                 // take damage for this many seconds
	public bool ontakedamageCallback;

	[HideInInspector]
	public int health;
	public LockedView _health;
	[HideInInspector]
	public bool invincible = false;
	public LockedView _invincible;

	Color startTint;
	SpriteRenderer spriteRenderer;

	void Awake(){
		health = startHealth;
		spriteRenderer = GetComponent<SpriteRenderer>();
		startTint = spriteRenderer.color;
	}

	void Start () {
		if (invincibleTime > 0) {
			invincible = true;
			Invoke ("MakeVincible", invincibleTime);
		}
	}

	void MakeVincible() {
		invincible = false;
	}

	public void TakeDamage( int amount, GameObject damager=null) {
		if (invincible || !enabled) return;
		health -= amount;
		if (ontakedamageCallback) {
			SendMessage("OnTakeDamage", damager);
		}
		if (doDamageTint) {
			spriteRenderer.color = Color.Lerp( tintColor, startTint, (float)health / startHealth);
		}
		if (health <= 0 && removeAt0) {
			stTools.Remove(gameObject, 0);
		}
	}

}
