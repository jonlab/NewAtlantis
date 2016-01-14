using UnityEngine;
using System.Collections;

public class NABounceOnCollide : MonoBehaviour {

	public float velocity = 5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision e)
	{
		//if (!NA.isClient ()) 
		{
			//ContactPoint 
			//e.contacts
			Rigidbody rb = GetComponent<Rigidbody>();
			Vector3 vel = e.relativeVelocity;
			vel.Normalize();
			rb.velocity = vel*velocity;


		}
	}
}
