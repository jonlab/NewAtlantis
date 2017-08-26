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
	FullMenu
}

public class NAInput 
{
	static string [] MAPPING_PS4_MAC = new string[13] {"button0","button1","button2","button3",
		"button4","button5","axis2","axis1","axis4","axis3","axis7","axis8","FullMenu"};

	static string [] MAPPING_PS4_WIN = new string[13] {"button0","button1","button2","button3",
		"button4","button5","axis3","axis1","axis7","axis4","axis8","axis9","FullMenu"};
	
	public static float PreviousPadX = 0;
	public static float PreviousPadY = 0;

	public static bool PadHorizontalPressed = false;
	public static bool PadHorizontalReleased = false;

	public static bool PadVerticalPressed = false;
	public static bool PadVerticalReleased = false;

	static string[] currentMapping;

	static public void InitializeControlMap()
	{
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer) {
			Debug.Log("Setting mapping to Mac");
			currentMapping = MAPPING_PS4_MAC;	
		} 
		else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) {
			Debug.Log("Setting mapping to Windows");
			currentMapping = MAPPING_PS4_WIN;
		}
	}

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
		float v=Input.GetAxis(axis);

		if (control == NAControl.MoveVertical)
			v *= -1;
		return v;
	}


	static private string GetControlName(NAControl control)
	{
		return currentMapping [(int)control];
	}
}
