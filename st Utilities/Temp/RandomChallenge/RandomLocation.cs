using UnityEngine;
using System.Collections; 

public class RandomLocation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.position = new Vector3( Random.Range( -6.0f, 6.0f), Random.Range (-4.0f, 4.0f), 0);
	}

}
