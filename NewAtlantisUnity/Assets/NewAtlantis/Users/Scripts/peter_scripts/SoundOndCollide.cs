using UnityEngine;
using System.Collections;

public class SoundOndCollide : MonoBehaviour {

	void OnCollisionEnter(Collision collision) {

			GetComponent<AudioSource>().Play();
		//Debug.Log ("rferge");
	}
}
