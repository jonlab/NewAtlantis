using UnityEngine;
using System.Collections;

public class WaterSurface : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//gestion du plan d'eau
		Vector3 cam = Camera.main.transform.position;
		Vector3 pos = transform.position;
		float depth = pos.y - cam.y;
		if (cam.y < pos.y) 
		{
			//underwater
			transform.eulerAngles = new Vector3 (180, 0, 0);
			//RenderSettings.ambientLight = new Color(0,0,0);
			RenderSettings.fog = true;
			RenderSettings.fogMode = FogMode.ExponentialSquared;
			RenderSettings.fogDensity = 0.04f;
			Camera.main.clearFlags = CameraClearFlags.Color;

			float step = depth / 900f;
			if (step > 1)
				step = 1;
			Color c = Color.Lerp(new Color(0.4f, 0.4f, 1f), Color.black, step);

			RenderSettings.fogColor = c;
			Camera.main.backgroundColor = c;

		} 
		else 
		{
			transform.eulerAngles = new Vector3 (0, 0, 0);
			//RenderSettings.ambientLight = new Color(0.2f,0.2f,0.2f);
			RenderSettings.fog = false;
			Camera.main.clearFlags = CameraClearFlags.Skybox;
		}
	
	}
}
