using UnityEngine;
using System.Collections;

public class NAGravityChanger : MonoBehaviour 
{

	Vector3 gravity = new Vector3(0,10,0);
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter(Collider collider) 
	{
		if (NA.isServer() || NA.isStandalone())
		{
			SetGravity(gravity);
		}
	}
	
	void SetGravity(Vector3 gravity)
	{
		Physics.gravity = gravity;
	}
}
