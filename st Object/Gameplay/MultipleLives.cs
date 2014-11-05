using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Gameplay/Multiple Lives")]
[RequireComponent(typeof(stObject))]

/// <summary>
/// This object has multiple lives and will respawn at starting point
/// after being Destroyed. stObject.archetype will be set to true on 
/// original object.
/// </summary>
public class MultipleLives : MonoBehaviour {

	[Tooltip("Number of times to respawn. -1 is infinite.")]
	public int lives = 3; 
	[Tooltip("Number of seconds after removal to respawn.")]
	public float secsBeforeSpawn = 2; // no pun intended
	[Tooltip("After last life is gone, this object is spawned. To go to a new level, spawn an object with LevelTimer component.")]
	public GameObject livesGoneObject; 

	MultipleLives multipleLivesArchetype;

	void Awake() {
		if (GetComponent<stObject>().archetype == false && GetComponent<stObject>().myArchetype == null) {
			GetComponent<stObject>().ArchetypeAwake();
		}
		if (multipleLivesArchetype != null) {
			lives = multipleLivesArchetype.lives;
		}
	}

	// special called by stObject
	void ArchetypeStart() { 
		// we are in original archetype. Store this component for future lives
		if (lives>0) {
			lives--;
		}
		multipleLivesArchetype = this;
		Respawn ();
	}

	void OnDestroy () {
		if (multipleLivesArchetype == null) {
			return;
		}
		if (multipleLivesArchetype.lives == 0) {
			// all out of lives
			if (livesGoneObject != null) {
				multipleLivesArchetype.Invoke("LivesGone", 0);
			}
		}
		else {
			// next life
			if (multipleLivesArchetype.lives>0) {
				multipleLivesArchetype.lives--;
			}
			// have to use other object because this one is being removed
			multipleLivesArchetype.Invoke("Respawn", secsBeforeSpawn);
			//multipleLivesArchetype
		}
	}

	void Respawn () {
		GameObject clone = stTools.Spawn ( multipleLivesArchetype.gameObject);
		clone.GetComponent<MultipleLives>().multipleLivesArchetype = this;
	}

	void LivesGone () {
		stTools.Spawn( livesGoneObject);
	}
}
