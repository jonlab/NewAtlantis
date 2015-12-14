<<<<<<< HEAD
﻿#pragma strict

var target : ParticleSystem;

function OnTriggerExit(coll : Collider) {
	Destroy(coll.attachedRigidbody.gameObject);
	target.Play();
=======
﻿#pragma strict

var target : ParticleSystem;

function OnTriggerExit(coll : Collider) {
	Destroy(coll.attachedRigidbody.gameObject);
	target.Play();
>>>>>>> cc58b2cb32f6563ea23f0550281efd5fb4b5637f
}