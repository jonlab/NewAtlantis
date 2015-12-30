using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
 * Asset Bundle Preview Generator
 * Example of use : AssetBundlePreviewGenerator.Test("Bundles/grass_ground.unity3d");
 */
public class AssetBundlePreviewGenerator
{
	public static void Test(string path)
	{
		
		/*byte[] data = System.IO.File.ReadAllBytes(path);
		byte[] png = GeneratePreviewPNG(data, 256, 256);
		System.IO.File.WriteAllBytes(path+".png", png);
		*/
	}

	public static byte[] GeneratePreviewPNG(byte[] bundledata, int width, int height)
	{
		//create Asset Bundle objects
		//we will use the layer 12 to render selectively
		Vector3 pos = Vector3.zero;
		AssetBundle b = AssetBundle.LoadFromMemory(bundledata);
		Object[] objs = b.LoadAllAssets();
		List<GameObject> temp = new List<GameObject>();
		if (b.mainAsset == null) //no main Asset in the bundle
		{
			foreach (Object o in objs)
			{
				GameObject go = GameObject.Instantiate(o) as GameObject;
				go.layer = 12;
				pos = go.transform.position;
				temp.Add (go);
			}
		}
		else
		{
			GameObject go = GameObject.Instantiate(b.mainAsset) as GameObject;
			go.layer = 12;
			pos = go.transform.position;
			temp.Add (go);
		}

		b.Unload(false);

		//camera
		GameObject goCamera = new GameObject();
		Camera cam = goCamera.AddComponent<Camera>();
		goCamera.transform.position = pos+Vector3.forward*10+Vector3.up*5; //FIXME
		goCamera.transform.LookAt(pos);
		goCamera.layer = 12;

		//light
		GameObject goLight = new GameObject();
		Light light = goCamera.AddComponent<Light>();
		light.type = LightType.Directional;
		light.intensity = 1;
		goLight.layer = 12;


		Texture2D screenShot = null;
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

		//destroy 
		RenderTexture.active = null;
		cam.targetTexture = null;
		RenderTexture.Destroy(rt);
		GameObject.DestroyImmediate(goCamera);
		GameObject.DestroyImmediate(goLight);
		foreach (GameObject go in temp)
		{
			GameObject.DestroyImmediate(go);
		}

		//Convert to PNG file
		byte[] bytes = screenShot.EncodeToPNG();
		Texture2D.Destroy(screenShot);
		return bytes;
	}
}
