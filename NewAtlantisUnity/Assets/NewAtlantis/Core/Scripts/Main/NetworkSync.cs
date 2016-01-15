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
		LogManager.Log("received AttachNetworkView " + path + " id=" + viewID);
		//FIXME : when duplicate objects (duplicate path), return the first one
		GameObject goChild = GameObject.Find(path);
		if (goChild)
		{
			NetworkView nView 		= goChild.AddComponent<NetworkView>();
			nView.viewID 			= viewID;
			nView.stateSynchronization = NetworkStateSynchronization.Unreliable;
			goChild.AddComponent<NetworkSync>();

		}
		else
		{
			LogManager.LogWarning("Can't find GameObject " + path);
			//Debug.Log("Can't find GameObject " + path);
			//postpone attachment (client is not ready)
			try
			{
				networkViews.Add (path, viewID);
			}
			catch (System.Exception e)
			{
				LogManager.LogWarning("Exception : network view on object " + path + " ID=" + viewID);
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
				LogManager.Log("NetworkView attached at " + path + " with id " + viewID);
				NetworkView nView 		= goChild.AddComponent<NetworkView>();
				nView.viewID 			= viewID;
				nView.stateSynchronization = NetworkStateSynchronization.Unreliable;
				goChild.AddComponent<NetworkSync>();
			}
			else
			{
				LogManager.LogError("Can't find GameObject after postponing : " + path);
			}
        }
    }


	[RPC]
	void SetAudioSynthLooperState(float pos, float duration)
	{
		LogManager.Log("received AudioSynthLooper state " + pos + " " + duration);
		NAAudioSynthLooper l = GetComponent<NAAudioSynthLooper>();
		if (l == null)
		{
			l = GetComponentInChildren<NAAudioSynthLooper>();
		}
		if (l != null)
		{
			l.pos = pos;
			l.duration = duration;
			l.Generate();
		}
	}


	public void SyncAudioSource()
	{
		LogManager.Log("SyncAudioSource");
		AudioSource audio = GetComponent<AudioSource>();
		if (audio == null)
			audio = GetComponentInChildren<AudioSource>(); //take children if nothing on the root

		if (audio == null)
			LogManager.LogError("Error sync AudioSource, no source found !");
		GetComponent<NetworkView>().RPC("SetAudioSourceState", RPCMode.OthersBuffered, audio.isPlaying, audio.volume, audio.pitch, audio.loop, audio.time);
	}


	[RPC]
	void SetAudioSourceState(bool playing, float volume, float pitch, bool loop, float time) 
	{
		LogManager.Log("received AudioSource state " + playing + " " + volume + " "  + pitch + " " + loop + " " + time);
		AudioSource audio = GetComponent<AudioSource>();

		if (audio == null)
			audio = GetComponentInChildren<AudioSource>();
		if (audio)
		{
			if (playing && !audio.isPlaying)
			{
				audio.Play();
			}
			else if (!playing && audio.isPlaying)
			{
				audio.Stop();
			}
			audio.volume = volume;
			audio.pitch = pitch;
			audio.loop = loop;
			audio.time = time;
		}
	}

	public void ServerSyncAudio()
	{
		//generate float data and send it with an RPC to the others
		AudioSource audio = GetComponent<AudioSource>();
		if (audio)
		{
			//ADPCM
			int samples = audio.clip.samples*audio.clip.channels;
			LogManager.Log ("raw audio samples sent = " + samples);
			byte[] data = CodecIMAADPCM.GetADPCMWAVData(audio.clip, samples);
			GetComponent<NetworkView>().RPC("SendAudioBufferADPCM", RPCMode.OthersBuffered, data);
		}

	}


	[RPC]
	void SendAudioBuffer(byte[] ba, int channels)
	{
		Debug.Log ("RPC SyncAudio");
		AudioSource audio = GetComponent<AudioSource>();
		if (audio)
		{
			float[] flar = ToFloatArray (ba);
			audio.clip = AudioClip.Create ("sync", flar.Length, channels, 22050, true, false);
			audio.clip.SetData (flar, 0);
		}
	}

	[RPC]
	void SendAudioBufferADPCM(byte[] adpcm)
	{
		AudioSource audio = GetComponent<AudioSource>();
		if (audio)
		{
			audio.clip = CodecIMAADPCM.SetADPCMData(adpcm);
			int samples = audio.clip.samples*audio.clip.channels;
			LogManager.Log ("raw audio samples received = " + samples);
		
		}
	}


	
	public byte[] ToByteArray(float[] floatArray) 
	{
		int len = floatArray.Length * 4;
		byte[] byteArray = new byte[len];
		int pos = 0;
		foreach (float f in floatArray) 
		{
			byte[] data = System.BitConverter.GetBytes(f);
			System.Array.Copy(data, 0, byteArray, pos, 4);
			pos += 4;
		}
		return byteArray;
	}


	public float[] ToFloatArray(byte[] byteArray) 
	{
		int len = byteArray.Length / 4;
		float[] floatArray = new float[len];
		for (int i = 0; i < byteArray.Length; i+=4) 
		{
			floatArray[i/4] = System.BitConverter.ToSingle(byteArray, i);
		}
		return floatArray;
	}
}
