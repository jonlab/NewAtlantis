using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Enable or disable the main light in a space, for when you want to create your own lighting 
*/

public class NAMainLightControl : MonoBehaviour {

	public bool enableMainLight = true;

	bool mainlightPreviousState;

	GameObject mainLightViewer_go=null;
	Light mainLight=null;
	// Use this for initialization
	void Start () {
		mainLightViewer_go = GameObject.Find("MainLightViewer");

		if (mainLightViewer_go != null)
		{
			mainLight = mainLightViewer_go.GetComponent<Light>();
			if (mainLight)
			{
				mainlightPreviousState = mainLight.enabled;
				mainLight.enabled = enableMainLight;
			}
		}
	}
	
	void OnDestroy () 
	{
		if (mainLight !=null)
		{
			mainLight.enabled = mainlightPreviousState;

		}

	}
}
