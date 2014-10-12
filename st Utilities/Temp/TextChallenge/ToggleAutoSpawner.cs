using UnityEngine;
using System.Collections;

public class ToggleAutoSpawner : MonoBehaviour {

	public GameObject autoSpawner;

	AutoSpawner spawner;

	void Awake(){
		spawner = autoSpawner.GetComponent<AutoSpawner>();
	}

	void OnClick(){

		if (spawner.isSpawning) {
			spawner.StopSpawning();
		}
		else {
			spawner.StartSpawning();
		}
		
	}
}
