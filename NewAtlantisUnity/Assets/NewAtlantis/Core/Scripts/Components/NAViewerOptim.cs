using UnityEngine;
using System.Collections;

public class NAViewerOptim : MonoBehaviour 
{
	Light[] 		lights;
	AudioSource[] 	sources;
	LensFlare[] 	flares;

	public float 	distance = 150f;
	public bool		bCulled = false;

	// Use this for initialization
	void Start () 
	{
		lights 		= GetComponentsInChildren<Light>();
		sources 	= GetComponentsInChildren<AudioSource>();
		flares 		= GetComponentsInChildren<LensFlare>();
	}

	void Cull(bool b)
	{
		bCulled = b;
		foreach (Light l in lights)
		{
			if (l.type != LightType.Directional)
			{
				l.enabled = !b;
			}
		}
		foreach (AudioSource s in sources)
		{
			//s.enabled = !b;

			//if (s.enabled && s.playOnAwake)
			//	s.Play();
			/*
			if (!b && s.playOnAwake)
			{
				s.Play();
			}
			else if (b)
			{
				s.Stop();
			}
			*/

			if (!b)
			{
				s.priority = 0;
			}
			else if (b)
			{
				s.priority = 255;
			}
		}
		foreach (LensFlare f in flares)
		{
			f.enabled = !b;
		}
	}


	// Update is called once per frame
	void Update () 
	{
		float d = (Camera.main.transform.position-transform.position).magnitude;
		if (bCulled)
		{
			if (d < distance)
			{
				Cull(false);
			}
		}
		else
		{
			if (d > distance)
			{
				Cull(true);
			}
		}
	}





}
