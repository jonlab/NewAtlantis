using UnityEngine;
using System.Collections;

public class NAPlayOnButtonPressed : MonoBehaviour 
	{	
		public string buttonName;
		// Use this for initialization
		void Start () 
		{
		}
		// Update is called once per frame
		void Update () 
		{
			if (!NA.isClient())
			{
	        	if (Input.GetButtonDown(buttonName))
				{
					GetComponent<AudioSource>().volume = 1f;
					GetComponent<AudioSource>().Play();
				}
			}
		}
	}

