using UnityEngine;
using System.Collections;

public class NAObjectLabel : MonoBehaviour {


	public string Title 		= "Your object name.";
	public string Author 		= "Your name.";
	public string Date 			= "2015";
	public string Synopsis 		= "Your object description.";
	public string Instructions 	= "How to use your object.";

	private Texture2D texWhite	= null;

	private bool bActive 		= true;
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
		timer += Time.deltaTime;
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
		if (!NA.isClient())
		{


		}
	}

	void OnTriggerExit(Collider other) 
	{

	}


	void DrawGUI()
	{
		float h = 200;
		float y = Screen.height-h;
		GUI.color = new Color(0,0,0,0.5f);
		GUI.DrawTexture(new Rect(0,y,Screen.width, h), texWhite);
		GUI.color = Color.white;
		GUI.Label(new Rect(100,y,200,30), Title);
		GUI.Label(new Rect(100,y+30,200,30), Author + ", " + Date);
		GUI.Label(new Rect(100,y+60,400,40), Synopsis);
		GUI.Label(new Rect(100,y+100,400,100), Instructions);
	}
}
