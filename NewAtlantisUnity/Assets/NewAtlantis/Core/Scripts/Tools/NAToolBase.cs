using UnityEngine;
using System.Collections;



public enum NAToolCategory
{
	Audio,
	Object,
	Hit,
	Misc,
	Physics


}


public class NAToolBase : MonoBehaviour
{
	public NAToolCategory 	category;
	public string 			name = "noname";
	public Texture2D 		icon = null;

	protected Texture2D 	white = null;
	private bool			ExtendedGUI = true;

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

	public void SetExtendedGUI(bool b)
	{
		ExtendedGUI = b;
	}

	virtual public void ExtendedControl()
	{
		
	}

	virtual public string GetName() 
	{
		return name;
	}

	public virtual void DrawExtendedGUI(Vector3 pos2d)
	{

	}
	
	void OnGUI()
	{
		//DrawBaseGUI(new Vector3(Screen.width/2, Screen.height-32, 0), false);
		DrawBaseGUI();
		if (ExtendedGUI)
		{
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			DrawExtendedGUI(new Vector3(Screen.width/2, Screen.height-80, 0));
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
		}
	}

	public void DrawBaseGUI()
	{
		DrawBaseGUI(new Vector3(Screen.width/2, Screen.height-32, 0), false);
	}

	public bool DrawBaseGUI(Vector3 pos, bool selected)
	{
		if (!white)
			white = Resources.Load("white") as Texture2D;
		//tool GUI
		/*if (selected)
			GUI.color = Color.red;
		else
			GUI.color = Color.white;
		*/

		if (selected)
			GUI.color = Color.white;
		else
			GUI.color = new Color(1,1,1,0.1f);
		

		GUI.DrawTexture(new Rect(pos.x-32,pos.y-32,64,64), white);
		GUI.color = Color.white;

		//GUI.DrawTexture(new Rect(pos.x-32,pos.y-32,64,64), icon);

		bool bClicked = GUI.Button(new Rect(pos.x-32,pos.y-32,64,64), icon, new GUIStyle());
		GUI.color = Color.red; //red
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(pos.x-32,pos.y-64,64,64), name);
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GUI.color = Color.white;

		return bClicked;
	}

}
