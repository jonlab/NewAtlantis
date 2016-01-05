using UnityEngine;
using System.Collections;

	public class NAPlayOnTrigger : MonoBehaviour 
	{
		public bool StopOnExit = false;
		public bool Toggle = false;
		void OnTriggerEnter(Collider collider) 
		{
			AudioSource audio = GetComponent<AudioSource>();

			if (Toggle)
			{
				if (!audio.isPlaying)
				{
					audio.Play();
				}
				else
				{
					audio.Stop();
				}
			}
			else
			{
				//if (!audio.isPlaying)
				{
					audio.Play();
				}
			}
		}

		void OnTriggerExit(Collider collider) 
		{
			if (StopOnExit)
				GetComponent<AudioSource>().Stop();
		}
	}

