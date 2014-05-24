using UnityEngine;
using System.Collections;

[AddComponentMenu("st Main/Data/Dictionary Start Value")]

// NOTE: this component has a specialized editor

/// <summary>
/// Set a starting value for an stDictionary.
/// </summary>
public class DictionaryStartValue : MonoBehaviour {

	public enum valType { String, Integer, Float, GameObject, Component}

	public string dictionary;
	public string key;
	public valType valueType;

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
		stData.InitializeDictionaryValue(dictionary, key, value);
	}
	
}
