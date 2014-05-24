using UnityEngine;
using System.Collections;

public class SpawnOnRemove : Spawner {

	void OnRemove(){
		if (!enabled) return;
		Spawn ();
	}
}
