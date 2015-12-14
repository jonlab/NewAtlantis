using UnityEngine;
using System.Collections;

public class NATranformSound : MonoBehaviour {



	public enum InChoice // menu
	{
		Position = 1, 
		Rotation = 2, 
		Size = 3,
	}
	
	public InChoice getTransformParameter;

	public Vector3 Percent = new Vector3 (33f, 33f, 33f);
//	float XPercent = Percent.x;
//	float YPercent = Percent.y;
//	float ZPercent = Percent.z;

	public AudioClip mySound;

	public enum OutChoice{Pitch, Filter_Freq, Volume};
	public OutChoice myOutchoice;


	public float pitchControl = 1F;
	public float pitchOffset = 1F;

	public float filterFrequencyControl = 1F;
	public float filterFrequencyOffset = 1F;

	public float volumeControl = 1F;
	public float volumeOffset = 1F;




	private float distance;

	void Start () {


		if( this.gameObject.GetComponent<AudioSource>() == null ) this.gameObject.AddComponent<AudioSource>();

		GetComponent<AudioSource>().clip = mySound;
		GetComponent<AudioSource>().Play();
		GetComponent<AudioSource>().loop = true;

		this.gameObject.AddComponent<AudioLowPassFilter> ();
		//print ("start ");
		}
	
	void Update () {


		AudioLowPassFilter lpf = this.gameObject.GetComponent<AudioLowPassFilter> ();

		float val = 0; // initialisation de val

		switch (getTransformParameter) 
		{
		case InChoice.Position:
			val = transform.position.magnitude;
			break;
		case InChoice.Rotation:
			val = transform.eulerAngles.magnitude;
			break;
		case InChoice.Size:
			val = transform.localScale.magnitude;
			break;
		}

float weithedValue = val * (((Percent.x / 100) * transform.position.x) + ((Percent.y / 100) * transform.position.y) + ((Percent.z / 100) * transform.position.z));
		//Debug.Log (myOutchoice);

//		switch (myOutchoice)
//		{
//		case OutChoice.Pitch:
//			audio.pitch = ((weithedValue*pitchControl) + pitchOffset) * 10f;//val;
//			print (" audioPitch " + audio.pitch );
//			break;
//		case OutChoice.Filter_Freq:
//			lpf.cutoffFrequency = Mathf.Pow(weithedValue,2) * 100f;	
//			print (" filterFreq " + lpf.cutoffFrequency );
//			break;
//		case OutChoice.Volume:
//			audio.volume = weithedValue*volumeControl+volumeOffset; 
//			print (" audioVolume " + audio.pitch );
//			break;
//		}

		GetComponent<AudioSource>().pitch = ((weithedValue*pitchControl) + pitchOffset) * 10f;//val;
		lpf.cutoffFrequency = Mathf.Pow(weithedValue,2) * 100f;	
		GetComponent<AudioSource>().volume = weithedValue*volumeControl+volumeOffset; 


		distance = Vector3.Distance(Camera.main.transform.position, transform.position);
		//print (" Distance " + distance );


			//audio.volume = (other.relativeVelocity.magnitude * impactVolumeControl) + volumeOffset;
			//Debug.Log("Volume " + " " + audio.volume);
			
		}
	}

