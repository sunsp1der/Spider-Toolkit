using UnityEngine;
using System.Collections;

public class RandomSpawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.position = new Vector2(Random.Range(-6.5f,6.5f),Random.Range (-5,5));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
