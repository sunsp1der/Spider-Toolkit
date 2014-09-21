using UnityEngine;
using System.Collections;

public class MatchSpawnerColor : MonoBehaviour {

	void OnSpawned( Spawner mySpawner) {
		GetComponent<SpriteRenderer>().color = mySpawner.GetComponent<SpriteRenderer>().color;
	}	
}
