using UnityEngine;
using System.Collections;

public class NA_RotateAdn : MonoBehaviour {

    public Vector3 rotateVector;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotateVector);
	}
}
