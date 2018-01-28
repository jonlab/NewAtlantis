using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAAiCarnivorous : NAAiBase 
{

	float weight = 1;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//si dans les parages d'un autre Carnivore
		GameObject eaten  =null;

		//to do : get 
		if (eaten != null)
		{
			Eat (eaten);
		}
	}

	void Eat(GameObject eaten)
	{
		
		NetworkView nv = eaten.GetComponent<NetworkView>();
		if (nv != null)
		{
			GetComponent<NetworkView>().RPC("DestroyObject", RPCMode.AllBuffered, nv.viewID);
		}
	}
}
