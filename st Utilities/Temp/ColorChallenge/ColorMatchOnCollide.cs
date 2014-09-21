using UnityEngine;
using System.Collections;

public class ColorMatchOnCollide : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D info) {
		GetComponent<SpriteRenderer>().color = info.gameObject.GetComponent<SpriteRenderer>().color;
	}
}
