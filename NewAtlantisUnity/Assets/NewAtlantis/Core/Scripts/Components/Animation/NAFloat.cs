using UnityEngine;
using System.Collections;

public class NAFloat : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Rigidbody rb = GetComponent<Rigidbody>();
		Vector3 force = new Vector3(Random.value-0.5f, Random.value-0.5f, Random.value-0.5f);
		float r = Random.value;
		if (r<0.1f)
		{
			rb.AddForce(force);
		}
	
	}
}
