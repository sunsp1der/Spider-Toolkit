using UnityEngine;
using System.Collections;

public class stObject : MonoBehaviour {
	// Base component for Spider Toolkit objects
	// Add's "Remove" method, which destroys an object but allows delaying and blocking for effects like Fade
	// or Shrink. Also sends a Remove callback to all children.

	[Tooltip("Deactivate object at level start, but keep spawnable.")]
	public bool archetype = false; // if true, object will be deactivated when game starts
								   // to be used for spawning and stTools.Spawn.
								   // when deactivated, this value will become false and 
								   // myArchetype will be set to this gameObject

	[HideInInspector]
	public GameObject myArchetype; // if copied from an archetype  
	[Tooltip("Archetype this object was spawned from.")]
	public LockedView _myArchetype;// = new LockedView("Archetype object this was spawned from");

	float destroyAtTime = 0;
	bool removing = false;

	void Awake () {
		if (archetype) {
			ArchetypeAwake();
		}
	}

	/// <summary>
	/// Set object up as an archetype. 
	/// Call this from awake function of other components if you want to force an object to be an archetype.
	/// </summary>
	public void ArchetypeAwake(){
		myArchetype = gameObject;
		archetype = false;
		SendMessage("ArchetypeStart",SendMessageOptions.DontRequireReceiver);
		gameObject.SetActive(false);
	}

	/// <summary>
	/// Remove this object.
	/// </summary>
	/// <param name="delay">Seconds to delay removal. Negative means immediately. Zero is at end of frame.</param>
	public void Remove( float delay ) {
		if (removing) return;
		Invoke ("Remove", delay);
	}

	/// <summary>
	/// Remove this object.
	/// </summary>
	public void Remove(){
		if (removing) return;
		removing = true;
		BroadcastMessage ("OnRemove", SendMessageOptions.DontRequireReceiver);
		// destroyAtTime allows other objects to delay removal using the DelayRemoval method below.
		if (destroyAtTime == 0 || destroyAtTime <= Time.time) {
			Destroy ( gameObject);
		}
	}


	/// <summary>
	/// Delays the removal of this object.
	/// This is usually called in the OnRemove callback. Useful for fadeout type effects.
	/// </summary>
	/// <param name="delay">Delay.</param>
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
