using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NADownloader 
{
	private static WWW 	www 		= null;
	private static List<string> urls = new List<string>();
	private static Dictionary<string, AssetBundle> bundles = new Dictionary<string, AssetBundle>();
	public static bool bUseCache = true;
	public static string url = "";

	public static void Process()
	{
		/*foreach( KeyValuePair<string, AssetBundle> kvp in bundles )
		{
			Debug.Log("Key=" + kvp.Key + " Value=" +  kvp.Value);
		}*/
		if (www != null) 
		{
			//Debug.Log ("www is not null");
			if (www.isDone)
			{
				//Debug.Log ("get asset bundle for url " + url);
				AssetBundle bundle = www.assetBundle;

				if (GetAssetBundle(url) == null && bundle != null)
				{
					if (!bundles.ContainsKey(url))
					{
						//Debug.LogWarning ("Add Bundle for key " + url + " val=" + bundle);
						bundles.Add (url, bundle);
					}
				}
				www.Dispose();
				www = null;
			}
			else if (!string.IsNullOrEmpty(www.error))
			{
				Debug.LogError (www.error);
				www.Dispose();
				www = null;
			}
		}
		else
		{
			Next(); //download next
		}
	}

	public static void Download(string url)
	{
		if (urls.Contains(url))
		{
			//Debug.LogWarning ("already downloading : " + url);
		    return; //already downloading
		}
		urls.Add (url);
	}

	public static AssetBundle GetAssetBundle(string url)
	{
		try
		{
			//Debug.Log ("get asset bundle from Dictionary for url " + url);
			//Debug.Log ("contains key " + url + " = " + bundles.ContainsKey(url));
			//we try to get the Asset Bundle from our cache
			AssetBundle b = bundles[url];
			//Debug.Log ("found " + b);
			return b;
		}
		catch (System.Collections.Generic.KeyNotFoundException e)
		{
			//Debug.Log ("NOT FOUND");
			return null;
		}
	}

	private static void Next()
	{
		//Debug.Log ("Next");
		if (urls.Count == 0)
			return;
		url = urls[0];
		Debug.Log ("trying to download " + url);
		urls.RemoveAt(0);
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
