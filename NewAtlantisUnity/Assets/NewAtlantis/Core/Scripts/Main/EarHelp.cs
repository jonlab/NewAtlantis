using UnityEngine;
using System.Collections;

public class EarHelp : MonoBehaviour 
{


	float clip = 0.02f;
	float amp = 0f;
	public float gain = 1;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnAudioFilterRead(float[] data, int channels)
	{


		for (int i=0;i<data.Length;++i)
		{
			/*
			if (data[i] > clip)
			{
				data[i] = clip;
			}
			if (data[i] < -clip)
			{
				data[i] = -clip;
			}

			data[i] /= clip;
			*/

			/*
			float t = 0.999f;
			float tg = 0.99999f;
			amp = amp * t + Mathf.Abs (data[i])*(1-0.999f);

			if (amp > 0.001f)
			{
				gain = gain*tg + 1f/amp*(1-tg);

				data[i] *= gain;
			}

			*/

		
			data[i] *= gain;
		}

	}
}
