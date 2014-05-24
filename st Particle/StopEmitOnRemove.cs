using UnityEngine;
using System.Collections;


[AddComponentMenu("st Particles/Stop Emit On Remove")]

[RequireComponent(typeof(ParticleSystem))]

public class StopEmitOnRemove : MonoBehaviour {

	public bool detachFromParent = true;
	public float destroyInSecs = 5; // if greater than zero, 
							 		// particle will automatically be destroyed in this many seconds

	void OnRemove(){
		particleSystem.enableEmission = false;
		if (detachFromParent) transform.parent = null;
		if (destroyInSecs > 0) Destroy (gameObject, destroyInSecs);
	}
}
