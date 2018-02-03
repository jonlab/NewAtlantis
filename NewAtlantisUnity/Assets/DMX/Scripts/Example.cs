﻿using UnityEngine;
using System.Collections;

public class Example : MonoBehaviour {

	public GameObject dmxObject;
	DP.DMX dmx;

	// Use this for initialization
	void Start () 
	{
		dmxObject = GameObject.Find("DMXObject");
		dmx = dmxObject.GetComponent<DP.DMX>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Set Value
		dmx[1] = (int)Mathf.PingPong(Time.time*100, 255);

		//Get Value
		dmx[3] = dmx[1];
		dmx[0] = dmx[1];
		dmx[2] = dmx[1];


		for (int i = 0; i < 512; ++i)
		{
			dmx [i] = 255;
		}
	}
}
