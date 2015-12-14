using UnityEngine;
using System.Collections;


public class NAToolLight : NAToolBase 
{
	// Use this for initialization
	void Start () 
	{
		Light light = GetComponent<Light>();
		if (light == null)
		{
			light 			= gameObject.AddComponent<Light>();
			light.type 		= LightType.Point;
			light.intensity = 2;
			light.range 	= 50;
			light.color 	= Color.white;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public override void Action() 
	{
		//local only action
		Light light = GetComponent<Light>();
		light.enabled = !light.enabled;
	}



}
