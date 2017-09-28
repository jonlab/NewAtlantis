using UnityEngine;
using System.Collections;

// Cubemap skybox
public class NASkyBox2 : MonoBehaviour 
{
	public Material skybox;
	private Material previous;
	void Start () 
	{
		previous = RenderSettings.skybox; //backup previous skybox
		RenderSettings.skybox = skybox;
	}
	
	void Update () 
	{
	
	}

	void OnDestroy()
	{
		RenderSettings.skybox = previous;
	}
}
