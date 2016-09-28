using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NAReverbEffector : MonoBehaviour 
{

	private List<NAReverbResonator> resonators = new List<NAReverbResonator>();
	// Use this for initialization
	void Start () 
	{
		//EnableAudioReverbFilter (false);

		AudioReverbFilter arf = gameObject.GetComponent<AudioReverbFilter>();
		if (arf == null)
		{
			arf = gameObject.AddComponent<AudioReverbFilter>();
		}

		Rigidbody rb = gameObject.GetComponent<Rigidbody>();
		if (rb == null)
		{
			rb = gameObject.AddComponent<Rigidbody>();
			rb.isKinematic = true;
		}

		Collider c = gameObject.GetComponent<Collider>();
		if (c == null)
		{
			c = gameObject.AddComponent<SphereCollider>();
			c.isTrigger = true;
		}
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void LateUpdate()
	{
		//return;

		AudioReverbFilter to = GetComponent<AudioReverbFilter> ();
		if (to == null)
			return;
		to.decayHFRatio = 0;
		to.decayTime = 0;
		to.density = 0;
		to.diffusion = 0;
		//to.reflections 		= from.reflections;
		//to.reflectionsDelay = from.reflectionsDelay;
		//to.reverb 			= from.reverb;
		to.reverbDelay = 0;
		to.reverbPreset = 0;
		to.room = 0;
		to.roomHF = 0;
		to.roomLF = 0;
		to.dryLevel = 0;
		//to.reverbPreset = AudioReverbPreset.User;
		//interpolation of all resonators and apply to this AudioReverbFilter
		int count = resonators.Count;
		foreach (NAReverbResonator r in resonators)
		{
			try
			{
				AudioReverbZone from = r.GetComponent<AudioReverbZone> ();
				//to.dryLevel 		+= from.dryLevel;
				//to.decayHFRatio 	+= from.decayHFRatio/count;
				//to.decayTime		+= from.decayTime/count;
				//to.density 			+= from.density/count;
				//to.diffusion 		+= from.diffusion/count;
				//to.reflections 		= from.reflections;
				//to.reflectionsDelay = from.reflectionsDelay;
				//to.reverb 			= from.reverb;
				//to.reverbDelay 		+= from.reverbDelay/count;
				//to.reverbPreset 	+= from.reverbPreset/count;
				//to.room				+= from.room/count;
				//to.roomHF 			+= from.roomHF/count;
				//to.roomLF 			+= from.roomLF/count;
				//to.roomRolloffFactor = from.roomRolloffFactor;


				to.reverbPreset = r.reverbPreset;
			}
			catch (System.Exception e)
			{
				count --;
			}

		}

		if (count > 0)
			EnableAudioReverbFilter (true);
		else
			EnableAudioReverbFilter (false);


	}

	public void AddResonator(NAReverbResonator r)
	{
		if (!resonators.Contains(r))
		{
			resonators.Add(r);
		}

	}

	public void RemoveResonator(NAReverbResonator r)
	{
		if (resonators.Contains(r))
		{
			resonators.Remove(r);
		}
		
	}

	public void EnableAudioReverbFilter(bool enabled)
	{
		AudioReverbFilter filter = GetComponent<AudioReverbFilter> ();
		if (filter)
		{
			filter.enabled = enabled;
		}
	}






}
