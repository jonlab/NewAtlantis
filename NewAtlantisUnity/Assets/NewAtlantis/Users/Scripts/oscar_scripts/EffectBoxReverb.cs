using UnityEngine;
using System.Collections;

public class EffectBoxReverb : MonoBehaviour {

	//public GameObject camera;
	AudioReverbFilter lowPass;

	public GameObject[] triggerCube;

	public int decay = 1000;


	// Use this for initialization
	void Start () {
	
		//lowPass = camera.GetComponent<AudioLowPassFilter> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider e){

		if (e.tag != "Player")
			return;



		for (int i = 0; i < triggerCube.Length; i++) {

			lowPass = triggerCube[i].GetComponent<AudioReverbFilter> ();
			if (lowPass == null){
				triggerCube[i].AddComponent<AudioReverbFilter> ();}
			else{
				
				
				triggerCube [i].GetComponent<AudioReverbFilter> ().enabled = true;
			}


			lowPass = triggerCube[i].GetComponent<AudioReverbFilter> ();
			lowPass.decayTime = decay;

		}

	}

	void OnTriggerExit(Collider e){

		
		for (int i = 0; i < triggerCube.Length; i++) {


			lowPass = triggerCube[i].GetComponent<AudioReverbFilter> ();
			if (lowPass != null)
				lowPass.enabled = false;

		
		}
	

	}
	

}
