using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityOSC;

public class NASendOSCMessageOnCollide : MonoBehaviour 
{
	public string OscAddress = "/collide";
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}


	void OnCollisionEnter(Collision e)
	{
		if (NAOSC.Ready)
		{
			//send an OSC message with x y z world position and collision relative magnitude as parameters
			List<object> parameters = new List<object>();
			Vector3 worldposition = e.gameObject.transform.position;
			parameters.Add(worldposition.x);
			parameters.Add(worldposition.y);
			parameters.Add(worldposition.z);
			parameters.Add(e.relativeVelocity.magnitude);
			OSCHandler.Instance.SendMessageToClient("NewAtlantis", OscAddress, parameters);
		}
		else
		{
			LogManager.LogWarning("OSC sending while not ready !");
		}
	}
}
