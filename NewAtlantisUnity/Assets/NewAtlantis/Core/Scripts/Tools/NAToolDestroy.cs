using UnityEngine;
using System.Collections;

public class NAToolDestroy : NAToolBase {


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

        //send Destroy call to server
		if (Network.isServer)
		{
			ServerDestroyObject(transform.position, transform.forward);
		}
		else
		{
			//we send to the server
			GetComponent<NetworkView>().RPC("ServerDestroyObject", RPCMode.Server, transform.position, transform.forward);
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



	
	
	
	[RPC]
	void ServerDestroyObject(Vector3 position, Vector3 forward) 
	{
		if (!Network.isServer)
		{
			return;
		}
        LogManager.Log ("ServerDestroyObject");

		RaycastHit hit;
		GameObject go = NA.PickObject(new Ray(position, forward), out hit);
		if (NA.isViewer(go))
		{
			LogManager.Log("destroy on Viewer, ignored by server.");
			return;
		}
		if (go != null)
		{
			NetworkView nv = go.GetComponent<NetworkView>();
			if (nv != null)
			{
				GetComponent<NetworkView>().RPC("DestroyObject", RPCMode.AllBuffered, nv.viewID);
			}
			else
			{
				/*LogManager.LogWarning("ServerDestroyObject received but no NV on Object to destroy, trying on parents.");
				nv = go.GetComponentInParent<NetworkView>();
				if (nv != null)
				{
					GetComponent<NetworkView>().RPC("DestroyObject", RPCMode.AllBuffered, nv.viewID);
				}
				else
				{
					LogManager.LogWarning("ServerDestroyObject received but no NV on Object to destroy on parents.");
					
				}
                */


			}
		}
	}
	


}
