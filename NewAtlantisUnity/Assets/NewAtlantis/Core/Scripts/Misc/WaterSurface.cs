using UnityEngine;
using System.Collections;

public class WaterSurface : MonoBehaviour {


	Texture2D textureColors;
	Color[] colors = null;

	// Use this for initialization
	void Start () 
	{
		//this texture is used as a look up texture to compute the fog and background color
		textureColors = Resources.Load ("ursula_gradient") as Texture2D;
		if (textureColors)
		{

			Color[] colorsAll = textureColors.GetPixels ();
			colors = new Color[textureColors.height];
			for (int i = 0; i < textureColors.height; ++i)
			{
				colors[textureColors.height-1-i] = colorsAll [i * textureColors.width];
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//gestion du plan d'eau
		Vector3 cam = Camera.main.transform.position;
		Vector3 pos = transform.position;
		float depth = pos.y - cam.y;
		if (cam.y < pos.y) 
		{
			//underwater
			transform.eulerAngles = new Vector3 (180, 0, 0);
			//RenderSettings.ambientLight = new Color(0,0,0);
			RenderSettings.fog = true;
			RenderSettings.fogMode = FogMode.ExponentialSquared;
			RenderSettings.fogDensity = 0.04f;
			Camera.main.clearFlags = CameraClearFlags.Color;

			float step = depth / 900f;
			step = Mathf.Clamp (step, 0f, 1f);
			Color c;
			if (colors != null)
			{
				//Debug.Log ("Color lookup");
				float rindex = (step) * (float)(colors.Length-1);
				int index1 = (int)rindex;
				float k = rindex - (float)index1;
				int index2 = index1 + 1;
				//look up in the texture with interpolation
				c = Color.Lerp (colors[index1], colors[index2], k);
			} 
			else
			{
				//Debug.Log ("Color interpolation");
				//simple interpolation
				c = Color.Lerp(new Color(0.4f, 0.4f, 1f), Color.black, step);
			}



			RenderSettings.fogColor = c;
			Camera.main.backgroundColor = c;

		} 
		else 
		{
			transform.eulerAngles = new Vector3 (0, 0, 0);
			//RenderSettings.ambientLight = new Color(0.2f,0.2f,0.2f);
			RenderSettings.fog = false;
			Camera.main.clearFlags = CameraClearFlags.Skybox;
		}
	
	}
}
