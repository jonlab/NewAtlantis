using UnityEngine;
using System.Collections;

public class NAToolBase : MonoBehaviour
{
	public string name = "noname";
	public Texture2D icon = null;
	private Texture2D white = null;
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

	virtual public void Press()
	{

	}

	virtual public void Maintain()
	{

	}

	virtual public void Release()
	{

	}

	virtual public void ExtendedControl()
	{
		
	}

	virtual public string GetName() 
	{
		return name;
	}
	
	void OnGUI()
	{
		//DrawBaseGUI(new Vector3(Screen.width/2, Screen.height-32, 0), false);
		DrawBaseGUI();
	}

	public void DrawBaseGUI()
	{
		DrawBaseGUI(new Vector3(Screen.width/2, Screen.height-32, 0), false);
	}

	public void DrawBaseGUI(Vector3 pos, bool selected)
	{
		if (!white)
			white = Resources.Load("white") as Texture2D;
		//tool GUI
		if (selected)
			GUI.color = Color.red;
		else
			GUI.color = Color.white;


		GUI.DrawTexture(new Rect(pos.x-32,pos.y-32,64,64), white);
		GUI.DrawTexture(new Rect(pos.x-32,pos.y-32,64,64), icon);
		GUI.color = Color.red;
		GUI.Label(new Rect(pos.x-32,pos.y-32,64,64), name);
		GUI.color = Color.white;
	}

}
