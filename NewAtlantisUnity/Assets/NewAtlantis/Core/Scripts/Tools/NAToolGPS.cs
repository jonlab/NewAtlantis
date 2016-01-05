using UnityEngine;
using System.Collections;

public class NAToolGPS : NAToolBase {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnGUI()
	{
		this.DrawBaseGUI();
		//current position display
		if (NA.CurrentSpace != null)
		{
			GUI.Label(new Rect(Screen.width/2-100,Screen.height-60,200,30), ""+NA.CurrentSpace.name);
		}
		GUI.Label(new Rect(Screen.width/2-100,Screen.height-30,200,30), ""+transform.position);
	}
}
