using UnityEngine;
using System.Collections;

public class RandMove : MonoBehaviour {

	public float moveTime = 1;
	Vector3 startPosition, targetPosition;
	float startTime;

	// Use this for initialization
	void Start () {
		Invoke ("StartMove",0);
	}

	void StartMove() {
		startPosition = transform.position;
		targetPosition = new Vector3( Random.Range( -6.0f, 6.0f), Random.Range (-4.0f, 4.0f), 0);
		startTime = Time.time;
	}

	// Update is called once per frame
	void Update () {
		float elapsedTime = Time.time - startTime;
		float fraction = elapsedTime / moveTime;
		transform.position = Vector3.Lerp( startPosition, targetPosition, fraction);
	}
}
