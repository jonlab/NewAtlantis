using UnityEngine;
using System;  // Needed for Random

public class Noise : MonoBehaviour
{
	// un-optimized version of a noise generator
	private System.Random RandomNumber = new System.Random();
	//public float offset = 0f;
	public float volume = 0.75f;
	
	void OnAudioFilterRead(float[] data, int channels)
	{
		for (int i = 0; i < data.Length; i++)
		{
			data[i] =  (float)RandomNumber.NextDouble()* volume;
			//data[i] =  offset -1.0f + (float)RandomNumber.NextDouble()* volume;
		}
	}
}