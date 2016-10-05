using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;

public class NAOSC : MonoBehaviour {

	public string 	Ip = "127.0.0.1";
	public int 		Port = 10008;
	static public bool		Ready = false;
	// Use this for initialization
	void Start () 
	{
		OSCHandler.Instance.Init();
		OSCHandler.Instance.CreateClient("NewAtlantis", IPAddress.Parse(Ip), Port);
		Ready = true;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//update the Avatar position



		if (NA.listener != null)
		{
			try
			{
				Vector3 worldposition = NA.listener.transform.position;
				Vector3 angles = NA.listener.transform.eulerAngles;
				List<object> parameters = new List<object>();

				parameters.Add(worldposition.x);
				parameters.Add(worldposition.y);
				parameters.Add(worldposition.z);

				parameters.Add(angles.x);
				parameters.Add(angles.y);
				parameters.Add(angles.z);

				OSCHandler.Instance.SendMessageToClient("NewAtlantis", "/listener", parameters);
			}
			catch (System.Exception e)
			{
				//silent fail
			}
		}
	}
}
