using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simulAscenseur : MonoBehaviour {

	float z;
	float t;

	float zMAX, zMIN;

	public float T = 60f * 10f;

	// Use this for initialization
	void Start () {

		zMAX = RissetManager.depthMAX;
		zMIN = RissetManager.depthMIN;

		z = zMAX;
		t = 0f;


	}

	// Update is called once per frame
	void Update () {

		if (RissetManager.hasRestarted == true) {
			t = 0f;
			z = zMAX;

			RissetManager.depth = z;

			GameObject.Find ("ShepardRisset_Eau1").GetComponent<lorenzOrbit_Eau1> ().resetOrbit ();
			GameObject.Find ("ShepardRisset_Eau2").GetComponent<lorenzOrbit_Eau2> ().resetOrbit ();
			GameObject.Find ("ShepardRisset_Eau3").GetComponent<lorenzOrbit_Eau3> ().resetOrbit ();
			GameObject.Find ("ShepardRisset_Terre1").GetComponent<lorenzOrbit_Terre1> ().resetOrbit ();
			GameObject.Find ("ShepardRisset_Terre2").GetComponent<lorenzOrbit_Terre2> ().resetOrbit ();

			RissetManager.hasRestarted = false;

		} else {
			t += (float)Time.deltaTime; 
			z = zMAX + (zMIN - zMAX) * t / T;

			RissetManager.depth = z;

			if (z >= zMIN)
				RissetManager.hasRestarted = true;
		}
			


		//Debug.Log ("depth = " + z);

		//transform.position = new Vector3 (transform.position.x, transform.position.y, z);
	}
}
