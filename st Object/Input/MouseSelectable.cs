using UnityEngine;
using System.Collections;
[AddComponentMenu("st Object/Input/Mouse Selectable")]

[RequireComponent(typeof(Selectable))]

/// <summary>
/// Object will be selectable by mouse-clicking.
/// </summary>
public class MouseSelectable : MonoBehaviour {

	static float lastSelectTime; // used to determine if any MouseSelectable objects have been selected this frame.

	void Update() {
		if (lastSelectTime != Time.time) {
			if (Input.GetMouseButtonDown(0)) {
				stSelection.UnselectAll ();
			}
		}
		lastSelectTime = Time.time; // prevents this from checking the mouse more than once per frame
	}

	void OnMouseDown() {
		Selectable selectable = GetComponent<Selectable>();
		// track whether something was selected
		lastSelectTime = Time.time;
		// multi-select or unselect with shift
		if (stSelection.selectedObjects.Count > 0 && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) {
			// unselect if selected
			if (selectable.selected) {
				selectable.Unselect ();
				return;
			}
			// return if  this or selected object doesn't allow multiple selection
			if (!selectable.allowMultiple || !stSelection.selectedObjects[0].GetComponent<Selectable>().allowMultiple) {
				return;
			}
		}
		else {
			// not multi-select
			stSelection.UnselectAll();
		}
		
		if (selectable.doCallbacks) {
			gameObject.SendMessage ("Select", SendMessageOptions.DontRequireReceiver);
		}
		else {
			selectable.Select();
		}
	}
}
