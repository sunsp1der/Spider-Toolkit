using UnityEngine;
using System.Collections;
[AddComponentMenu("st Object/Gameplay/Alter Float")]

/// <summary>
/// Change an stDictionary and/or component float value on object events
/// </summary>
public class AlterFloat : MonoBehaviour { 

	public enum EventEnum { Start, Remove, EndScene};
	public enum OperationEnum {Add, SetTo, Multiply};

	[Tooltip("If dictionary and key are set, store data in dictionary")]
	public string dictionary = "";
	[Tooltip("If dictionary and key are set, store data in dictionary")]
	public string key = "";
	[Tooltip("If dictionary key doesn't exist, set value to this")]
	public float startValue = 0;
	[Tooltip("Component field to alter. It has to be a float value!")]
	public ComponentField field;
	[Tooltip("Type of event to alter on")]
	public EventEnum alterOnEvent = EventEnum.Remove;
	[Tooltip("Ttype of change to make")]
	public OperationEnum operation = OperationEnum.Add; 
	[Tooltip("Amount to change")]
	public float value = 1;
	stDictionary dict = null;

	void Start() {
		if (dictionary != "" && key != "") {
			dict = stData.InitializeDictionaryValue(dictionary,key,startValue);
		}
		if (alterOnEvent == EventEnum.Start) {
			Alter();
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

	void Alter() {
		switch (operation) {
		case OperationEnum.Add:
			if (dict) dict.Set (key, dict.GetFloat(key) + value);
			if (field.member != "") field.SetValue (field.GetFloat() + value);
			break;
		case OperationEnum.SetTo:
			if (dict) dict.Set (key, value);
			if (field.member != "") field.SetValue (value);
			break;
		case OperationEnum.Multiply:
			if (dict) dict.Set (key, dict.GetFloat(key) * value);
			if (field.member != "") field.SetValue (field.GetFloat() * value);
			break;
		}
	}
}
