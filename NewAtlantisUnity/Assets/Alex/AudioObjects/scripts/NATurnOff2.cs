using UnityEngine;
using System.Collections;

public class NATurnOff2 : MonoBehaviour {
	public float volume = 1.0f;
	public float acceleration = 0.01f;
	AudioSource aud;

	public int delay = 0;

	//public bool destroyEnd = false;
	public float timer = 0;
	// Use this for initialization
	void Start () {

		aud = GetComponent<AudioSource> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (NA.isClient() && NA.syncMode != SyncMode.NoInDepthSync)
			return;
		//if (!NA.isClient()) {

			timer += Time.deltaTime;

			if (timer < delay) {
				return;
			}

			volume = aud.volume;
			volume -= acceleration;
			if (volume < 0)
				volume = 0;

			aud.volume = volume;

		//}


	

	}

	void restart(){

		// DEFINE WHICH EVENT CALL THIS.
		aud.volume = 1.0f;
		//delay = 0;

	}

	void OnCollisionEnter(Collision e){

	//if(NA.isClient)return;

		timer = 0;
		restart ();

	}

	void OnCollisionExit(Collision e){
	//if(NA.isClient)return;

		timer = delay;
	}


	void OnTriggerEnter(Collider e){
	//if(NA.isClient)return;

		timer = 0; 
		restart ();

	}

	void OnTriggerExit(Collider e){
	//if(NA.isClient)return;

		timer = delay;
	}


	void OnMouseDown(){
	//if(NA.isClient)return;

		timer = delay; 
		restart ();
	}






}
