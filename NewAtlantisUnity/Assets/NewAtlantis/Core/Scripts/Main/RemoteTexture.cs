using UnityEngine;
using System.Collections;

public class RemoteTexture 
{
	public Texture2D texture = null;
	private WWW www;
	private string original_url = "";

	public void Download(string url)
	{
		original_url = url;
		www = new WWW(url);
	}

	public void Process()
	{
		
		if (www != null)
		{
			if (www.isDone)
			{
				/*
				string strH = "";
				foreach (string s in www.responseHeaders.Keys)
				{
					//Debug.Log(www.responseHeaders[s]);
					strH += s + " = " + www.responseHeaders[s] + "\n";
						
				}
				Debug.Log(strH);
				*/
				//Debug.Log(www.responseHeaders.ToString());
				//if (www.url == original_url)
				/*string returned_url = www.responseHeaders["LOCATION"];
				if (returned_url == original_url)
				{
					texture = www.texture;
				}*/
				try
				{
					string status = www.responseHeaders["STATUS"];
					if (status.Contains("OK"))
					{
						texture = www.texture;
					}
				}
				catch (System.Exception e)
				{
					Debug.LogWarning("RemoteTexture - no response headers");
				}

				www.Dispose();
				www = null;
			}
		}
	}



}
