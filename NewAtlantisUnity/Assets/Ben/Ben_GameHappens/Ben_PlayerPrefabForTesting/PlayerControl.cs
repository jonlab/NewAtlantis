using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour {

	public GameObject followCam;
	public GameObject followCamTarget;

	public float followCamDistance=2.0f; 
	public float turningSpeed = 1.0f;
	public float flySpeed = 1.0f;
	public float camVerticalOffset = 0.0f;
	public Transform pivotTransform;

	Vector3 mouseLastPos;
	bool dragging =false;

	Rigidbody rb;


	void Start () {
		rb=GetComponent<Rigidbody>();
	}
	
	void Update () {

	// NA style navigation: arrows are forward/backwards and strafe left/right
	// mouse click and drag to rotate

		CheckRotationInput();


		float strafe = Input.GetAxis("Horizontal") * flySpeed;
		float moveForward = Input.GetAxis("Vertical") * flySpeed;

		Vector3 forceVector = (moveForward * pivotTransform.forward) + (strafe * pivotTransform.right);


		rb.AddForce(forceVector);

		UpdateFollowCam();

	}
	void CheckRotationInput ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			mouseLastPos = Input.mousePosition;
			dragging=true;
		}
		if (Input.GetMouseButtonUp(0))
		{
			dragging=false;
		}

		if (dragging)
		{
			Vector3 mouseDelta = Input.mousePosition - mouseLastPos;

			transform.Rotate(0,mouseDelta.x,0);
			pivotTransform.Rotate(-mouseDelta.y,0,0);
			mouseLastPos=Input.mousePosition;

		}

	}


	void UpdateFollowCam()
	{
		Transform t=followCam.transform;
		t.position = followCamTarget.transform.position - pivotTransform.forward * followCamDistance + new Vector3(0,camVerticalOffset,0);
		t.LookAt(followCamTarget.transform);

		/*
		Vector3 targetDirection = followCam.transform.position -transform.position;

		float step = turningSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0F);
        Debug.DrawRay(followCam.transform.position, newDir, Color.red);
        followCam.transform.rotation = Quaternion.LookRotation(newDir);	
        */

	}

}