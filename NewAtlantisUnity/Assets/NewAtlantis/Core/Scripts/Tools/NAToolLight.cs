using UnityEngine;
using System.Collections;


public class NAToolLight : NAToolBase 
{
	// Use this for initialization
	void Start () 
	{
		Light light = GetComponent<Light>();
		if (light == null)
		{
			//if no light, we add one standard
			light 			= gameObject.AddComponent<Light>();
			light.type 		= LightType.Point;
			light.intensity = 2;
			light.range 	= 50;
			light.color 	= Color.white;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public override void Action() 
	{
		//local only action
		Light light = GetComponent<Light>();
		light.enabled = !light.enabled;
	}


	//manages the Extended control
	public override void ExtendedControl()
	{
		float dt = Time.deltaTime;

		float x1 = NAInput.GetAxis(NAControl.MoveHorizontal);
		float y1 = NAInput.GetAxis(NAControl.MoveVertical);
		float x2 = NAInput.GetAxis(NAControl.ViewHorizontal);
		float y2 = NAInput.GetAxis(NAControl.ViewVertical);

		bool buttonJump 	= NAInput.GetControlDown(NAControl.Jump); 
		bool buttonCamera 	= NAInput.GetControlDown(NAControl.Camera);
		bool buttonMenu 	= NAInput.GetControlDown(NAControl.Menu);


		Light light 		= GetComponent<Light>();
		light.spotAngle 	+= x1*dt*10f;
		light.range 		+= y1*dt*1f;
		light.intensity 	-= y2*dt*1f;

		if (buttonCamera)
			light.color = new Color(Random.value, Random.value, Random.value);
		if (buttonJump)
			light.color = Color.white;
	}

	public override void DrawExtendedGUI(Vector3 pos2d)
	{
		//GUI.BeginGroup(
		Light light 		= GetComponent<Light>();
		string str = "";
		str += "angle=" + light.spotAngle;
		str += " range=" + light.range;
		str += " intensity=" + light.intensity;
		str += " color=" + light.color;

		GUI.Label(new Rect(pos2d.x-200, pos2d.y-15, 400, 30), str);

	}


}
