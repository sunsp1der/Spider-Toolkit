using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Input/Mouse Follow")]

public class MouseFollow : MonoBehaviour {
	// object follows the mouse pointer

	[Range(0,5)]
	public float facingSpeed = 0; // rotate object to direction of motion this fast
	public bool keepOnScreen = false;

	Rect screen; // used for boundaries (left, top, right, bottom)

	// Use this for initialization
	void Start () {
		Vector3 topleft = new Vector3(0,0,-Camera.main.transform.position.z);
		Vector3 bottomright = new Vector3(Screen.width, Screen.height,-Camera.main.transform.position.z);
		topleft = Camera.main.ScreenToWorldPoint(topleft);
		bottomright = Camera.main.ScreenToWorldPoint(bottomright);
		screen = new Rect( topleft.x, topleft.y, bottomright.x, bottomright.y);
	}
	
	// Update is called once per frame
	Vector3 GetMouseLocation () {
		Vector3 mouseLocation = Input.mousePosition;
		mouseLocation.z = -Camera.main.transform.position.z;
		mouseLocation = Camera.main.ScreenToWorldPoint(mouseLocation);

		if (keepOnScreen) {
			if (mouseLocation.x < screen.x ) {
				mouseLocation.x = screen.x ;
				}
			else if (mouseLocation.x > screen.width) {
				mouseLocation.x = screen.width;
			}
			if (mouseLocation.y < screen.y) {
				mouseLocation.y = screen.y;
			}
			else if (mouseLocation.y > screen.height) {
				mouseLocation.y = screen.height;
			}
		}
		if (mouseLocation == transform.position) {
			return mouseLocation;
		}
		if (facingSpeed>0) {
			Vector3 difference = mouseLocation - transform.position;
			float distance = difference.magnitude;
			float angle = Mathf.Atan2(difference.y,difference.x) * Mathf.Rad2Deg;
			Quaternion target = Quaternion.AngleAxis(angle - 90, Vector3.forward);
			transform.rotation = Quaternion.Lerp( transform.rotation, target, distance * facingSpeed);
			//transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
		}
		return mouseLocation;
	}

	void Update() {
		transform.position = GetMouseLocation();
	}
}
