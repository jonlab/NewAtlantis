using UnityEngine;
using System.Collections;

public class NAWind : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{

		Rigidbody[] rbs = GameObject.FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];
		foreach (Rigidbody rb in rbs)
		{
			rb.AddForce(new Vector3(0,500,0));
		}
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	
	}
}
