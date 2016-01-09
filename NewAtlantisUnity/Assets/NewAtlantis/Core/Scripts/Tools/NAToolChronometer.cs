using UnityEngine;
using System.Collections;

public class NAToolChronometer : NAToolBase {

	float startTime = 0f;
	// Use this for initialization
	void Start () 
	{
		startTime = Time.time;

	}

	// Update is called once per frame
	void Update () 
	{

	}

	void OnGUI()
	{


		Font bak = GUI.skin.font;




		this.DrawBaseGUI();
		//current time display
		float time = Time.time-startTime;

		int minutes = (int)(time/60f);
		int seconds = (int)(time-minutes*60f);
		GUI.skin.font = NA.GetFont(2);
		string strTime = "";
		if (minutes<10)
			strTime+="0";
		strTime+=minutes;
		strTime+=":";
		if (seconds<10)
			strTime+="0";
		strTime+=seconds;

		GUI.Label(new Rect(Screen.width/2-100,Screen.height-30,200,30), strTime);
		GUI.skin.font = bak;
	}
}
