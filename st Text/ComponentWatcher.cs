using UnityEngine;
using System.Collections;
using System;

[AddComponentMenu("st Text/ComponentWatcher")]
[RequireComponent(typeof(TextMesh))]

public class ComponentWatcher : MonoBehaviour {
	// tracks a component's member value in the text field

	public ComponentField value; 
	public string formatString = "{0}"; // C# format string. {0} is the watched data

	bool stMainComponent;
	string text = "";
	TextMesh mesh;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<TextMesh>();
		if (formatString == "") {
			formatString = "{0}";
			Debug.LogWarning ("WatchComponent has empty Format String. Changed to '{0}'");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (value.component == null) {
			return;
		}
		string newtext = String.Format(formatString, value.GetString());
		if (newtext != text){
			text = newtext;
			mesh.text = text;
		}
	}
}
