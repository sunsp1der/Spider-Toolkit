using UnityEngine;
using System.Collections;
[AddComponentMenu("st Object/Gameplay/Alter Value Over Time")]

/// <summary>
/// Change an stDictionary and/or component value over time. Only works for ints and floats!
/// </summary>
public class AlterValueOverTime : MonoBehaviour {

	[Tooltip("If dictionary and key are set, store data in dictionary. Use DictionaryStartValue to initialize.")]
	public string dictionary = "";
	[Tooltip("If dictionary and key are set, store data in dictionary. Use DictionaryStartValue to initialize.")]
	public string key = "";
	[Tooltip("Component field to alter.")]
	public ComponentField field;
	[Tooltip("Amount to increase value per second.")]
	public float increasePerSecond = 0;
	[Tooltip("Time between each increment. Zero is every frame, which can be SLOW!")]
	public float secondsBetweenIncrements = 0.1f;
	[Tooltip("If true, automatically disable this on clones of this object. If false, value will change on clones, not on original archetype.")]
	public bool disableOnClones = true;
	[Tooltip("Use maximum and minimum values below")]
	public bool useMaxAndMin = false;
	public float minimum = 0;
	public float maximum = 0;

	stDictionary dict;
	float lastTime;
	bool keepChanging = false; // setting this to false will stop the changing until StartValueChange is called again.
	
	void Start () {
		dict = stData.GetDictionary(dictionary);
		if (GetComponent<stObject>().myArchetype != null && disableOnClones) {
			enabled = false;
		}
		else {
			StartValueChange();
		}
	}

	void ArchetypeStart() {
		dict = stData.GetDictionary(dictionary);
		if (disableOnClones) {
			StartValueChange();
		}
	}

	/// <summary>
	/// Starts the value change.
	/// </summary>
	public void StartValueChange () {
		if (keepChanging) {
			// already changing
			return;
		}
		lastTime = Time.time;
		keepChanging = true;
		StartCoroutine( ValueChange ());
	}

	/// <summary>
	/// Stops the value change.
	/// </summary>
	public void StopValueChange () {
		keepChanging = false;
	}

	IEnumerator ValueChange(){
		while (keepChanging) {
			if (enabled) {
				// This gets a bit complicated because we want to deal with both ints and floats
				float newDictVal = 0;
				float newFieldVal = 0;

				// Do the actual alteration
				float change = (Time.time - lastTime) * increasePerSecond;
				if (dict) newDictVal = dict.GetFloat(key) + change;
				if (field.member != "") newFieldVal = field.GetFloat() + change;
				if (useMaxAndMin) {
					newDictVal = newDictVal.LimitToRange( minimum, maximum);
					newFieldVal = newFieldVal.LimitToRange( minimum, maximum);
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
			lastTime = Time.time;
			yield return new WaitForSeconds(secondsBetweenIncrements);
		}
	}
}
