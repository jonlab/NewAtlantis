#pragma strict

var target : loulcopieur;

function OnTriggerStay() {
	target.cswitch = false;
}
function OnTriggerExit() {
	target.cswitch = true;
}