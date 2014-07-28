using UnityEngine;
using System.Collections;

[AddComponentMenu("st Particles/Destroy When Particle Done")]

[RequireComponent(typeof(ParticleSystem))]
public class DestroyWhenParticleDone : MonoBehaviour {

	[Tooltip("Add this to the particle's duration to determine total lifetime.")]
	public float extraDelay = 0;

// destroy the particle system after it's 'duration' passes

	void Start () {
		Invoke("DestroyParticle", GetComponent<ParticleSystem>().duration + extraDelay);
	}
	
	void DestroyParticle () {
		stObject obj = GetComponent<stObject>();
		if (obj != null) {
			obj.Remove();
		}
		else {
			Destroy(gameObject);
		}
	}
}
