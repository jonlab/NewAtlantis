using UnityEngine;
using System.Collections;

public class HelpAmplifier : MonoBehaviour 
{

	public float Amplitude = 1f;
	public float Gain = 1f;
	public float target_gain = 1f;

	void Update()
	{
		float k = 0.999f;
		if (Amplitude > 0.01f)
		{
			target_gain = 1f/Amplitude/10f;
		}
		if (target_gain < Gain)
			k = 0.8f;
		else
			k = 0.95f;
		Gain = Gain*k +  target_gain*(1-k); //Poor man compressor
	}



	void OnAudioFilterRead(float[] data, int channels)
	{
		//get current amplitude
		float a = 0f;
		Amplitude = 0f;
		for (int i=0;i<data.Length;++i)
		{
			a += Mathf.Abs (data[i]);
		}
		a /= data.Length;
		Amplitude = Amplitude * 0.9f + a * 0.1f;
		for (int i=0;i<data.Length;++i)
		{
			data[i] *= Gain;
		}
		
	}

	void OnGUI()
	{
		GUI.Label (new Rect(0,Screen.height-30,200,30), "Compressor Gain=" + Gain);
	}
}
