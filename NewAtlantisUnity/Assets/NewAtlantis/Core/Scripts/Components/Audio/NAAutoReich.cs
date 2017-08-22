using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]


public class NAAutoReich : NAObjectBase 
{
	public AudioClip[] clips;
	public int CurrentIndex = -1;
	public bool Loop = true;
	// Use this for initialization
	void Start () 
	{
		if (!NA.isClient ()) {
			Change ();
		}
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
		
    void Change()
    {
		if (CurrentIndex == -1)
			CurrentIndex = 4;
		else
		{
			int move = Random.Range(-2,2);
			if (move >=0)
				move ++;

			CurrentIndex += move;
			if (CurrentIndex >= clips.Length)
				CurrentIndex = 4;
			if (CurrentIndex < 0)
				CurrentIndex = 4;
		}
        
        
		PatternSamplerPlay(CurrentIndex);
		if (NA.isClient() || NA.isServer())
		{
			GetComponent<NetworkView>().RPC("PatternSamplerPlay", RPCMode.Others, CurrentIndex);
		}

		// set timer to change again 
		if (!NA.isClient ()) {
			Invoke ("Change", clips [CurrentIndex].length);

		}

    }

	[RPC]
	void PatternSamplerPlay(int _index)
	{
		AudioSource source = GetComponent<AudioSource>();
		if (source)
		{
			
			source.loop = true;
			source.Stop();
            AudioClip clip = clips[_index];

			if (clip != null)
			{
                source.clip = clip;
                source.pitch = 1;
				source.Play();
			}
		}
	}


}
