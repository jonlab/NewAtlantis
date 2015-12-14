using UnityEngine;
using System.Collections;

public class jumpy : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnMouseDown()
	{
		GetComponent<Rigidbody>().AddForce(new Vector3(0,300,0));
	}
}
