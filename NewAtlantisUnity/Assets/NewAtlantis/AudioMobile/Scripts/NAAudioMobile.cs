using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.XPath;

public class NAAudioMobile : MonoBehaviour 
{
	private static string url = "http://audio-mobile.org/rest/views/getcontent"; 
	private static WWW 		www 				= null;
	// Use this for initialization
	void Start () 
	{
		www = new WWW(url);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (www != null)
		{
			if (www.isDone)
			{
				Debug.Log (www.text);
				ParseXML(www.text);
				www.Dispose();
				www = null;
			}
		}
		
	}

	void ParseXML(string str)
	{
		Debug.Log("parse " + str);
		XmlDocument xml = new XmlDocument();
		XPathNavigator  	xpn			= null;
		XmlNamespaceManager xnm 		= null;

		xml.XmlResolver = null;
		try
		{
			xml.LoadXml(str);
		}
		catch (XmlException e)
		{
			Debug.Log(e.Message);
		}
		xpn =  xml.CreateNavigator();
		XPathNodeIterator xpni_items = xpn.Select("/result");
		xpni_items.MoveNext();
		if (xpni_items.Current != null)
		{
			XPathNodeIterator xpnic = xpni_items.Current.SelectChildren(XPathNodeType.Element);
			if (xpnic == null)
				return;
			while(xpnic.MoveNext())
			{
				XPathNodeIterator xpnicc = xpnic.Current.SelectChildren(XPathNodeType.Element);
				string audio_url = "";
				string image_url = "";
				while(xpnicc.MoveNext())
				{
					if(xpnicc.Current.Name.Equals("image"))
					{
						image_url = xpnicc.Current.InnerXml;
					}
					if(xpnicc.Current.Name.Equals("audio"))
					{
						audio_url = xpnicc.Current.InnerXml;
					}
				}

				//create component

				Debug.Log(audio_url);
				Debug.Log(image_url);

				GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
				go.transform.position = new Vector3(Random.value*100f, 1f, Random.value*100f);
				NAAudioMobileSource am = go.AddComponent<NAAudioMobileSource>();
				AudioSource source = go.AddComponent<AudioSource>();
				source.loop = true;
				NAPhysicsAudioSource pas = go.AddComponent<NAPhysicsAudioSource>();
				pas.SoundPressureLevel = 60;

				am.AudioUrl = audio_url;
				am.ImageUrl = image_url;
				am.Download();

				/*if(xpnic.Current.Name.Equals("item"))
				{
					string audio_url = xpnic.Current.InnerXml;
					Debug.Log(audio_url);
				}
				*/
			}
		}

	}
}
