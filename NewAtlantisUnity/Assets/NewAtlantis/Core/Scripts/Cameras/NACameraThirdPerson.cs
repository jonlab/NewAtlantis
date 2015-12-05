using UnityEngine;
using System.Collections;

public class NACameraThirdPerson : NACamera 
{

	public float 	distance = 0f;
	public float 	smoothing = 0f;
	public Camera 	camera;
	Vector3 		pos;
	Quaternion 		rot;


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (camera != null)
		{
			pos = camera.transform.position;
			rot = camera.transform.rotation;
		}
		Camera cam = GetComponent<Camera>();
		//cam.orthographic = true;
	}

	void LateUpdate()
	{
		//on fait un orbit au lieu de faire un lookat
		if (camera != null)
		{
			float k = smoothing;
			//Vector3 targetpos = transform.parent.position + transform.parent.forward * -distance;
			Vector3 targetpos = transform.position + transform.forward * -distance;
			camera.transform.position = pos * k + targetpos * (1-k);
			
			//Quaternion targetrotation = transform.parent.rotation;
			Quaternion targetrotation = transform.rotation;
			float angle = Quaternion.Angle(targetrotation, rot);
			camera.transform.rotation = Quaternion.Lerp(rot, targetrotation, (1-k));
		}
	}
}
