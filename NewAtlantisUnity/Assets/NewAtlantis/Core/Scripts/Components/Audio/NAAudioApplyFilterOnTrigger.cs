using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAAudioApplyFilterOnTrigger : MonoBehaviour {

	//public Behaviour fx = null;
	public string fx = "distortion";
	List<Behaviour> filters = new List<Behaviour>();
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}


	void OnTriggerEnter(Collider collider)
	{
		Debug.Log("trigger enter");
		AudioSource source = collider.gameObject.GetComponent<AudioSource>();
		if (source)
		{
			if (fx.Contains("distortion"))
			{
				//Apply Filter
				AudioDistortionFilter f = collider.gameObject.GetComponent<AudioDistortionFilter> ();
				if (f == null)
				{
					f = collider.gameObject.AddComponent<AudioDistortionFilter> ();
				}
				f.enabled = true;
				f.distortionLevel = 0.8f;
				filters.Add(f);
			}
			if (fx.Contains("echo"))
			{
				//Apply Filter
				AudioEchoFilter f = collider.gameObject.GetComponent<AudioEchoFilter> ();
				if (f == null)
				{
					f = collider.gameObject.AddComponent<AudioEchoFilter> ();
				}
				f.enabled = true;
				f.delay = 100f;
				f.decayRatio = 0.9f;
				filters.Add(f);
			}
			if (fx.Contains("chorus"))
			{
				//Apply Filter
				AudioChorusFilter f = collider.gameObject.GetComponent<AudioChorusFilter> ();
				if (f == null)
				{
					f = collider.gameObject.AddComponent<AudioChorusFilter> ();
				}

				//f.depth = 0.99f;
				f.enabled = true;
				filters.Add(f);
			}
			if (fx.Contains("highpass"))
			{
				//Apply Filter
				AudioHighPassFilter f = collider.gameObject.GetComponent<AudioHighPassFilter> ();
				if (f == null)
				{
					f = collider.gameObject.AddComponent<AudioHighPassFilter> ();
				}
					
				f.enabled = true;
				filters.Add(f);

			}

			if (fx.Contains("lowpass"))
			{
				//Apply Filter
				AudioLowPassFilter f = collider.gameObject.GetComponent<AudioLowPassFilter> ();
				if (f == null)
				{
					f = collider.gameObject.AddComponent<AudioLowPassFilter> ();
				}
				f.lowpassResonanceQ = 2;
				f.cutoffFrequency = 1000;
				f.enabled = true;
				filters.Add(f);
			}
		}


	}

	void OnTriggerExit(Collider collider)
	{
		AudioSource source = collider.gameObject.GetComponent<AudioSource>();
		if (source)
		{
			foreach (Behaviour b in filters)
			{
				b.enabled = false;
			}
		}

		
	}



}
