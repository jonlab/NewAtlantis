using UnityEngine;
using System.Collections;

public class NAToolBase : MonoBehaviour
{
	public string name = "noname";
	public Texture2D icon = null;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	virtual public void Action() 
	{
		//implement action in subclasses
		Debug.Log ("Action !");
	}

	virtual public string GetName() 
	{
		return name;
	}
	
	void OnGUI()
	{
		DrawBaseGUI();
	}

	protected void DrawBaseGUI()
	{

		//tool GUI
		GUI.DrawTexture(new Rect(Screen.width/2-32,Screen.height-64,64,64), icon);
		GUI.color = Color.red;
		GUI.Label(new Rect(Screen.width/2-32,Screen.height-64,64,64), name);
		GUI.color = Color.white;
	}

}
