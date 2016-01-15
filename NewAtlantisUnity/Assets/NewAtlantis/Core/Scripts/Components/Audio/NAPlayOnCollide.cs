using UnityEngine;
using System.Collections;



public class NAPlayOnCollide : MonoBehaviour 
{
	public float VelocityThreshold = 0.5f; //m.s-1
	//retrig ?
	//public AnimationCurve curveVolume = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
	public AnimationCurve curveVolume = AnimationCurve.Linear(0,0,1,1);

	public bool StopOnExit;
	public bool PitchOnStay;
	public bool Toggle;

	private float InitialVolume = 1f;

	public GameObject target; //if null, ge take this GameObject as the target (AudioSource to play)
	public float delay = 0f; //delay in seconds

	void Start()
	{
		if (target == null)
			target = this.gameObject;

		AudioSource audio = target.GetComponent<AudioSource>();
		if (audio)
		{
			InitialVolume = audio.volume;
		}

	}

	void OnCollisionEnter(Collision collision) 
	{
		if (NA.isClient() && NA.syncMode != SyncMode.NoInDepthSync)
			return;
		//volume is relative to relative velocity
		//float magnitude = collision.relativeVelocity.magnitude;
		float magnitude = curveVolume.Evaluate(collision.relativeVelocity.magnitude);
		if (magnitude > VelocityThreshold)
		{
			float vol = magnitude*4f;
			vol = Mathf.Clamp(vol, 0, 1f) * InitialVolume;
			AudioSource audio = target.GetComponent<AudioSource>();
			if (audio != null && audio.clip != null)
			{
				ulong delaysamples = (ulong)(delay * (float)audio.clip.frequency);
				audio.volume = vol;

				if (Toggle)
				{
					if (!audio.isPlaying)
					{
					audio.Play(delaysamples);
					}
					else
					{
						audio.Stop();
					}
				}
				else
				{
					if (!audio.isPlaying)
					{
					audio.Play(delaysamples);
					}
				}
			}
		}
		//collision.contacts
	}
	void OnCollisionStay(Collision collision) 
	{
		if (NA.isClient() && NA.syncMode != SyncMode.NoInDepthSync)
			return;
		AudioSource audio = target.GetComponent<AudioSource>();
		float s = 0f;
		if (PitchOnStay && audio != null)
		{
			audio.pitch = 1f+collision.relativeVelocity.magnitude;
		}
	}

	void OnCollisionExit(Collision collision) 
	{
		if (NA.isClient() && NA.syncMode != SyncMode.NoInDepthSync)
			return;
		if (target == null)
			target = this.gameObject;
		AudioSource audio = target.GetComponent<AudioSource>();
		if (StopOnExit && audio != null)
		{
			audio.Stop();
		}
	}




}

