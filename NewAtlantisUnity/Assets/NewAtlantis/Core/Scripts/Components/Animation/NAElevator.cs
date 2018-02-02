using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAElevator : MonoBehaviour {

	public float minimumHeight = 0;
	public float maximumHeight = 1000;
	public float speed = 1; //speed in m/s

	// Use this for initialization
	void Start () 
	{
		speed *= 2;
		//speed = 0;
		if (!NA.isClient ()) 
		{
			GameObject goRootAvatars = GameObject.Find ("Main Viewer");
			goRootAvatars.transform.parent = transform;
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (!NA.isClient ()) 
		{
			Vector3 pos = transform.position;
			pos += Vector3.up * Time.deltaTime * speed;
			if (pos.y > maximumHeight)
			{
				pos.y = minimumHeight;
				//RissetManager.hasRestarted = true; //crash
			}
			transform.position = pos;
		}
	}
}
