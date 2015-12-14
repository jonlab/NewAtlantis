<<<<<<< HEAD
﻿#pragma strict

var target : colloul;
var angle : float;

function Update() {
	angle = this.GetComponent.<HingeJoint>().angle;
	if (angle > 10) {
		target.bswitch = false;
	} else {
		target.bswitch = true;
	}
=======
﻿#pragma strict

var target : colloul;
var angle : float;

function Update() {
	angle = this.GetComponent.<HingeJoint>().angle;
	if (angle > 10) {
		target.bswitch = false;
	} else {
		target.bswitch = true;
	}
>>>>>>> cc58b2cb32f6563ea23f0550281efd5fb4b5637f
}