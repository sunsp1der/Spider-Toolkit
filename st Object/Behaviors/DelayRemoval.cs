using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Behaviors/Delay Removal")]

[RequireComponent(typeof(stObject))]

public class DelayRemoval : MonoBehaviour {
	// simply delay Destroying this object when Remove is called

	public float delaySecs = 1;

	void OnRemove(){
		GetComponent<stObject>().DelayRemoval( delaySecs);

	}
}
