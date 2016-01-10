using UnityEngine;
using System.Collections;

public class NA_ModulateCamera : MonoBehaviour {
    Camera cam;

    public Matrix4x4 matrix;

    public bool init = true;
	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
      if(init)  matrix = cam.projectionMatrix;
	}
	
	// Update is called once per frame
	void Update () {
        cam.projectionMatrix = matrix;
	}
}
