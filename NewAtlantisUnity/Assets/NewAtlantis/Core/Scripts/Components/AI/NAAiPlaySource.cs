using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//plays an audio source sometimes
public class NAAiPlaySource : NAAiBase 
{

	//public parameters
	public float 	interval 			= 4;
	[Range(0.0f, 1f)]
	public float 	intervalVariance 	= 0.5f;

	private float timer = 0f;
	private float currentInterval = 0;

	private AudioSource source = null;


	// Use this for initialization
	void Start () 
	{
		source = GetComponent<AudioSource> ();
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!NA.isClient ()) //server and standalone
		{
			timer += Time.deltaTime;
			if (timer > currentInterval)
			{
				timer -= currentInterval;
				currentInterval = interval + interval * (Random.value - 0.5f) * intervalVariance*2f;
				NetworkView 			nv 	= GetComponent<NetworkView>();
				if (nv != null)
				{
					nv.RPC("Apply", RPCMode.All, 1f, 1f);
				}
				else
				{
					if (source) 
					{
						Apply (1f, 1f);
					}
				}


			}
		}
		
	}

	[RPC]
	void Apply(float volume, float pitch)
	{
		source.Stop();
		source.volume = volume;
		source.pitch = pitch;
		source.Play ();
	}
}
