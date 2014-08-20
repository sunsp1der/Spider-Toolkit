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
	[Tooltip("Time between each increment. Zero is every frame, which can be slow on non-archetype objects.")]
	public float secondsBetweenIncrements = 0;
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
			// For clones we just fake the value alteration by calculating what it would be.
			if (secondsBetweenIncrements <= 0) {
				float startTime = GetComponent<stObject>().myArchetype.GetComponent<AlterValueOverTime>().lastTime;
				ChangeValue( (Time.time - startTime) * increasePerSecond);
			}
			enabled = false;
		}
		else {
			StartValueChange();
		}
	}

	void ArchetypeStart() {
		// set value for calculations
		lastTime = Time.time;
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
		StartCoroutine( ValueCoroutine ());
	}

	/// <summary>
	/// Stops the value change.
	/// </summary>
	public void StopValueChange () {
		keepChanging = false;
	}

	void ChangeValue( float change) {
		// This gets a bit complicated because we want to deal with both ints and floats

		float newDictVal = 0;
		float newFieldVal = 0;
		
		// Do the actual alteration
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

	IEnumerator ValueCoroutine(){
		while (keepChanging) {
			if (enabled) {
				float change = (Time.time - lastTime) * increasePerSecond;
				ChangeValue( change);
			}
			lastTime = Time.time;
			yield return new WaitForSeconds(secondsBetweenIncrements);
		}
	}
}
