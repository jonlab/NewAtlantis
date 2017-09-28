using UnityEngine;
using System.Collections;

public class NASkyBox : MonoBehaviour 
{
	public Material skybox;
	private Material previous;
	// Use this for initialization
	void Start () 
	{
		previous = RenderSettings.skybox; //backup previous skybox
		RenderSettings.skybox = skybox;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnDestroy()
	{
		//restore initial skybox
		RenderSettings.skybox = previous;
	}
}
