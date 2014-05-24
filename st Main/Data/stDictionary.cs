using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AddComponentMenu("st Main/Data/st Dictionary")]
[RequireComponent(typeof(stMain))] 

// NOTE: this component has a specialized editor

/// <summary>
/// St dictionary. A monobehavior that wraps a dictionary. Data can be of various kinds... up to 
/// user to track. Get+type functions will return default values if key is not found. If you
/// need to test whether a key exists, use the ContainsKey method.
/// </summary>

public class stDictionary : MonoBehaviour {

	public string dataName = "Game"; // dataName of data item
	public bool networked = false; // send values across network if true (requires NetworkView component)
	public Dictionary<string, object> dict = new Dictionary< string, object>();

	void Start(){
		if (networked && networkView == null) {
			Debug.LogError("st Main must have a NetworkView component for networked data");
		}
	}

	public bool ContainsKey(string key) {
		return dict.ContainsKey( key);
	}

	public object Get (string key) {
		object data;
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

	public void Set (string key, object value, bool allowNetwork = true) {
		if (dict.ContainsKey(key) && object.Equals (dict[key], value)) {
			return;
		}
		dict[key] = value;
		if (allowNetwork && networked && Network.peerType != NetworkPeerType.Disconnected && Network.peerType != NetworkPeerType.Connecting) {
			networkView.RPC ("SetDictValue", RPCMode.All, dataName, key, value);
		}
#if UNITY_EDITOR
		EditorUtility.SetDirty(this);
#endif
	}

	public JSONObject CreateJSON ( string[] order=null) { 
		JSONObject json = new JSONObject();
		if (order == null) {
			order = dict.Keys.ToArray();
			foreach (string key in order){
				json.AddField(key, dict[key]);
			}
		}
		return json;
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
			JSONObject json = JSONObject.Create( jstring);
			for(int i = 0; i < json.list.Count; i++){
				string key = (string)json.keys[i];
				JSONObject entry = (JSONObject)json.list[i];
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
		return "";
	}
}
