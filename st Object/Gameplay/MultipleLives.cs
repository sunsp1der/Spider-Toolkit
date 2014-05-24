using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Gameplay/Multiple Lives")]
[RequireComponent(typeof(stObject))]


public class MultipleLives : MonoBehaviour {
	// This object has multiple lives and will respawn at starting point
	// after being Destroyed. stObject.archetype will be set to true on 
	// original object.

	public int lives = 3; // number of lives 
	public float pauseBeforeSpawn = 2; // number of seconds before respawn
	public GameObject livesGoneObject; // spawn this object when lives run out
								       // if None, don't spawn anything. To go to 
								       // a new level, spawn an object with a 
	                                   // Level Timer component
	
	MultipleLives multipleLivesArchetype;

	void Awake() {
		if (multipleLivesArchetype == null) {
			// we are in original archetype. Store this component for future lives
			lives--;
			multipleLivesArchetype = this;
		}
		else {
			// use this component from the original archetype
			lives = multipleLivesArchetype.lives;
		}
	}

	// special called by stObject
	void ArchetypeStart() { 
		Respawn ();
	}

	void OnDestroy () {
		if (multipleLivesArchetype == null) {
			return;
		}
		if (multipleLivesArchetype.lives == 0) {
			// all out of lives
			if (livesGoneObject != null) {
				multipleLivesArchetype.Invoke("LivesGone", pauseBeforeSpawn);
			}
		}
		else {
			// next life
			multipleLivesArchetype.lives--;
			// have to use other object because this one is being removed
			multipleLivesArchetype.Invoke("Respawn", pauseBeforeSpawn);
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
