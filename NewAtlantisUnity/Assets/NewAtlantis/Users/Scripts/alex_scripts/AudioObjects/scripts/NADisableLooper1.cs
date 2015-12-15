using UnityEngine;
using System.Collections;

public class NADisableLooper1 : MonoBehaviour {
	public int interval = 320;
	public int secondInterval = 160;
	int saveInterval;
	int step = 0;

	// Use this for initialization
	void Start () {
		saveInterval = interval;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Time.frameCount % interval == 0){

			NALooper[] looper = transform.GetComponents<NALooper>();
			NALooperSequence[] looperSequence = transform.GetComponents<NALooperSequence>();


			for(int i = 0 ; i < looper.Length ; i++){
			
			if(step%2==0){
					looper[i].enabled = false;

				}else{
					looper[i].enabled = true;

			}


			}

			for(int i = 0 ; i < looperSequence.Length ; i++){
				
				if(step%2==0){
					looperSequence[i].enabled = false;
					
				}else{
					looperSequence[i].enabled = true;
					
				}
				
				
			}



			if(step%2==0){interval = secondInterval;}else{interval = saveInterval; }

			step++;

		}

	}
}
