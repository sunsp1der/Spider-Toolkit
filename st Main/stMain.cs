using UnityEngine;
using System.Collections;

public class stMain : MonoBehaviour {
	// main game object for ST based games
	// the stMain object loaded in the first scene will persist through all levels... 
	// stMain objects in future level files will be ignored

	public bool utilityKeys = true; 
	/*
	 * Restart: R
	 * Pause: P
	 */
	[HideInInspector]
	public bool undying = false;

	string startLevel = "";
	void Awake() {
		stData.SetupDictionaries();
	}

	void Start () {
		if (!gameObject.CompareTag("st Main")) {
			Debug.LogWarning("st Main object needs to have tag: 'st Main'");
		}
		if (startLevel == ""){
			startLevel = Application.loadedLevelName;
		}
		GameObject[] mains = GameObject.FindGameObjectsWithTag("st Main");
		if ( mains.Length > 1) {
			for (int i = mains.Length-1; i>=0; i--) {
				GameObject main = mains[i];
				if (main.GetComponent<stMain>().undying) {
					main.transform.parent = Camera.main.transform;
				}
				else {
					Destroy( main);
				}
			}
		}
		DontDestroyOnLoad(this);
	}

	void Update() {
		if (utilityKeys) {
			if (Input.GetKeyDown (KeyCode.R))
			{
				Application.LoadLevel (startLevel);
			}
			if (Input.GetKeyDown (KeyCode.P))
			{
				if (Time.timeScale == 1) {
					Time.timeScale = 0;
				}
				else {
					Time.timeScale = 1;
				}
			}
		}
	}

}
