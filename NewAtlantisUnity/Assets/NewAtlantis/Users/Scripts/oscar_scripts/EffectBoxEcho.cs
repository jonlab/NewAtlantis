using UnityEngine;
using System.Collections;

public class EffectBoxEcho: MonoBehaviour {

//	public GameObject camera;
	AudioEchoFilter lowPass;

	public GameObject[] triggerCube;

	public float decay = 0.8f;


	// Use this for initialization
	void Start () {
	
		//lowPass = camera.GetComponent<AudioLowPassFilter> ();

	}
	
	// Update is called once per frame
	void Update () {
	
		if (decay > 1)
			decay = 1;
	}

	void OnTriggerEnter(Collider e){

		if (e.tag != "Player")
			return;




		for (int i = 0; i < triggerCube.Length; i++) {

			lowPass = triggerCube[i].GetComponent<AudioEchoFilter> ();
			if (lowPass == null){
				triggerCube[i].AddComponent<AudioEchoFilter> ();
			}else{
				
				
				triggerCube [i].GetComponent<AudioEchoFilter> ().enabled = true;
			}


			lowPass = triggerCube[i].GetComponent<AudioEchoFilter> ();
			lowPass.decayRatio = decay;

		}

	}

	void OnTriggerExit(Collider e){

		
		for (int i = 0; i < triggerCube.Length; i++) {

			lowPass = triggerCube[i].GetComponent<AudioEchoFilter> ();
			if (lowPass != null)
				lowPass.enabled = false;

		
		}

	}
	

}
