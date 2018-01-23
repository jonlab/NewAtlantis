using UnityEngine;
using System.Collections;

/*
 * A very simple First Person Camera
 */
public class NACameraCinematic : NACamera {

	public Camera 	camera;
	// Use this for initialization
	GameObject goAvatar = null;

	void OnEnable()
	{
		goAvatar = GameObject.Find(NAServer.strLogin);
		if (goAvatar != null)
		{
			goAvatar.transform.GetChild(0).gameObject.SetActive(false);
		}
	}

	void OnDisable()
	{
		GameObject goAvatar = GameObject.Find(NAServer.strLogin);
		if (goAvatar != null)
		{
			goAvatar.transform.GetChild(0).gameObject.SetActive(true);
		}
	}
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!goAvatar) 
		{
			goAvatar = GameObject.Find(NAServer.strLogin);
			if (goAvatar != null)
			{
				goAvatar.transform.GetChild(0).gameObject.SetActive(false);
			}
		}

		//make the viewer turn slowly
		Vector3 angles = transform.eulerAngles;
		angles.y += Time.deltaTime * 1f;
		transform.eulerAngles = angles;

	}

	void LateUpdate()
	{
		//Pure First Person camera
		if (camera != null)
		{
			camera.transform.position = transform.position;
			camera.transform.rotation = transform.rotation;

		}
	}
}
