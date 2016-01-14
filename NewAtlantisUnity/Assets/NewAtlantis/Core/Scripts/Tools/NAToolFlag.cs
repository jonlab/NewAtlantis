using UnityEngine;
using System.Collections;

public class NAToolFlag : NAToolBase 
{
	public GameObject prefab; 
	public float distance = 1f;
	public Vector3 localForce = Vector3.forward;
	public string objectName = "cube";


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
	void CommandSpawnObject(string name, Vector3 position, Vector3 forward, Vector3 color) 
	{
		if (!Network.isServer)
		{
			return;
		}
		LogManager.Log ("CommandSpawnObject");
		GetComponent<NetworkView>().RPC("SpawnObject", RPCMode.AllBuffered, name, Network.AllocateViewID(), position, forward, color);
	}

	[RPC]
	void SpawnObject(string name, NetworkViewID viewID, Vector3 location, Vector3 forward, Vector3 color) 
	{
		//envoyé à tous
		GameObject clone = null;
		clone = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		NetworkView nView = clone.AddComponent<NetworkView>();
		nView.viewID = viewID;
		clone.transform.position = location;

		MeshRenderer renderer = clone.GetComponent<MeshRenderer>();
		if (renderer != null)
		{
			renderer.material.color = new Color(color.x, color.y, color.z);
		}
		clone.transform.forward = transform.rotation * Vector3.Normalize(forward) ;
		//Rigidbody rb = clone.AddComponent<Rigidbody>();
		if (NA.isServer() || NA.isStandalone())
		{
			Rigidbody rb = clone.AddComponent<Rigidbody>();
			rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
			rb.AddForce(forward*200f);
		}
		else
		{
			//client, we need the RB for local collisions but in kinematic mode only
			//rb.isKinematic = true;
		}
		NA.player_objects.Add(clone);
	}

}
