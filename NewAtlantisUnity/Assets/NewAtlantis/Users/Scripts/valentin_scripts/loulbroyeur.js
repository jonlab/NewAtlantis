#pragma strict

var target : ParticleSystem;

function OnTriggerExit(coll : Collider) {
	Destroy(coll.attachedRigidbody.gameObject);
	target.Play();
}