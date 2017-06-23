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
		//patch
		//skybox.shader = Shader.Find(skybox.shader.name);
		//patch
		skybox.shader = Shader.Find("Skybox/6 Sided");

		//if (skybox.shader.name == null)
		//	skybox.shader = Shader.Find("Skybox/6 Sided");
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnDestroy()
	{
		//restore initial skybox
		RenderSettings.skybox = previous;
		//patch
		skybox.shader = Shader.Find(skybox.shader.name);
	}
}
