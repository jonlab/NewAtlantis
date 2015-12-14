using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {


	public GameObject mycamera;
	public GameObject point;

	public float speed = 1;

	// Update is called once per frame
	void Update () {
	
		if(Input.GetMouseButton(1))	mycamera.transform.RotateAround(point.transform.position , new Vector3(0,1,0) , speed );
		if(Input.GetMouseButton(2))	mycamera.transform.RotateAround(point.transform.position , new Vector3(0,1,0) , -speed );
	
	}

	public void setPoint(GameObject o){
		point = o;
	}
}
