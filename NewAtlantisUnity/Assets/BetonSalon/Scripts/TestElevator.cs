using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestElevator : MonoBehaviour {

	public float minimumHeight = 0;
	public float maximumHeight = 1000;
	public float speed = 1;

	// Use this for initialization
	void Start () 
	{

	}

	// Update is called once per frame
	void Update () 
	{
		Vector3 pos = transform.position;
		pos += Vector3.up * Time.deltaTime * speed;
		if (pos.y > maximumHeight)
			pos.y = minimumHeight;
		transform.position = pos;
	}
}
