using UnityEngine;
using UnityEditor;

public class DictionaryWizard : ScriptableWizard {
	public string key;
	public DictValue.Types type;
	[HideInInspector]
	public stDictionary dict;
	void OnWizardCreate () {
		dict.startKeys.Insert (0,key);
		switch(type){
		case DictValue.Types.Int:
			dict.startValues.Insert (0, new DictValue((int)0));
			break;
		case DictValue.Types.Float:
			dict.startValues.Insert (0, new DictValue(0.0f));
			break;
		case DictValue.Types.String:
			dict.startValues.Insert (0, new DictValue(""));
			break;
		case DictValue.Types.GameObject:
			GameObject ob = new GameObject();
			dict.startValues.Insert (0, new DictValue(ob));
			DestroyImmediate(ob);
			break;
		}
	}  
	void OnWizardUpdate () {
		helpString = "Select unique key and data type";
	}   
}
