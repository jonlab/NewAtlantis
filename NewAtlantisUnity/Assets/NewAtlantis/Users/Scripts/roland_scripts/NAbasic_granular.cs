using UnityEngine;
using System.Collections;

public class basic_granular : MonoBehaviour {
	public int chunkSize = 100;
	public int 	GrainCount = 10;
	// clip 1 //
	public 	AudioClip sourceClip1;
	public float[] grain1;
	public float[] samples1 = new float[50]; // container for grains
	// slider interface
	public float hSliderValue = 0.0f;
	public float prevhSlider = 0.0f;


	// Start script //
	void Start() 
	{
		ProcessAudio();
	}


	// Update audio when slider is moved //
	void Update()
	{
		if (hSliderValue != prevhSlider) 
		{
			ProcessAudio();
			prevhSlider = hSliderValue;
		}
	}

	// Draw slider on screen //
	void OnGUI()
	{
		hSliderValue = GUI.HorizontalSlider(new Rect(0, Screen.height-15, 200, 30), hSliderValue, 0, 100); //Debug.Log(hSliderValue);
	}

	// Update audio //
	void ProcessAudio()
	{
		int len = 44100;

		GetComponent<AudioSource>().Stop(); // first turn off audio

		if (GetComponent<AudioSource>().clip == null)
		{
			//	AudioClip 	grains = 	AudioClip.Create("output", len, 1, 44100, true, false); // 3D sound
			AudioClip grains = 	AudioClip.Create("output", len, 1, 44100, false, false); // 2D sound
			GetComponent<AudioSource>().clip = grains;
		}
		

		float[] source_samples1 = new float[sourceClip1.samples * sourceClip1.channels]; // the source file for samples
		float[] destination_samples1 = new float[GetComponent<AudioSource>().clip.samples * GetComponent<AudioSource>().clip.channels]; // buffer to store samples
		sourceClip1.GetData(source_samples1, 0); // get the actual samples
		

		//extract N grains
		for (int g=0;g<	GrainCount;++g)
		{
			float[] grain1 = ExtractGrain(source_samples1); // extract grains from the original
			CopyGrainAtRandomLocation(grain1, destination_samples1); // copy grains to the storage buffer 

			/*
			CopyGrainWithOverlap(grain, destination_samples1, g);
			*/
		}

		GetComponent<AudioSource>().clip.SetData(destination_samples1, 0); // write grains to output file

		GetComponent<AudioSource>().Play();
	}


	// Extract grains based on slider position //
	float[] ExtractGrain(float[] buffer)
	{
		float[] grain = new float[chunkSize];
		
		int sliderPos = (int)(hSliderValue * (float)buffer.Length);

		for (int i=0;i<chunkSize;++i)
		{
			int index = (sliderPos +i)%buffer.Length;

			/* make envelope */
			float scale_chunk = (float)i / (float)chunkSize; //Debug.Log (scale_chunk); 
			float half_pi = Mathf.PI * scale_chunk; //Debug.Log (half_pi);
			float envelope = Mathf.Sin (half_pi); //Debug.Log (envelope);

			float sample = buffer[index];
			float samp = envelope * sample;
			grain[i] = samp;
		}

		return grain;
	}


	// Copy grains at random //
	void CopyGrainAtRandomLocation(float[] grain, float[] target)
	{
		int randompos = (int)(Random.value*(float)target.Length);
		for (int i=0;i<grain.Length;++i)
		{
			int index = (randompos+i);
			if (index < target.Length)
			{
				target[index] += grain[i];
			}
			else
			{
				//overlapping, do nothing
				//continue;
			}
		}
	}

}
