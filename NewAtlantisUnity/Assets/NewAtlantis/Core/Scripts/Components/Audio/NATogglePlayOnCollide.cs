using UnityEngine;
using System.Collections;



	public class NATogglePlayOnCollide : MonoBehaviour 
	{
		public float VelocityThreshold = 0.5f; //m.s-1
		//retrig ?
		//public AnimationCurve curveVolume = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public AnimationCurve curveVolume = AnimationCurve.Linear(0,0,1,1);

		public bool StopOnExit;
		public bool PitchOnStay;


		void Start()
		{
			//curveVolume = AnimationCurve.Linear(
		}
		void OnCollisionEnter(Collision collision) 
		{
			//volume is relative to relative velocity
			//float magnitude = collision.relativeVelocity.magnitude;
			float magnitude = curveVolume.Evaluate(collision.relativeVelocity.magnitude);
			if (magnitude > VelocityThreshold)
			{
				float vol = magnitude*4f;
                AudioSource audio = GetComponent<AudioSource>();
				audio.volume = vol;
                if (!audio.isPlaying)
                {
				    audio.Play();
                }
                else
                {
                    audio.Stop();
                }
			}
			//collision.contacts
		}
		void OnCollisionStay(Collision collision) 
		{
			float s = 0f;
			if (PitchOnStay)
			{
				GetComponent<AudioSource>().pitch = 1f+collision.relativeVelocity.magnitude;
			}
		}




	}

