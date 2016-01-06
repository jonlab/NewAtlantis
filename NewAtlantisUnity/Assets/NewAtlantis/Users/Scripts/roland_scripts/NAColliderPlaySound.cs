using UnityEngine;
using System.Collections;

public class NAColliderPlaySound : MonoBehaviour {
	
	public AudioClip impactSound;
	public float impactThreshold = 0F;
	public float impactPitchControl = 1F;
	public float impactVolumeControl = 1F;
	public float volumeOffset = 1F;
	public float pitchOffset = 1F;

	
	void OnCollisionEnter(Collision other) {
		//print ("collider" + other);
		if (other.relativeVelocity.magnitude > impactThreshold){      
			//Debug.Log("CollisionVelocity " + " " + other.relativeVelocity.magnitude);
			GetComponent<AudioSource>().PlayOneShot(impactSound);
			GetComponent<AudioSource>().pitch = (other.relativeVelocity.magnitude * impactPitchControl) + (1F - impactPitchControl);
			GetComponent<AudioSource>().volume = (other.relativeVelocity.magnitude * impactVolumeControl) + volumeOffset;
			//Debug.Log("Volume " + " " + audio.volume);
			
		}
	}
}

