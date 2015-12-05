using UnityEngine;
using System.Collections;

public class NARemoteAudioSource : MonoBehaviour 
{
	public AudioSource RemoteAudioSource;

	// Use this for initialization
	void Start () 
	{
		if (GetComponent<AudioSource>() == null)
		{
			gameObject.AddComponent<AudioSource>();
		}
		UpdateFromRemote();
	}	
	// Update is called once per frame
	void Update () 
	{

		//mimic remote audio source
		UpdateFromRemote(); //???
	}

	void UpdateFromRemote()
	{
		if (RemoteAudioSource)
		{
			GetComponent<AudioSource>().clip	 	= RemoteAudioSource.clip;
			GetComponent<AudioSource>().pitch 	= RemoteAudioSource.pitch;
			GetComponent<AudioSource>().volume	= RemoteAudioSource.volume;
			float timediff = Mathf.Abs(GetComponent<AudioSource>().time-RemoteAudioSource.time);
			if (timediff > 0.01f) //allow for 10 ms diff
			{
				GetComponent<AudioSource>().time 		= RemoteAudioSource.time;
			}
			GetComponent<AudioSource>().loop 		= RemoteAudioSource.loop;
			if (RemoteAudioSource.isPlaying && !GetComponent<AudioSource>().isPlaying)
			{
				GetComponent<AudioSource>().Play();
			}
			if (!RemoteAudioSource.isPlaying && GetComponent<AudioSource>().isPlaying)
			{
				GetComponent<AudioSource>().Stop();
			}
		}

	}
}
