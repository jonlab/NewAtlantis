using UnityEngine;
using System.Collections;


public class NAToolLight : NAToolBase 
{
	public GameObject target = null;
	public string LightName = ""; //if empty, avatar lamp

	private bool dirty = false;
	// Use this for initialization
	void Start () 
	{
		if (target == null)
			target = gameObject;
		Light light = target.GetComponent<Light>();
		if (light == null)
		{
			//if no light, we add one standard
			light 			= target.AddComponent<Light>();
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
		Light l = target.GetComponent<Light>();
		//local only action

		if (dirty)
		{
			dirty = false;
		}
		else
		{
			l.enabled = !l.enabled;
		}
		//sync
		string n = LightName;
		if (n == "")
			n = NAServer.strLogin;
		GetComponent<NetworkView>().RPC("SetLightState", RPCMode.AllBuffered, n, l.enabled, l.intensity, l.spotAngle, target.transform.eulerAngles);

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


		Light light 		= target.GetComponent<Light>();

		//light.range 		+= y1*dt*1f;
		light.spotAngle 	+= x2*dt*45f;
		light.intensity 	-= y2*dt*1f;

		light.intensity = Mathf.Clamp(light.intensity, 0, 2f);
        light.spotAngle = Mathf.Clamp(light.spotAngle, 0, 179f);
		if (LightName == "MainLightViewer")
		{
			//target.transform.Rotate(y1*dt*45f,x1*dt*45f,0);
			target.transform.RotateAround(transform.position, transform.up, x1*dt*45f);
			target.transform.RotateAround(transform.position, transform.right, y1*-1f*dt*45f);
		}

		if (buttonCamera)
			light.color = new Color(Random.value, Random.value, Random.value);
		if (buttonJump)
			light.color = Color.white;

		dirty = true;
	}

	public override void DrawExtendedGUI(Vector3 pos2d)
	{
		//GUI.BeginGroup(
		Light light 		= target.GetComponent<Light>();
		string str = "";
		str += "angle=" + light.spotAngle;
		str += " range=" + light.range;
		str += " intensity=" + light.intensity;
		str += " color=" + light.color;

		GUI.Label(new Rect(pos2d.x-200, pos2d.y-15, 400, 30), str);

	}


}
