using UnityEngine;
using System;  // Needed for Math

public class Sinus : MonoBehaviour
{
	// un-optimized version
	public double frequency = 440;
	public double gain = 0.4;
	
	private double increment;
	private double phase;
	private double sampling_frequency = 48000;
	
	void Start(){
		// randomize freq
		CreatureBehaviour[] tmp = this.gameObject.GetComponents<CreatureBehaviour>();
		
		// todo: make this sound good
		if(tmp.Length==1) frequency = 20 + tmp[0].uniqueNb*20; // in current dome setup, uniqueNb goes from 0 to ±80
	}
	
	void OnAudioFilterRead(float[] data, int channels)
	{
		// update increment in case frequency has changed
		increment = frequency * 2 * Math.PI / sampling_frequency;
		for (var i = 0; i < data.Length; i = i + channels)
		{
			phase = phase + increment;
			// this is where we copy audio data to make them “available” to Unity
			data[i] = (float)(gain*Math.Sin(phase));
			// if we have stereo, we copy the mono data to each channel
			if (channels == 2) data[i + 1] = data[i];
			if (phase > 2 * Math.PI) phase = 0;
		}
	}
} 