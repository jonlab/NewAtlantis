using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Note : System.Object
{
	[SerializeField]	public int MIDI_value;
	[SerializeField]	public AudioClip sample;
	public float pitchMultiplier;

	[SerializeField] string noteName;

	public Note (int midivalue, AudioClip s, int halfStepOffset, string name)
	{
		MIDI_value = midivalue;
		sample = s;
		noteName = name;

		float a = Mathf.Pow (2.0f, (1.0f / 12.0f));
		pitchMultiplier = Mathf.Pow (a, halfStepOffset);

		// search the input array for note with closest midi value
		// if find one with same midi value, copy it
		// otherwise, calculate the halftoneOffset (MIDI_Value - i ) and use that to calculate the pitchshift 

		/* 
			 * f(n) = f(0) * a^n

	n=offset in half-steps relative to f(0)
	f(0) base frequency
	a=2^(1/12) = the 12th root of 2 = the number which when multiplied by itself 12 times = 2
	a=1.0594630943592952645618252949463
	
	now, what we actually want is just a pitch multiplier for the sample.  so, 1.0 when halfStepOffset=0.  2.0 when it equals 12.  
	

			 */
	}

}


public class Instrument : MonoBehaviour 
{
	[SerializeField] Note[] notes;
	Note[] allNotes;	// fill in this array with the rest, using the first array as the basis 

	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();

		allNotes = new Note[128];
		for (int i = 0; i < 128; i++) {
			Note n = notes [0];
			for (int j = 0; j < notes.Length; j++) {
				if (Math.Abs (notes [j].MIDI_value - i) < Math.Abs (n.MIDI_value - i))
					n = notes [j];
			}

			// n is closest note

			Note newNote = new Note (i, n.sample, i-n.MIDI_value, "");	
			allNotes [i] = newNote;

			// ableton samples,middle C is C3, technically octave 5, MIDI value is 60
			// C3 = octave 5, C2=octave 4, C1=octave 3, C0=octave 2
		}

	}

	public void PlayNote (int MIDIValue)
	{
		Note note = allNotes [MIDIValue];
		audioSource.pitch = note.pitchMultiplier;
		audioSource.PlayOneShot (note.sample, 1.0f);

	}
	void Update () {	}
}
