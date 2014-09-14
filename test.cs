using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class test : MonoBehaviour {

	public GameObject spawnObject;
	public MethodButton _CreateObjects;
	public MethodButton _DeleteAll;

	List<GameObject> spawnedObjects = new List<GameObject>();

	// Use this for initialization
	void Start () {
		CreateObjects();
	}

	void CreateObjects() {
		for (int i = 0; i < 10; i++) {
			GameObject ob = stTools.Spawn(spawnObject);
			ob.transform.position = new Vector2 (0, i * 0.3f);
			spawnedObjects.Add (ob);
		}
	}

	void DeleteAll () {
		foreach( GameObject ob in spawnedObjects){
			Destroy(ob);
		}
	}
}
