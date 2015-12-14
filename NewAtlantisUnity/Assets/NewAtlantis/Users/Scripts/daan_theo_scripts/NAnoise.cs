using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]

public class NAnoise : MonoBehaviour {
	// un-optimized version of a noise generator
	private System.Random RandomNumber = new System.Random();
	public  float offset = 0;

	void OnAudioFilterRead(float[] data, int channels)
	{
		for (int j = 0; j < data.Length; j++)
		{
			data[j] =  offset -1.0f + (float)RandomNumber.NextDouble()*2.0f;
		}
	}
}
