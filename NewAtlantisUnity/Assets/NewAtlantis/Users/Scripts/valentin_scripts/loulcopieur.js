#pragma strict

var coll;
var bswitch : boolean;
var cswitch : boolean;
var posold : Vector3;

function Start() {
	bswitch = false;
	cswitch = true;
	InvokeRepeating("boolswitch", 0, 1);
}

function Update() {
	posold = this.transform.position;
}

function boolswitch() {
	if (bswitch == true && cswitch == true) {
		clone();
	}
}

function clone () {
	var posnew = Vector3(posold.x, posold.y + 3.5, posold.z);
	Instantiate(coll, posnew, this.transform.rotation);
}

function OnTriggerStay (other : Collider) {
	if (other.attachedRigidbody) {
		bswitch = true;
		coll = other;
	} else {
		bswitch = false;
	}
}