using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailboatWind : MonoBehaviour {

	public float windStrength = 10.0f;
	public float turningSpeed = 1.0f;
	public float waveMagnitude = 1.0f;
	Rigidbody rb;
	Vector3 targetDirection;
	AudioSource audioSource;
	void Start () {
		rb=GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource> ();
		targetDirection = transform.forward;
	}

	// add in some kind of up/down wave motion
	void FixedUpdate() {
		// bobbing

		float upDown = Mathf.Sin(Time.time);

		rb.AddForce(new Vector3(0,upDown*waveMagnitude,0),ForceMode.Force);

		// update turning 

		float step = turningSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);	
	}
	void Update () {
		UpdateAudioVolume ();


	}

	// apply a wind force
	void ApplyWind (Vector3 windVelocity)
	{
		
		rb.AddForce(windVelocity * windStrength, ForceMode.Impulse);
		targetDirection = windVelocity;
		

	}


	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player")
		{
			GameObject go = other.gameObject;
			Vector3 windVec = transform.position - go.transform.position;
			windVec.y=0;
			windVec.Normalize();
			Rigidbody rb = go.GetComponent<Rigidbody>();

	        ApplyWind(windVec);
	    }
    }

	// control audio volume by velocity
	// top speed seems to be around 5
	void UpdateAudioVolume()
	{
		float speed = rb.velocity.magnitude;
		float vol = Mathf.Min (speed / 4.0f, 1.0f);
		if (vol < 0.05f) {
			// pause playback when it gets quiet
			audioSource.Stop ();
		} else {
			if (!audioSource.isPlaying) {
				audioSource.Play ();
			}
			audioSource.volume = vol;
		}
	}

}
