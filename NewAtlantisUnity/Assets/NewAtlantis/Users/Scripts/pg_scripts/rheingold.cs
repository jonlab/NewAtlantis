using UnityEngine;
using System.Collections;

public class rheingold: MonoBehaviour {
	public AudioClip mysound;
	bool on = false;

	void Start () {
		if (GetComponent<AudioSource>())
			GetComponent<AudioSource>().Stop ();
	}
	void OnMouseEnter ()
	{
		GetComponent<Renderer>().material.color = Color.white;

		on = !on;
		if (on)
			
			GetComponent<AudioSource>().Play ();
		
		else
			
			GetComponent<AudioSource>().Stop ();
	}
	void Update () 
	{
		transform.Rotate(new Vector3(Time.deltaTime*0.55f, Time.deltaTime*12.0f, 0.0f));

	}
}