using UnityEngine;
using System.Collections;

public class NARandomizeAudioSource : MonoBehaviour 
{
	public float pitch = 1;
	public float pitchVariance= 0.1f;
	public float volume=1;
	public float volumeVariance=0.1f;
	public AudioClip[] clips;

	private float current_volume = 1;
	private float current_pitch = 1;
	private int current_index = 0;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public float GetCurrentVolume()
	{
		return current_volume;
	}
	public float GetCurrentPitch()
	{
		return current_pitch;
	}
	public int GetCurrentIndex()
	{
		return current_index;
	}

	public void Randomize()
	{

		AudioSource audio = GetComponent<AudioSource>();
		if (audio == null)
		{
			Debug.LogWarning("no AudioSource ! you have to attach an AudioSource");
			return;
		}

		//Apply(
		current_volume = volume+(Random.value-0.5f)*volume*volumeVariance;
		current_pitch = pitch+(Random.value-0.5f)*pitch*pitchVariance;

		//audio.volume = volume+(Random.value-0.5f)*volume*volumeVariance;
		//audio.pitch = pitch+(Random.value-0.5f)*pitch*pitchVariance;
		if (clips != null)
		{
			if (clips.Length > 0)
			{
				current_index = (int)(Random.value*clips.Length);
				//audio.clip = clips[(int)(Random.value*clips.Length)];
			}
		}
	}

	public void Apply(int clipindex, float volume, float pitch)
	{
		AudioSource audio = GetComponent<AudioSource>();
		if (audio == null)
		{
			Debug.LogWarning("no AudioSource ! you have to attach an AudioSource");
			return;
		}
		audio.volume = volume;
		audio.pitch = pitch;
		if (clips != null)
		{
			if (clips.Length > 0 && clipindex != -1)
			{
				audio.clip = clips[clipindex];

			}
		}

	}
}
