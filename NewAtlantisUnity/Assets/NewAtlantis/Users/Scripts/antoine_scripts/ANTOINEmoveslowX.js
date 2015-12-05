#pragma strict

var x : float;
var speed = 100f;

function Start () {

}

function Update () {

	x = transform.position.x;
	if (x >= 1000){
		speed = -100f;
	}
	if (x <= -1000){
		speed = 100f;
	}
	transform.Translate(speed * Time.deltaTime,0,0);
}