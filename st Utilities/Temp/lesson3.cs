using UnityEngine;
using System.Collections;

public class lesson3 : MonoBehaviour {

	public float speed = 1;

	Vector3 origin;
	Vector3 destination;
	float startTime;

	// Use this for initialization
	void Start () {
		MoveTo (new Vector3( 3, 3, transform.position.z), 5);
	}


	// The following standardized summary comments can be created by typing "///" above a function.
	/// <summary>
	/// Moves to a destination point.
	/// </summary>
	/// <param name="dest">Destination.</param>
	/// <param name="speed">Speed multiplier.</param>
	void MoveTo(Vector3 dest, float moveSpeed){
		destination = dest;
		origin = transform.position;
		speed = moveSpeed;
		startTime = Time.time;
	}

	// Update is called once per frame
	void Update () {
		if (origin == destination) {
			DestinationReached();
		}
		else {
			print (Time.time - startTime);
			print ((Time.time - startTime) * speed);
			transform.position = Vector3.Lerp( origin, destination, (Time.time - startTime) * speed);
		}
	}

	void DestinationReached(){
		print ("!");
	}
}
