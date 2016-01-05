using UnityEngine;
using System.Collections;


	public class NAPlayOnKeyPressed : MonoBehaviour 
	{	
		public KeyCode key;
		// Use this for initialization
		void Start () 
		{
		}
		// Update is called once per frame
		void Update () 
		{
			if (!NA.isClient())
			{
				if (Input.GetKeyDown(key))
				{
					GetComponent<AudioSource>().volume = 1f;
					GetComponent<AudioSource>().Play();
				}
			}
		}
	}

