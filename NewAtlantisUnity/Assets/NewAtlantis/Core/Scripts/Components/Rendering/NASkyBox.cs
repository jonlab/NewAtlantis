using UnityEngine;
using System.Collections;

public class NASkyBox : MonoBehaviour 
{
	public Material skybox;
	// Use this for initialization
	void Start () 
	{
		RenderSettings.skybox = skybox;
		//patch
		skybox.shader = Shader.Find(skybox.shader.name);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
