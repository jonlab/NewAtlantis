using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ascenseurCamera : MonoBehaviour {

	float z;
	float t;
	public float T = 60f * 10f;

	// Use this for initialization
	void Start () {

		z = -900f;
		t = 0f;
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log ("target = " + Application.targetFrameRate + " / mesured = " + Time.deltaTime);

		//t += (float)Time.deltaTime; 
		//z = -900f + 900f * t / T + 20f;

		z = RissetManager.depth + 20f;

		transform.position = new Vector3 (0f, z, 0f);
	}
}
