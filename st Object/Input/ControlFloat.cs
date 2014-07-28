// keyboard controls for a float value

using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Input/Control Float")]

public class ControlFloat : MonoBehaviour {


	[Tooltip("If dictionary and key are set, store data in dictionary")]
	public string dictionary = "";
	[Tooltip("If dictionary and key are set, store data in dictionary")]
	public string key = "";
	[Tooltip("If dictionary key doesn't exist, set value to this")]
	public float startValue = 0;
	[Tooltip("Component field to alter. Make sure it's a float!")]
	public ComponentField field;
	[Tooltip("Amount to change per click or frame")]
	public float delta = 1;
	[Tooltip("Apply delta once per click. Every frame if false.")]
	public bool oncePerClick = false;
	[Tooltip("Auto-soften changes. Only works when oncePerClick false")]
	public bool softenInput = true;
	[Tooltip("See project settings/input")]
	public string input = "Vertical"; 
	[Tooltip("Apply minLimit and maxLimit limits")]
	public bool limits = false;
	public float minLimit = 0;
	public float maxLimit = 100;

	stDictionary dict = null;

	void Start() {
		if (dictionary != "" && key != "") {
			dict = stData.InitializeDictionaryValue( dictionary, key, startValue);
		}
	}

	void Update () { 
		float change = 0;

		if (oncePerClick) {
			if (Input.GetButtonDown(input)) {
				change = stTools.GetAxisBool(input) * delta;
			}
		}
		else {
			if (softenInput) {
				change = Input.GetAxis(input) * delta;
			}
			else {
				change = stTools.GetAxisBool(input) * delta;
			}
		}
		if (change != 0) {
			float newVal;
			if (dict) {
				newVal = dict.GetFloat(key) + change;
				if (limits) {
					newVal = newVal.LimitToRange(minLimit, maxLimit);
				}
				dict.Set (key, newVal);
			}
			if (field.member != "") {
				newVal = field.GetFloat() + change;
				if (limits) {
					newVal = newVal.LimitToRange(minLimit, maxLimit);
				}
				field.SetValue (newVal);
			}
		}
	}
}
