using UnityEngine;
using System.Collections;



	//NaUnmuteOnCollide turns the AudioSource volume to zero at start and on to it's set value while colliding with any collider.
	// Typically play on awake all loops to keep them synchonized and turn volume on during collision.

	public class NAUnmuteOnCollide : MonoBehaviour 
	{

	AudioSource audio;
	float InitialVolume ;	


		void Start(){

			audio = 	GetComponent<AudioSource>();

			InitialVolume = audio.volume;	
			audio.volume = 0f;


	}

	
		void OnCollisionEnter(Collision collision) 
		{
			//if (!NA.isClient ())
		if (NA.isClient() && NA.syncMode != SyncMode.NoInDepthSync)
			return;
		
			{
				audio.volume = InitialVolume * 1f;
				//Debug.Log("on " + GetComponent<AudioSource>().volume);
			}
			
		}
		//void OnCollisionStay(Collision collision)
		//{
		//GetComponent<AudioSource>().volume = 1f;
		//}	
		
		void OnCollisionExit(Collision collision) 
		{
			//if (!NA.isClient ())
		if (NA.isClient() && NA.syncMode != SyncMode.NoInDepthSync)
			return;
			{
				audio.volume = InitialVolume * 0f;
				//Debug.Log("off " + GetComponent<AudioSource>().volume );
			}
		}
		
		
	}
