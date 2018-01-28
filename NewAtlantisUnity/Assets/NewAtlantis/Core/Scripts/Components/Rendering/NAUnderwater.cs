using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAUnderwater : MonoBehaviour {


	float timer = 0;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		return;
		timer += Time.deltaTime;
		RenderSettings.fog = true;

		RenderSettings.fogMode = FogMode.ExponentialSquared;
		RenderSettings.fogDensity = 0.05f;
		Camera.main.clearFlags = CameraClearFlags.Color;

		float step = timer / 300f;
		Color c = Color.Lerp(Color.black, new Color(0.2f, 0.2f, 1f), step);
			
		RenderSettings.fogColor = c;
		Camera.main.backgroundColor = c;
	}
}
