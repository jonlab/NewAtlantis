using UnityEngine;
using System.Collections;

// Génere une vue aléatoire
public class NA_CameraRandomView : MonoBehaviour {

	public KeyCode viewKey = KeyCode.O;
	
	public GameObject myCamera;
	public GameObject focusPoint;

	// valeurs pour la position aleatoire
	public float xGap = 3;
	public float yGap = 3;
	public float zGap = 3;


	void Update(){

		//appuyer sur la touche
		if (Input.GetKeyDown (viewKey)) {
		    // la position de la camera = random position autour du focus point
			myCamera.transform.position = focusPoint.transform.position + randomPosition();
			// camera , regarde le focus point
			myCamera.transform.LookAt(focusPoint.transform.position);
		
		}

	}

	Vector3 randomPosition(){

		return new Vector3 (Random.Range (-xGap, xGap), Random.Range (-xGap, xGap), Random.Range (-xGap, xGap));

	}

}
