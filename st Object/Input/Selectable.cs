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
	[Tooltip("Attach a clone of this object to transform when selected")]
	public GameObject selectMarker;
	[Tooltip("Scale the selectMarker to the selected object")]
	public bool scaleMarker;
	[Tooltip("Send Select() and Unselect() callbacks. Leave this to use ST OnSelect components")]
	public bool doCallbacks = true;

	static List<GameObject> selected = new List<GameObject>();
	GameObject myMarker;

	void OnMouseDown() {
		if (clickSelect) {
			if (doCallbacks) {
				foreach (GameObject unselected in selected) {
					unselected.SendMessage("Unselect", SendMessageOptions.DontRequireReceiver);
				}
			}
			selected.Clear();
			selected.Add(gameObject);
			if (doCallbacks) {
				gameObject.SendMessage("Select", SendMessageOptions.DontRequireReceiver );
			}
		}
	}

	void Select() {
		if (selectMarker != null) {
			myMarker = stTools.Spawn(selectMarker);
			myMarker.transform.position = gameObject.transform.position;
			myMarker.transform.parent = gameObject.transform;
			if (scaleMarker) {
				myMarker.transform.localScale = new Vector3(1,1,1);
			}
		}
	}

	void Unselect() {
		stTools.Remove(myMarker);
	}

}
