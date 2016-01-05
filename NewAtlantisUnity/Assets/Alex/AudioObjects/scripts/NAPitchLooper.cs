using UnityEngine;
using System.Collections;

public class NAPitchLooper : MonoBehaviour {
	public int interval = 50;
	AudioSource aud;
	public float pitch = 1.0f;
	// Use this for initialization
	void Start () {
		aud = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		interval = Mathf.Clamp (interval, 1, 10000000);
		pitch = Mathf.Clamp (pitch, -3, 3);

		if (Time.frameCount % interval == 0) {

			aud.Play ();
		}

		aud.pitch = pitch;

	}
}
