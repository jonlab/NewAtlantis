using UnityEngine;
using System.Collections;

public class NAAudioSynthFM : NAObjectBase 
{
	private int 		samplerate 			= 44100;
	public float 		duration 			= 10f;
	public WaveForm 	CarrierWaveform 	= WaveForm.Sin;
	public float 		CarrierFrequency 	= 440;
	public WaveForm 	ModulatorWaveform 	= WaveForm.Sin;
	public float 		ModulatorFrequency 	= 440;
	public float		ModulationAmount	= 1f;
	//Carrier
	//Modulator


	void Awake () 
	{
		Compute();
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void Compute()
	{
		AudioSource audio = GetComponent<AudioSource>();
		if (audio)
		{
			int samplecount = (int)(samplerate*duration);
			//if (audio.clip == null)
				audio.clip = AudioClip.Create("NAAudioSynthSin", samplecount, 1, samplerate, false);
			float[] data = new float[samplecount];
			audio.clip.GetData(data, 0);
			
			float[] dataModulator = new float[samplecount];
			DSP.GenerateSinus(dataModulator, samplerate, ModulatorFrequency);
			float Amp = 0.1f;
			for (int i = 0; i < data.Length; i++) 
			{
				float ModulatedFrequency = CarrierFrequency+CarrierFrequency*dataModulator[i]*ModulationAmount;
				float angle = (float)i*ModulatedFrequency/samplerate*Mathf.PI*2;

				//angle = angle%360f;
				//data[i] += Mathf.Sin(angle+dataModulator[i]*ModulationAmount*Mathf.PI);
				data[i] += Mathf.Sin(angle)*Amp;
			}
			
			
			audio.clip.SetData(data, 0);
			audio.Play();


		}

	}
}
