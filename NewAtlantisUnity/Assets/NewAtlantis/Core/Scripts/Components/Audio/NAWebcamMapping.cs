using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NAWebcamMappingData
{
	AverageRed,
	AverageGreen,
	AverageBlue
}
public class NAWebcamMapping : MonoBehaviour 
{
	private float smoothed_output = 0f;
	private float k = 0.9f;

	public NAWebcamMappingData input = NAWebcamMappingData.AverageBlue;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!NA.isClient())
		{
			float output = 0f;
			if (input == NAWebcamMappingData.AverageRed)
			{
				output = WebcamProcess.average_red;
			}
			else if (input == NAWebcamMappingData.AverageGreen)
			{
				output = WebcamProcess.average_green;
			}
			else if (input == NAWebcamMappingData.AverageBlue)
			{
				output = WebcamProcess.average_blue;
			}
			output = output * output;
			AudioSource source = GetComponent<AudioSource> ();
			smoothed_output = smoothed_output * k + output * (1 - k);
			source.volume = smoothed_output;
		}
		
	}
}
