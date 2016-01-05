using UnityEngine;
using System.Collections;

public class NAPitchLooper2 : MonoBehaviour {
	public int interval = 50;
	AudioSource aud;
	public float pitch = 1.0f;
	int step=0;
	public float accelerationPitch = 0.001f;
	// Use this for initialization
	void Start () {
		aud = GetComponent<AudioSource> ();
		aud.pitch = pitch;
	}
	
	// Update is called once per frame
	void Update () {
	
		interval = Mathf.Clamp (interval, 1, 10000000);

		if (Time.frameCount % interval == 0) {

			aud.Play ();
		}


		if (pitch == 3 || pitch == -3)
			step++;

		if(step%2 == 0){
			pitch+=accelerationPitch;

		}else{

			pitch-=accelerationPitch;

		}

		aud.pitch = pitch;


		pitch = Mathf.Clamp (pitch, -3, 3);


	}
}
