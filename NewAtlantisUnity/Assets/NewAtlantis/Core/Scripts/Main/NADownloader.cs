using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NADownloadInfo
{
	public string url = "";
	public string id = "";
	public string name = "";
}
public class NADownloader 
{
	private static WWW 	www 		= null;
	private static List<NADownloadInfo> downloads = new List<NADownloadInfo>();
	private static Dictionary<string, AssetBundle> bundles = new Dictionary<string, AssetBundle>();
	public static bool bUseCache = true;
	public static NADownloadInfo current = null;

	public static float GetCurrentProgress()
	{
		if (www != null)
		{
			return www.progress;
		}
		else
		{
			return 0;
		}
	}
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
				Debug.Log ("get asset bundle for url " + current.url);
				AssetBundle bundle = www.assetBundle;
				if (GetAssetBundle(current.id) == null && bundle != null)
				{
					if (!bundles.ContainsKey(current.id))
					{
						//Debug.LogWarning ("Add Bundle for key " + url + " val=" + bundle);
						bundles.Add (current.id, bundle);
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

	public static string Download(string url, string name)
	{
		//JT : we have to download again even if already downloaded because the Unload() destroys the Asset Bundle
		/*
		if (urls.Contains(url))
		{
			//Debug.LogWarning ("already downloading : " + url);
		    return; //already downloading
		}
		*/
		NADownloadInfo info = new NADownloadInfo();
		info.url = url;
		info.name = name;
		//info.id = ""+Time.time*1000; //"unique ID"
		info.id = ""+Random.value*10000000f;
		Debug.Log ("id = " + info.id);
		downloads.Add (info);
		return info.id;

	}

	public static AssetBundle GetAssetBundle(string id)
	{
		try
		{
			AssetBundle b = bundles[id];
			return b;
		}
		catch (System.Collections.Generic.KeyNotFoundException e)
		{
			return null;
		}
	}

	private static void Next()
	{
		//Debug.Log ("Next");
		if (downloads.Count == 0)
			return;
		current = downloads[0];
		Debug.Log ("trying to download " + current.url);
		downloads.RemoveAt(0);
		if (bUseCache)
		{
			www = WWW.LoadFromCacheOrDownload(current.url, 1);
		}
		else
		{
			www = new WWW(current.url);
		}

	}

}
