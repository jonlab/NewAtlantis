<<<<<<< HEAD
﻿#pragma strict

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
=======
﻿#pragma strict

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
>>>>>>> cc58b2cb32f6563ea23f0550281efd5fb4b5637f
}