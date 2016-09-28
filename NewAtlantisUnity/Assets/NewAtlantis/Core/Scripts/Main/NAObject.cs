using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;


//New Atlantis shared "Object"
public class NAObject 
{
	public string 	name 		= "";
	public Vector3 	position 	= Vector3.zero;
	public Vector3 	angles 		= Vector3.zero;
	public Vector3 	scale 		= Vector3.one;
	public string 	file 		= "";
	public GameObject go 		= null;
	public GameObject goGizmo 	= null;
	public string 	id 			= "";
	public int 		downloaded 	= 0;
	private string 	url 		= "";
	private NetworkViewID NetworkViewId;
	public bool downloading 	= false;
	public string download_id 	= "";
	public AudioSource[] sources = null;

	//private static Dictionary<string, AssetBundle> AssetBundles = new Dictionary<string, AssetBundle>();

	public NAObject(GameObject _root, string _name, Vector3 _position, Vector3 _angles, Vector3 _scale, string _file, NetworkViewID _NetworkViewId)
	{
		Debug.Log ("Creating NAObject " + _name);
		name 				= _name;
		position 			= _position;
		angles 				= _angles;
		scale 				= _scale;
		file 				= _file;
		NetworkViewId 		= _NetworkViewId;

		//create and sync the Main GameObject
		go 							= new GameObject(_name+_NetworkViewId);
		go.transform.parent			= _root.transform;
		go.transform.position 		= _position;
		go.transform.eulerAngles 	= _angles;
		//go.transform.localScale 	= _scale;

		NetworkView nView 			= go.AddComponent<NetworkView>();
		nView.viewID 				= this.NetworkViewId;

		NetworkSync nSync 			= go.AddComponent<NetworkSync>();

		//if (name.Contains("Coffee") || name.Contains("Radio") || name.Contains("Object"))
		if (name.StartsWith("X"))
		{
			LogManager.Log("add " + name + " to the instanciables.");
			NA.instanciables.Add(this);
		}
	}

	public void Download()
	{
		url = "http://tanant.info/newatlantis2/objects/"+file;
		Debug.Log ("try to download object from " + url);
		download_id = NADownloader.Download(url, name);
		downloading = true;
	}
	public void DownloadLocal()
	{
		//www = new WWW ("file://" + file);
		//DEPRECATED
	}



	public void Process()
	{
		if (downloading)
		{
			AssetBundle bundle = NADownloader.GetAssetBundle(download_id);
			if (bundle != null) //downloaded
			{
				downloading = false;
				GameObject goChild = null;
				bool bInstantiateObjects = true;
				if (bInstantiateObjects)
				{
					Debug.Log ("instantiate object " + name);
					Object[] objs = bundle.LoadAllAssets();
					Debug.Log ("Asset Bundle Objects count = " + objs.Length);
					string[] strAssets = bundle.GetAllAssetNames();

					/*
					foreach (string s in strAssets)
					{
						Debug.Log ("Asset = " + s);
					}
					*/
					/*
					foreach (Object o in objs)
					{
						Debug.Log ("Object " + o.name + " type:" + o.GetType());
					}
					*/
					if (bundle.mainAsset == null) //no main Asset in the bundle
					{
						Debug.Log("no mainAsset in bundle, manually instantiating objects");
						foreach (Object o in objs)
						{
							if (o != null)
							{
								goChild = GameObject.Instantiate(o) as GameObject;
								if (goChild != null)
								{
									goChild.name = o.name;
									goChild.transform.parent 			= go.transform;
									goChild.transform.localPosition 	= Vector3.zero;
									goChild.transform.localEulerAngles 	= Vector3.zero;
									//goChild.transform.localScale 		= Vector3.one;
								}
							}
						}
					}
					else
					{
						Debug.Log("instantiating mainAsset in bundle");
						goChild = GameObject.Instantiate(bundle.mainAsset) as GameObject;
						goChild.name = bundle.mainAsset.name + id;
						goChild.transform.parent = go.transform;
						goChild.transform.localPosition = Vector3.zero;
						goChild.transform.localEulerAngles = Vector3.zero;
						//goChild.transform.localScale = Vector3.one;
	                }
					//NA.PatchMaterials(go); //crashes on 5.3.1
					//if (!name.Contains("elevation"))
					//{
					NA.PatchAllMaterials(goChild); //go
					//}

					NAObjectLabel lbl = go.GetComponentInChildren<NAObjectLabel>();
					if (lbl)
					{
						/*Renderer lblr = lbl.gameObject.GetComponent<Renderer>();
						if (lblr != null)
						{
							lblr.enabled = false;
						}
						*/
						//fix du collider
						Collider col = lbl.gameObject.GetComponent<Collider>();
						if (col != null)
						{
							col.isTrigger = true;
						}

					}	

					Light[] 		lights = go.GetComponentsInChildren<Light>();
					Transform[]	 	gameobjects = go.GetComponentsInChildren<Transform>();
					AudioListener[] listeners = go.GetComponentsInChildren<AudioListener>();

					//exceptions
					/*if (name.Contains("maki") || name.Contains("SYN"))
					{
						//unactivate lights
						foreach (Light l in lights)
						{
							l.enabled = false;
							
						}

						
					}
					*/


					//we have to scale after instantiation
					go.transform.localScale 	= scale;
					bundle.Unload(false); 
					go.AddComponent<NAViewerOptim>();
		            
					sources = go.GetComponentsInChildren<AudioSource>();
					//audio sources have to be augmented with specific NA behaviour
					foreach (AudioSource s in sources)
					{
						//NA.DecorateAudioSource(s);
						s.spatialBlend = 1f;
						//s.rolloffMode = AudioRolloffMode.Linear;

						if (s.name == "LightBeam")
						{
							s.rolloffMode = AudioRolloffMode.Custom;
							s.maxDistance = 100f;
						}
					}

					foreach (AudioListener al in listeners)
					{
						al.enabled = false;
					}
					LogManager.LogWarning (name + " - objects:" + gameobjects.Length + " sources:" + sources.Length + " lights:"+ lights.Length);
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

		            
					//jonathan : removed on 1/9/2016


					NetworkSync nSync 		= go.GetComponent<NetworkSync>();

					List<GameObject> listSync = new List<GameObject>();
					if (NA.syncMode == SyncMode.RigibodiesAndAudioSources)
					{
						/*Rigidbody[] rbs = go.GetComponentsInChildren<Rigidbody>();
						foreach (Rigidbody rb in rbs)
						{
							listSync.Add(rb.gameObject);
						}
						AudioSource[] audios = go.GetComponentsInChildren<AudioSource>();
						foreach (AudioSource audio in audios)
						{
							listSync.Add(audio.gameObject);
						}
						*/

						Collider[] colliders = go.GetComponentsInChildren<Collider>();
						foreach (Collider collider in colliders)
						{
							listSync.Add(collider.gameObject);
						}

						if (NA.isServer())
						{
							foreach (GameObject o in listSync)
							{
								string path = NA.GetGameObjectPath(o.transform);
								NetworkViewID id = Network.AllocateViewID();
								//LogManager.Log("PrepareAsServer " + path + " id=" + id);
								go.GetComponent<NetworkView>().RPC("AttachNetworkView", RPCMode.AllBuffered, path, id);

							}
						}
						else if (NA.isClient())
						{
							nSync.AttachNetworkViews();
						}




					}




					/*

					nSync.Prepare(); //client and server (for now, just stops all AudioSources)
					if (Network.isServer)
					{
						if (goChild != null)
						{
							PrepareAsServer(goChild, goChild.name);
						}
					}
					else
					{
						nSync.AttachNetworkViews();
					}

					*/

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


	/*
	 * Prepare this Object as Server
	 * We call the AttachNetworkView on the clients so that the same NetworkViews are created in sync 
	 * on the clients :
	 * The server allocates the Network Ids and send them to the clients with the path to object
	 * 
	 */
	public void PrepareAsServer(GameObject root, string path)
	{
		//Debug.Log ("PrepareAsServer " + path);
		//NetworkView nView 		= root.AddComponent<NetworkView>();
		//nView.viewID 			= Network.AllocateViewID();
		NetworkViewID id = Network.AllocateViewID();
		//LogManager.Log("PrepareAsServer " + path + " id=" + id);
		go.GetComponent<NetworkView>().RPC("AttachNetworkView", RPCMode.AllBuffered, path, id);
		for (int i=0;i<root.transform.childCount;++i)
		{
			GameObject goChild = root.transform.GetChild(i).gameObject;
			PrepareAsServer(goChild, path+"/"+goChild.name);
		}
	}

	public int GetAudioSourcesCount()
	{
		if (sources != null)
		{
			return sources.Length;
		}
		else
		{
			return -1;
		}
	}





}
