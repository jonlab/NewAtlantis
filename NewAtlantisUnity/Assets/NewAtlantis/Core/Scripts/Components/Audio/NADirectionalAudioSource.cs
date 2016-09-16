using UnityEngine;
using System.Collections;

/**
 * Experimental directional effect applied on a audio source
 * Usage : 
 * set the Directivity Curve to define the audible zone
 * curve's point 0 represents the center of the audible zone
 * curve's point 1 represents the end of the audible zone
**/

public class NADirectionalAudioSource : MonoBehaviour {
	static AudioListener mListener;
	private AudioSource audio;
	public AnimationCurve DirectivityCurve=AnimationCurve.Linear(0,1,1,0);

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		mListener = GameObject.FindObjectOfType(typeof(AudioListener)) as AudioListener;

		var targetDir = mListener.transform.position - transform.position;
		var forwardDir = transform.forward;
		targetDir.y = forwardDir.y;
		var delta = Vector3.Angle(targetDir, forwardDir);

		var vol = DirectivityCurve.Evaluate( delta/180 );
		audio.volume = vol;
	}
}
