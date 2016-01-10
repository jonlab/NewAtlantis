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
			ServerCloneObject(objectName, pos, worldforce, new Vector3(1,0,0)/*colorAvatar*/);
		}
		else
		{
			//we send to the server
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

	
	[RPC]
	void ServerCloneObject(string name, Vector3 position, Vector3 forward, Vector3 color) 
	{
		if (!Network.isServer)
		{
			return;
		}
		LogManager.Log ("ServerCloneObject");
		GetComponent<NetworkView>().RPC("CloneObject", RPCMode.AllBuffered, name, Network.AllocateViewID(), position, forward, color);
	}
	
	[RPC]
	void CloneObject(string name, NetworkViewID viewID, Vector3 location, Vector3 forward, Vector3 color) 
	{
		GameObject clone = null;
		GameObject model = null;
		foreach (NAObject o in NA.instanciables)
		{
			if (o.name == name)
			{
				model = o.go;
			}
		}
		clone = GameObject.Instantiate(model, Vector3.zero, Quaternion.identity) as GameObject;
		LogManager.LogWarning("clone " + name);

		//remove previous network view
		NetworkView nViewOriginal = clone.GetComponent<NetworkView>();
		if (nViewOriginal)
		{
			NetworkView.Destroy(nViewOriginal);
		}

		NetworkView nView = clone.AddComponent<NetworkView>();
		nView.viewID = viewID;
		
		clone.transform.position = location;
		
		MeshRenderer renderer = clone.GetComponent<MeshRenderer>();
		if (renderer != null)
		{
			renderer.material.color = new Color(color.x, color.y, color.z);
		}
		//clone.transform.forward = transform.rotation * Vector3.Normalize(forward) ;
		//Rigidbody rb = clone.AddComponent<Rigidbody>();
		if (NA.isServer() || NA.isStandalone())
		{
			/*Rigidbody rb = clone.AddComponent<Rigidbody>();
			rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
			rb.AddForce(forward*200f);
			*/
		}
		else
		{
			//client, we need the RB for local collisions but in kinematic mode only
			//rb.isKinematic = true;
		}
		NA.player_objects.Add(clone);
	}

}
