using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(stMain))]

public class stData : MonoBehaviour {

	static public Dictionary< string, stDictionary> dictionaries = 
		new Dictionary< string, stDictionary>();
	
	static public object GetDictionaryValue( string dictionaryName, string key) {
	 	try {
			return dictionaries[dictionaryName].dict[key];
		}
		catch {
			Debug.LogError("GetDictionaryValue data not found: "+dictionaryName+"["+key+"]");
			return null;
		}
	}

	static public stDictionary AddDictionary( string dictName, bool net=false) {
		if ( dictionaries.ContainsKey(dictName)) {
			Debug.LogError ("AddDictionary: "+dictName+" already exists!");
			dictionaries[ dictName].networked = net;
			return dictionaries[dictName];
		}
		stDictionary dict = stTools.stMain.gameObject.AddComponent<stDictionary>(); 
		dict.dataName = dictName;
		dict.networked = net;
		dictionaries.Add (dict.dataName, dict);
		return dict;
	}

	static public stDictionary GetDictionary( string dataName) {
		stDictionary retValue = null;
		dictionaries.TryGetValue(dataName, out retValue);
		return retValue;
	}

	public static void SetupDictionaries () {
		stDictionary[] dictList = stTools.stMain.GetComponents<stDictionary>();
		foreach (stDictionary dict in dictList) {
			dictionaries[dict.dataName] = dict;
		}
	}

	/// <summary>
	/// Initializes the dictionary value if it doesn't exist.  Creates dictionary if necessary
	/// </summary>
	/// <returns>The stDictionary object</returns>
	/// <param name="dictionaryName">Dictionary name.</param>
	/// <param name="key">Key.</param>
	/// <param name="value">Value.</param>
	public static stDictionary InitializeDictionaryValue (string dictionaryName, string key, object value) {
		stDictionary dict = GetDictionary(dictionaryName);
		if (dict == null) {
			dict = AddDictionary(dictionaryName);
		}
		if (!dict.ContainsKey(key )) {
			SetDictionaryValue( dictionaryName, key, value);
		}
		return dict;
	}

	[RPC]
	/// <summary>
	/// Sets the dictionary value.
	/// </summary>
	/// <param name="dictionaryName">Dictionary name.</param>
	/// <param name="key">Key.</param>
	/// <param name="value">Value to set.</param>
	public static void SetDictionaryValue (string dictionaryName, string key, object value) {
		stDictionary dict = GetDictionary( dictionaryName);
		try {
			dict.Set(key, value, false);
		}
		catch {
			Debug.LogError("Dictionary not found: "+dictionaryName);
			return;
		}
	}


}
