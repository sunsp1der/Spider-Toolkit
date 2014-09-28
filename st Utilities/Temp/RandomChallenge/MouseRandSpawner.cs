using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseRandSpawner : MouseClickSpawner { 

	public List<GameObject> objects;

	void OnSpawn( GameObject spawnedObject) {
		int objectIndex = Random.Range(0,objects.Count);
		spawnObject = objects[objectIndex];
	}

}
