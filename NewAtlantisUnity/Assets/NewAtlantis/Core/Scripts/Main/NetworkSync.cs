using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 
 * 
 * 
 * 
 * 
 * */


public class NetworkSync : MonoBehaviour 
{

	AudioSource[] sources = null;
	NetworkView nv = null;
	Dictionary<string,NetworkViewID>  networkViews = new Dictionary<string, NetworkViewID>();



	//Audio Source state
	AudioSource source;

	bool audio_playing 		= false;
	bool audio_loop 		= false;
	float audio_panStereo 	= 0f;
	float audio_pitch 		= 0f;
	float audio_volume 		= 0f;


	// Use this for initialization
	void Start () 
	{
		nv = GetComponent<NetworkView>();
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			Prepare();
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			GetComponent<NetworkView>().RPC("sync", RPCMode.AllBuffered);
		}
	}

	void LateUpdate()
	{
		//let's do the sync

		if (Network.isServer)
		{
			if (sources != null)
			{
				foreach (AudioSource s in sources)
                {
					if (s.isPlaying != audio_playing)
					{
						audio_playing = s.isPlaying;
						GetComponent<NetworkView>().RPC("NetworkPlay", RPCMode.OthersBuffered, audio_playing);
					}

					if (s.loop != audio_loop)
					{
						audio_loop = s.loop;
						GetComponent<NetworkView>().RPC("NetworkSetAudioSourceLoop", RPCMode.OthersBuffered, audio_loop);
					}

					if (s.panStereo != audio_panStereo)
					{
						audio_panStereo = s.panStereo;
						GetComponent<NetworkView>().RPC("NetworkSetAudioSourcePanStereo", RPCMode.OthersBuffered, audio_panStereo);
					}

					if (s.volume != audio_volume)
					{
						audio_volume = s.volume;
						GetComponent<NetworkView>().RPC("NetworkSetAudioSourceVolume", RPCMode.OthersBuffered, audio_volume);
					}

					if (s.pitch != audio_pitch)
					{
						audio_pitch = s.pitch;
						GetComponent<NetworkView>().RPC("NetworkSetAudioSourcePitch", RPCMode.OthersBuffered, audio_pitch);
					}
				}
			}

		}

	}

	void PrepareAsClient()
	{

	}

	public void Prepare()
	{
		sources = gameObject.GetComponentsInChildren<AudioSource>();
		if (sources != null)
		{
			Debug.Log ("found " + sources.Length + " AudioSources in " + gameObject.name);
		}
		else
		{
			Debug.Log ("found 0 AudioSources in " + gameObject.name);
		}

		//stop all Audio sources : NO
		foreach (AudioSource s in sources)
        {

			//s.Stop(); 
			if (Network.isServer)
			{
				//FIXME : handle correctly multiple sources
				audio_playing = s.isPlaying;
			}
        }


	}

	[RPC]
	void sync() 
	{
		if (sources != null)
		{
			foreach (AudioSource s in sources)
			{
				//string log = "Source name = "  + s.name + " instance ID = " + s.GetInstanceID();
				//LogManager.Log(log);
			}
		}

	}

	[RPC]
	void NetworkPlay(bool val) 
	{
		if (sources != null)
		{
			foreach (AudioSource s in sources)
			{
				//TO DO : gérer les sources indépendemment
				//string log = "Source name = "  + s.name + " instance ID = " + s.GetInstanceID();
                //LogManager.Log(log);
				if (val)
					s.Play();
				else
					s.Stop();
            }
        }
    }

	[RPC]
	void NetworkSetAudioSourceLoop(bool val) 
	{
		if (sources != null)
		{
			foreach (AudioSource s in sources)
			{
				//TO DO : gérer les sources indépendemment
				s.loop = val;
			}
		}
	}

	[RPC]
	void NetworkSetAudioSourcePitch(float val) 
	{
		if (sources != null)
		{
			foreach (AudioSource s in sources)
			{
				//TO DO : gérer les sources indépendemment
				s.pitch = val;
			}
		}
	}

	[RPC]
	void NetworkSetAudioSourceVolume(float val) 
	{
		if (sources != null)
		{
			foreach (AudioSource s in sources)
			{
				//TO DO : gérer les sources indépendemment
				s.volume = val;
			}
		}
	}

	[RPC]
	void NetworkSetAudioSourcePanStereo(float val) 
	{
		if (sources != null)
		{
			foreach (AudioSource s in sources)
			{
				//TO DO : gérer les sources indépendemment
				s.panStereo = val;
			}
		}
	}


	[RPC]
	void AttachNetworkView(string path, NetworkViewID viewID)
	{
		//FIXME : when duplicate objects (duplicate path), return the first one
		GameObject goChild = GameObject.Find(path);
		if (goChild)
		{
			NetworkView nView 		= goChild.AddComponent<NetworkView>();
			nView.viewID 			= viewID;
		}
		else
		{
			//Debug.Log("Can't find GameObject " + path);
			//postpone attachment (client is not ready)
			try
			{
				networkViews.Add (path, viewID);
			}
			catch (System.Exception e)
			{
				Debug.LogError("Exception : network view on object " + path + " ID=" + viewID);
			}
		}
	}

	public void AttachNetworkViews()
	{

		foreach (string path in networkViews.Keys)
		{
			NetworkViewID viewID = networkViews[path];
			GameObject goChild = GameObject.Find(path);
			if (goChild)
            {
				NetworkView nView 		= goChild.AddComponent<NetworkView>();
				nView.viewID 			= viewID;

			}
			else
			{

				Debug.Log("Can't find GameObject after postponing : " + path);
			}

        }
        
    }
}
