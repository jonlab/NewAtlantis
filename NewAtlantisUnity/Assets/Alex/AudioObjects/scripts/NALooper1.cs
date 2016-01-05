using UnityEngine;
using System.Collections;

public class NALooper1 : MonoBehaviour {
	public int interval = 50;
	AudioSource aud;
	// Use this for initialization
	void Start () {
		aud = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		interval = Mathf.Clamp (interval, 1, 10000000);

		if (Time.frameCount % interval == 0) {

			aud.Play ();
		}

	}
}
