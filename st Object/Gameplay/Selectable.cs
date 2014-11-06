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

	public MethodButton _Select; // editor button
	public MethodButton _Unselect; // editor button
	[HideInInspector]
	public bool selected = false; // object is selected
	public LockedView _selected; // locked editor view of selected
	GameObject myMarker; // marker to show object is selected. Will be a child of this object's transform.

	static List<GameObject> selectedObjects = new List<GameObject>(); // list of all selected objects
	static float lastSelectTime; // used to determine if any objects have been selected this frame.

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

	void Update() {
		if (lastSelectTime != Time.time) {
			if (Input.GetMouseButtonDown(0)) {
				UnselectAll ();
			}
		}
		lastSelectTime = Time.time; // prevents this from checking the mouse more than once per frame
	}

	void OnMouseDown() {
		// only select if we're allowing click selection
		if (!clickSelect) {
			return;
		}
		// track whether something was selected
		lastSelectTime = Time.time;
		// multi-select or unselect with shift
		if (selectedObjects.Count > 0 && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) {
			// unselect if selected
			if (selected) {
				Unselect ();
				return;
			}
			// return if  this or selected object don't allow multiple selection
			if (!allowMultiple || !selectedObjects[0].GetComponent<Selectable>().allowMultiple) {
				return;
			}
		}
		else {
			// not multi-select
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
