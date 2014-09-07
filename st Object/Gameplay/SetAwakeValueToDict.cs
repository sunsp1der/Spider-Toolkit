using UnityEngine;
using System.Collections;
using System;

[AddComponentMenu("st Main/Set Value To Dict")]

// on awake (before start) the chosen value is set to an stDictionary value
// currently only works when the second level is loaded

public class SetAwakeValueToDict : MonoBehaviour {

	[Tooltip("If dictionary and key are set, use this dictionary. Use DictionaryStartValue to initialize.")]
	public string dictionary = "";
	[Tooltip("If dictionary and key are set, use this key in dictionary. Use DictionaryStartValue to initialize.")]
	public string key = "";
	[Tooltip("The component field to set. Be sure data types match!")]
	public ComponentField field;

	void Awake () {
		SetField ();
	}
	
	void SetField () {
		stDictionary dict = stData.GetDictionary (dictionary);
		if (dict) {
			field.SetValue( dict.Get ( key));
		} 
	}
} 
