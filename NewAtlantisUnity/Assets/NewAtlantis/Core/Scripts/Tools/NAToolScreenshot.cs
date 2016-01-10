using UnityEngine;
using System.Collections;

public class NAToolScreenshot : NAToolBase {

	public Texture2D screenShot = null;
	/*public NAToolScreenshot ()
	{
		name = "screenshot";
	}*/
	public override void Action() 
	{
		//take a screenshot and store it on disk
		System.DateTime now = System.DateTime.Now;
		TakeScreenshot(Camera.main, 3840,2160, "screen_" + now.Year+"_"+now.Month+"_"+now.Day+"_"+now.Hour+"_"+now.Minute+"_"+now.Second+".jpg");
		TransitionManager.Start(TransitionManager.FadeIn,1f,Color.white, null);
	}

	public void TakeScreenshot(Camera cam, int width, int height, string path)
	{
		
		//Create the Oversized Render Texture
		RenderTexture rt  = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
		//Render Camera
		RenderTexture.active = rt;
		cam.targetTexture = rt;
		Rect rectBak = cam.rect;
		cam.rect = new Rect(0,0,1,1);
		cam.Render();
		cam.rect = rectBak;
		
		//currentCamera.GetComponent<GUILayer>();

		//Create the blank texture container
		if (screenShot == null)
		{
			screenShot = new Texture2D(width, height, TextureFormat.ARGB32, false);

		}
		//Assign rt as the main render texture, so everything is drawn at the higher resolution
		RenderTexture.active = rt;
		//Read the current render into the texture container, screenShot
		screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
		screenShot.Apply();
		//--Clean up--
		RenderTexture.active = null;
		cam.targetTexture = null;
		RenderTexture.Destroy(rt);
		//--End Clean up--
		
		//Convert to PNG file
		//byte[] bytes = screenShot.EncodeToPNG();
		byte[] bytes = screenShot.EncodeToJPG();
		//Save the file
		#if !UNITY_WEBPLAYER
		System.IO.File.WriteAllBytes(path, bytes);
		//Second Clean up
		//Texture2D.DestroyImmediate(screenShot);
		#endif	
	}

	virtual public string GetName() 
	{
		return "noname";
	}

	void OnGUI()
	{
		this.DrawBaseGUI();
		if (screenShot != null)
		{
			GUI.DrawTexture(new Rect(Screen.width/2+32, Screen.height-64, 64*16/9, 64), screenShot);
		}
	}
}
