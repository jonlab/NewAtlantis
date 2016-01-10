#pragma strict

var doorClear : boolean;
var coll : Collider;
var porte : GameObject;
var opendoor : boolean;
var closedoor : boolean;

var cubeInside : boolean;
var openingMode : int;

var doorSpeed : float; //Time in seconds for door operation.

function Start() {
	cubeInside = false;
	
	openingMode = 0;
}

function Update(){
	
	ControlDoor(openingMode);
	
	if(cubeInside && doorClear){
		fire();
	}
	
	
	
}

function ControlDoor(mode:int){
	if(openingMode == 1 ) {
		CloseDoor();
	}
	if(openingMode == 3) {
		OpenDoor();
	}
	
}

function OpenDoor(){
	porte.transform.parent.Rotate(Vector3(0, 0, 1) * -1, Time.deltaTime * (90 / doorSpeed));
	if(porte.transform.parent.rotation.eulerAngles.z >= 340) {
		openingMode = 0;
	}
}


function CloseDoor(){
	porte.transform.parent.Rotate(Vector3(0, 0, 1), Time.deltaTime * (90 / doorSpeed));
	if(porte.transform.parent.rotation.eulerAngles.z >= 90 && porte.transform.parent.rotation.eulerAngles.z <= 340) {
		openingMode = 2;
	}
}



function OnTriggerEnter(other : Collider) {
	coll = other;
	cubeInside = true;	
}


function OnTriggerExit(other : Collider) {
	
	cubeInside = false;
	openingMode = 3;
}



function fire() {
	openingMode = 1;
	cubeInside = false;
	yield WaitForSeconds(doorSpeed);
	var aim : Vector3 = transform.up;
	coll.GetComponent(Rigidbody).AddForce(aim * 30, ForceMode.VelocityChange);
}

