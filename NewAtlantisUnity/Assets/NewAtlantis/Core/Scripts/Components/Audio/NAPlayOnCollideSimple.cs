using UnityEngine;
using System.Collections;

public class NAPlayOnCollideSimple : MonoBehaviour {

	AudioSource aud;
	// Use this for initialization
	void Start () {
	
		Rigidbody rg = GetComponent<Rigidbody> ();
		if (rg == null) {
			this.gameObject.AddComponent<Rigidbody>();
			rg = GetComponent<Rigidbody> ();
			rg.useGravity = false;
			rg.isKinematic = true;

		}

		aud = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision e){

		if (aud != null)
		//print ("colliding"+e.gameObject);
		aud.Play ();

	}

}
