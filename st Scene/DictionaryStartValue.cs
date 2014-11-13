using UnityEngine;
using System.Collections;

[AddComponentMenu("st Scene/Dictionary Start Value")]

// NOTE: this component has a specialized editor

/// <summary>
/// Set a starting value for an stDictionary when scene loads. Usually it is better to use the start values in the 
/// dictionary itself, but this can be used for resetting values when a level is reloaded, etc.
/// </summary>
public class DictionaryStartValue : MonoBehaviour {

	public enum valType { String, Integer, Float, GameObject, Component}

	public string dictionary = "Game";
	public string key = "Score";
	public valType valueType;
	[Tooltip("If true, set value even if already set")]
	public bool overrideValue = true;

	public int intValue = 0;
	public float floatValue = 0;
	public string stringValue = "";
	public GameObject gameObjectValue = null;
	public Behaviour componentValue = null;
	
	// Use this for initialization
	void Start () {
		object value = null;
		switch (valueType) {
		case valType.String:
			value = stringValue;
			break;
		case valType.Integer:
			value = intValue;
			break;
		case valType.Float:
			value = floatValue;
			break;
		case valType.GameObject:
			value = gameObjectValue;
			break;
		case valType.Component:
			value = componentValue;
			break;
		}
		if (overrideValue) {
			stData.SetDictionaryValue( dictionary, key, value);
		}
		else {
			stData.InitializeDictionaryValue(dictionary, key, value);
		}
	}
	
}
