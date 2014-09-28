using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class RandSpawner : MonoBehaviour {


	public List<GameObject> objects;
	public MethodButton _SpawnRandom;
	
	// Update is called once per frame
	void SpawnRandom () {
		int objectIndex = Random.Range(0,objects.Count);
		stTools.Spawn( objects[objectIndex]);
	}
}
