using UnityEngine;
using System.Collections;

public class SetColor : MonoBehaviour {

	public float red = 1;
	public float green = 1;
	public float blue = 1;
	public MethodButton _DoSetColor;

	public byte r255 = 255;
	public byte g255 = 255;
	public byte b255 = 255;
	public byte a255 = 255;
	public MethodButton _DoSetColor32;

	void DoSetColor () {
		GetComponent<SpriteRenderer>().color = new Color(red, green, blue);
	}
	
	void DoSetColor32 () {
		GetComponent<SpriteRenderer>().color = new Color32(r255, g255, b255, a255);
	}

}
