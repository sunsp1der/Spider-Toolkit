using UnityEngine;
using System.Collections;

[AddComponentMenu("st Particles/Destroy When Particle Done")]

[RequireComponent(typeof(ParticleSystem))]
public class DestroyWhenParticleDone : MonoBehaviour {

// destroy the particle system after it's 'duration' passes

	void Start () {
		Invoke("DestroyParticle", GetComponent<ParticleSystem>().duration);
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
