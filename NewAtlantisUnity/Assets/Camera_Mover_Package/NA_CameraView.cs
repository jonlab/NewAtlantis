using UnityEngine;
using System.Collections;

//Genere une vue selon un tableau de gameObject
public class NA_CameraView : MonoBehaviour {

	public KeyCode viewKey = KeyCode.O;

	public GameObject myCamera;
	public GameObject focusPoint;

	public GameObject[] views;

	int step =0;

	void Update(){

	    	GameObject otherView =  views[step%views.Length];
		    myCamera.transform.position = otherView.transform.position;
		 	myCamera.transform.LookAt(focusPoint.transform.position);

	
		if(Input.GetKeyDown(viewKey))step++;


	}

  


}
