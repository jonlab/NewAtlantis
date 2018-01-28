using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//main logic for the Ursula Beton Salon installation version
public class NAUrsulaLogic : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		//input webcam process
		gameObject.AddComponent<WebcamProcess> ();
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
