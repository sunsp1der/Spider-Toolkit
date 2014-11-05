using UnityEngine;
using System.Collections;
using System;

[AddComponentMenu("st Object/Gameplay/Set Awake Value To Dict")]

/// <summary>
/// on awake (before start) the chosen value is set to an stDictionary value
/// </summary>
public class SetAwakeValueToDict : MonoBehaviour {

	[Tooltip("If dictionary and key are set, use this dictionary. Use DictionaryStartValue to initialize.")]
	public string dictionary = "";
	[Tooltip("If dictionary and key are set, use this key in dictionary. Use DictionaryStartValue to initialize.")]
	public string key = "";
	[Tooltip("The component field to set. Be sure data types match!")]
	public ComponentField field;

	stDictionary dict;

	void Awake () {
		dict = stData.GetDictionary (dictionary);
		SetField ();
	}
	
	public void SetField () {
		field.SetValue( dict.Get ( key));
	}
} 
