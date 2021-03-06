﻿using UnityEngine;
using System.Collections;

public class lesson2 : MonoBehaviour {
	// The lesson2 component moves an object back and forth in the four directions, rotating 
	// through up, right, down, left.
	 
	// You can add information about your properties with "attributes", which use square brackets.
	// Using the Tooltip attribute, you can add tooltips visible in the Unity editor. 
	[Tooltip("Start object automatically")]
	public bool autostart = true;

	[Tooltip("How fast the object moves")]
	public float speed = 10;

	[Tooltip("How far the object moves")]
	public float distance = 3;

	// Properties without the 'public' keyword can only be 
	// used internally by the class they are declared in.

	// currentDirection will track the direction the object is currently moving.
	// 0 is up, 1 is right, 2 is down, 3 is left
	int currentDirection = 0;
	// originalPosition will store where the object started from
	Vector3 originalPosition;
	// destination will store where the object is currently moving to
	Vector3 destination;

	// The Start function runs when the object is created or when the scene starts
	// If autostart is false, StartMove has to be called manually from other code.
	void Start () {
		if (autostart) {
			// Call the StartMove function, defined below.
			// Calling a function runs all the code inside it.
			StartMove();
		}
		// The Start function is a good place to initialize values
		// Remember that the transform.position is the location of the object this behavior is attached to.
		originalPosition = transform.position;
	}

	// The StartMove function starts the object moving.
	// If autostart is false, StartMove has to be called manually from other code.
	// In order to allow outside calls like this, we make this function public, 
	// which lets code outside this component call it.
	public void StartMove() {
		// Call the SetNextMove function, and send it our currentDirection
		SetNextMove(currentDirection);
	}

	// The SetNextMove function takes an argument, a bit of information it needs to do its work
	// The argument 'direction' will tell us which way to set our movement to.
	void SetNextMove(int direction)
	{
		// the switch statement lets your code choose what code to run depending on a certain variable
		// in this example, we use currentDirection as the switch. If currentDirection's value is 0,
		// only the code in the "case 0" block below will run.
		switch (currentDirection) 
		{
		case 0:		
			// Move up, so set our destination to the start point + 'distance' in the y axis
			// Notice that you have to create a new vector to set a vector value.
			destination = new Vector3(originalPosition.x, 
			                          originalPosition.y + distance, 
			                          originalPosition.z);
			// the break statement is required when ending a switch case
			break;
		case 1:		
			// Move right, so set our destination to the start point + 'distance' in the x axis
			destination = new Vector3(originalPosition.x + distance, 
			                          originalPosition.y, 
			                          originalPosition.z);
			break;
		case 2:		
			// Move down, so set our destination to the start point - 'distance' in the y axis
			destination = new Vector3(originalPosition.x, 
			                          originalPosition.y - distance, 
			                          originalPosition.z);
			break;
		case 3:		
			// Move left, so set our destination to the start point - 'distance' in the x axis
			destination = new Vector3(originalPosition.x - distance, 
			                          originalPosition.y, 
			                          originalPosition.z);
			break;
		}
	}

	// Update is called once per frame
	void Update () {
		// Use the Lerp function to change a value smoothly over time.
		// It takes the arguments 'from point', 'to point', and 't' which
		// is a time value and is usually scaled by a speed value.
		// Basically, Lerp returns a point slightly closer to the 'to point'.
		// Time.deltaTime is the elapsed time since the last frame. We use
		// that to create smooth movement no matter what the framerate is.
		transform.position = Vector3.Lerp( transform.position, destination, Time.deltaTime * speed);

		// Now we test if we reached our destination.
		// When using Lerp, it is generally better to do this using the 
		// Vector3.Distance function, because vectors can be very close 
		// to each other without being equal.
		if (Vector3.Distance(transform.position, destination) < 0.02) {
			// We are close enough, so set us to our destination exactly.
			transform.position = destination;
			// Now we need to know if we're back at start or at the endpoint of a move
			if (destination == originalPosition) {
				// we're at original position... do the next move

				// incrememnt currentDirection
				currentDirection++;
				// if it's past our number of directions, set it back to 0
				if (currentDirection > 3) {
					currentDirection = 0;
				}
				SetNextMove(currentDirection);
			}
			else {
				// we're at the end of a move... go back to start
				destination = originalPosition;
			}
		}
	}
}

/*
 * Puzzles:
 * When trying these puzzles, make your own component copied from this 
 * so that you don't alter this original.
 * The puzzles are in order of increasing difficulty. Don't worry if you can't do all
 * of them right away.
 * 
 * 1) Rotate through movements in the other direction... up, left, down, right.
 * 2) Make the object go a little faster after each movement.
 * 		Up and back, right and back a little faster, down and back a little faster still etc.
 * 3) Make the object go a little faster after each FULL SET of movements.
 * 		Up, right, down, left, faster, up, right, down, left, even faster
 * 4) Move twice in each direction before changing to the next. That is,
 * 		up and back, up and back, right and back, right and back, etc.
 * 5) Create a new component that has a Start method that calls this object's StartMove method 
 * 
 */
