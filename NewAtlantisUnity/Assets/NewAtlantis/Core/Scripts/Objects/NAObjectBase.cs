using UnityEngine;
using System.Collections;

//Base class for New Atlantis interactive objects 
public class NAObjectBase : MonoBehaviour {


	private bool 	bShowGUI	= true;
	private bool	ExtendedGUI = true;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetGUI(bool b)
	{
		bShowGUI = b;
	}

	public void SetExtendedGUI(bool b)
	{
		ExtendedGUI = b;
	}

	protected int GUIParamEdit(Rect rect, string caption, string unit, int value)
	{
		GUI.Label (new Rect(rect.x, rect.y, rect.width/3, rect.height), caption);
		string strVal = ""+value;
		strVal = GUI.TextField(new Rect(rect.x+rect.width/3, rect.y, rect.width/3, rect.height), strVal); 
		GUI.Label (new Rect(rect.x+2*rect.width/3, rect.y, rect.width/3, rect.height), unit);
		int.TryParse(strVal, out value);
		return value;
	}

	protected float GUIParamEdit(Rect rect, string caption, string unit, float value)
	{
		GUI.Label (new Rect(rect.x, rect.y, rect.width/3, rect.height), caption);
		string strVal = ""+value;
		strVal = GUI.TextField(new Rect(rect.x+rect.width/3, rect.y, rect.width/3, rect.height), strVal); 
		GUI.Label (new Rect(rect.x+2*rect.width/3, rect.y, rect.width/3, rect.height), unit);
		float.TryParse(strVal, out value);
		return value;
	}

	void OnGUI()
	{
		if (bShowGUI)
		{
			Vector3 pos2d = Camera.main.WorldToViewportPoint(transform.position);
			if (pos2d.z > 0)
			{
				if (ExtendedGUI)
				{
					DrawExtendedGUI(pos2d);
					DrawAudioSourceControl(pos2d);

				}
				else
				{
					DrawSimpleGUI(pos2d);
				}
			}
		}
	}

	public virtual void DrawSimpleGUI(Vector3 pos2d)
	{
		
	}
	public virtual void DrawExtendedGUI(Vector3 pos2d)
	{

	}

	virtual public void ExtendedControl()
	{
		//Debug.Log("EXT");
		AudioSource audio = GetComponent<AudioSource>();
		bool buttonAction 	= NAInput.GetControlDown(NAControl.Action); 
		bool buttonJump 	= NAInput.GetControlDown(NAControl.Jump); 
		bool buttonCamera 	= NAInput.GetControlDown(NAControl.Camera);
		bool buttonMenu 	= NAInput.GetControlDown(NAControl.Menu);

		if (buttonAction)
		{
			Debug.Log("stop");
			audio.Stop();
		}
		if (buttonCamera)
		{
			audio.Play();
		}

	}

	public void DrawAudioSourceControl(Vector3 pos2d)
	{
		int x = (int)(pos2d.x*Screen.width);
		int y = (int)(Screen.height-pos2d.y*Screen.height);
		 
		AudioSource audio = GetComponent<AudioSource>();
		GUI.Box(new Rect(x,y-20,200,20), "");
		GUI.color = audio.isPlaying ? Color.green : Color.white;
		if (GUI.Button (new Rect(x,y-20,60,20), "play (∆)"))
		{
			audio.Play();
		}
		GUI.color = Color.white;
		if (GUI.Button (new Rect(x+60,y-20,60,20), "stop (□)"))
		{
			audio.Stop();
		}

		audio.volume = GUI.HorizontalSlider(new Rect(x+120,y-20,80,20), audio.volume, 0f, 1f);
		/*
		if (GUI.Button (new Rect(x+120,y-30,60,30), "loop"))
		{
			audio.loop = !audio.loop;
		}
		*/


	}

	 



}
