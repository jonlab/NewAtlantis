using UnityEngine;
using System.Collections;

public class NATimeChanger : MonoBehaviour 
{
	float TimeScale = 0.5f;
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
		if (NA.isServer() || NA.isStandalone())
		{
			Time.timeScale = TimeScale;
		}
	}
}
