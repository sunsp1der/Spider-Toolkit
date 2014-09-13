using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MethodButtonAndLockedView : MonoBehaviour {

	[Tooltip("The object to spawn when spawn button is pressed in component")]
	public GameObject spawnObject;

	// MethodButton creates a button in the editor to run a function on this component.
	// Simply name the MethodButton variable "_"+<methodname>
	[Tooltip("Call the spawn method of this component")]
	public MethodButton _Spawn;  
	
	// LockedView creates a view of a variable that you don't want the user to be able to edit
	[Tooltip("Number of objects spawned")]
	public LockedView _spawnedCount;

	int spawnedCount;

	List<GameObject> spawnedObjects = new List<GameObject>();
	
	void Spawn() {
		GameObject ob = stTools.Spawn(spawnObject); // using stTools.Spawn makes st object features work properly
		ob.transform.position = new Vector3(Random.Range(-3,3), Random.Range(-3,3));
		spawnedObjects.Add (ob); // put the object in our list
		spawnedCount = spawnedObjects.Count;
		print (spawnedCount); // just for giggles
	}
}
