using UnityEngine;
using System.Collections;



public enum WaveForm
{
	Sin,
	Cos,
	Square,
	Triangle,
	Sawtooth
}
public class NAAudioSynthOscillator : MonoBehaviour 
{
	public float 	duration 	= 10f;
	public float 	frequency	= 440;
	public WaveForm waveform 	= WaveForm.Sin;
	private int 	samplerate 	= 44100;

	// Use this for initialization
	void Awake () 
	{
		AudioSource audio = GetComponent<AudioSource>();
		if (audio)
		{
			int samplecount = (int)(samplerate*duration);
			if (audio.clip == null)
				audio.clip = AudioClip.Create("NAAudioSynthSin", samplecount, 1, samplerate, false);
			float[] data = new float[samplecount];
			audio.clip.GetData(data, 0);
			if (waveform == WaveForm.Cos)
			{
				DSP.GenerateCosinus(data, samplerate, frequency);
			}
			else if (waveform == WaveForm.Sin)
			{
				DSP.GenerateSinus(data, samplerate, frequency);
			}
			else if (waveform == WaveForm.Square)
			{
				DSP.GenerateSquare(data, samplerate, frequency);
			}
			//DSP.GenerateSquare(data, samplerate, frequency);

			audio.clip.SetData(data, 0);
			audio.Play();
		}
		Debug.Log ("Generate Oscillator");
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
