using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAAudioMobileSource : MonoBehaviour 
{

	public string AudioUrl = "";
	public string ImageUrl = "";
	private WWW 		wwwAudio 				= null;
	private WWW 		wwwImage 				= null;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (wwwImage != null)
		{
			if (wwwImage.isDone)
			{
				Renderer renderer = GetComponent<Renderer>();
				renderer.material.mainTexture = wwwImage.texture;
				wwwImage.Dispose();
				wwwImage = null;
			}
		}
		if (wwwAudio != null)
		{
			if (wwwAudio.isDone)
			{
				AudioSource source = GetComponent<AudioSource>();
				source.clip = wwwAudio.GetAudioClip();
				source.Play();
				wwwAudio.Dispose();
				wwwAudio = null;
			}
		}
	}

	public void Download()
	{
		wwwAudio = new WWW(AudioUrl);
		wwwImage = new WWW(ImageUrl);
	}
}
