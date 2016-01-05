using UnityEngine;
using System.Collections;

public class NAObjectLabel : MonoBehaviour 
{
	public string Title 		= "Your object name.";
	public string Author 		= "Your name.";
	public string Date 			= "2015";
	public string Synopsis 		= "Your object description.";
	public string Instructions 	= "How to use your object.";

	private Texture2D texWhite	= null;

	private bool bActive 		= false;
	private float Duration 		= 10f;
	private float timer 		= 0f;
	
	// Use this for initialization
	void Start () 
	{
		texWhite = Resources.Load ("white") as Texture2D;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (bActive)
		{
			timer += Time.deltaTime;
			if (timer > Duration)
			{
				bActive = false;
				timer = 0f;
			}
		}

	}

	void OnGUI ()
	{
		if (bActive)
		{
			DrawGUI();
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		if (NA.isViewer(other.gameObject)) //is the colliding object a viewer object ?
		{
			bActive = true;
			timer = 0f;
		}
	}

	void OnTriggerExit(Collider other) 
	{
		if (NA.isViewer(other.gameObject))
		{
			bActive = false;
		}
	}


	void DrawGUI()
	{
		float h = 200;
		float y = Screen.height-h;
		GUI.color = new Color(0,0,0,0.5f);
		GUI.DrawTexture(new Rect(0,y,Screen.width, h), texWhite);
		GUI.color = Color.white;
		GUI.Label(new Rect(100,y,500,30), Title);
		GUI.Label(new Rect(100,y+30,500,30), Author + ", " + Date);
		GUI.Label(new Rect(100,y+60,500,40), Synopsis);
		GUI.Label(new Rect(100,y+100,500,100), Instructions);
	}
}
