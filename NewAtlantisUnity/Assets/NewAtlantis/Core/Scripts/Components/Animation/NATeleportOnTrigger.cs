using UnityEngine;
using System.Collections;


//teleport the triggering object to another position in the world
public class NATeleportOnTrigger : MonoBehaviour 
{
	public GameObject Target = null;
	public bool ResetVelocity = true;
	public float OutputVelocity = 1.0f;

	void OnTriggerEnter(Collider collider) 
	{
		//Debug.Log ("NATeleportOnCollide OnCollisionEnter");
		//if (!NA.isClient()) //FIXME jonathan ?
		{

			//send the colliding object to the target position
	        collider.gameObject.transform.position = Target.transform.position;
			Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
			if (rb != null)
			{
				if (ResetVelocity)
				{
					rb.velocity = new Vector3(Random.value-0.5f, Random.value-0.5f, Random.value-0.5f)*OutputVelocity*2f;
				}
			}
		}
	}
}
