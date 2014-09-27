using UnityEngine;
using System.Collections;

public class RandColor : MonoBehaviour {

	public bool total255;
	public bool lerpToNew;
	public float lerpTime = 2;
	Color32 startColor, targetColor;
	float startTime;

	
	Color32 randomColor(){
		byte r,g,b;
		if (total255) {
			r = (byte)Random.Range (0,256);
			g = (byte)Random.Range (0,256 - r);
			b = (byte) (255 - r - g);
		}
		else {
			r = (byte)Random.Range (0,256);
			g = (byte)Random.Range (0,256);
			b = (byte)Random.Range (0,256);
		}
		return new Color32(r,g,b,255);
	}

	// Use this for initialization
	void Start () {
		startColor = randomColor ();
		GetComponent<SpriteRenderer>().color = startColor;
		if (lerpToNew) {
			Invoke ("LerpColor", 0);
		}
	}

	void LerpColor() {
		targetColor = randomColor();
		print (startColor+ " to "+ targetColor);
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		float elapsedTime = Time.time - startTime;
		float fraction = elapsedTime / lerpTime;
		GetComponent<SpriteRenderer>().color = Color32.Lerp( startColor, targetColor, fraction);
	}

}
