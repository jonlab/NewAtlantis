using UnityEngine;
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
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public override void Action() 
	{
		Debug.Log ("NAToolPhysicsInteraction action");
		//pick object in front and do action
		RaycastHit hit;
		GameObject go = PickObject(new Ray(transform.position, transform.forward), out hit);
		if (go != null)
		{
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
}
