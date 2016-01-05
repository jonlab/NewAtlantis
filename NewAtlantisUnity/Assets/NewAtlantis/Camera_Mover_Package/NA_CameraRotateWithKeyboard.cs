using UnityEngine;
using System.Collections;

//Fais tourner la camera autour d'une position avec deux touches clavier
public class NA_CameraRotateWithKeyboard : MonoBehaviour {

	public GameObject myCamera;
	public GameObject focusPoint;

	public KeyCode rotateKey = KeyCode.J;
	public KeyCode antiRotateKey = KeyCode.H;

	public Vector3 rotateVector = new Vector3(0,2,0);
	public float angle = 1.0f;


	void Update () {
	
			if (Input.GetKey (rotateKey))
				myCamera.transform.RotateAround (focusPoint.transform.position, rotateVector, angle);
			if (Input.GetKey (antiRotateKey))
				myCamera.transform.RotateAround (focusPoint.transform.position, rotateVector, -angle);

	}
}
