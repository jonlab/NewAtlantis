using UnityEngine;
using System.Collections;

public class NALooper : MonoBehaviour {
	public int interval = 50;
	AudioSource aud;
	float timer=0;

	// Use this for initialization
	void Start () {
		aud = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	    
		interval = Mathf.Clamp (interval, 1, 10000000);
		if (NA.isClient() && NA.syncMode != SyncMode.NoInDepthSync)
			return;
		
		//if (!NA.isClient()) //server and standalone
		{
			timer += Time.deltaTime;
			if (timer > interval)
			{
				timer -= interval;

			}
		}

		//if (!NA.isClient ()) {

			if (timer % interval == 0) {

				aud.Play ();
			//}



		}

	

	}






}
