using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("st Object/Gameplay/Enable Components On Select")]


/// <summary>
/// List behaviors that are only enabled when object is select,
/// and behaviors that are only enabled when object is not selected.
/// Use with the Selectable component.
/// Behaviours that are disabled when game is started will never be enabled!
/// </summary>
public class EnableComponentsOnSelect : MonoBehaviour {
	// The enabled bool is technically only in behaviours, so we use that type instead of components

	[Tooltip("Components that are enabled only when object is selected")]
	[SerializeField] 
	public List<Behaviour> selectedComponents = new List<Behaviour>();

	[Tooltip("Components that are enabled only when object is NOT selected")]
	[SerializeField] 
	public List<Behaviour> nonselectedComponents = new List<Behaviour>();

	void Awake() {
		// disregard any behaviors
		for (int i = selectedComponents.Count-1; i>=0; i--) {
			if (!selectedComponents[i].enabled) {
				selectedComponents.RemoveAt(i);
			}
		}
		for (int i = nonselectedComponents.Count-1; i>=0; i--) {
			if (!nonselectedComponents[i].enabled) {
				nonselectedComponents.RemoveAt(i);
			}
		}
	}

	// Use this for initialization
	void Start () {
		Invoke ("DisableSelected",0);
	}

	/// <summary>
	/// Disables components in the selectedComponents list.
	/// </summary>
	void DisableSelected(){
		foreach (Behaviour component in selectedComponents) {
			component.enabled = false;
		}
	}
	
	void Select(){
		CancelInvoke("DisableSelected"); // just in case we select the object in Start
		foreach (Behaviour component in selectedComponents) {
			component.enabled = true;
		}
		foreach (Behaviour component in nonselectedComponents) {
			component.enabled = false;
		}
	}

	void Unselect(){
		CancelInvoke("DisableSelected"); // just in case we select the object in Start
		foreach (Behaviour component in nonselectedComponents) {
			component.enabled = true;
		}
		foreach (Behaviour component in selectedComponents) {
			component.enabled = false;
		}
	}

}
