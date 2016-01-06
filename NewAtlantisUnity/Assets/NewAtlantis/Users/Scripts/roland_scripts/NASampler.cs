using UnityEngine;
using System.Collections;

public class NASampler: MonoBehaviour {
		
//	public enum InChoice // menu
//	{
//		Position = 1, 
//		Rotation = 2, 
//		Size = 3,
//	}
	
	//public InChoice getTransformParameter;
	//public Vector3 Percent = new Vector3 (33f, 33f, 33f);

//	public static AnimationCurve CurveField(AnimationCurve value, params GUILayoutOption[] options);

	public AudioClip mySound;
	public bool loop;

	//public enum OutChoice{Pitch, Filter_Freq, Volume};
	//public OutChoice myOutchoice;

	public float pitchInputValue;
	public AnimationCurve pitchCurve = AnimationCurve.Linear(-10,0,10,10);
	public float pitchOutputValue;

	
	public float filterInputValue;
	public AnimationCurve filterCurve = AnimationCurve.Linear(-10,0,10,10);
	public float filterOutputValue;
	
	public float volumeInputValue;
	public AnimationCurve volumeCurve = AnimationCurve.Linear(-10,0,10,1);
	public float volumeOutputValue;


//	public float pitchControl = 1F;
//
//	public float pitchOffset = 1F;
//
//	public float filterFrequencyControl = 1F;
//
//	public float filterFrequencyOffset = 1F;
//
//	public float volumeControl = 1F;
//
//	public float volumeOffset = 1F;
//
//	private float distance;

	void Start () {

		if( this.gameObject.GetComponent<AudioSource>() == null ) this.gameObject.AddComponent<AudioSource>();
		GetComponent<AudioSource>().clip = mySound;
		GetComponent<AudioSource>().Play();
		GetComponent<AudioSource>().loop = loop;
		this.gameObject.AddComponent<AudioLowPassFilter> ();
		}
	
	void Update () {

		AudioLowPassFilter lpf = this.gameObject.GetComponent<AudioLowPassFilter> ();

		pitchInputValue = transform.localPosition.x;
		filterInputValue = transform.localPosition.y;
		volumeInputValue = transform.localPosition.z;

		pitchOutputValue = pitchCurve.Evaluate(pitchInputValue);
		filterOutputValue = filterCurve.Evaluate(filterInputValue);
		volumeOutputValue = volumeCurve.Evaluate(volumeInputValue);

//		EditorUtility.SetDirty(pitchOutputValue);

	GetComponent<AudioSource>().pitch = pitchOutputValue;
		print ("pitch " + GetComponent<AudioSource>().pitch);
	lpf.cutoffFrequency = filterOutputValue * 1000f;	
	GetComponent<AudioSource>().volume = volumeOutputValue; 


		//distance = Vector3.Distance(Camera.main.transform.position, transform.position);
		//print (" Distance " + distance );


			//audio.volume = (other.relativeVelocity.magnitude * impactVolumeControl) + volumeOffset;
			//Debug.Log("Volume " + " " + audio.volume);
			
		}
	}

