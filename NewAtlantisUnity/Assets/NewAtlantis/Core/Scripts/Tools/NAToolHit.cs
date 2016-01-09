using UnityEngine;
using System.Collections;

public class NAToolHit : NAToolBase 
{
    public GameObject  target = null;

	// Use this for initialization
	void Start () 
	{
        //target.transform.localScale = Vector3.zero;
        target.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public override void Action() 
	{

	}

	public override void Press() 
	{
		target.SetActive(true);
        //target.transform.localScale = Vector3.one;
		
	}

	public override void Maintain() 
	{
		
	}

	public override void Release() 
	{
		//target.tra
		target.SetActive(false);
        //target.transform.localScale = Vector3.zero;
	}

	/*
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
    */


}
