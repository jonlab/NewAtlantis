using UnityEngine;
using System.Collections;

/*

A script to apply a force to a RigidBody.  You can set an initial force (so something starts moving when instantiated)
 and also apply a force, to push or animate it in response to an event. 
*/

public class NAForce : MonoBehaviour {
	private Rigidbody rigidBody;
	public float initialForce; 	// initial force along the forward vector when instantiated

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		if (rigidBody != null)
		{
			rigidBody.AddForce (initialForce * transform.forward);
		}

	}

	public void applyForce (Vector3 f)
	{
		if (rigidBody != null)
		{
			rigidBody.AddForce (f);
		}
	}

	public void applyForwardForce (float f)
	{
		rigidBody.AddForce (f * transform.forward);
	}

}
