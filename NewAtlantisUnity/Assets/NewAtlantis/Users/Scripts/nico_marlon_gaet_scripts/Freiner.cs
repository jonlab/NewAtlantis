using UnityEngine;
using System.Collections;

public class Freiner : MonoBehaviour {
	public KeyCode space;
	public float ratio=1.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (space)) {
			Rigidbody rg = transform.GetComponent<Rigidbody>();
			rg.velocity/=ratio;
		}
			

	
	}
}
