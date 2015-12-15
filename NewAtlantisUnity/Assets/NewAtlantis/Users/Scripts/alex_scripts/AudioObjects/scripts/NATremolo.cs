using UnityEngine;
using System.Collections;

public class NATremolo : MonoBehaviour {

	AudioSource aud;
	public int gate= 6;
	// Use this for initialization
	void Start () {
		aud = GetComponent<AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {
		gate = Mathf.Clamp (gate, 2, 10000);
	if (Time.frameCount % gate < gate / 2) {
			aud.volume = 0;
		} else {
			aud.volume = 1;

		}





	}

	void OnDisable(){

		aud.volume = 1;
	}
}
