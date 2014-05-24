using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Behaviors/Remove Offscreen")]

public class RemoveOffscreen : MonoBehaviour {
	// Remove this object when it is not shown in any camera
	// NOTE: This includes the editor camera, so game may behave differently when run in Editor
	public bool doRemoveEffects = false; // do other remove effects (fade, explode, score etc.)
	public bool checkVisibility = true; // make sure object has been visible before removal

	bool visibilityChecked = false; // necessary in some first frame cases

	// Use this for initialization
	void Start () {
		if (!checkVisibility) visibilityChecked = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!renderer.isVisible) {
			if (visibilityChecked) {
				if (doRemoveEffects) {
					stTools.Remove(gameObject);
				}
				else {
					Destroy( gameObject);
				}
			}
		}
		else {
			visibilityChecked = true;
		}
	}
}
