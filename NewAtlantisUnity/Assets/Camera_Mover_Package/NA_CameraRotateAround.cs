using UnityEngine;
using System.Collections;

//Faire tourner la caméra autour d'une position
public class NA_CameraRotateAround : MonoBehaviour {

	public GameObject myCamera;

	public GameObject focusPoint;

	public Vector3 axis = new Vector3(0,1,0);
	public float angleRotate = 0.5f;
	
	float time = 0;
	public int steps = 360;
	int mode = 0;

	public float acceleration = 0.000001f;
	public float maximalAcceleration =  0.0001f;
	public float randomMagnitude = 2.0f;

	public int maximalTime = 200;
	public float minimalRotationAngle = 0.04f;
	public float divideAngleRotation = 1.002f;


	void initAcceleration(){
		time = 0;
		// on change le mode
		mode++;
		//acceleration aleatoire
		acceleration = Random.Range (0,maximalAcceleration);
	}
	
	void calculateAxis(){
		// axe aléatoire
		Vector3 randomAxis = 
			new Vector3 (Random.Range (-randomMagnitude, randomMagnitude), Random.Range (-randomMagnitude, randomMagnitude), 0);

		axis -= randomAxis / steps;
		// si le mode modulo 2 est egal a 1 on ajoute a l'angle de rotation la valeur d'acceleration
		if (mode % 2 == 1)
			angleRotate += acceleration;
		else {
			//sinon on divise la valeur de l'angle de rotation
			angleRotate /= divideAngleRotation;
		}
	}

	void Update () {
		time++;
		//Quand le temps depasse un seuil , on redemarre
		if (time > maximalTime) {
			initAcceleration();
		} else {
			//sinon on calcule l'axe de rotation
			calculateAxis();
		}
		//si l'angle de rotation est inferieur à l'angle minimal on accelere
		if (angleRotate < minimalRotationAngle) initAcceleration();
		//la camera tourne autour du focus point
		myCamera.transform.RotateAround (focusPoint.transform.position, axis, angleRotate);
	}
}
