#pragma strict

var target : calloul;

function OnTriggerStay(other : Collider) {
	if(other.attachedRigidbody) {
		target.doorClear = false;
	}
}

function OnTriggerExit(other : Collider) {
	if(other.attachedRigidbody) {
		target.doorClear = true;
	}
}