// main game object for ST based games
// the stMain object loaded in the first scene will persist through all levels... 
// stMain objects in future level files will be ignored
// stMain objects have to be tagged for persistence to work.

using UnityEngine;
using System.Collections;

public class stMain : MonoBehaviour {

	[Tooltip("Automatically add basic utility keys.")]
	public int audioSources = 4;
	public bool utilityKeys = true; 
	/*
	 * Restart: ctrl-R
	 * Pause: ctrl-P
	 * Quit: ctrl-Q
	 */
	[HideInInspector]
	public bool undying = false;

	string startLevel = "";

	void Awake() {
		stData.SetupDictionaries();
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

	void Start () {
		if (!gameObject.CompareTag("st Main")) {
			Debug.LogWarning("st Main object needs to have tag: 'st Main'");
		}
		if (startLevel == ""){
			startLevel = Application.loadedLevelName;
		}
	}

	void Update() {
		if (utilityKeys) {
			if (!Input.GetKey (KeyCode.LeftControl) && !Input.GetKey (KeyCode.RightControl) &&
			    !Input.GetKey (KeyCode.LeftCommand) && !Input.GetKey(KeyCode.RightCommand)) {
				return;
			}
			if (Input.GetKeyDown (KeyCode.Q)){
				if (Application.isEditor) {
					UnityEditor.EditorApplication.isPlaying = false;
				}
				else {
					Application.Quit ();
				}
			}
			if (Input.GetKeyDown (KeyCode.R)) {
				Application.LoadLevel (startLevel);
			}
			if (Input.GetKeyDown (KeyCode.P)) {
				if (Time.timeScale == 1) {
					Time.timeScale = 0;
				}
				else {
					Time.timeScale = 1;
				}
			}
		}
	}

	public void CheckEndScene() {
		if (stTools.endSceneAtTime == 0 || stTools.endSceneAtTime <= Time.time) {
			stTools.DoEndScene ();
		}
		else if (stTools.endSceneAtTime > Time.time) {
			Invoke ("CheckEndScene", stTools.endSceneAtTime - Time.time);
		}
	}


}
