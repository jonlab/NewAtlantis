#pragma strict

var z : float;
var speed = 100f;

function Start () {

}

function Update () {

	z = transform.position.z;
	if (z >= 1000){
		speed = -100f;
	}
	if (z <= -1000){
		speed = 100f;
	}
	transform.Translate(0,0, speed * Time.deltaTime);
}