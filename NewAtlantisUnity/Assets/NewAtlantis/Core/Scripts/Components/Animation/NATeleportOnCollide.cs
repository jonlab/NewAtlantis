using UnityEngine;
using System.Collections;


//teleport the colliding object to another position in the world
public class NATeleportOnCollide : MonoBehaviour 
{
	public GameObject Target = null;
	void OnCollisionEnter(Collision collision) 
	{
		Debug.Log ("NATeleportOnCollide OnCollisionEnter");
		//send the colliding object to the target position
		collision.gameObject.transform.position = Target.transform.position;
	}
}
