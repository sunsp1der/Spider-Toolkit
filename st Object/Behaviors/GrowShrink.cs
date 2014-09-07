using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Behaviors/Grow Shrink")]
[RequireComponent(typeof(SpriteRenderer))]

public class GrowShrink : MonoBehaviour {
	// Grow object in on creation, and shrink out on removal
	
	public float shrinkInSeconds = 3; // 0 is no fade in
	public float shrinkOutSeconds = 3; // 0 is no fade out
	
	void Start () {
		if (shrinkInSeconds > 0) {
			ShrinkIn( shrinkInSeconds);
		}
	}
	
	void ShrinkIn( float seconds = -1, Vector3 targetScale = default(Vector3)){
		if (seconds < 0) {
			seconds = shrinkInSeconds;
		}
		if (seconds == 0) {
			return;
		}
		if (targetScale == default(Vector3)) {
			targetScale = transform.localScale;
		}
		transform.localScale = Vector3.zero;
		StartCoroutine(ScaleTo (targetScale, seconds));
	}
	
	void OnRemove(){
		if (!enabled) return;
		if (shrinkOutSeconds > 0) {
			// delay the actual removal
			GetComponent<stObject>().DelayRemoval( shrinkOutSeconds);
			// do the fade
			StartCoroutine(ScaleTo (Vector3.zero, shrinkOutSeconds));
		}
	}
	
	IEnumerator ScaleTo(Vector3 targetScale, float seconds)
	{
		Vector3 scale = transform.localScale;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / seconds)
		{
			transform.localScale = Vector3.Lerp(scale, targetScale, t);
			yield return null;
		}
		transform.localScale = targetScale;
	}
}
