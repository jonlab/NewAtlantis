using UnityEngine;
using System;  // Needed for Random

public class BandPassFilter : MonoBehaviour
{

	public float cutoffFrequency = 500f;
	public float Quality = 1f;
	AudioLowPassFilter lpf;
	AudioHighPassFilter hpf;

	void Start () {
	
		if( this.gameObject.GetComponent<AudioSource>() == null ) this.gameObject.AddComponent<AudioSource>();	
		if( this.gameObject.GetComponent<AudioLowPassFilter>() == null ) this.gameObject.AddComponent<AudioLowPassFilter> ();
		if( this.gameObject.GetComponent<AudioHighPassFilter>() == null ) this.gameObject.AddComponent<AudioHighPassFilter> ();

		
		lpf = this.gameObject.GetComponent<AudioLowPassFilter> ();
		hpf = this.gameObject.GetComponent<AudioHighPassFilter> ();
	}
	
	void Update () {
		


		lpf.cutoffFrequency = cutoffFrequency;
		lpf.lowpassResonanceQ = Quality; // Resona(n)ce !!! learn how to wright please !!!

		hpf.cutoffFrequency = cutoffFrequency;
		hpf.highpassResonanceQ = Quality;

	}

}