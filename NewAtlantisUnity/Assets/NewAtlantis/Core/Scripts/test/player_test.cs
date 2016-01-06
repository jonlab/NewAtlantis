using UnityEngine;
using System.Collections;

public class player_test : MonoBehaviour {
	public int chunkSize = 100;
	public AudioClip sourceClip;
	public float[] grain;
	public float[] samples = new float[100];
	public int GrainCount = 100;
	public int len = 44100;

	void Start() 
	{
		ProcessAudio();

	}

	void OnGUI()
	{
		if (GUI.Button (new Rect(0,0, 100, 30), "recompute"))
		{
			ProcessAudio();
		}
	}
	void ProcessAudio()
	{
		GetComponent<AudioSource>().Stop ();
		AudioClip grains = AudioClip.Create("output", len, 1, 44100, false, false);
		GetComponent<AudioSource>().clip = grains;
		float[] source_samples = new float[sourceClip.samples * sourceClip.channels];
		float[] destination_samples = new float[grains.samples * grains.channels];
		sourceClip.GetData(source_samples, 0);
		
		
		
		//PackSamps (source_samples);
		
		//extract 10 grains
		for (int g=0;g<GrainCount;++g)
		{
			float[] grain = ExtractGrain(source_samples);
			CopyGrainAtRandomLocation(grain, destination_samples);
		}
		
		GetComponent<AudioSource>().clip.SetData (destination_samples, 0);
		
		GetComponent<AudioSource>().Play ();

	}

	/*
	void PackSamps(float[] samps) 
	{
		int i = 0;

		while (i < chunkSize) {
			float temp_samp = samps[i]; Debug.Log(temp_samp);
			//samples [i] = samps[i];
			++i;
		}
		audio.clip.SetData (samples, 0);
	}
	*/


	float[] ExtractGrain(float[] buffer)
	{
		float[] grain = new float[chunkSize];

		int randompos = (int)(Random.value*(float)buffer.Length);
		Debug.Log ("picked randompos = " + randompos);

		for (int i=0;i<chunkSize;++i)
		{
			int index = (randompos+i)%buffer.Length;
			float sample = buffer[index];
			grain[i] = sample;

		}

		return grain;

	}

	void CopyGrainAtRandomLocation(float[] grain, float[] target)
	{
		int randompos = (int)(Random.value*(float)target.Length);
		for (int i=0;i<grain.Length;++i)
		{
			int index = randompos+i;
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



/*
public class PlayChunk : MonoBehaviour {
	public int position = 0;
	public int sampleRate = 0;
	public float frequency = 440;
	void Start() {
		AudioClip myClip = AudioClip.Create("chunk", 44100, 1, 44100, false, true, OnAudioRead, OnAudioSetPosition);
		sampleRate = AudioSettings.outputSampleRate;
		audio.clip = myClip;
		audio.Play();
	}
	void OnAudioRead(float[] data) {
		int count = 0;
		while (count < data.Length) {
			data[count] = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * frequency * position / sampleRate));
			position++;
			count++;
		}
	}
	void OnAudioSetPosition(int newPosition) {
		position = newPosition;
	}
}
*/