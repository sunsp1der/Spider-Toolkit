using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("st Object/Input/Selectable")]

/// <summary>
/// Make object selectable. Selectable.selected is a List of selected GameObjects.
/// </summary>
public class Selectable : MonoBehaviour {

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
	public GameObject myMarker; // marker to show object is selected. Will be a child of this object's transform.

	bool isSelected; // stores whether object is selected
	[HideInInspector]
	public bool selected { // public accessor for isSelected
		get{ return isSelected;}
		set{
			if (value) {
				if (!stSelection.selectedObjects.Contains(gameObject)) {
					Select ();
				}
				else {
					isSelected = true;
					if (selectMarker != null) {
						CreateMarker();
					}
				}
			}
			else {
				if (stSelection.selectedObjects.Contains(gameObject)) {
					Unselect ();
				}
				else {
					isSelected = false;
					if (myMarker != null) {
						stTools.Remove(myMarker);
					}
				}
			}
		}
	}
	public LockedView _selected; // locked editor view of selected

	void Awake(){
		if (myMarker != null) {
			// can happen when a selected object is cloned
			Destroy(myMarker);
		}
	}

	/// <summary>
	/// Select this object.
	/// Will add to selection if this allows multiple. Use SelectSingle if you want to force this to be the only object selected.
	/// </summary>
	public void Select() {
		stSelection.Select (gameObject, allowMultiple);
	}

	/// <summary>
	/// Selects this object. All others will be unselected
	/// </summary>
	public void SelectSingle() {
		stSelection.Select (gameObject, false);
	}

	/// <summary>
	/// Unselect this object.
	/// </summary>
	public void Unselect() {
		stSelection.Unselect (gameObject);
	}

	/// <summary>
	/// Creates the selection marker, such as a box or glow as defined by selectMarker.
	/// The selection marker instance will be stored in myMarker
	/// </summary>
	public void CreateMarker ()
	{
		myMarker = stTools.Spawn (selectMarker);
		myMarker.transform.position = gameObject.transform.position;
		myMarker.transform.parent = gameObject.transform;
		if (scaleMarker) {
			myMarker.transform.localScale = new Vector3 (myMarker.transform.localScale.x * transform.localScale.x, myMarker.transform.localScale.y * transform.localScale.y, myMarker.transform.localScale.z * transform.localScale.z);
		}
	}

}
