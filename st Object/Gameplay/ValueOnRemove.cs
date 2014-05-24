using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Gameplay/Value On Remove")]

public class ValueOnRemove : MonoBehaviour {
	// when object is removed, change an int value

	public int amount = 1;
	public ComponentField value;

	void OnRemove() {
		value.SetValue( amount + value.GetInt());
	}
}
