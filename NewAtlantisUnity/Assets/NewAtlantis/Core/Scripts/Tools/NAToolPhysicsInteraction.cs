﻿using UnityEngine;
using System.Collections;


public enum PhysicsInteraction
{
	Push,
	Pull,
	Drag,
	Explode

}
public class NAToolPhysicsInteraction : NAToolBase {

	public PhysicsInteraction interaction = PhysicsInteraction.Push;
	// Use this for initialization
	private GameObject current = null;
	private Quaternion current_rotation = Quaternion.identity;
	private Vector3 dir;
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Debug.Log("current="+current);
		if (current != null)
		{
			//Vector3 newposition = transform.position+dir;
			Vector3 newposition = transform.position+transform.forward*dir.magnitude;


			Vector3 velocity = (newposition-current.transform.position)/Time.deltaTime;
			current.transform.position = newposition;
			Rigidbody rb = current.GetComponent<Rigidbody>();
			if (rb != null)
			{
				rb.velocity = velocity;
			}

		}
	
	}

	public override void Action() 
	{
		Debug.Log ("NAToolPhysicsInteraction action");
		//pick object in front and do action
		RaycastHit hit;
		GameObject go = PickObject(new Ray(transform.position, transform.forward), out hit);
		if (go != null)
		{
			Debug.Log ("object name = " + go.name);
			//look at distance
			Rigidbody rb = go.GetComponent<Rigidbody>();
			if (rb != null)
			{
				if (interaction == PhysicsInteraction.Push)
				{
					rb.AddForce(transform.forward*400f);
				}
				else if (interaction == PhysicsInteraction.Pull)
				{
					rb.AddForce(transform.forward*-400f);
				}
				else if (interaction == PhysicsInteraction.Explode)
				{
					rb.AddExplosionForce(400, go.transform.position, 3f);
				}

			} 
		}

	}

	public GameObject PickObject(Ray ray, out RaycastHit hit)
	{
		if (Physics.Raycast(ray, out hit))
		{
			return hit.collider.gameObject;
		}
		hit = new RaycastHit();
		return null;
	}


	public override void Press() 
	{
		if (interaction == PhysicsInteraction.Drag)
		{
			if (current == null)
			{
				NA.app.goReticle.GetComponent<Renderer>().material.SetColor("_TintColor", Color.red);
				RaycastHit hit;
				GameObject go = PickObject(new Ray(transform.position, transform.forward), out hit);
				if (go != null)
				{
					current = go;
					dir = go.transform.position-transform.position; //offset
					current_rotation = transform.rotation;
				}
			}
			else
			{
				NA.app.goReticle.GetComponent<Renderer>().material.SetColor("_TintColor", Color.white);
				current = null;
			}
		}

	}

	public override void Maintain() 
	{
		/*if (current != null)
		{
			Vector3 newposition = transform.position+dir;
			Vector3 velocity = (newposition-current.transform.position)/Time.deltaTime;
			current.transform.position = newposition;
			Rigidbody rb = current.GetComponent<Rigidbody>();
			if (rb != null)
			{
				rb.velocity = velocity;
			}
		}
		*/
	}

	public override void Release() 
	{
		//current = null;

	}



	//network implementation
	/*
	[RPC] 
	void ServerPhysicsInteractionAction(string name, Vector3 position, Vector3 forward, Vector3 color) 
	{
		
	}
	*/


}
