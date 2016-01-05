using UnityEngine;
using System.Collections;

public class NALookAtCamera : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(Camera.main.transform.position);
	}
}
