using UnityEngine;
using System.Collections;

public class NAAudioSource : MonoBehaviour 
{
	public static bool bDisplayAudioSourceName = false;
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
		if (bDisplayAudioSourceName)
		{
			Vector3 pos2d = Camera.main.WorldToViewportPoint(transform.position);
			if (pos2d.z > 0)
			{
				GUI.color = Color.white;
				string strDisplay = name;
				int x = (int)(pos2d.x*Screen.width);
				int y = (int)(Screen.height-pos2d.y*Screen.height);
				GUI.Box (new Rect(x,y,100,30), strDisplay);
			}
		}
	}


}
