using UnityEngine;
using System.Collections;

public class NAToolBuilder : NAToolBase 
{
	public GameObject   prefab; 
	public float        distance = 1f;

    private GameObject  current = null;
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
		/*Vector3 worldforce = transform.rotation * localForce;
		if (Network.isServer)
		{
			ServerSpawnObject(objectName, transform.position+transform.forward*distance, worldforce, new Vector3(1,0,0));
		}
		else
		{
			//we send to the server
			GetComponent<NetworkView>().RPC("ServerSpawnObject", RPCMode.Server, objectName, transform.position+transform.forward*distance, worldforce, new Vector3(1,0,0));
		}
    */
	}

	public override void Press() 
	{
        //Network
		current = Network.Instantiate(prefab, transform.position+transform.forward*distance, transform.rotation, 0) as GameObject;
		//GetComponent<NetworkView>().RPC("SpawnObject", RPCMode.AllBuffered, name, Network.AllocateViewID(), position, forward, color);
		
	}

	public override void Maintain() 
	{
		if (current != null)
		{
			current.transform.position = transform.position+transform.forward*distance;
		}
	}

	public override void Release() 
	{
		current = null;
	}

	[RPC]
	void ServerSpawnObject(string name, Vector3 position, Vector3 forward, Vector3 color) 
	{
		if (!Network.isServer)
		{
			return;
		}
		LogManager.Log ("ServerSpawnObject");
		GetComponent<NetworkView>().RPC("SpawnObject", RPCMode.AllBuffered, name, Network.AllocateViewID(), position, forward, color);
	}

	[RPC]
	void SpawnObject(string name, NetworkViewID viewID, Vector3 location, Vector3 forward, Vector3 color) 
	{
		GameObject clone = null;
		if (name == "cube")
		{
			clone = GameObject.Instantiate(prefab, location, Quaternion.identity) as GameObject;
		}
	}


}
