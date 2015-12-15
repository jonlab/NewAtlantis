using UnityEngine;
using System.Collections;

public class NAModulateEcho1 : MonoBehaviour {

	AudioEchoFilter echo;
	public float minFeedbackRatio = 0.5f;

	public int modulationFreq = 10;
	public int divideFeedback = 10;
	// Use this for initialization
	void Start () {
		echo = GetComponent<AudioEchoFilter> ();

		if (echo == null) {
			print ("null");
			this.gameObject.AddComponent<AudioEchoFilter>();
			echo = GetComponent<AudioEchoFilter> ();

		}

	}
	
	// Update is called once per frame
	void Update () {
	
		float delay = ((float)Time.frameCount % (float)modulationFreq);
		echo.delay = delay ;
		echo.decayRatio = minFeedbackRatio +  delay / divideFeedback;

	}
}
