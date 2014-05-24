using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("st Object/Spawners/Spawner")]

public class Spawner : MonoBehaviour {
	// object spawns other objects
	// this base spawner can only be activated via script. Just call StartSpawning()
	
	public GameObject spawnObject = null; // object to spawn
	public Sound sound; // sound to play for spawn
	
	[System.Serializable]
	public class Timing { 
		public float interval = 2.0f; // seconds until next spawn
		public float intervalVariance = 1.0f; // interval can vary this many seconds
		public float delay = 0; // Wait this many seconds before beginning to spawn
	}
	public Timing timing;
	
	[System.Serializable]
	public class Count {
		public int numToSpawn = 1; // Number of objects created per spawn
		public int numToSpawnVariance = 0; // numToSpawn can vary by this much
		public int maxSpawnsInScene = -1; // Maximum number of spawned objects in scene at one time (< 0 = unlimited)
		public int totalNumToSpawn = -1; // Total number of objects owner can create over its lifetime (<0 = unlimited)
		public bool deleteWhenDone = false; // Delete this object after totalObjectsSpawned have been spawned
	}
	public Count count;

	[System.Serializable]
	public class Placement {
		public Vector2 offset = new Vector2(0,0); // offset from center of spawner, in world units
		                                          // offset will be rotated with spawner.
		public Vector2 offsetVariance = new Vector2(0,0); //offset can vary this much
		public float angle = 0; // Angle to add to spawned object's rotation (after offset)
		public float angleVariance = 0; // angle can vary by this much
		public bool addRotation = true; // Add owner's rotation to spawned object's rotation
		public bool addVelocity = false; // Add owner's velocity to spawned object's velocity"],
		public bool multiplyScale = false; // Multiply spawned object's scale by owner's scale
		public bool sortToBottom = true; // move object below other objects after spawning,
		public bool onspawnCallback = false; // SendMessage OnSpawn(obj) to this object
		public bool onspawnedCallback = false; // SendMessage OnSpawned() to spawned objects
	}
	public Placement placement;

	[HideInInspector]
	public int spawnCount = 0; // total objects spawned by this spawner
	[HideInInspector]
	public List<GameObject> spawnedObjects; // list of spawned objects currently in scene
	[HideInInspector]
	public List<GameObject> justSpawned; // object(s) created at last spawn

	public MethodButton _Spawn;// = new MethodButton("Do Spawn");

	static int ord = 0;

	public List<GameObject> Spawn(){
		// track newly spawned objects
		justSpawned.Clear ();
		// make sure have something to spawn
		if (spawnObject == null){
			return justSpawned;
		}
		// clean up list of spawnedObjects
		for (int i = spawnedObjects.Count - 1; i >= 0; i--) {
			if (spawnedObjects[i] == null) {
				spawnedObjects.RemoveAt(i);
			}
		}
		// spawning
		int numToSpawn = count.numToSpawn + Random.Range(-count.numToSpawnVariance, count.numToSpawnVariance);
		for (int i = 0; i < numToSpawn; i++){
			// check if we're done spawning
			if (count.totalNumToSpawn > -1 && count.totalNumToSpawn <= spawnCount) {
				if (count.deleteWhenDone){
					Destroy (gameObject);
				}
				break;
			}
			// check if there are too many in scene already
			if (count.maxSpawnsInScene > -1 && spawnedObjects.Count >= count.maxSpawnsInScene) {
				break;
			}

			// ACTUAL CREATION OF OBJECT HERE
			// create and track object
			GameObject obj = stTools.Spawn( spawnObject);

			// sort new object to bottom
			if (placement.sortToBottom) {
				SpriteRenderer objrenderer = obj.GetComponent<SpriteRenderer>();
				if (objrenderer) {
					objrenderer.sortingOrder = ord--;
				}
			}
			// track objects
			spawnedObjects.Add(obj);
			justSpawned.Add (obj);
			spawnCount++;
			// set up position and rotation
			Quaternion oldrot = obj.transform.rotation;
			obj.transform.position = gameObject.transform.position;
			obj.transform.position += new Vector3(
					placement.offset.x + Random.Range (-placement.offsetVariance.x, placement.offsetVariance.x),
					placement.offset.y + Random.Range (-placement.offsetVariance.y, placement.offsetVariance.y),0);
			obj.transform.RotateAround( gameObject.transform.position, Vector3.forward, gameObject.transform.rotation.eulerAngles.z);
			if (!placement.addRotation) {
				obj.transform.rotation = oldrot;
			}
			obj.transform.Rotate( 0, 0, -placement.angle + Random.Range(-placement.angleVariance,
			                                                         placement.angleVariance));
			// set up velocity
			if (placement.addVelocity) { 
				if (obj.rigidbody2D != null) {
					obj.rigidbody2D.velocity += gameObject.rigidbody2D.velocity;
				}
			}
			// set up scale
			// callbacks
			if (placement.onspawnedCallback)
			{
				obj.SendMessage( "OnSpawned", SendMessageOptions.DontRequireReceiver);
			}
			if (placement.onspawnCallback)
			{
				gameObject.SendMessage( "OnSpawn", obj, SendMessageOptions.DontRequireReceiver);
			}
		}
		if (justSpawned.Count != 0 && sound != null) {
			sound.Play();
		}
		return justSpawned;
	}

	protected List<GameObject> CheckSpawn( bool scheduleNext = true){
		// do a spawn if criteria are met
		// This method checks against count.maxSpawnsInScene and spawnCount. Another spawn will be
		//	scheduled unless totalNumToSpawn has been reached. """
		// are we done spawning?
		if (count.totalNumToSpawn > -1 && spawnCount > count.totalNumToSpawn) {
			justSpawned.Clear();
			return justSpawned;
		}
		justSpawned = Spawn();
		// schedule next spawn
		if (scheduleNext) {
			Invoke ( "DoCheckSpawn", GetNextSpawnWait());
		}
		return justSpawned;
	}

	protected void DoCheckSpawn(){
		// Invoke methods can't have arguments so this is just a simple call to CheckSpawn
		CheckSpawn();
	}

	protected void Awake() {
		spawnedObjects = new List<GameObject>();
		justSpawned = new List<GameObject>();
	}
	
	public void StartSpawning(){
		Invoke ( "DoCheckSpawn", timing.delay);
	}

	protected float GetNextSpawnWait(){
		if (timing.interval == 0 && timing.intervalVariance == 0) return 0.5f;
		float wait = timing.interval + Random.Range(-timing.intervalVariance, timing.intervalVariance);
		return Mathf.Max (0, wait);
	}
	
	public void StopSpawning () {
		CancelInvoke("DoCheckSpawn");
	}
}
