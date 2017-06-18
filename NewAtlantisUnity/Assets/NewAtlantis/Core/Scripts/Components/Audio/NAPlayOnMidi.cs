using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class NAPlayOnMidi : MonoBehaviour {

	public bool Enabled = true;
	private bool last_play = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!Enabled)
		{
			last_play = false;
			return;
		}
		AudioSource audio = GetComponent<AudioSource>();
		int refnote = 60;
		bool play = false;
		int note = 0;
		float note_vel = 0;
		for (int n=48;n<=72;++n)
		{
			
			float vel = MidiMaster.GetKey(n);

			//if (!audio.isPlaying)
			{
				if (vel > 0)// && last_play == false)
				{
					
					play = true;
					note = n;
					note_vel = vel;
				}
			}

			//Debug.Log("check midinote " + n);
			/*if (MidiMaster.GetKeyDown(n))
			{
				Debug.Log("midi key down");
				//pitch
				float pitch = NoteToFreq(n);
				audio.pitch = pitch;
				float vel = MidiMaster.GetKey(n);
				audio.volume = vel;
				audio.Play();
			}
			else if (MidiMaster.GetKeyUp(n))
			{
				Debug.Log("midi key up");
				audio.Stop();
			}
			*/
			
		}
		Debug.Log("play = "+ play);
		if (!play)// && last_play)
		{
			/*if (audio.isPlaying)
			{
				audio.Stop();
			}
			*/
			//last_play = play;
		}


		if (play && !last_play)
		{
			float pitch = NoteToFreq(note);
			audio.pitch = pitch;
			//float vel = MidiMaster.GetKey(n);
			audio.volume = note_vel;
			if (!audio.isPlaying)
			{
				audio.Play();
			}
		}
		else if (!play && last_play)
		{
			if (audio.isPlaying)
			{
				audio.Stop();
			}
			
		}
		last_play = play;

	}

	public static float NoteToFreq(float note)
	{
		return 1f*Mathf.Pow(2f,(note-60f)/12f);
	}

}
