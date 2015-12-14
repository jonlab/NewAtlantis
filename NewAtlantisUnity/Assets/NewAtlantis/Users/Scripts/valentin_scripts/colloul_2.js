#pragma strict

var target : colloul;
var angle : float;

function Update() {
	angle = this.GetComponent.<HingeJoint>().angle;
	if (angle > 10) {
		target.bswitch = false;
	} else {
		target.bswitch = true;
	}
}