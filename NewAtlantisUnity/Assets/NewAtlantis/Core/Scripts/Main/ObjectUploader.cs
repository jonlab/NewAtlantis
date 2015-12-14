using UnityEngine;
using System.Collections;


//AudioUploader helper class
public class ObjectUploader  
{
	static WWWForm form = null;
	static WWW www = null;

	static private float current_upload_time = 0f;

	public static void UploadObject(string url, byte[] data, string space, string name)
	{
		form = new WWWForm();
		form.AddField("action", "upload");
		form.AddField("uid", SystemInfo.deviceUniqueIdentifier);
		form.AddField("file", "file");
		//form.AddBinaryData("file", data, "test.wav");//, "audio/x-wav");
		form.AddBinaryData("file", data);
		form.AddField("space", space);
		form.AddField("name", name);
		www = new WWW(url, form);
		current_upload_time = Time.time;

	}
	public static void Cancel()
	{
		if (www != null)
		{
			www.Dispose();
			www = null;
		}
	}

	public static bool IsWorking()
	{
		if (www != null)
			return true;
		else
			return false;
	}

	public static int Process()
	{
		if (www != null)
		{
			if (www.error != null)
			{
				Debug.Log (www.error);
				www.Dispose();
				www = null;
				return -1;
			}
			if (www.isDone)
			{
				float upload_duration = Time.time-current_upload_time;
				Debug.Log ("upload took " + upload_duration + " s");
				if (www.uploadProgress == 1)
				{
					Debug.Log ("SERVER RESPONSE : " + www.text);
					Debug.Log ("upload done !");
					www.Dispose();
					www = null;
					return 1;
				}
				else
				{
					Debug.Log ("progress : " + www.uploadProgress);
				}
			}
		}
		return 0;
	
	}

}
