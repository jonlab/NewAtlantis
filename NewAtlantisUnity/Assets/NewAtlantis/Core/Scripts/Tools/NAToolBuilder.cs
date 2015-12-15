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
		
	}

	public override void Maintain() 
	{
		current.transform.position = transform.position+transform.forward*distance;
	}

	public override void Release() 
	{
		current = null;
		
	}




}
