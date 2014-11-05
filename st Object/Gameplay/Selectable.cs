using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("st Object/Input/Selectable")]

/// <summary>
/// Make object selectable. Selectable.selected is a List of selected GameObjects.
/// </summary>
public class Selectable : MonoBehaviour {

	[Tooltip("Object is selected by clicking on it")]
	public bool clickSelect;
	[Tooltip("Shift or Ctrl multiple selects objects")]
	public bool allowMultiple = true;
	[Tooltip("Attach a clone of this object when selected")]
	public GameObject selectMarker;
	[Tooltip("Scale the selectMarker to the selected object")]
	public bool scaleMarker = true;
	[Tooltip("Send Select() and Unselect() callbacks. Leave this on to use ST OnSelect components")]
	public bool doCallbacks = true;

	public MethodButton _Select;
	public MethodButton _Unselect;

	[HideInInspector]
	public bool selected = false;
	public LockedView _selected;

	static List<GameObject> selectedObjects = new List<GameObject>();
	GameObject myMarker;

	void Select() {
		if (selected) {
			return;
		}
		selectedObjects.Add (gameObject);
		selected = true;
		if (selectMarker != null) {
			CreateMarker ();
		}
	}

	void Unselect() {
		if (!selected) {
			return;
		}
		selected = false;
		selectedObjects.Remove(gameObject);
		stTools.Remove(myMarker);
	}

	void CreateMarker ()
	{
		myMarker = stTools.Spawn (selectMarker);
		myMarker.transform.position = gameObject.transform.position;
		myMarker.transform.parent = gameObject.transform;
		if (scaleMarker) {
			myMarker.transform.localScale = new Vector3 (myMarker.transform.localScale.x * transform.localScale.x, myMarker.transform.localScale.y * transform.localScale.y, myMarker.transform.localScale.z * transform.localScale.z);
		}
	}

	/// <summary>
	/// Unselects all objects.
	/// </summary>
	void UnselectAll( ) {
		// loop through backwards so removals don't disrupt list
		for (int i = selectedObjects.Count - 1; i >= 0; i--){
			GameObject unselected = selectedObjects[i];
			if (unselected == gameObject) {
				continue;
			}
			Selectable selectable = unselected.GetComponent<Selectable> ();
			if (selectable.doCallbacks) {
				unselected.SendMessage ("Unselect", SendMessageOptions.DontRequireReceiver);
			}
			else {
				selectable.Unselect ();
			}
		}
		selectedObjects.Clear ();
	}

	void OnMouseDown() {
		// only do this if we're allowing click selection
		if (!clickSelect) {
			return;
		}
		if (!allowMultiple || (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift) &&
		    		!Input.GetKey(KeyCode.LeftCommand) && !Input.GetKey(KeyCode.RightCommand) &&
		    		!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))) {
			UnselectAll();
		}
		if (doCallbacks) {
			gameObject.SendMessage ("Select", SendMessageOptions.DontRequireReceiver);
		}
		else {
			Select ();
		}
	}
}
