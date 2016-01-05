using UnityEngine;
using System.Collections;

public class magcub : MonoBehaviour {
	public AudioClip mysound;
	Vector3 target;
	Vector3 startpos1;
	bool on = false;
	// Use this for initialization
	void Start () {
		target = new Vector3 (-0.0410f, -2.0f, -0.9652214f);
		if (GetComponent<AudioSource>())
			GetComponent<AudioSource>().Stop ();
		startpos1 = transform.position;
		
	}
	void OnMouseEnter ()
	{
		GetComponent<Renderer>().material.color = Color.green;
	}
	
	void OnMouseDown()
	{
		on = true;
		GetComponent<AudioSource>().Play ();
	}
	// Update is called once per frame
	void Update () {
		
		if (on)
		{
			Vector3 direction = target - transform.position;
			
			
			if (direction.magnitude > 00.0082)
			{
				direction.Normalize ();
				//transform.Translate (Vector3.forward * Time.deltaTime * 1);
				transform.Translate (direction * Time.deltaTime * 0.21f);
				//transform.LookAt (target);
			}
			else
			{
				on = false;
				GetComponent<AudioSource>().Stop ();
				transform.position = startpos1;
				GetComponent<Renderer>().material.color = Color.white;
				
			}
		}
		
		
		
	}
}