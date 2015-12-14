<<<<<<< HEAD
﻿#pragma strict

var bswitch : boolean;
var coll : Collider;
var coll2: Collider;

function Start() {
	bswitch = true;
}

function OnTriggerEnter(other : Collider) {
	coll = other;
	if (coll.attachedRigidbody) {
		coll.attachedRigidbody.constraints = RigidbodyConstraints.FreezeAll;
	}
}

function OnTriggerStay(other : Collider) {
	coll2 = other;
}

function Update() {
	if (bswitch == false) {
		coll2.attachedRigidbody.constraints = RigidbodyConstraints.None;
	}
=======
﻿#pragma strict

var bswitch : boolean;
var coll : Collider;
var coll2: Collider;

function Start() {
	bswitch = true;
}

function OnTriggerEnter(other : Collider) {
	coll = other;
	if (coll.attachedRigidbody) {
		coll.attachedRigidbody.constraints = RigidbodyConstraints.FreezeAll;
	}
}

function OnTriggerStay(other : Collider) {
	coll2 = other;
}

function Update() {
	if (bswitch == false) {
		coll2.attachedRigidbody.constraints = RigidbodyConstraints.None;
	}
>>>>>>> cc58b2cb32f6563ea23f0550281efd5fb4b5637f
}