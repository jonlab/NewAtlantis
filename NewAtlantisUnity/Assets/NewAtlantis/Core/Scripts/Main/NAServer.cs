using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//interface with the New Atlantis web server
public class NAServer
{	
	private static WWW 		www 				= null;
	public static WWW 		wwwPost 			= null;
	private static List<WWW> 	requests 		= new List<WWW>();
	public static string 	strLogin 			= "";
	public static string 	strPassword 		= "";
	public static string 	strPasswordRetype 	= "";
	public static string 	strEmail 			= "";
	public static string 	strSpaceName 		= "";
	public static bool		bSpacePublic 		= true;
	public static bool		bAssetPublic 		= true;
	
	public static void Process()
	{
		if (www != null)
		{
			if (www.isDone)
			{
				Debug.Log (www.text);
				NA.app.ParseXML(www.text);
				www.Dispose();
				www = null;
			}
		}
		
		
		
		if (wwwPost != null)
		{
			if (wwwPost.error != null)
			{
				Debug.Log (wwwPost.error);
				LogManager.LogError("HTTP ERROR - please check your internet connection.");
				wwwPost.Dispose();
				wwwPost = null;
				//return -1;
				return;
			}
			if (wwwPost.isDone)
			{
				Debug.Log ("Web Server returned " + wwwPost.text);
				string xml = wwwPost.text;	
				wwwPost.Dispose();
				wwwPost = null;
				
				NA.app.ParseXML(xml);
			}
		}

		foreach (WWW w in requests) 
		{
			if (w.isDone)
			{
				Debug.Log ("WWW is done : " + w.text);
				w.Dispose();
				requests.Remove(w);
			}
		}
	}


	public static void UserConnect()
	{
		PlayerPrefs.SetString("login", strLogin);
		PlayerPrefs.SetString("pwd", strPassword);
		NA.app.strName = strLogin;
		WWWForm form = new WWWForm();
		form.AddField("login", strLogin);
		form.AddField("pwd", strPassword);
		
		wwwPost = new WWW("http://tanant.info/newatlantis2/login.php", form);
	}
	
	
	public static void UserRegister()
	{
		NA.app.strName = strLogin;
		WWWForm form = new WWWForm();
		form.AddField("login", 	strLogin);
		form.AddField("pwd", 	strPassword);
		form.AddField("email", 	strEmail);
		wwwPost = new WWW("http://tanant.info/newatlantis2/adduser.php", form);
	}
	
	
	public static void SpaceCreate()
	{
		WWWForm form = new WWWForm();
		form.AddField("login", 	strLogin);
		form.AddField("pwd", 	strPassword);
		form.AddField("name", 	strSpaceName);
		if (bSpacePublic)
			form.AddField("type", 	"public");
		else
			form.AddField("type", 	"private");
		
		wwwPost = new WWW("http://tanant.info/newatlantis2/addspace.php", form);
	}
	
	public static void AssetAdd(byte[] data, string name)
	{
		WWWForm form = new WWWForm();
		form.AddField("file", "file");
		form.AddBinaryData("file", data);
		form.AddField("login", 	strLogin);
		form.AddField("pwd", 	strPassword);
		form.AddField("name", 	name);
		if (bAssetPublic)
			form.AddField("type", 	"public");
		else
			form.AddField("type", 	"private");
		
		wwwPost = new WWW("http://tanant.info/newatlantis2/addasset.php", form);
		
	}
	
	public static void AssetUpdate(int asset_id, byte[] data, string name)
	{
		WWWForm form = new WWWForm();
		form.AddField("file", "file");
		form.AddBinaryData("file", data);
		form.AddField("login", 	strLogin);
		form.AddField("pwd", 	strPassword);
		form.AddField("name", 	name);
		if (bAssetPublic)
			form.AddField("type", 	"public");
		else
			form.AddField("type", 	"private");
		
		form.AddField("asset_id", ""+asset_id);
		
		wwwPost = new WWW("http://tanant.info/newatlantis2/addasset.php", form);
		
	}
	
	public static void ObjectAdd(Space space, Asset asset, Vector3 position)
	{
		WWWForm form = new WWWForm();
		form.AddField("login", 	strLogin);
		form.AddField("pwd", 	strPassword);
		form.AddField("asset_id", 	asset.id);
		form.AddField("space_id", 	space.id);
		form.AddField("x", 	""+position.x);
		form.AddField("y", 	""+position.y);
		form.AddField("z", 	""+position.z);
		wwwPost = new WWW("http://tanant.info/newatlantis2/addobject.php", form);
	}
	
	public static void ObjectDelete(string id)
	{
		WWWForm form = new WWWForm();
		form.AddField("login", 	strLogin);
		form.AddField("pwd", 	strPassword);
		form.AddField("object_id", 	id);
		wwwPost = new WWW("http://tanant.info/newatlantis2/deleteobject.php", form);
	}
	
	//AssetDelete
	//SpaceDelete
	//AssetUpdate
	
	public static void ObjectUpdate(string id, Vector3 position, Vector3 angles, Vector3 scale)
	{
		WWWForm form = new WWWForm();
		form.AddField("login", 	strLogin);
		form.AddField("pwd", 	strPassword);
		form.AddField("object_id", 	id);
		form.AddField("x", 	""+position.x);
		form.AddField("y", 	""+position.y);
		form.AddField("z", 	""+position.z);
		form.AddField("ax", 	""+angles.x);
		form.AddField("ay", 	""+angles.y);
		form.AddField("az", 	""+angles.z);
		form.AddField("sx", 	""+scale.x);
		form.AddField("sy", 	""+scale.y);
		form.AddField("sz", 	""+scale.z);
		wwwPost = new WWW("http://tanant.info/newatlantis2/setobject.php", form);
	}
	
	public static void Get()
	{
		WWWForm form = new WWWForm();
		form.AddField("login", 	strLogin);
		form.AddField("pwd", 	strPassword);
		wwwPost = new WWW("http://tanant.info/newatlantis2/get.php", form);
	}


	//deprecated :

	public static void GetSpaceDescription(string space)
	{
		string url = Settings.URLWebServer + "getspace.php?password=qkvnhr7d3Y";
		url += "&space=" + space;
		www = new WWW (url);
	}
	
	public static void GetWorldDescription(string space)
	{
		string url = Settings.URLWebServer + "getworld.php?password=qkvnhr7d3Y";
		url += "&space=" + space;
		www = new WWW (url);
	}

	//move an object to a given space name
	public static void SetObjectSpace(string id, string space)
	{
		//string url = "http://www.tanant.info/newatlantis/set.php?password=qkvnhr7d3Y&action=setspace";
		string url = Settings.URLWebServer + "set.php?password=qkvnhr7d3Y&action=setspace";
		url += "&space=" + space;
		url += "&id=" + id;
		Debug.Log ("Request : " + url);
		WWW lwww = new WWW (url);
		requests.Add (lwww);
	}
	

	
}
