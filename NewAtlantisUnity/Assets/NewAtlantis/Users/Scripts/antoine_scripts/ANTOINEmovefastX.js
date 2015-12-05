#pragma strict

var x : float;
var speed = 5000f;

function Start () {

}

function Update () {

	x = transform.position.x;
	if (x >= 10000){
		speed = -5000f;
	}
	if (x <= -10000){
		speed = 5000f;
	}
	transform.Translate(speed * Time.deltaTime,0,0);
}