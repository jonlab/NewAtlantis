using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lorenzOrbit_Terre1 : MonoBehaviour {


	float a = 20f;
	float b = 8f/3f - 1f;
	float r = 28f;

	float x, x0;
	float y, y0;
	float z, z0;

	float dt;

	float thetaRot;
	float cosRot;
	float sinRot;

	float x_scaled;
	float y_scaled;
	float z_scaled;

	// Use this for initialization
	void Start () {

		x = x0 = 10f;
		y = y0 = 0f;
		z = z0 = -10f;

		thetaRot = Mathf.PI/3f;
		cosRot = Mathf.Cos(thetaRot);
		sinRot = Mathf.Sin(thetaRot);
	}

	// Update is called once per frame
	void Update () {

		if (RissetManager.hasRestarted == true) {
			x = x0;
			y = y0;
			z = z0;

			this.transform.Find ("Sphere_Terre1").GetComponent<ShepardRissetBarberpole_Terre1> ().reloadInputWave ();

			//RissetManager.hasRestarted = false;
		}

		dt = 0.13f/3f * Time.deltaTime; 

		x = x + a * (y-x )*dt;
		y = y + (r*x - y - x*z )*dt;
		z = z + (x*y -b*z )*dt;

		z_scaled = z;
		x_scaled = x * cosRot - y * sinRot;
		y_scaled = x * sinRot + y * cosRot;


		transform.position = new Vector3 (x_scaled, RissetManager.depth, y_scaled);

	}

	public void resetOrbit()
	{
		x = x0;
		y = y0;
		z = z0;

		this.transform.Find ("Sphere_Terre1").GetComponent<ShepardRissetBarberpole_Terre1> ().reloadInputWave ();
	}
}

	



