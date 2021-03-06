using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class ComponentField {

	public Component component = null;
	public string member = "";

	public string GetString() {
		var data = Introspector.GetSmartValue( component, member);
		return (data.ToString());
	}

	public int GetInt() {
		var data = Introspector.GetSmartValue( component, member);
		return Convert.ToInt32(data);
	}

	public float GetFloat() {
		var data = Introspector.GetSmartValue( component, member);
		return (float)Convert.ToDouble(data);
	}

	public object GetObject() {
		var data = Introspector.GetSmartValue( component, member);
		return data;
	}

	public void SetValue( object val) {
		Introspector.SetSmartValue( component, member, val);
	}

}
