using UnityEngine;
using System.Collections;

public class SpawnOnCollision : Spawner {

	[Tooltip("Offset spawn location by location of hit")]
	public bool useHitLocation = false;

	void OnCollisionEnter2D( Collision2D info){
		if (!enabled) return;

		if (useHitLocation) {
			// go through each contact point
			foreach (ContactPoint2D contact in info.contacts){
				var spawns = Spawn();
				// go through each object spawned
				foreach (GameObject obj in spawns) {
					// offset locations
					obj.transform.position += (Vector3) contact.point - transform.position;
				}
			}
		}
		else {
			Spawn();
		}
	}
}
