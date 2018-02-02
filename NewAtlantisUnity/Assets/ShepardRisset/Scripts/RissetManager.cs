using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RissetManager 
{

	public static float depth = -160f;
	public static float depthMAX = 0f;
	public static float depthMIN = 1000f;

	public static float dureeMontee = 10f * 60f;

	public static float dist2Listener = 10f;

	public static bool hasRestarted = false;

	public static float t;
	public static float z;

	public static void Start()
	{
		t = 0f;
		z = depthMAX;
	}


	public static void resetOrbit()
	{

			//if (hasRestarted == true) {
				t = 0f;
				z = depthMAX;

				depth = z;

				GameObject.Find ("ShepardRisset_Eau1").GetComponent<lorenzOrbit_Eau1> ().resetOrbit ();
				GameObject.Find ("ShepardRisset_Eau2").GetComponent<lorenzOrbit_Eau2> ().resetOrbit ();
				GameObject.Find ("ShepardRisset_Eau3").GetComponent<lorenzOrbit_Eau3> ().resetOrbit ();
				GameObject.Find ("ShepardRisset_Terre1").GetComponent<lorenzOrbit_Terre1> ().resetOrbit ();
				GameObject.Find ("ShepardRisset_Terre2").GetComponent<lorenzOrbit_Terre2> ().resetOrbit ();

				hasRestarted = false;

//			} else {
//				t += (float)Time.deltaTime; 
//				z = depthMAX + (depthMIN - depthMAX) * t / T;
//
//				depth = z;
//
//				if (z >= depthMIN)
//					hasRestarted = true;
//			}



			//Debug.Log ("depth = " + z);

			//transform.position = new Vector3 (transform.position.x, transform.position.y, z);
	}
}

