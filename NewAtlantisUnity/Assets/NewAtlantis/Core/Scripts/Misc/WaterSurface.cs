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
		if (cam.y < pos.y) 
		{
			//underwater
			transform.eulerAngles = new Vector3 (180, 0, 0);
			//RenderSettings.ambientLight = new Color(0,0,0);
			RenderSettings.fog = true;
		} 
		else 
		{
			transform.eulerAngles = new Vector3 (0, 0, 0);
			//RenderSettings.ambientLight = new Color(0.2f,0.2f,0.2f);
			RenderSettings.fog = false;
		}
	
	}
}
