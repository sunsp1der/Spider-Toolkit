using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// St selection.
/// Utility class that manages selected objects. Works with Selectable component and various other OnSelect components.
/// Objects with the Selectable component will get Select and Unselect callbacks at the appropriate times.
/// Objects without the Selectable component can be added to the selection list, but Spider Toolkit functionality will 
/// not be implemented.
/// </summary>
public static class stSelection {

	public static List<GameObject> selectedObjects = new List<GameObject>(); // list of all selected objects

	/// <summary>
	/// Unselects all objects.
	/// </summary>
	public static void UnselectAll( ) {
		// loop through backwards so removals don't disrupt list
		for (int i = selectedObjects.Count - 1; i >= 0; i--){
			GameObject unselected = selectedObjects[i];
			Selectable selectable = unselected.GetComponent<Selectable> ();
			if (selectable != null) {
				if (selectable.doCallbacks) {
					unselected.SendMessage ("Unselect", SendMessageOptions.DontRequireReceiver);
				}
				else {
					selectable.Unselect ();
				}
			}
		}
		selectedObjects.Clear ();
	}

	/// <summary>
	/// Select the specified object.
	/// </summary>
	/// <param name="ob">Ob.</param>
	/// <param name="multiSelect">If set to <c>true</c> add this object to selection.</param>
	public static void Select(GameObject ob, bool multiSelect = false ) {
		if (!multiSelect) {
			UnselectAll();
		}
		selectedObjects.Add (ob);
		Selectable selectable = ob.GetComponent<Selectable> ();
		if (selectable != null) {
			selectable.Select ();
		}
	}

	/// <summary>
	/// Unselect the specified object.
	/// </summary>
	/// <param name="ob">Ob.</param>
	public static void Unselect(GameObject ob) {
		selectedObjects.Remove(ob);
		Selectable selectable = ob.GetComponent<Selectable> ();
		if (selectable != null) {
			selectable.Unselect();
		}
	}

}
