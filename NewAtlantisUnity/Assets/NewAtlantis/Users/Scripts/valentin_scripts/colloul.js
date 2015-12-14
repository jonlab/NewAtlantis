#pragma strict

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
}