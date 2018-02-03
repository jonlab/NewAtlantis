using UnityEngine;
using System.Collections;

public class NAAudioSynthGranularSynthesis : MonoBehaviour 
{
	public 	AudioClip SourceClip;
	[Range(0f, 1.0f)]
	public float Gain = 0.5f;
	public int GrainSize = 1000;
	[Range(0f, 1.0f)]
	public float GrainSizeVariance = 0.2f;
	public int 	GrainCount = 50;
	[Range(0f, 4.0f)]
	public float GrainPitch = 1;
	[Range(0f, 1.0f)]
	public float GrainPitchVariance = 0.4f;
	[Range(0f, 10f)]
	public float TotalDuration = 1f;
	// clip 1 //

	private float[] grain1;
	//private float[] samples1 = new float[50]; // container for grains
	[Range(0f, 1.0f)]
	public float position = 0.5f;
	[Range(0f, 1.0f)]
	public float PositionVariance = 0.1f;

	private float prevhSlider = 0.0f;


	// Start script //
	void Start() 
	{
		ProcessAudio();
	}


	// Update audio when slider is moved //
	void Update()
	{
		if (position != prevhSlider) 
		{
			ProcessAudio();
			prevhSlider = position;
		}
	}

	// Draw slider on screen //
	void OnGUI()
	{
		//position = GUI.HorizontalSlider(new Rect(0, Screen.height-15, 200, 30), position, 0, 1); //Debug.Log(hSliderValue);
	}

	// Update audio //
	void ProcessAudio()
	{
		//Debug.Log ("Process Audio");
		int len = (int)(TotalDuration*44100f);
		AudioSource source = GetComponent<AudioSource> ();
		//GetComponent<AudioSource>().Stop(); // first turn off audio

		//if (GetComponent<AudioSource>().clip == null)
		{
			//	AudioClip 	grains = 	AudioClip.Create("output", len, 1, 44100, true, false); // 3D sound
			AudioClip grains = 	AudioClip.Create("output", len, 1, 44100, false, false); // 2D sound
			source.clip = grains;
		}

		float[] source_samples1 = new float[SourceClip.samples * SourceClip.channels]; // the source file for samples
		int size = source.clip.samples * source.clip.channels;
		//Debug.Log ("size=" + size);
		float[] destination_samples1 = new float[size]; // buffer to store samples
		SourceClip.GetData(source_samples1, 0); // get the actual samples

		//Debug.Log ("samples :" + source_samples1 [566]);

		//extract N grains
		for (int g=0;g<	GrainCount;++g)
		{
			int grainsize = (int)(GrainSize* (1f+GrainSizeVariance*(Random.value-0.5f)*2f));
			float[] grain = ExtractGrain(source_samples1, grainsize); // extract grains from the original
			float pitch = GrainPitch* (1f+GrainPitchVariance*(Random.value-0.5f)*2f);
			grain = PitchGrain(grain, pitch);
			CopyGrainAtRandomLocation(grain, destination_samples1, Gain); // copy grains to the storage buffer 
		}
		source.clip.SetData(destination_samples1, 0); // write grains to output file
		source.Play();
	}

	float[] PitchGrain(float[] input, float pitch)
	{
		int output_size = (int)((float)input.Length / pitch);

		float[] output = new float[output_size];

		//interpolate
		for (int i = 0; i < output_size; ++i) 
		{
			float findex = (float)i * pitch;
			int index = (int)findex;
			float k = findex - index;
			output [i] = input [index%input.Length] * (1f - k) + input [(index + 1)%input.Length] * k;
		}
		return output;
	}


	// Extract grains based on slider position //
	float[] ExtractGrain(float[] buffer, int size)
	{
		float[] grain = new float[size];
		int sliderPos = (int)(position * (float)buffer.Length);
		sliderPos += (int)(((Random.value - 0.5f) * 2f * PositionVariance) * (float)buffer.Length); 
		sliderPos += buffer.Length;
		for (int i=0;i<size;++i)
		{
			int index = (sliderPos +i)%buffer.Length;

			//apply enveloppe
			float scale_chunk = (float)i / (float)size; //Debug.Log (scale_chunk); 
			float half_pi = Mathf.PI * scale_chunk; //Debug.Log (half_pi);
			float envelope = Mathf.Sin (half_pi); //Debug.Log (envelope);

			float sample = buffer[index];
			float samp = envelope * sample;
			grain[i] = samp;
		}

		return grain;
	}


	// Copy grains at random //
	void CopyGrainAtRandomLocation(float[] grain, float[] target, float gain)
	{
		int randompos = (int)(Random.value*(float)target.Length);
		for (int i=0;i<grain.Length;++i)
		{
			int index = (randompos+i) % target.Length;
			if (index < target.Length)
			{
				target[index] += grain[i]*gain;
			}
			else
			{
				//overlapping, do nothing
				//continue;
			}
		}
	}

}
