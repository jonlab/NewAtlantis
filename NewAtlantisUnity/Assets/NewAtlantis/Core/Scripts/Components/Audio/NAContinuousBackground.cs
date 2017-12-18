using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

	For looping sounds that should play continuously, even when nobody is in the space.
	When the space loads, calculate start time for the sound using modulo operator on the sound duration, 
	to make it sound as if it's continued playing while you've been gone. 

*/



public class NAContinuousBackground : MonoBehaviour {
	AudioSource audio;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
		float length = audio.clip.length;
		float startTime = Time.time % length;
		audio.time = startTime;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
