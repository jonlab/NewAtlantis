using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapZtoSynthParams_Terre1 : MonoBehaviour {

	public float dist;
	float z;
	float t;
	float noiseAmp;
	float noiseValue;
	float phi;
	public float distLim;
	bool canBeChanged;

	float seuil1;
	float seuil2;
	float seuil3;
	float seuil4;

	int NmaxOctaves;

	//public float T = 60f * 10f;

	// Use this for initialization
	void Start () {

		//t = 0f;
		phi = 0f * 2f*Mathf.PI/6f;
		distLim = 10f;
		canBeChanged = true;

		seuil1 = RissetManager.depthMAX + 1 * (RissetManager.depthMIN - RissetManager.depthMAX);
		seuil2 = RissetManager.depthMAX + 2 * (RissetManager.depthMIN - RissetManager.depthMAX);
		seuil3 = RissetManager.depthMAX + 3 * (RissetManager.depthMIN - RissetManager.depthMAX);
		seuil4 = RissetManager.depthMAX + 4 * (RissetManager.depthMIN - RissetManager.depthMAX);

		NmaxOctaves = 2;

	}

	// Update is called once per frame
	void Update () {

		//z = transform.position.z;
		z = RissetManager.depth;
		t = RissetManager.dureeMontee * (RissetManager.depthMAX - z) / (RissetManager.depthMIN - RissetManager.depthMAX);

		// Update Bandwidth if distqnceToListener >= distLim

		//float dist = Vector3.Distance(this.transform.position, GameObject.Find("Listener").transform.position);
		//float dist = RissetManager.dist2Listener;

		//Debug.Log ("dist1 = " + dist + " - distLim = " + distLim);

		if (dist >= distLim && canBeChanged) {

			int NmaxOctaves = 2;

			if (z < seuil1)
				NmaxOctaves = 2;
			else if (z >= seuil1 && z < seuil2)
				NmaxOctaves = 2;
			else if (z >= seuil2 && z < seuil3)
				NmaxOctaves = 3;
			else if (z >= seuil3 && z < seuil4)
				NmaxOctaves = 4;
			else if (z >= seuil4)
				NmaxOctaves = 5;

			int newOctavesNumber = (int) Mathf.Floor(Random.Range (2f, (float)NmaxOctaves + 1f));

			this.transform.Find("Sphere_Terre1").GetComponent<ShepardRissetBarberpole_Terre1> ().bandwidth = newOctavesNumber;

			//Debug.Log ("z = " + z + " - Nmax = " + NmaxOctaves + " - newOctavesNumber = " + newOctavesNumber);

			canBeChanged = false;

		} else if (dist < distLim && !canBeChanged) {
			canBeChanged = true;
		}


		// Update Lower Bound

		float pow = 5f * (RissetManager.depth - RissetManager.depthMAX) / (RissetManager.depthMIN - RissetManager.depthMAX);

		this.transform.Find("Sphere_Terre1").GetComponent<ShepardRissetBarberpole_Terre1> ().lowerBound = 13.75f * Mathf.Pow (2f, pow);

		// Update Tonal/Noisy

		noiseAmp = 1f - (RissetManager.depthMIN - z) / (RissetManager.depthMIN - RissetManager.depthMAX);

		// Oscillations BF
		noiseValue = 0.5f - 0.5f*Mathf.Cos(2f*Mathf.PI*0.1f*t + phi);

		float _tonalVsNoisy = 1f - noiseAmp * noiseValue;

		this.transform.Find("Sphere_Terre1").GetComponent<ShepardRissetBarberpole_Terre1> ().tonalVsNoisy = _tonalVsNoisy;

	}
}
