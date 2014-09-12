using UnityEngine;
using System.Collections;
using System;

[AddComponentMenu("st Main/Set Value To Dict")]

// on update and awake, the chosen value is set to an stDictionary value

public class SetValueToDict : SetAwakeValueToDict {

	// this is in a derived component because it's slow to use update method. SetField is slow enough already!

	void Update (){
		SetField ();
	}

} 
