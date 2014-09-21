using UnityEngine;
using System.Collections;

public class SpawnedColorMatch : MonoBehaviour {

	void OnSpawn( GameObject mySpawn) {
		mySpawn.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
	}
}
