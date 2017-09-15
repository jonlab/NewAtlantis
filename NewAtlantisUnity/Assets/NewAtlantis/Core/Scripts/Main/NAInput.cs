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

// to enable both keyboard input, configured through the Input Manager, AND this remappable thing ... 
// maybe we could only do Input.GetKeyDown, and manage the mapping ourselves, 
// but in that case it won't nicely translate the keys to axes for movement.  
// so I think we need to create key mappings in the Input Manager, by name, and also check those 

public class NAInput 
{
	static string [] MAPPING_PS4_MAC = new string[13] {"button0","button1","button2","button3",
		"button5","button4","axis2","axis1","axis4","axis3","axis11","axis12","FullMenu"};

	static string [] MAPPING_PS4_WIN = new string[13] {"button0","button1","button2","button3",
		"button5","button4","axis3","axis1","axis7","axis4","axis8","axis9","FullMenu"};

	static string [] MAPPING_BASE = new string[13] {"Action","Jump","Menu","Camera","NextTool","PreviousTool",
		"Vertical","Horizontal","ViewVertical","ViewHorizontal","PadHorizontal","PadVertical","FullMenu"};


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

		bool value1 = Input.GetButtonDown(button);
		bool value2 = Input.GetButtonDown(MAPPING_BASE[(int)control]);

		bool result = value1 || value2; 
		return result; 

	}

	static public bool GetControlUp(NAControl control)
	{
		string button = GetControlName(control);
		bool value1 = Input.GetButtonUp(button);
		bool value2 = Input.GetButtonUp(MAPPING_BASE[(int)control]);

		bool result = value1 || value2; 

		return result;
	}

	static public bool GetControl(NAControl control)
	{
		string button = GetControlName(control);
		bool value1 = Input.GetButton(button);
		bool value2 = Input.GetButton(MAPPING_BASE[(int)control]);

		bool result = value1 || value2;

		return result;
	}

	static public float GetAxis(NAControl control)
	{
		string axis = GetControlName(control);
		float v1=Input.GetAxis(axis);

		if (control == NAControl.MoveVertical)
			v1 *= -1;

		float v2=Input.GetAxis(MAPPING_BASE[(int)control]);

		float result = (v1+v2);
		return result;
	}


	static private string GetControlName(NAControl control)
	{
		return currentMapping [(int)control];
	}
}
