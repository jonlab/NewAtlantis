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

	}



}
