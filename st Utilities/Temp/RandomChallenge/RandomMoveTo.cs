using UnityEngine;
using System.Collections;

public class RandomMoveTo : MonoBehaviour {

	// Use this for initialization
	public bool total255 = true;
	Color32 startColor, targetColor;
	Vector3 startPosition, targetPosition;
	float startTime;
	public float moveTime = 1;

	void Start () {
		Invoke("StartMove",0);
		startColor = new Color32(255,255,255,255);
		targetColor = randomColor();
	}

	void StartMove(){
		startPosition = transform.position;
		targetPosition= new Vector2(Random.Range (-6.5f,6.5f), Random.Range (-5,5));
		startTime = Time.time;

	}
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
	// Update is called once per frame
	void Update () {
		float elapsedTime = Time.time -startTime;
		float fraction = elapsedTime/moveTime;
		transform.position = Vector3.Lerp (startPosition, targetPosition, fraction);
		GetComponent<SpriteRenderer>().color = Color32.Lerp(startColor, targetColor, fraction);
	}
}

