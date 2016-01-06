using UnityEngine;
using System.Collections;

public class MaCollisionVelocity : MonoBehaviour {

	public AudioClip impactSound;
	public float ImpactSoundForce;
	//public float ImpactSoundForceTrigger;
 
		void OnCollisionEnter(Collision other) {
		//Debug.Log("CollisionVelocity " + other + other.relativeVelocity.magnitude);
   			//if (other.relativeVelocity.magnitude > ImpactSoundForceTrigger){      
				
				GetComponent<AudioSource>().PlayOneShot(impactSound);
				GetComponent<AudioSource>().pitch = other.relativeVelocity.magnitude/ImpactSoundForce;
				GetComponent<AudioSource>().volume = other.relativeVelocity.magnitude;
				

		//}
   }
}

