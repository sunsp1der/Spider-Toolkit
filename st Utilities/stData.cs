/// St data.
/// Quick and easy access to stDictionaries. Also network functionality for said dictionaries.
/// IMPORTANT: Must be attached to stMain object for networking to work

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(stMain))]

public class stData : MonoBehaviour {
	
	static public Dictionary< string, stDictionary> dictionaries = new Dictionary< string, stDictionary>();

	static public object GetDictionaryValue( string dictionaryName, string key) {
		try {
			return dictionaries[dictionaryName].dict[key];
		}
		catch (KeyNotFoundException) {
			Debug.LogError("GetDictionaryValue data not found: "+dictionaryName+"["+key+"]");
			return null;
		}
	}
	static NetworkView _networkViewStatic;
	static NetworkView networkViewStatic{ 
		get{
			if (_networkViewStatic == null) {
				_networkViewStatic = stTools.stMain.GetComponent<NetworkView>();
			}
			return _networkViewStatic;
		}
	}
	
	static public stDictionary AddDictionary( string dataName, bool net=false) {
		stDictionary dict = stTools.stMain.gameObject.AddComponent<stDictionary>(); 
		dict.dataName = dataName;
		dict.networked = net;
		dictionaries.Add (dict.dataName, dict);
		return dict;
	}

	static public bool ChangeDictionaryName( string fromName, string toName) {
		if (GetDictionary (toName)) {
			Debug.Log (toName+" dictionary already exists");
			return false;
		}
		stDictionary dict = GetDictionary( fromName);
		if (dict) {
			dictionaries.Remove(fromName);
			dict.dataName = toName;
			dictionaries.Add(toName, dict);
			return true;
		}
		return false;
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
	
	public static void SetDictionaryValue (string dictionaryName, string key, object value, bool allowNetwork = false) {
		try {
			GetDictionary( dictionaryName).Set(key, value, allowNetwork);
		}
		catch {
			Debug.LogError("Dictionary not found: "+dictionaryName);
			return;
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

	public static void RemoveDictionaryKey (string dictionaryName, string key, bool allowNetwork = false) {
		try {
			GetDictionary( dictionaryName).RemoveKey(key, allowNetwork);
		}
		catch {
			Debug.LogError("Dictionary not found: "+dictionaryName);
			return;
		}
	}

	public static stDictionary SetDictionary (string dictionaryName, string json, bool allowNetwork = true){
		stDictionary dict = GetDictionary(dictionaryName);
		if (dict == null) {
			dict = AddDictionary (dictionaryName, true);
		}
		dict.FillFromJsonString(json, false);
		return dict;
	}

	#region network	
	[RPC]
	public void NetSetDictionaryValue (string dictionaryName, string key, string value) {
		stDictionary dict = GetDictionary (dictionaryName);
		if (dict != null)	{
			SetDictionaryValue(dictionaryName, key, value, false);
		}
	}

	[RPC]
	public void NetSetDictionaryValue (string dictionaryName, string key, NetworkPlayer value) {
		stDictionary dict = GetDictionary (dictionaryName);
		if (dict != null)	{
			SetDictionaryValue(dictionaryName, key, value, false);
		}
	}

	[RPC]
	public void NetSetDictionaryValue (string dictionaryName, string key, int value) {
		stDictionary dict = GetDictionary (dictionaryName);
		if (dict != null)	{
			SetDictionaryValue(dictionaryName, key, value, false);
		}
	}

	[RPC]
	public void NetSetDictionaryValue (string dictionaryName, string key, NetworkViewID value) {
		stDictionary dict = GetDictionary (dictionaryName);
		if (dict != null)	{
			SetDictionaryValue(dictionaryName, key, value, false);
		}
	}

	[RPC]
	public void NetSetDictionaryValue (string dictionaryName, string key, float value) {
		stDictionary dict = GetDictionary (dictionaryName);
		if (dict != null)	{
			SetDictionaryValue(dictionaryName, key, value, false);
		}
	}

	[RPC]
	public void NetSetDictionaryValue (string dictionaryName, string key, Quaternion value) {
		stDictionary dict = GetDictionary (dictionaryName);
		if (dict != null)	{
			SetDictionaryValue(dictionaryName, key, value, false);
		}
	}

	[RPC]
	public void NetSetDictionaryValue (string dictionaryName, string key, Vector3 value) {
		stDictionary dict = GetDictionary (dictionaryName);
		if (dict != null)	{
			SetDictionaryValue(dictionaryName, key, value, false);
		}
	}

	[RPC]
	public void NetRemoveDictionaryKey (string dictionaryName, string key) {
		stDictionary dict = GetDictionary (dictionaryName);
		if (dict != null)	{
			RemoveDictionaryKey(dictionaryName, key, false);
		}
	}
	
	[RPC]
	public void NetSetDictionary (string dictionaryName, string json){
		SetDictionary(dictionaryName, json, false);
//		if (dict.networked && Network.peerType == NetworkPeerType.Server && dict.shareWithPlayers) {
//			networkView.RPC ("NetSetDictionary", RPCMode.Others, dictionaryName, json);
//		}
	}

	[RPC]
	public void NetClearDictionary (string dictionaryName){
		stDictionary dict = GetDictionary(dictionaryName);
		if (dict != null) {
			dict.Clear( false);
//			if (dict.networked && Network.peerType == NetworkPeerType.Server && dict.shareWithPlayers) {
//				networkView.RPC ("NetClearDictionary", RPCMode.Others, dictionaryName);
//			}
		}
	}

//	public void NetRebroadcast(string method, NetworkPlayer source, params object[] args)
//	{
//		foreach (NetworkPlayer player in Network.connections) {
//			if (player == source) {
//				continue;
//			}
//	 		switch (args.Length) {
//			case 0:
//				networkView.RPC (method,)
//			}
//		}
//	}

	public static void SendDictionary(string dictionaryName, RPCMode mode = RPCMode.Others){
		if (mode == RPCMode.All) {
			mode = RPCMode.Others;
			Debug.Log ("SendDictionary: RPCMode changed to RPCMode.Others to avoid endless loop");
		}
		stDictionary dict = GetDictionary (dictionaryName);
		if (dict != null) {
			networkViewStatic.RPC ("NetSetDictionary", mode, dictionaryName, dict.JSONString());
		}
		else {
			Debug.Log ("SendDictionary: dictionary "+dictionaryName+" not found");
		}
	}

	public static void SendDictionary(string dictionaryName, NetworkPlayer player){
		stDictionary dict = GetDictionary (dictionaryName);
		if (dict != null) {
			networkViewStatic.RPC ("NetSetDictionary", player, dictionaryName, dict.JSONString());
		}
		else {
			Debug.Log ("SendDictionary: dictionary "+dictionaryName+" not found");
		}
	}

	#endregion


}
