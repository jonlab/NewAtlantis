using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//main logic for the Ursula Beton Salon installation version
public class NAUrsulaLogic : MonoBehaviour 
{
	WebcamProcess webcam;
	GameObject ShepardRisset_Eau1;
	GameObject ShepardRisset_Eau2;
	GameObject ShepardRisset_Eau3;
	GameObject ShepardRisset_Terre1;
	GameObject ShepardRisset_Terre2;

	GameObject autospawn_hydro;
	GameObject autospawn_voices;

	GameObject particles;

	GameObject MainLight;

	//private DP.DMX dmx;



	// Use this for initialization
	void Start () 
	{
		//input webcam process
		gameObject.AddComponent<WebcamProcess> ();

		ShepardRisset_Eau1 = GameObject.Find("ShepardRisset_Eau1");
		ShepardRisset_Eau2 = GameObject.Find("ShepardRisset_Eau2");
		ShepardRisset_Eau3 = GameObject.Find("ShepardRisset_Eau3");
		ShepardRisset_Terre1 = GameObject.Find("ShepardRisset_Terre1");
		ShepardRisset_Terre2 = GameObject.Find("ShepardRisset_Terre2");

		autospawn_hydro = GameObject.Find("autospawnerHydrophone");
		autospawn_voices = GameObject.Find("autospawnerVoices");

		MainLight = GameObject.Find("MainLightViewer");
		//dmx = new DP.DMX ();

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (particles == null)
		{
			particles = GameObject.Find("OceanParticles");
			if (particles != null)
			{
				particles.SetActive (false);
			}
		}
		//send the webcam control
		Vector3 pos = NA.app.transform.position;
		RissetManager.depth = pos.y;
		//AudioListener listener = NA.app.GetComponent<AudioListener> ();
		RissetManager.dist2Listener = 2;
		if (ShepardRisset_Eau1)
			ShepardRisset_Eau1.GetComponent<mapZtoSynthParams_Eau1>().dist = (ShepardRisset_Eau1.transform.position-pos).magnitude;
		if (ShepardRisset_Eau2)
			ShepardRisset_Eau2.GetComponent<mapZtoSynthParams_Eau2>().dist = (ShepardRisset_Eau2.transform.position-pos).magnitude;
		if (ShepardRisset_Eau3)
			ShepardRisset_Eau3.GetComponent<mapZtoSynthParams_Eau3>().dist = (ShepardRisset_Eau3.transform.position-pos).magnitude;
		if (ShepardRisset_Terre1)
			ShepardRisset_Terre1.GetComponent<mapZtoSynthParams_Terre1>().dist = (ShepardRisset_Terre1.transform.position-pos).magnitude;
		if (ShepardRisset_Terre2)
			ShepardRisset_Terre2.GetComponent<mapZtoSynthParams_Terre2>().dist = (ShepardRisset_Terre2.transform.position-pos).magnitude;


		if (pos.y > 900)
		{
			MainLight.GetComponent<Light> ().enabled = true;
			
		} else
		{
			MainLight.GetComponent<Light> ().enabled = false;
			
		}
		//250 700 voice
		//500 1000 hydro

		if (pos.y > 250 && pos.y < 500)
		{
			autospawn_voices.GetComponent<NAAiAutoSpawner> ().Enabled = true;
		} 
		else
		{
			autospawn_voices.GetComponent<NAAiAutoSpawner>().Enabled = false;
		}

		if (pos.y > 500 && pos.y < 1000)
		{
			autospawn_hydro.GetComponent<NAAiAutoSpawner> ().Enabled = true;

		} 
		else
		{
			autospawn_hydro.GetComponent<NAAiAutoSpawner>().Enabled = false;
		}

		if (Input.GetKeyDown (KeyCode.W))
		{
			webcam.bGUI = !webcam.bGUI;
		}


		//DMX control of the ambient space lighting
		float red = Camera.main.backgroundColor.r;
		float green = Camera.main.backgroundColor.g;
		float blue = Camera.main.backgroundColor.b;

		DP.DMX dmx = GetComponent<DP.DMX>();
		int nred = (int)((red * red) * 255f);
		int ngreen = (int)((green * green) * 255f);
		int nblue = (int)((blue * blue) * 255f);


		dmx [0] = nred;
		dmx [1] = ngreen;
		dmx [2] = nblue;
		dmx [3] = nred;
		dmx [4] = ngreen;
		dmx [5] = nblue;
		dmx [6] = nred;
		dmx [7] = ngreen;
		dmx [8] = nblue;
		/*
		// Use this for initialization
		void Start () 
		{
			dmxObject = GameObject.Find("DMXObject");
			dmx = dmxObject.GetComponent<DP.DMX>();
		}

		// Update is called once per frame
		void Update () 
		{
			//Set Value
			dmx[1] = (int)Mathf.PingPong(Time.time*100, 255);

			//Get Value
			dmx[3] = dmx[1];

			for (int i = 0; i < 512; ++i)
			{
				dmx [i] = 255;
			}
		}
		*/

	}
}
