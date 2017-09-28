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
		skybox.shader = Shader.Find(skybox.shader.name);

		RenderSettings.skybox = skybox;

	}
	
	void Update () 
	{
	
	}

	void OnDestroy()
	{
		RenderSettings.skybox = previous;
		RenderSettings.skybox.shader = Shader.Find(previous.shader.name);

	}
}
