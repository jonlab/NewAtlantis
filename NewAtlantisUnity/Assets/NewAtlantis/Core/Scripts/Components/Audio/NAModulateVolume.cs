using UnityEngine;
using System.Collections;

public class NAModulateVolume : MonoBehaviour {
	public float volume = 1.0f;
	public float acceleration = 0.01f;
	AudioSource aud;

	int step = 0;
	// Use this for initialization
	void Start () {
		
		aud = GetComponent<AudioSource> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		//if (!NA.isClient ()) 
		if (NA.isClient() && NA.syncMode != SyncMode.NoInDepthSync)
			return;
		
		{ //server and standalone
	


			volume = aud.volume;

			if (step % 2 == 0) {
				volume -= acceleration;
			} else {
				volume += acceleration;
			}

			if (volume <= 0 || volume >= 1)
				step++;



		
			aud.volume = volume;

		}

	}
}
