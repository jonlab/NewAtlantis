using UnityEngine;
using System.Collections;

public class NALooperSequence : MonoBehaviour {
	public int interval = 50;
	AudioSource aud;
	int step=0;
	public float[] tempos;

	float timer=0;



	// Use this for initialization
	void Start () {
		aud = GetComponent<AudioSource> ();


	}
	
	// Update is called once per frame
	void Update () 
    {
		interval = Mathf.Clamp (interval, 1, 10000000);

		//if (!NA.isClient()) //server and standalone
		if (NA.isClient() && NA.syncMode != SyncMode.NoInDepthSync)
			return;
		{
			timer += Time.deltaTime;
			if (timer > interval)
			{
				timer -= interval;
                aud.pitch = tempos[step%tempos.Length];
                step++;
                aud.Play ();
			}
		}


	}
}
