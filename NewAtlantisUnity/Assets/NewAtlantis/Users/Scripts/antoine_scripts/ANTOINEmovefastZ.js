#pragma strict

var z : float;
var speed = 5000f;

function Start () {

}

function Update () {

	z = transform.position.z;
	if (z >= 10000){
		speed = -5000f;
	}
	if (z <= -10000){
		speed = 5000f;
	}
	transform.Translate(0,0, speed * Time.deltaTime);
}