using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NASample
{
	public AudioClip clip = null;
	public int ReferenceNote = 60;
	public int MinNote = 0;
	public int MaxNote = 127;
}
public class NASamplerInstrument : NAObjectBase 
{
	public NASample[] samples;
	public int MinNote = 60;
	public int MaxNote = 65;
	public int CurrentNote = 60;
	public bool Loop = true;
	// Use this for initialization
	void Start () 
	{
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	NASample getSample(int note)
	{
		//get the first fitting sample if any
		foreach (NASample s in samples)
		{
			if (note >= s.MinNote && note <= s.MaxNote)
			{
				return s;
			}
		}
		return null;
		
	}

	public override void DrawSimpleGUI(Vector3 pos2d)
	{
		GUI.color = Color.white;
		string strDisplay = name;
		int x = (int)(pos2d.x*Screen.width);
		int y = (int)(Screen.height-pos2d.y*Screen.height);
		GUI.Box (new Rect(x,y,100,30), "Sampler");

	}

	public override void DrawExtendedGUI(Vector3 pos2d)
	{
		GUI.color = Color.white;
		string strDisplay = name;
		int x = (int)(pos2d.x*Screen.width);
		int y = (int)(Screen.height-pos2d.y*Screen.height);
		GUI.Box (new Rect(x,y,200,100), "Sampler");
		int ox = 0;
		int oy = 0;
		for (int i=MinNote;i<=MaxNote;++i)
		{
			if (GUI.Button (new Rect(x+ox,y+30+oy, 30, 30), ""+i))
			{
				SamplerPlay(i, Loop);
				GetComponent<NetworkView>().RPC("SamplerPlay", RPCMode.Others, i, Loop);
			}
			ox += 30;
			if (ox > 170)
			{
				ox = 0;
				oy += 30;
			}
		}
		/*if (GUI.Button (new Rect(x,y+30, 100, 30), "next note"))
		{
			CurrentNote += 1;
			if (CurrentNote > MaxNote)
				CurrentNote = MinNote;

			SamplerPlay(CurrentNote);
			GetComponent<NetworkView>().RPC("SamplerPlay", RPCMode.Others, CurrentNote);
		}*/
	}




	public static float NoteToPitch(float note, float refnote)
	{
		return 1f*Mathf.Pow(2f,(note-refnote)/12f);
	}

	[RPC]
	void SamplerPlay(int _note, bool _loop)
	{
		AudioSource source = GetComponent<AudioSource>();
		if (source)
		{
			source.loop = _loop;
			source.Stop();
			NASample s = getSample(_note);
			if (s != null)
			{
				source.clip = s.clip;
				source.pitch = NoteToPitch(_note, s.ReferenceNote);
				source.Play();
			}
		}
	}


}
