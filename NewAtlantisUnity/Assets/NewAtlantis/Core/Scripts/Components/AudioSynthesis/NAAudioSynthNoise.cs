using UnityEngine;
using System.Collections;

public class NAAudioSynthNoise : MonoBehaviour 
{
	public float 	duration 	= 10f;
	private int 	samplerate 	= 44100;

	// Use this for initialization
	void Awake () 
	{
		AudioSource audio = GetComponent<AudioSource>();
		if (audio)
		{
			int samplecount = (int)(samplerate*duration);
			if (audio.clip == null)
				audio.clip = AudioClip.Create("NAAudioSynthNoise", samplecount, 1, samplerate, false);
			float[] data = new float[samplecount];
			audio.clip.GetData(data, 0);
			DSP.GenerateNoise(data);
			audio.clip.SetData(data, 0);
			audio.Play();
		}

		Debug.Log ("Generate noise");
	}
	
	// Update is called once per frame
	void Update () 
	{

	
	}
}
