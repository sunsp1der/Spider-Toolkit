using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AddComponentMenu("st Main/st Dictionary")]

/// <summary>
/// St dictionary. A monobehavior that wraps a dictionary. Data can be of various kinds... up to 
/// user to track. Get+type functions will return default values if key is not found. If you
/// need to test whether a key exists, use the ContainsKey method.
/// </summary>

// this utility class creates a serializable data type for dictionary start values
[System.Serializable]
public class DictValue {
	public enum Types { New, Int, Float, String, GameObject, Vector2, Vector3}; // New is a dummy for the editor
	public Types dataType;
	public int intValue;
	public GameObject objectValue;
	public float floatValue;
	public string stringValue;
	public Vector2 vector2Value;
	public Vector3 vector3Value;

	public DictValue (object data){
		if (data is Int32) {
			intValue = (Int32)data;
			dataType = Types.Int;
		}
		else if (data is Single) {
			floatValue = (float)data;
			dataType = Types.Int;
		}
		else if (data is string) {
			stringValue = (string)data;
			dataType = Types.String;
		}
		else if (data is GameObject) {
			objectValue = (GameObject) data;
			dataType = Types.GameObject;
		}
		else if (data is Vector2) {
			vector2Value = (Vector2) data;
			dataType = Types.Vector2;
		}
		else if (data is Vector3) {
			vector2Value = (Vector3) data;
			dataType = Types.Vector3;
		}
	}
}

public class stDictionary : MonoBehaviour {
	
	public string dataName = "Game"; // dataName of data item
	public bool networked = false; // send values across network if true (requires NetworkView component)
	public bool shareWithPlayers = true; // rebroadcast data to all players
	public Dictionary<string, object> dict = new Dictionary<string, object>();

	// dictionaries can't be serialized so simulate one for start values
	[SerializeField] 
	public List<string> startKeys = new List<string>();
	[SerializeField] 
	public List<DictValue> startValues = new List<DictValue>();

	bool startValuesSet;

	void Awake(){
		SetStartValues();
	}

	public void SetStartValues (){
		if (startValuesSet) return; // these can also be set by stData 
		startValuesSet = true;
		for( int i = 0; i < startKeys.Count; i++) {
			if (startKeys[i] == "#ERROR#") {
				continue;
			}
			switch (startValues[i].dataType) {
			case DictValue.Types.Float:
				Set (startKeys[i], startValues[i].floatValue);
				break;
			case DictValue.Types.Int:
				Set (startKeys[i], startValues[i].intValue);
				break;
			case DictValue.Types.String:
				Set (startKeys[i], startValues[i].stringValue);
				break;
			case DictValue.Types.GameObject:
				Set (startKeys[i], startValues[i].objectValue);
				break;
			case DictValue.Types.Vector2:
				Set (startKeys[i], startValues[i].vector2Value);
				break;
			case DictValue.Types.Vector3:
				Set (startKeys[i], startValues[i].vector3Value);
				break;
			}
		}
	}

	void Start(){
		if (networked && networkView == null) {
			Debug.LogError("st Main must have a NetworkView component for networked data");
		}
	}
	
	public bool ContainsKey(string key) {
		return dict.ContainsKey( key);
	}
	
	public object Get (string key) {
		object data = null;
		dict.TryGetValue (key, out data);
		return data;
	}
	
	public string GetString (string key) {
		object data;
		dict.TryGetValue( key, out data); 
		return Convert.ToString(data);
	}
	
	public int GetInt (string key) {
		object data;
		dict.TryGetValue( key, out data); 
		return Convert.ToInt32( data);
	}
	
	public float GetFloat (string key) {
		object data;
		dict.TryGetValue( key, out data); 
		return (float)Convert.ToDouble( data);
	}
	
	public double GetDouble (string key) {
		object data;
		dict.TryGetValue( key, out data); 
		return Convert.ToDouble( data);
	}

	public void Clear(bool allowNetwork = true){
		dict.Clear ();
		if (allowNetwork && networked && (Network.peerType == NetworkPeerType.Client || 
		                                  Network.peerType == NetworkPeerType.Server)) {
			networkView.RPC ("NetClearDictionary", RPCMode.Others, dataName);
		}
	}

	public void Set (string key, object value, bool allowNetwork = true) {
		if (dict.ContainsKey(key) && object.Equals (dict[key], value)) {
			return;
		}
		dict[key] = value;
		if (allowNetwork && networked && (Network.peerType == NetworkPeerType.Client || 
		                                  Network.peerType == NetworkPeerType.Server)) {
			networkView.RPC ("NetSetDictionaryValue", RPCMode.Others, dataName, key, value);
		}
		#if UNITY_EDITOR
		EditorUtility.SetDirty(this);
		#endif
	}

	public bool RemoveKey(string key, bool allowNetwork = true){
		if (dict.ContainsKey(key)) {
			dict.Remove(key);
			if (allowNetwork && networked && (Network.peerType == NetworkPeerType.Client || 
			                                  Network.peerType == NetworkPeerType.Server)) {
				networkView.RPC ("NetRemoveDictionaryKey", RPCMode.Others, dataName, key);
			}
			#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
			#endif
			return true;
		}
		else {
			return false;
		}
	}

# region EventSystem
	//TODO
	public enum ChangeEventType { Set, Remove, Clear};

	public class ChangeEventArgs : EventArgs {
		ChangeEventType eventType;
		string key;
	}

#endregion

	#region JSON 
	public string JSONString() {
		return CreateJSON().ToString ();
	}
	
	public JSONObject CreateJSON ( string[] order=null) { 
		JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
		json.type = JSONObject.Type.OBJECT;
		json.AddField ("dataName", dataName);
		json.AddField ("networked", networked);
		json.AddField ("shareWithPlayers", shareWithPlayers);
		JSONObject dictData = new JSONObject(JSONObject.Type.OBJECT);
		if (order == null) {
			order = dict.Keys.ToArray();
		}
		foreach (string key in order){
			dictData.AddField(key, dict[key]);
		}
		json.AddField("dictData",dictData);
		return json;
	}

	public string FillFromJsonString(string jstring, bool allowNetwork = true) {
		try {
			dict.Clear();
			JSONObject json = JSONObject.Create( jstring);
			json.GetField (ref dataName, "dataName");
			json.GetField (ref networked, "networked");
			json.GetField (ref shareWithPlayers, "shareWithPlayers");
			JSONObject dictData = (JSONObject)json["dictData"];
			for(int i = 0; i < dictData.list.Count; i++){
				string key = (string)dictData.keys[i];
				JSONObject entry = (JSONObject)dictData.list[i];
				switch(entry.type){
				case JSONObject.Type.STRING:
					dict[key] = entry.str;
					break;
				case JSONObject.Type.NUMBER:
					if (entry.isFloat) dict[key] = entry.n;
					else dict[key] = (int)entry.n;
					break;
				case JSONObject.Type.BOOL:
					dict[key] = entry.b;
					break;
				default:
					dict[key] = "(?) "+entry.str;
					break;
				}
			}
		}
		catch (Exception e) {
			Debug.Log (e.ToString ());
			return e.ToString ();
		}
		if (allowNetwork && networked && (Network.peerType == NetworkPeerType.Client || 
		                                  Network.peerType == NetworkPeerType.Server)) {
			networkView.RPC ("NetSetDictionary", RPCMode.Others, dataName, jstring);
		}
		return "";
	}

	public string SaveToFile(string filename="", bool pretty=true, string[] order=null) {
		// filename defaults to dataname
		// pretty makes it easy to read
		// order defines order of keys
		if (filename=="") filename = dataName+".txt";
		try {
			JSONObject json = CreateJSON(order);
			string jstring = json.ToString(pretty);
			File.WriteAllText (Application.persistentDataPath+"/"+filename, jstring);
		}
		catch(Exception e)
		{
			Debug.Log (e.ToString ());
			return e.ToString ();
		}
		return "";
	}
	
	public string LoadFromFile(string filename="") {
		if (filename=="") filename = dataName+".txt";
		try {
			string jstring = File.ReadAllText(Application.persistentDataPath+"/"+filename);
			FillFromJsonString(jstring);
		}
		catch (Exception e) {
			Debug.Log (e.ToString ());
			return e.ToString ();
		}
		return "";	
	}
	#endregion
}
