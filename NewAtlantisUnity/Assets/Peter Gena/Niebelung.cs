using UnityEngine;
using System.Collections;

public class Niebelung : MonoBehaviour {
	public AudioClip mysound;
	bool on = false;

	void Start () {
		if (GetComponent<AudioSource>())
			GetComponent<AudioSource>().Stop ();
	}
	void OnMouseEnter ()
	{
	
	}

    void OnCollisionEnter(Collision e)
    {

        GetComponent<Renderer>().material.color = Color.white;

        on = !on;
        if (on)

            GetComponent<AudioSource>().Play();

        else

            GetComponent<AudioSource>().Stop();


    }


	void Update () 
	{
		transform.Rotate(new Vector3(Time.deltaTime* -0.35f, Time.deltaTime*3.0f, 0.1f));

	}
}