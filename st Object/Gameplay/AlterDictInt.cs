using UnityEngine;
using System.Collections;
[AddComponentMenu("st Object/Gameplay/Alter Dictionary Integer")]

/// <summary>
/// Change an stDictionary integer value when added or removed
/// </summary>
public class AlterDictInt : MonoBehaviour {

	public enum EventEnum { Start, Remove};
	public enum OperationEnum {Add, SetTo};

	public string dictionary = "Game";
	public string key = "Score"; 
	public EventEnum alterOnEvent = EventEnum.Remove;
	public OperationEnum operation = OperationEnum.Add; // type of change to make
	public int value = 1; // amount to add on remove
	stDictionary dict;

	void Start() {
		dict = stData.InitializeDictionaryValue(dictionary,key,0);
		if (alterOnEvent == EventEnum.Start) {
			AlterInt();
		}
	}

	void OnRemove() {
		if (alterOnEvent == EventEnum.Remove) {
			AlterInt();
		} 
	}

	void AlterInt() {
		switch (operation) {
		case OperationEnum.Add:
			dict.Set (key, dict.GetInt(key)+value);
			break;
		case OperationEnum.SetTo:
			dict.Set (key, value);
			break;
		}
	}
}
