using UnityEngine;
using System.Collections;



public class NASyncTransform : MonoBehaviour {

	NetworkView nv = null;
	float timer = 0f;
	public float interval = 0.2f;
	Vector3 last_received_position = Vector3.zero;
	Vector3 last_received_angles = Vector3.zero;
	Vector3 last_received_velocity = Vector3.zero;
	Quaternion last_received_rotation = Quaternion.identity;
	// Use this for initialization
	void Start () 
	{
		nv = GetComponent<NetworkView>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (nv.owner == Network.player)
		{
			timer+=Time.deltaTime;
			if (timer > interval)
			{
				timer -= interval;
				//send a sync frame
				nv.RPC("SetTransformState", RPCMode.Others, transform.position, transform.rotation, Vector3.zero);
			}
		}
		else
		{
			//kind of dead reckoning
			Vector3 position = transform.position;
			Quaternion rotation = transform.rotation;
			//to do : improve this !!!
			position = Vector3.Lerp(position, last_received_position, 0.1f);
			rotation = Quaternion.Lerp(rotation, last_received_rotation, 0.1f);

			transform.position = position;
			transform.rotation = rotation;
		}
	}
	[RPC]
	void SetTransformState(Vector3 position, Quaternion rotation, Vector3 velocity) 
	{
		last_received_position 	= position;
		last_received_rotation 	= rotation;
		last_received_velocity 	= velocity;
	}
}
