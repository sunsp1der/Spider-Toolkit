using UnityEngine;
using System.Collections;

public class stObject : MonoBehaviour {
	// Base component for Spider Toolkit objects
	// Add's "Remove" method, which destroys an object but allows delaying and blocking for effects like Fade
	// or Shrink. Also sends a Remove callback to all children.

	public bool archetype = false; // if true, object will be deactivated when game starts
								   // to be used for spawning and stTools.Spawn.
								   // when deactivated, this value will become false and 
								   // myArchetype will be set to this gameObject

	[HideInInspector]
	public GameObject myArchetype; // if copied from an archetype  
	public LockedView _myArchetype;// = new LockedView("Archetype object this was spawned from");

	float destroyAtTime = 0;
	bool removing = false;

	void Awake () {
		if (archetype) {
			ArchetypeAwake();
		}
	}

	public void ArchetypeAwake(){
		myArchetype = gameObject;
		archetype = false;
		SendMessage("ArchetypeStart",SendMessageOptions.DontRequireReceiver);
		gameObject.SetActive(false);
	}

	public void Remove( float seconds=0 ) {
		// delay: seconds until removal
		if (removing) return;
		if ( seconds > 0) {
			Invoke ("Remove", seconds);
			return;
		}
		removing = true;
		BroadcastMessage ("OnRemove", SendMessageOptions.DontRequireReceiver);
		if (destroyAtTime == 0 || destroyAtTime <= Time.time) {
			Destroy ( gameObject);
		}
	}

	public void DelayRemoval( float delay) {
		// delay: seconds to wait to destroy
		if (delay <= 0) return;
		float dTime = delay + Time.time;
		if (dTime > destroyAtTime) {
			destroyAtTime = delay + Time.time;
			CancelInvoke( "CheckDestroy");
			Invoke ("CheckDestroy", delay);
		}
	}

	void CheckDestroy() {
		if (destroyAtTime == 0 || destroyAtTime <= Time.time) {
			Destroy ( gameObject);
		}
		else if (destroyAtTime > Time.time) {
			Invoke ("CheckDestroy", destroyAtTime - Time.time);
		}
	}
}
