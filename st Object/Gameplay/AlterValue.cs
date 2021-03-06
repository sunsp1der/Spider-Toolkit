﻿using UnityEngine;
using System.Collections;
using System;
[AddComponentMenu("st Object/Gameplay/Alter Value")]

/// <summary>
/// Change an stDictionary and/or component value on object events. Only works for ints and floats!
/// </summary>
public class AlterValue : MonoBehaviour { 

	public enum EventEnum { Start, Remove, Collide, EndScene};
	public enum OperationEnum {Add, SetTo, Multiply};

	[Tooltip("If dictionary and key are set, store data in dictionary. Use DictionaryStartValue to initialize.")]
	public string dictionary = "";
	[Tooltip("If dictionary and key are set, store data in dictionary. Use DictionaryStartValue to initialize.")]
	public string key = "";
	[Tooltip("Component field to alter.")]
	public ComponentField field;
	[Tooltip("Type of event to alter on")]
	public EventEnum alterOnEvent = EventEnum.Remove;
	[Tooltip("Type of change to make")]
	public OperationEnum operation = OperationEnum.Add; 
	[Tooltip("Amount to change")]
	public float value = 1;

	stDictionary dict = null;

	void Start() {
		dict = stData.GetDictionary( dictionary); 
		if (alterOnEvent == EventEnum.Start) {
			Invoke("Alter", 0);
		}
	}

	void OnRemove() {
		if (alterOnEvent == EventEnum.Remove) {
			Alter();
		} 
	}

	void OnEndScene() {
		if (alterOnEvent == EventEnum.EndScene){
			Alter();
		}
	}

	void OnCollisionEnter2D(Collision2D info){
		if (alterOnEvent == EventEnum.Collide) {
			Alter();
		}
	}

	void Alter() {
		if (!enabled) return;

		// This gets a bit complicated because we want to deal with both ints and floats
		float newDictVal = 0;
		float newFieldVal = 0;

		// Do the actual alteration
		switch (operation) {
		case OperationEnum.Add:
			if (dict) newDictVal = dict.GetFloat(key) + value;
			if (field.member != "") newFieldVal = field.GetFloat() + value;
			break;
		case OperationEnum.SetTo:
			if (dict) newDictVal = value;
			if (field.member != "") newFieldVal = value;
			break;
		case OperationEnum.Multiply:
			if (dict) newDictVal = dict.GetFloat(key) * value;
			if (field.member != "") newFieldVal = field.GetFloat() * value;
			break;
		}

		// Set the values, making sure we use correct type
		if (dict) {
			if ( dict.Get(key) is int) {
				dict.Set (key, (int)newDictVal);
			}
			else {
				dict.Set (key, newDictVal);
			}
		}
		if (field.member != "") {
			if ( field.GetObject() is int) {
				field.SetValue((int)newFieldVal);
			}
			else {
				field.SetValue (newFieldVal);
			}
		}

	}
}
