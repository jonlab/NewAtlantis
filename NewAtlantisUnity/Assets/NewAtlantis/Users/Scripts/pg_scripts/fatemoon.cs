using UnityEngine;
using System.Collections;

public class fatemoon : MonoBehaviour {
	public AudioClip mysound;
	bool on = false;

	// Use this for initialization
	void Start () {
		if (GetComponent<AudioSource>())
			GetComponent<AudioSource>().Stop ();
		
	}
	void OnMouseEnter ()
	{
		GetComponent<Renderer>().material.color = Color.white;
	}
	
	void OnMouseDown()
	{
		on = !on;

	}
	// Update is called once per frame
	void Update () {


			if (on)
		
			GetComponent<AudioSource>().Play ();

			else

				GetComponent<AudioSource>().Stop ();
			
		
	}
}

