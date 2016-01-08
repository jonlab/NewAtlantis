using UnityEngine;
using System.Collections;


//base class for all New Atlantis Cameras
public class NACamera : MonoBehaviour {

	public string name = "";
	void OnEnable()
	{
		LogManager.Log("CAMERA : " + name);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
