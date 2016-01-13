using UnityEngine;
using System.Collections;

public class NAToggleVolumeOnTrigger : MonoBehaviour {

	public float VolumeOn = 1f;
	public float VolumeOff = 0f;
	private bool State = false;

	// Use this for initialization
	void Start () 
	{
		//FIXME : sync ?
		AudioSource audio = GetComponent<AudioSource>();
		audio.volume = VolumeOff;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter(Collider collision) 
	{
		AudioSource audio = GetComponent<AudioSource>();
		if (audio)
		{
			State = !State;
			if (State)
			{
				audio.volume = VolumeOn;
			}
			else
			{
				audio.volume = VolumeOff;
			}

		}
	}
}
