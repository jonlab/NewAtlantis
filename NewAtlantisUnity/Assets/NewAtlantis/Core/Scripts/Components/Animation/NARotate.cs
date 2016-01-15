using UnityEngine;
using System.Collections;

public class NARotate : MonoBehaviour 
{

	public Vector3 rotateVector = new Vector3(0,0,0);
	public float vectorSpeed = 1.0f;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (NA.isClient() && NA.syncMode != SyncMode.NoInDepthSync)
			return;
		//if (!NA.isClient ()) 
        {

			transform.Rotate(rotateVector * vectorSpeed * Time.deltaTime);

		}
	
	}
}
