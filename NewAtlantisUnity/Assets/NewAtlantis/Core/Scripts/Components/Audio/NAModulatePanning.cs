using UnityEngine;
using System.Collections;

public class NAModulatePanning : MonoBehaviour {

	public float panning = 0.0f;
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
			panning = aud.panStereo;
			
			if (step % 2 == 0) {
				panning -= acceleration;
			} else {
				panning += acceleration;
			}
			
			if (panning <= -1 || panning >= 1)
				step++;
			
			
			
			
			aud.panStereo = panning;
			
		}
	
}
}
