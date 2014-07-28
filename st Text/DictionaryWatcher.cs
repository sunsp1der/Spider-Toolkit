// displays stDictionary data in an stText object

using UnityEngine;
using System;
using System.Collections;

[AddComponentMenu("st Text/Dictionary Watcher")]
[RequireComponent(typeof(TextMesh))]

public class DictionaryWatcher : MonoBehaviour {

	public string dictionary;
	public string key;
	public string formatString = "{0}"; // C# format string. {0} is the watched data
 
	[HideInInspector]
	public stDictionary dict;
	[HideInInspector]
	public TextMesh mesh;
	string text;


	// Use this for initialization
	void Start () {
		mesh = GetComponent<TextMesh>();
		dict = stData.GetDictionary(dictionary);
		if (dict == null) {
			Debug.LogError ("stDictionary '"+dictionary+"' not found.");
		}
		if (formatString == "") {
			formatString = "{0}";
			Debug.LogError ("WatchComponent has empty Format String. Changed to '{0}'");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (dict == null) {
			dict = stData.GetDictionary(dictionary);
			if (dict == null) return;
		}
		string newtext;
		try {
			newtext = String.Format(formatString, dict.Get(key));
		}
		catch {
			newtext = String.Format(formatString, dict.GetString(key));
		}
		if (newtext != text){
			text = newtext;
			mesh.text = text;
		}
	}
}
