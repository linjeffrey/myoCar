using UnityEngine;
using System.Collections;

public class JCollide : MonoBehaviour {
	public GameObject prefab;
	public Collider handleMe;

	void OnCollisionEnter(Collision c) {
		int len = c.contacts.Length;
		foreach (ContactPoint p in c.contacts) {
			float mag = c.relativeVelocity.magnitude;
			if (((p.thisCollider == handleMe) || (p.otherCollider == handleMe))) {
				GameObject go = (GameObject) Instantiate(prefab, p.point, Quaternion.identity);
				ParticleEmitter e = go.particleEmitter; 
				e.minEmission = mag * mag * 10 / len;
				e.maxEmission = mag * mag * 10 / len;
				e.emit = true;
				e.emitterVelocityScale = 0.01f * mag / len;
			}
		}
	}
}
