using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class ComponentField {

	public Component component;
	public string member;

	public string GetString() {
		var data = Introspector.GetValue( component, member);
		return (data.ToString());
	}

	public int GetInt() {
		var data = Introspector.GetValue( component, member);
		if (data is int) {
			return (int) data;
		}
		else {
			Debug.LogError ("GetInt invalid for "+component.ToString()+" - "+member);
			return 0;
		}
	}

	public void SetValue( int val) {
		Introspector.SetValue( component, member, val);
	}
}
