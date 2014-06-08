using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Behaviors/Fade")]
[RequireComponent(typeof(SpriteRenderer))]

public class Fade : MonoBehaviour {
	// Fade object in on creation, and out on removal
	
	public float fadeInSeconds = 3; // 0 is no fade in
	public float fadeOutSeconds = 3; // 0 is no fade out

	SpriteRenderer spriteRenderer;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Start () {
		if (fadeInSeconds > 0) {
			FadeIn( fadeInSeconds);
		}
	}

	void FadeIn( float seconds = -1, float targetAlpha = -1){
		if (seconds < 0) {
			seconds = fadeInSeconds;
		}
		if (seconds == 0) {
			return;
		}
		if (targetAlpha < 0) {
			targetAlpha = spriteRenderer.color.a;
		}
		spriteRenderer.color = new Color( spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b,
		                           0);
		StartCoroutine(FadeTo (targetAlpha, seconds));
	}

	void OnRemove(){
		if (!enabled) return;
		if (fadeOutSeconds > 0) {
			// delay the actual removal
			GetComponent<stObject>().DelayRemoval( fadeOutSeconds);
			// do the fade
			StartCoroutine(FadeTo (0, fadeOutSeconds));
		}
	}

	IEnumerator FadeTo(float targetAlpha, float seconds)
	{
		float alpha = spriteRenderer.color.a;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / seconds)
		{
			spriteRenderer.color = new Color( spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b,
			                           Mathf.Lerp(alpha,targetAlpha,t));
			yield return null;
		}
	}
}
