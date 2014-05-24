using UnityEngine;
using System.Collections;

[AddComponentMenu("st Particles/Emit On Move")]

[RequireComponent(typeof(ParticleSystem))]
public class EmitOnMove : MonoBehaviour {
	// particle emitter only on when moving

	Vector3 position;
	ParticleSystem particles;

	void Start () {
		position = transform.position;
		particles = GetComponent<ParticleSystem>();
		particles.enableEmission=false;
	}
	
	void Update () {
		if (position == transform.position) {
			particles.enableEmission=false;
		}
		else {
			particles.enableEmission=true;
		}	
		position = transform.position;
	}
}
