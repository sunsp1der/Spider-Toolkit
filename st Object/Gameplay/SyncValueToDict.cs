using UnityEngine;
using System.Collections;
using System;
[AddComponentMenu("st Object/Gameplay/Sync Value To Dict")]

/// <summary>
/// on update and awake, the chosen value is set to an stDictionary value
/// </summary>
public class SyncValueToDict : SetAwakeValueToDict {

	// this is in a derived component because it's slow to use update method. SetField is slow enough already!

	void Update (){
		SetField ();
	}

} 
