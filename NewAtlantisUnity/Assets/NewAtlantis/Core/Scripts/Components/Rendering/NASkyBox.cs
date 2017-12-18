using UnityEngine;
using System.Collections;

public class NASkyBox : MonoBehaviour 
{
	public Material skybox;
	private Material previous;
	void Start () 
	{
		previous = RenderSettings.skybox; 

		Debug.Log("NASkybox: shadername=[" + skybox.shader.name + "]");

		//patch

		if (skybox.shader.name=="Hidden/InternalErrorShader")
		{
			skybox.shader = Shader.Find("Skybox/6 Sided");
		}
		if (skybox.shader.name==null || skybox.shader.name=="")
		{
			skybox.shader = Shader.Find("Skybox/6 Sided");
		}
		else
		{
			skybox.shader = Shader.Find(skybox.shader.name);
		}

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
