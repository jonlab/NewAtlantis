using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class NAAiTest : NAAiBase 
{

	float MaxDistance = 40;
	float MinDistance = 1;

	// Use this for initialization
	void Start () 
	{
		
	}	
	// Update is called once per frame
	void Update () 
	{
		if (!NA.isClient ()) 
		{
			Collider[] colliders = Physics.OverlapSphere (transform.position, MaxDistance);
			Debug.Log ("colliders=" + colliders.Length);

			foreach (Collider c in colliders)
			{
				if (c.gameObject == gameObject) //no self interaction
				{
					continue;
				} 
				else
				{


					Debug.Log ("" + c.name);
				}
			}

		} 
		else 
		{
			//do nothing
		}
	}
}
