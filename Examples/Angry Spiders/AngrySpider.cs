/// Specific to Angry Spiders Example
/// manages launching and destruction of the angry spider

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SetMotion))] 
[RequireComponent(typeof(Rigidbody2D))] 
public class AngrySpider : MonoBehaviour {

	public string launchKey = "Fire1";
	public float multiplier = 10; // multiply Game Thrust by this to get start velocity

	bool ready = true; // only launch once
	bool hasFallen = false; // destroy when we reach still point, but after top of arc

	void OnRemove(){
		stData.SetDictionaryValue("Game","Thrust",1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 v =  rigidbody2D.velocity;
		if (v.magnitude > 0) {
			if (v.y > 0) {
				hasFallen = true;
			}
		}
		else if (hasFallen){
			stTools.Remove(gameObject);
		}

		if (ready) {
			if (Input.GetButtonUp(launchKey)) {
				ready = false;
				SetMotion motion = GetComponent<SetMotion>();
				motion.speed = multiplier * (float) stData.GetDictionaryValue("Game","Thrust");
				motion.DoSetMotion();
				GetComponent<ControlsRotateDrive>().enabled = false;
				GetComponent<MouseFace>().enabled = false;
				rigidbody2D.gravityScale = 1;
			}
		}
	}

	void OnCollisionEnter2D (Collision2D info) {
		FaceMotion faceMotion = GetComponent<FaceMotion>();
		if (faceMotion) faceMotion.enabled = false;
	}
}