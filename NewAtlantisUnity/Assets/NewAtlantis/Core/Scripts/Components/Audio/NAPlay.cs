using UnityEngine;
using System.Collections;


	public class NAPlay : MonoBehaviour 
	{
		// Use this for initialization
		void Start () 
		{
			if (GetComponent<AudioSource>())
				GetComponent<AudioSource>().Play();
			else
				Debug.Log ("You have to attach an AudioSource !");
		}
		
		// Update is called once per frame
		void Update () 
		{

		}
	}

