using UnityEngine;
using System.Collections;

public class NAToolCloner : NAToolBase {


	public GameObject goPrefabCubeSimple; 
	public float distance = 1f;
	public Vector3 localForce = Vector3.forward;
	public string objectName = "";
	private int current = 0;

	private Texture2D preview = null;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public override void Action() 
	{
		//int r = (int)(Random.value * (NA.instanciables.Count));
		objectName = NA.instanciables[current].name;
		Vector3 worldforce = transform.rotation * localForce;
		Vector3 pos = transform.position+transform.forward*distance;
		if (Network.isServer)
		{
			NA.network.ServerCloneObject(objectName, pos, worldforce, new Vector3(1,0,0));
		}
		else
		{
			//we send to the server
			LogManager.Log("Send RPC ServerCloneObject");
			GetComponent<NetworkView>().RPC("ServerCloneObject", RPCMode.Server, objectName, pos, worldforce, new Vector3(1,0,0)/*colorAvatar*/);
		}
	}

	public override void Press() 
	{
		
	}

	public override void Maintain() 
	{
		
	}

	public override void Release() 
	{
		
	}

	//manages the Extended control
	public override void ExtendedControl()
	{
		float x1 = NAInput.GetAxis(NAControl.MoveHorizontal);

		float padx = NAInput.GetAxis(NAControl.PadHorizontal);
		float pady = NAInput.GetAxis(NAControl.PadVertical);


		if (NAInput.PadHorizontalPressed && padx > 0)
		{
			current++;
			current = current%NA.instanciables.Count;
			Preview();

		}
		else if (NAInput.PadHorizontalPressed && padx < 0)
		{
			current--;
			if (current < 0)
				current = current+NA.instanciables.Count;
			Preview();
		}
	}

	private void Preview()
	{
		string strName = NA.instanciables[current].name;
		GameObject model = null;
		foreach (NAObject o in NA.instanciables)
		{
			if (o.name == strName)
			{
				model = o.go;
			}
		}
		preview = NA.GeneratePreviewPNG(model, 128,128);

	}
	public override void DrawExtendedGUI(Vector3 pos2d)
	{
		if (preview != null)
		{
			GUI.DrawTexture(new Rect(pos2d.x-32, pos2d.y-64, 64, 64), preview);
		}
		GUI.Label(new Rect(pos2d.x-200, pos2d.y-15, 400, 30), NA.instanciables[current].name);

	}

	


}
