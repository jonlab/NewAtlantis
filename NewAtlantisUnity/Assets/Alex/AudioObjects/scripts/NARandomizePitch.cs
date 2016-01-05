using UnityEngine;
using System.Collections;

public class NARandomizePitch : MonoBehaviour {
	AudioSource aud;
	public float minPitch , maxPitch;
	// Use this for initialization
	void Start () {
		aud = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		aud.pitch = (float)Random.Range (minPitch, maxPitch);
	}
}
