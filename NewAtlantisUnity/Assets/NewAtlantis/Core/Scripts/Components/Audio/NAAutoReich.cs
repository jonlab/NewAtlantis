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
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	

	public override void DrawSimpleGUI(Vector3 pos2d)
	{
		GUI.color = Color.white;
		string strDisplay = name;
		int x = (int)(pos2d.x*Screen.width);
		int y = (int)(Screen.height-pos2d.y*Screen.height);
		GUI.Box (new Rect(x,y,100,30), "AutoReich");
	}

	public override void DrawExtendedGUI(Vector3 pos2d)
	{
		GUI.color = Color.white;
		string strDisplay = name;
		int x = (int)(pos2d.x*Screen.width);
		int y = (int)(Screen.height-pos2d.y*Screen.height);
		GUI.Box (new Rect(x,y,200,100), "AutoReich");
		
		if (GUI.Button (new Rect(x,y+30, 100, 30), "next!"))
		{
            Change();
		}

        //auto-behaviour on server ?
        if (!NA.isClient())
        {


        }
	}

	void OnCollisionEnter(Collision collision) 
	{
		if (NA.isClient())
			return;

		Change();
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
