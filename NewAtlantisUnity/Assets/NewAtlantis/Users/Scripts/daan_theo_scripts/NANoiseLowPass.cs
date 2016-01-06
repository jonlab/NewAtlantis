using UnityEngine;
using System.Collections; // Needed for Random

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
public class NANoiseLowPass : MonoBehaviour
{
	// THIS CLASS IS MEANT TO BE REMOVED
	// DEPRECIATED // TEMPORARY CLASS

	//public double frequency = ;
	[Header("Sound Settings")]
	[Tooltip("Any value from 10 to 22000")]
	public float minCutoffFrequency = 60;
	[Tooltip("Any value from 10 to 22000")]
	public float maxCutoffFrequency = 22000;
	public double maxVolume = 0.4;
	
	[Header("Animation Settings")]
	public float sensitivity = 1.0f;
	
	private NAAccelerometer acceleration = null;
	private NAFlubberMesh flubber = null;
	private NAMeshDistorsion meshDistort = null;
	private AudioLowPassFilter lpf = null;
	private System.Random RandomNumber = new System.Random();
	
	public void Start(){
		// check for audiosource (ensures OnAudioFilterRead() is available)
		if( this.gameObject.GetComponent<AudioSource>() == null ) this.gameObject.AddComponent<AudioSource>();
		
		// check for AudioLowPassFilter
		lpf = this.gameObject.GetComponent<AudioLowPassFilter>();
		if( lpf == null ) lpf = this.gameObject.AddComponent<AudioLowPassFilter>();
		
		// check for NAAccelerometer component
		acceleration=this.gameObject.GetComponent<NAAccelerometer>();
		if(acceleration!=null) Debug.Log("Notice: NASinusWave["+this.gameObject.name+"] has been bound with it's own NAAccelerometer component.");
		
		// check for flubberMesh
		flubber=this.gameObject.GetComponent<NAFlubberMesh>();
		meshDistort=this.gameObject.GetComponent<NAMeshDistorsion>();
	}
	
	void OnAudioFilterRead(float[] data, int channels){

		// update increment in case frequency has changed
		//increment = frequency * 2 * Math.PI / sampling_frequency;
		
		for (var i = 0; i < data.Length; i = i + channels){
			data[i] = (float)(maxVolume* (-1f + RandomNumber.NextDouble()*2f) );
			
			// if we have stereo, we copy the mono data to each channel
			if (channels == 2) data[i + 1] = data[i];
		}
	}
	
	void Update(){
		if(acceleration){
			maxVolume = .9f-Mathf.Sqrt(.9f+acceleration.averageFriction);
		}
		
		// apply low pass filter
		if(lpf && flubber){
			float newCutoff;
			newCutoff = ( lpf.cutoffFrequency + flubber.getFlubberMovement()* 1000 * sensitivity );
			newCutoff -= minCutoffFrequency;
			newCutoff %= maxCutoffFrequency-minCutoffFrequency;
			newCutoff += minCutoffFrequency;
			
			lpf.cutoffFrequency = newCutoff;
			
			lpf.lowpassResonanceQ = 1f+Mathf.Round( flubber.getFlubberMovement() * sensitivity );
		}
		else if(lpf && meshDistort){
			float newCutoff;
			newCutoff = ( lpf.cutoffFrequency + meshDistort.getDistorsion()* 1000 * sensitivity );
			newCutoff -= minCutoffFrequency;
			newCutoff %= maxCutoffFrequency-minCutoffFrequency;
			newCutoff += minCutoffFrequency;
			
			lpf.cutoffFrequency = newCutoff;
			
			lpf.lowpassResonanceQ = 1f+Mathf.Round( meshDistort.getDistorsion() * sensitivity );
		}
		else if(lpf && acceleration){
			// note: the inspector won't show these variations
			// Lowpass cutoff frequency in hz. 10.0 to 22000.0.
			//lpf.cutoffFrequency += acceleration.averageacceleration*100*(acceleration.direction.x+acceleration.direction.z);
			lpf.cutoffFrequency += Mathf.Abs( acceleration.averageFriction* sensitivity );
			//print (lpf.cutoffFrequency);
		}
	}
} 