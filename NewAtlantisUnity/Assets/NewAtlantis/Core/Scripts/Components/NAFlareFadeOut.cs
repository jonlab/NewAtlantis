using UnityEngine;
using System.Collections;

public class NAFlareFadeOut : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		float distance = (Camera.main.transform.position-transform.position).magnitude;
		LensFlare[] flares = GetComponentsInChildren<LensFlare>();
		bool bEnabled = (distance < 20) ? true : false;
		foreach (LensFlare f in flares) 
		{
			f.enabled = bEnabled;
		}
	}
}
