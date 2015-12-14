using UnityEngine;
using System.Collections;

public class EffectBoxLowPass : MonoBehaviour {

//	public GameObject camera;
	AudioLowPassFilter lowPass;

	public GameObject[] triggerCube;

	public int frequency = 1000;


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

		print ("trigger ");
		for (int i = 0; i < triggerCube.Length; i++) {


			lowPass = triggerCube [i].GetComponent<AudioLowPassFilter> ();
			if (lowPass == null){
				//print ("add component because colliding with ");
				triggerCube [i].AddComponent<AudioLowPassFilter> ();
			}else{


				triggerCube [i].GetComponent<AudioLowPassFilter> ().enabled = true;
			}


			lowPass = triggerCube [i].GetComponent<AudioLowPassFilter> ();
			lowPass.cutoffFrequency = frequency;

		}


	}

	void OnTriggerExit(Collider e){

		
		for (int i = 0; i < triggerCube.Length; i++) {


			lowPass = triggerCube[i].GetComponent<AudioLowPassFilter> ();
			if (lowPass != null)
				lowPass.enabled = false;

		
		}
	

	}
	

}
