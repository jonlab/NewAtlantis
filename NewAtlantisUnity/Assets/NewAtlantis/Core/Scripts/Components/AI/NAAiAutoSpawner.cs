using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAAiAutoSpawner : MonoBehaviour 
{

	float timer = 0;
	float duration = 20;
	public string ObjectName = "Voices";
	public bool Enabled = true;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (NA.isServer () && Enabled)
		{
			
			timer += Time.deltaTime;
			if (timer > duration)
			{
				timer -= duration;
				duration = Random.value * 40f;
				//"autotrunk nog"
				Spawn (ObjectName); 
			}

		}
		
	}

	void Spawn(string name)
	{
		//we spawn objects around the viewer
		//string name = "autotrunk nog";
		Vector3 viewerpos = NA.app.gameObject.transform.position; //get the view current pos
		Vector3 random = new Vector3(Random.value-0.5f, Random.value-0.5f, Random.value-0.5f);

		Vector3 position = viewerpos + random * 10f;
		Vector3 forward = new Vector3 (0, 1, 0);
		Vector3 color = new Vector3(0,1,0); // color as Vector3
		NA.app.gameObject.GetComponent<NetworkView> ().RPC ("SpawnObject", RPCMode.AllBuffered, name, Network.AllocateViewID (), position, forward, color);

	}
}
