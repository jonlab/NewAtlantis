using UnityEngine;
using System.Collections;

	public class Notung : MonoBehaviour {
	
	public AudioClip mysound;
	Vector3 startpos;
	// Use this for initialization
	void Start () 
	{
		if (GetComponent<AudioSource>())
			GetComponent<AudioSource>().Stop ();
		startpos = transform.position;
	}

	// Update is called once per frame
	void Update () 
	{
		if (!GetComponent<AudioSource>().isPlaying)
		{
			transform.position = startpos;
			GetComponent<Rigidbody>().useGravity = false;
			GetComponent<Renderer>().material.color = Color.red;

		}
	}

	void OnMouseDown ()
	{
		GetComponent<Renderer>().material.color = Color.white;
		GetComponent<AudioSource>().Play ();
		GetComponent<Rigidbody>().useGravity = true;
	}
	
	void OnMouseExit ()
	{
		//renderer.material.color = Color.white;
	}
	


	void OnMouseUp () 
	{
						//audio.Stop ();
						//transform.position = startpos;
						//rigidbody.useGravity = false;
						//renderer.material.color = Color.white;
	}

}
