using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundNodeWithDelay : MonoBehaviour {

	public float loopTime = 10.0f; // duration of the sequence, in seconds
	float timeCounter;
	float eventTime = -1;
	Vector3 baseScale; 

	Instrument instrument;
	AudioSource audioSource;
	public int midiNote=30;	// note to play when triggered

	GameObject pulsar;
	public GameObject pulsarPrefab;

	public void SetMidiNote (int note)
	{
		midiNote = note;
	}

	// Use this for initialization
	void Start () 
	{


		pulsar=Instantiate(pulsarPrefab,transform.position,Quaternion.identity);
		pulsar.transform.parent = transform;

		instrument = GetComponent<Instrument> ();

		if (!NA.isClient())
		{
			timeCounter=Time.time;
			restartLoop();
		}
	}


	// Update is called once per frame
	void Update () 
	{
		if (!NA.isClient())
		{
			timeCounter+=Time.deltaTime;

			if (timeCounter > loopTime)
			{
				restartLoop();
			}
		}
	}

	void restartLoop()
	{
		timeCounter = timeCounter % loopTime;
		if (eventTime>=0)
		{
			Invoke("Bang",eventTime-timeCounter);
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		Debug.Log("OnTriggerEnter");
		if (NA.isClient())
		{
			GetComponent<NetworkView>().RPC("Server_Bang",RPCMode.Server);
		}
		else
		{
			// play sound
			// remember the timing delay, and schedule it to play again
			Bang();
		}
	}

	[RPC]
	public void Server_Bang()
	{
		Bang();
	}

	public void Bang()
	{
		eventTime = timeCounter;
		if (NA.isServer())
		{
			GetComponent<NetworkView>().RPC("Play", RPCMode.Others);
		}
		else
		{
			Play();
		}
	}

	[RPC]
	public void Play()
	{
		if (instrument != null)
		{
			LogManager.Log("play " + midiNote);
			instrument.PlayNote(midiNote);
		}
		pulsar.GetComponent<ScalePulse>().Pulse(1.0f);
	}

}
