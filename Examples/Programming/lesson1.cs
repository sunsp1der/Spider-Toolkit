using UnityEngine;
using System.Collections;

/**
  * This is an example component designed to teach
  * some basic programming concepts. The comments
  * explain each line and each new concept introduced.
  * Comments are either surrounded by /* and */ 
/** or they start with //
**/

// This first line creates a Unity component. All C# components
// are derived from the 'MonoBehavior' class. The name of this
// component class is 'lesson1', and it is 'public' so that it
// shows up in the editor and can be accessed by other code.
public class lesson1 : MonoBehaviour {
	// The lesson1 component moves an object left and right across the screen.


	// These top lines declare properties for the component.
	// All properties with the 'public' keyword will show up in 
	// the editor for this component.

	// 'autostart' lets you control if the object automatically starts moving 
	public bool autostart = true;
	// 'speed' lets you control how fast your object moves
	public float speed = 3;
	// 'leftBorder' controls where the object starts moving to the right
	public float leftBorder = -4;
	// 'rightBorder' controls where the object starts moving to the left
	public float rightBorder = 4;

	// The Start function runs when the object is created or when the scene starts
	void Start () {

		// Check the 'autostart' property to see if we should start moving
		if (autostart) {
			// Call the StartMove function, defined below.
			// Calling a function runs all the code inside it.
			StartMove();
		}
	}

	// The StartMove function starts the object moving.
	// If autostart is false, StartMove has to be called manually from other code.
	// In order to allow outside calls like this, we make this function public, 
	// which lets code outside this component call it.
	public void StartMove(){

		// 'rigidbody2D' controls the physics of a Unity 2D object.
		// The velocity of rigidbody2D controls its speed.
		// In order to set the velocity, we have to create a new
		// Vector2 object, which contains an x (horizontal) and
		// y (vertical) value.
		// We set the x (horizontal) value to our 'speed' property 
		// above, and the y value to zero (we're not moving vertically).
		// This makes the object move to the right.
		rigidbody2D.velocity = new Vector2 (speed, 0);
	}

	// The Update function is called automatically once per frame
	// We will be using Update to check if we need to start moving the other direction.
	void Update () {

		// 'transform' contains position, rotation, and scale data of a Unity 2D object.
		// transform.position.x is the x (horizontal) value of the position of our object

		// This if statement checks to see if our x position is less than (left of) the
		// value of the 'leftBorder' property.
		if (transform.position.x < leftBorder) {
			// if the if statement is true (meaning we are left of the leftBorder), 
			// we tell the object to move to the right
			rigidbody2D.velocity = new Vector2( speed, 0);
		}

		// These lines do the same as the above lines, but for the rightBorder.
		if (transform.position.x > rightBorder) {
			rigidbody2D.velocity = new Vector2( -speed, 0);
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
 * 1) Make the object start out moving left instead of right.
 * 2) Make the object move up and down instead of left and right. 
 * 3) Make the object move diagonally. 
 * 4) Make the borders relative to the starting point of the object. 
 * 5) Make the object move left and right again or recopy the original component.
 * 		Using the editor, put a negative number in for the speed. 
 * 		Find the problem with this and debug it.
 * 6) Using the editor, make the leftBorder a larger number than the rightBorder.
 * 		Find the problem with this and alter the Start function so that if the
 * 		user makes this mistake, the two values are swapped so that leftBorder 
 *      is always lower than rightBorder.
 * 
 */