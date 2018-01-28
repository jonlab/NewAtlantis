using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAAiAutoSpawner : MonoBehaviour 
{

	float timer = 0;
	float duration = 3;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (NA.isServer ())
		{

			timer += Time.deltaTime;
			if (timer > duration)
			{
				timer -= duration;
				duration = Random.value * 10f;
				//"autotrunk nog"
				Spawn ("monster"); 
			}

		}
		
	}

	void Spawn(string name)
	{
		//we spawn objects around the viewer
		//string name = "autotrunk nog";
		Vector3 viewerpos = NA.app.gameObject.transform.position; //get the view current pos
		Vector3 position = new Vector3 (viewerpos.x, viewerpos.y+Random.value*100, viewerpos.z);
		Vector3 forward = new Vector3 (0, 1, 0);
		Vector3 color = new Vector3(0,1,0); // color as Vector3
		NA.app.gameObject.GetComponent<NetworkView> ().RPC ("SpawnObject", RPCMode.AllBuffered, name, Network.AllocateViewID (), position, forward, color);
	}
}
