using UnityEngine;
using System.Collections;

public class NAPlayOnAvatarClose : MonoBehaviour {

	public float MaximumDistance = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{


		if (NA.isServer() || NA.isStandalone())
		{
			GameObject goClosest = NA.GetClosestAvatar(transform.position, MaximumDistance);
			if (goClosest != null)
			{
				if (!GetComponent<AudioSource>().isPlaying)
				{
					GetComponent<AudioSource>().Play();
				}
			}
			else
			{
				GetComponent<AudioSource>().Stop();
			}
		}
	}
}
