using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;


//New Atlantis shared "Object"
public class NAObject 
{
	public string 	name = "";
	public Vector3 	position = Vector3.zero;
	public Vector3 	angles = Vector3.zero;
	public string 	file = "";
	//public WWW 		www = null;
	public GameObject go = null;
	public GameObject goGizmo = null;
	public string 	id = "";
	public int 		downloaded = 0;
	private string url = "";
	private NetworkViewID NetworkViewId;
	public bool downloading = false;

	private static Dictionary<string, AssetBundle> AssetBundles = new Dictionary<string, AssetBundle>();

	public NAObject(GameObject _root, string _name, Vector3 _position, Vector3 _angles, string _file, NetworkViewID _NetworkViewId)
	{

		Debug.Log ("Creating NAObject " + _name);
		name 				= _name;
		position 			= _position;
		angles 				= _angles;
		file 				= _file;
		NetworkViewId 		= _NetworkViewId;

		//create and sync the Main GameObject
		go 						= new GameObject(_name+_NetworkViewId);
		go.transform.position 	= _position;
		go.transform.eulerAngles = _angles;
		go.transform.parent		= _root.transform;
		NetworkView nView 		= go.AddComponent<NetworkView>();
		nView.viewID 			= this.NetworkViewId;
		NetworkSync nSync 		= go.AddComponent<NetworkSync>();


	}

	public void Download()
	{
		//FIXME : move outside :
		url = "http://tanant.info/newatlantis2/objects/"+file;
		Debug.Log ("try to download object from " + url);
		//www = WWW.LoadFromCacheOrDownload(url, 1);
		NADownloader.Download(url);
		downloading = true;
		//www = new WWW (url);

	}
	public void DownloadLocal()
	{
		//www = new WWW ("file://" + file);
	}



	public void Process()
	{
		if (downloading)
		{
		AssetBundle bundle = NADownloader.GetAssetBundle(url);
		if (bundle != null)
		{
			downloading = false;
			GameObject goChild = null;
			bool bInstantiateObjects = true;
			if (bInstantiateObjects)
			{
				Object[] objs = bundle.LoadAllAssets();
				Debug.Log ("Asset Bundle Objects count = " + objs.Length);
				string[] strAssets = bundle.GetAllAssetNames();
				foreach (string s in strAssets)
				{
					Debug.Log ("Asset = " + s);
				}
				foreach (Object o in objs)
				{
					//Debug.Log ("Object " + o.name + " type:" + o.GetType());
				}
				if (bundle.mainAsset == null)
				{
					//no main Asset in the bundle
					//go = new GameObject("object_root");
					foreach (Object o in objs)
					{
						if (o != null)
						{
							goChild = GameObject.Instantiate(o) as GameObject;
							if (goChild != null)
							{
								goChild.name = o.name;
								goChild.transform.parent = go.transform;
								goChild.transform.localPosition = Vector3.zero;
								goChild.transform.localEulerAngles = Vector3.zero;
							}
						}
					}
				}
				else
				{
					goChild = GameObject.Instantiate(bundle.mainAsset) as GameObject;
					goChild.name = bundle.mainAsset.name + id;
					goChild.transform.parent = go.transform;
					goChild.transform.localPosition = Vector3.zero;
					goChild.transform.localEulerAngles = Vector3.zero;
                }
				//Debug.Log ("unloading AB " + url);
				//bundle.Unload(false);
			
			goGizmo = new GameObject("object_gizmo");
			goGizmo.transform.parent = go.transform;
			goGizmo.transform.localPosition = Vector3.zero;
			
			GameObject goGizmoX = GameObject.CreatePrimitive(PrimitiveType.Cube);
			GameObject goGizmoY = GameObject.CreatePrimitive(PrimitiveType.Cube);
			GameObject goGizmoZ = GameObject.CreatePrimitive(PrimitiveType.Cube);
			
			goGizmoX.transform.parent = goGizmo.transform;
			goGizmoY.transform.parent = goGizmo.transform;
			goGizmoZ.transform.parent = goGizmo.transform;
			goGizmoX.transform.localPosition = Vector3.zero;
			goGizmoY.transform.localPosition = Vector3.zero;
			goGizmoZ.transform.localPosition = Vector3.zero;
			float gizmolen = 4f;
			float gizmothick = 0.1f;
			goGizmoX.transform.localScale = new Vector3(gizmolen,gizmothick,gizmothick);
			goGizmoY.transform.localScale = new Vector3(gizmothick,gizmolen,gizmothick);
			goGizmoZ.transform.localScale = new Vector3(gizmothick,gizmothick,gizmolen);
			goGizmoX.GetComponent<Renderer>().material.color = Color.red;
            goGizmoY.GetComponent<Renderer>().material.color = Color.green;
            goGizmoZ.GetComponent<Renderer>().material.color = Color.blue;
            
            goGizmo.SetActive(false);
            

			//AudioSource[] sources = AudioSource.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
			AudioSource[] sources = go.GetComponentsInChildren<AudioSource>();
			//audio sources have to be augmented with specific NA behaviour
			Debug.Log ("Sources count = " + sources.Length);
			foreach (AudioSource s in sources)
			{
				NA.DecorateAudioSource(s);
				//s.rolloffMode = AudioRolloffMode.Linear;
			}

			//unactivate all loaded directional lights
			Light[] lights = go.GetComponentsInChildren<Light>();
			/*foreach (Light l in lights)
            {
				if (l.type == LightType.Directional)
					l.enabled = false;
			}
			*/

			AudioListener al = go.GetComponent<AudioListener> ();
			if (al != null)
				al.enabled = false;


			//ESPACES RESONANTS
			/*
			NAReverbResonator[] resonators = NAReverbResonator.FindObjectsOfType(typeof(NAReverbResonator)) as NAReverbResonator[];
			
			Debug.Log ("NAReverbResonator count = " + resonators.Length);
			foreach (NAReverbResonator r in resonators)
			{
				NAReverbEffector eff = r.gameObject.GetComponent<NAReverbEffector>();
				if (eff == null)
					r.gameObject.AddComponent<NAReverbEffector>();

				AudioReverbFilter arf = r.gameObject.GetComponent<AudioReverbFilter>();
				if (arf == null)
                    r.gameObject.AddComponent<AudioReverbFilter>();
                
            }
            */

            
			NetworkSync nSync 		= go.GetComponent<NetworkSync>();
			nSync.Prepare(); //client and server (for now, just stops all AudioSources)

			if (Network.isServer)
			{
				PrepareAsServer(goChild, goChild.name);
			}
			else
			{
				nSync.AttachNetworkViews();
			}
			//}

				//www.Dispose();
				//www = null;
			
            }
        }
		}
    }
    
    
    
    
    public string GetStatus()
	{
		if (downloading)
			return "downloading";
		else
			return "done";
		/*
		if (www == null) 
		{
			return "inactive";
		}
		else
		{
			if (www.isDone)
			{
				return "done " + www.bytesDownloaded;
			}
			else
			{

				return "in progress " + www.bytesDownloaded;
			}
		}
		*/
	}

	public void PrepareAsServer(GameObject root, string path)
	{
		//Debug.Log ("PrepareAsServer " + path);
		//NetworkView nView 		= root.AddComponent<NetworkView>();
		//nView.viewID 			= Network.AllocateViewID();
		go.GetComponent<NetworkView>().RPC("AttachNetworkView", RPCMode.AllBuffered, path, Network.AllocateViewID());
		for (int i=0;i<root.transform.childCount;++i)
		{
			GameObject goChild = root.transform.GetChild(i).gameObject;
			PrepareAsServer(goChild, path+"/"+goChild.name);
		}
	}






}
