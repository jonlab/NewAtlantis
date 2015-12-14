using UnityEngine;
using System.Collections;


public enum NAControl
{
	Action,
	Jump,
	Menu,
	Camera,
	NextTool,
	PreviousTool,
	MoveVertical,
	MoveHorizontal,
	ViewVertical,
	ViewHorizontal
}

public class NAInput 
{

	static public bool GetControlDown(NAControl control)
	{
		string button = GetControlName(control);
		return Input.GetButtonDown(button);
	}

	static public bool GetControlUp(NAControl control)
	{
		string button = GetControlName(control);
		return Input.GetButtonUp(button);
	}

	static public bool GetControl(NAControl control)
	{
		string button = GetControlName(control);
		return Input.GetButton(button);
	}

	static public float GetAxis(NAControl control)
	{
		string axis = GetControlName(control);
		return Input.GetAxis(axis);
	}

	static private string GetControlName(NAControl control)
	{
		//the button name has to match the Input manager
		switch (control)
		{
		case NAControl.Action:
			return "Action";
			break;
		case NAControl.Jump:
			return "Jump";
			break;
		case NAControl.Menu:
			return "Menu";
			break;
		case NAControl.Camera:
			return "Camera";
			break;
		case NAControl.NextTool:
			return "Next";
			break;
		case NAControl.PreviousTool:
			return "Previous";
			break;

		case NAControl.MoveVertical:
			return "Vertical";
			break;
		case NAControl.MoveHorizontal:
			return "Horizontal";
			break;
		case NAControl.ViewVertical:
			return "ViewY";
			break;
		case NAControl.ViewHorizontal:
			return "ViewX";
			break;

		}
		return "";
	}
}
