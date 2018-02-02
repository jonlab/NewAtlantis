using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebcamProcess : MonoBehaviour 
{


	WebCamTexture webcamTexture;
	public static float average_red = 0;
	public static float average_green = 0;
	public static float average_blue = 0;
	public bool bGUI = false;
	// Use this for initialization
	void Start () 
	{
		WebCamDevice[] cam_devices = WebCamTexture.devices;
		for (int i = 0; i < cam_devices.Length; i++) 
		{
			LogManager.Log ("Webcam available: " + cam_devices [i].name);
		}
		webcamTexture = new WebCamTexture(160,120); //fixme

		if (webcamTexture == null)
			return;
		//rawimage.texture = webcamTexture;
		//rawimage.material.mainTexture = webcamTexture;
		webcamTexture.Play();
		Debug.Log ("WebcamTexture : " + webcamTexture.width + "x" + webcamTexture.height);
		Renderer renderer = GetComponent<Renderer> ();
		if (renderer != null)
		{
			renderer.material.mainTexture = webcamTexture;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (webcamTexture == null)
			return;
		
		//Debug.Log ("WebcamTexture : " + webcamTexture.width + "x" + webcamTexture.height);
		Color[] colors = webcamTexture.GetPixels ();

		for (int i = 0; i < colors.Length; ++i)
		{
			Color c = colors [i];
			average_red += c.r;
			average_green += c.g;
			average_blue += c.b;
		}
		average_red /= colors.Length;
		average_green /= colors.Length;
		average_blue /= colors.Length;
	}

	void OnGUI()
	{
		if (bGUI)
		{
			GUI.Label (new Rect (0, 0, 300, 30), "R=" + average_red + " G=" + average_green + " B=" + average_blue);
			GUI.DrawTexture (new Rect (0, 30, 160, 90), webcamTexture);
		}
	}
}
