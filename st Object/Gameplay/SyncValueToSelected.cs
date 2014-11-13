using UnityEngine;
using System.Collections;
using System;
[AddComponentMenu("st Object/Gameplay/Sync Value To Selected")]


/// <summary>
/// on update, the chosen value is set to the FIRST OBJECT in stSelection.selectedObjects
/// </summary>
public class SyncValueToSelected : MonoBehaviour {

	[Tooltip("The component field to set. Must be a gameobject.")]
	public ComponentField field;

	GameObject lastSelected;

	void Update () {
		GameObject selected;
		try {
			selected = stSelection.selectedObjects[0];
		}
		catch {
			selected = null;
		}
		if (lastSelected != selected) {
			field.SetValue( selected);
		}
		lastSelected = selected;
	}

} 
