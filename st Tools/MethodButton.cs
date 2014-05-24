using UnityEngine;
using System;

[Serializable]
public class MethodButton {
	// dummy class used to automatically create a button for a member function
	// name your MethodButton field _[Name of member function]...
	public string tooltip;

	public MethodButton (string mytooltip= ""){
		tooltip = mytooltip;
	}
}
