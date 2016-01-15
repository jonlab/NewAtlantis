using UnityEngine;
using System.Collections;

public class NARotateAround : MonoBehaviour 
{

	public Vector3 rotateVector = new Vector3(0,0,0);
	public float vectorSpeed = 1.0f;

	//public GameObject point;

	public Vector3 pivotPoint = new Vector3(2,0,0);
	Vector3 initPos;



	// Use this for initialization
	void Start () 
	{
		initPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{

		//if (!NA.isClient ()) 
		if (NA.isClient() && NA.syncMode != SyncMode.NoInDepthSync)
			return;
        {
			transform.RotateAround(initPos + pivotPoint, rotateVector ,vectorSpeed);
		}
	
	}





}
