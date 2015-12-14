using UnityEngine;
using System.Collections;

public class moon1 : MonoBehaviour {
	public AudioClip mysound;
	bool on = false;

	void Start () {
		if (GetComponent<AudioSource>())
			GetComponent<AudioSource>().Stop ();
		GameObject parent = GameObject.Find ("Vahalla");
		transform.parent = parent.transform;
		
	}
	void OnMouseEnter ()
	{
		GetComponent<Renderer>().material.color = Color.white;
	
	/*void OnMouseDown()
		{*/
		on = !on;
		if (on)
			
			GetComponent<AudioSource>().Play ();
		
		else
			
			GetComponent<AudioSource>().Stop ();
		}

	// Update is called once per frame
	void Update () {

			
			//transform.Rotate(new Vector3(Time.deltaTime*1f, Time.deltaTime*0.3f, Time.deltaTime*2f));\
			Vector3 centerrotation = transform.parent.position;
			transform.RotateAround(centerrotation, new Vector3(0.1f, 0.8f,0.2f), -Time.deltaTime*1.6f);
			
		}
		
		
		
	}

