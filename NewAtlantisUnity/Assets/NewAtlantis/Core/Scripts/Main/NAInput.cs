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
	ViewHorizontal,
	PadHorizontal,
	PadVertical,
	PadUp,
	PadDown,
	PadLeft,
	PadRight
}

public class NAInput 
{
	public static float PreviousPadX = 0;
	public static float PreviousPadY = 0;

	public static bool PadHorizontalPressed = false;
	public static bool PadHorizontalReleased = false;

	public static bool PadVerticalPressed = false;
	public static bool PadVerticalReleased = false;

	static public void Process()
	{
		float CurrentPadX = NAInput.GetAxis(NAControl.PadHorizontal);

		if (Mathf.Abs(CurrentPadX)>Mathf.Abs(PreviousPadX))
		{
			PadHorizontalPressed = true;
		}
		else
		{
			PadHorizontalPressed = false;
		}
		if (Mathf.Abs(CurrentPadX)<Mathf.Abs(PreviousPadX))
		{
			PadHorizontalReleased = true;
		}
		else
		{
			PadHorizontalReleased = false;
		}
		PreviousPadX = CurrentPadX;

		float CurrentPadY = NAInput.GetAxis(NAControl.PadVertical);
		if (Mathf.Abs(CurrentPadY)>Mathf.Abs(PreviousPadY))
		{
			PadVerticalPressed = true;
		}
		else
		{
			PadVerticalPressed = false;
		}
		if (Mathf.Abs(CurrentPadY)<Mathf.Abs(PreviousPadY))
		{
			PadVerticalReleased = true;
		}
		else
		{
			PadVerticalReleased = false;
		}
		PreviousPadY = CurrentPadY;
	}
		
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
		case NAControl.PadVertical:
			return "PadY";
			break;
		case NAControl.PadHorizontal:
			return "PadX";
			break;
		}
		return "";
	}
}
