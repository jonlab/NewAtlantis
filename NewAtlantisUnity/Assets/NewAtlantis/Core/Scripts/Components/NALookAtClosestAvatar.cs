using UnityEngine;
using System.Collections;

public class NALookAtClosestAvatar : MonoBehaviour 
{
	public float MaximumDistance = 10f;
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (NA.isServer() || NA.isStandalone())
		{
			GameObject goClosest = NA.GetClosestAvatar(transform.position, MaximumDistance);
			if (goClosest != null)
			{
				transform.LookAt(goClosest.transform);
			}
		}
	}
}
