using UnityEngine;
using System;

[Serializable]
public class LockedView {
	// dummy class used to automatically create a locked view in the editor
	// name your LockedView field _[Name of viewed member]...
	public string tooltip;

	public LockedView (string myTooltip= ""){
		tooltip = myTooltip;
	}
}