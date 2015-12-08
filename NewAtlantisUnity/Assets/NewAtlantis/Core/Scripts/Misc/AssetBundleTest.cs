using UnityEngine;
using System.Collections;

public class AssetBundleTest : MonoBehaviour 
{
	private bool	bUseCache 	= true;
	private WWW 	www 		= null;
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (www != null) 
		{
			if (www.isDone)
			{
				Debug.Log ("get asset bundle");
				AssetBundle bundle = www.assetBundle;
				bundle.LoadAllAssets();
				bundle.Unload(false);
				//bundle.LoadAllAssets();
				//bundle.Unload(false);
				www.Dispose();
				www = null;
			}
		}
		else
		{
			Download(); //download again
		}
	}

	void Download()
	{
		string url = "http://tanant.info/newatlantis2/objects/object_564a09f49e4000.49203890.unity3d";
		Debug.Log ("download from " + url);
		if (bUseCache)
		{
			www = WWW.LoadFromCacheOrDownload(url, 1);
		}
		else
		{
			www = new WWW(url);
		}
	}
}
