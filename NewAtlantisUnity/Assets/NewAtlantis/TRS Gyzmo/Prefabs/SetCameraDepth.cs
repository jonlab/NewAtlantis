using UnityEngine;
using System.Collections;

public class SetCameraDepth : MonoBehaviour {
	//GameObject mainCamera;
	// Use this for initialization
	void Start () {
		Camera camera = transform.GetComponent<Camera> ();

		camera.depth = Camera.main.depth + 1 ;

		//Camera main = mainCamera.transform.GetComponent<Camera> ();
		//Camera.current = main;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
