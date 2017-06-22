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

	public void SetMidiNote (int note)
	{
		midiNote = note;
	}

	// Use this for initialization
	void Start () 
	{
		Debug.Log("start");
		if (!NA.isClient())
		{
			baseScale = transform.localScale;
			timeCounter=Time.time;
			instrument = GetComponent<Instrument> ();
			Debug.Log("instrument =" + instrument);
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
		if (!NA.isClient())
		{
			if (other.gameObject.tag == "Player")
			{
				// play sound
				// remember the timing delay, and schedule it to play again

				Bang();
				eventTime = timeCounter;
			}

		}
	}


	public void Bang()
	{
		//Debug.Log("NA.isServer() = " + NA.isServer());
		if (NA.isServer())
		{
			GetComponent<NetworkView>().RPC("Play", RPCMode.Others);
		}
		Play(); //server and standalone
	}

	[RPC]
	public void Play()
	{
		Debug.Log("instrument=" + instrument);
		if (instrument != null)
		{
			instrument.PlayNote(midiNote);
		}
		StartCoroutine(DoScaleAnimation());
	}

	IEnumerator DoScaleAnimation()
	{
		// play sound
		// trigger animation or whatever

		Vector3 newScale = baseScale * 1.5f;

		float duration = 0.3f;
		float startTime = Time.time;
		float endTime =startTime+duration;

		while (Time.time<=endTime)
		{
			float t=(Time.time-startTime)/duration; 
			transform.localScale = Vector3.Lerp(newScale, baseScale,t);
			yield return null;
		}

	}
}
