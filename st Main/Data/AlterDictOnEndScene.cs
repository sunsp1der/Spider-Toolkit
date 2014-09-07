using UnityEngine;
using System.Collections;
[AddComponentMenu("st Data/Alter Dict On End Scene")]

/// <summary>
/// Change an stDictionary value when a scene is loaded. Only works for ints and floats!
/// </summary>
public class AlterDictOnEndScene : MonoBehaviour {

	[Tooltip("If dictionary and key are set, store data in dictionary. Use DictionaryStartValue to initialize.")]
	public string dictionary = "";
	[Tooltip("If dictionary and key are set, store data in dictionary. Use DictionaryStartValue to initialize.")]
	public string key = "";
	[Tooltip("Amount to increase each time level is loaded.")]
	public float increasePerLevel = 0;
	[Tooltip("Use maximum and minimum values below")]
	public bool useMaxAndMin = false;
	public float minimum = 0; 
	public float maximum = 0;

	stDictionary dict;

	void OnEndScene() {
		ChangeValue ();
	}

	void ChangeValue() {
		float newDictVal = 0;
		dict = stData.GetDictionary (dictionary);
		// Do the actual alteration
		if (dict) newDictVal = dict.GetFloat(key) + increasePerLevel;
		if (useMaxAndMin) {
			newDictVal = newDictVal.LimitToRange( minimum, maximum);
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
	}

}
