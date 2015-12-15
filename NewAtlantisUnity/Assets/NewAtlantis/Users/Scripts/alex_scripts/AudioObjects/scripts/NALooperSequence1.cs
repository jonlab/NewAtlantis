using UnityEngine;
using System.Collections;

public class NALooperSequence1 : MonoBehaviour {
	public int interval = 50;
	AudioSource aud;
	int step=0;
	public float[] pitches;
	// Use this for initialization
	void Start () {


		aud = GetComponent<AudioSource> ();


	}
	
	// Update is called once per frame
	void Update () {
	
		interval = Mathf.Clamp (interval, 1, 10000000);

		if (Time.frameCount % interval == 0) {

			aud.Play ();
			aud.pitch = pitches[step%pitches.Length];

			step++;
		}



	}
}
