using UnityEngine;
using System.Collections;



public class NAAudioRecorder : MonoBehaviour 
{
	AudioClip record 			= null;
	public int SampleRate 		= 22050;
	public int Duration 		= 10;
	private bool bShowGUI		= true;

	// Use this for initialization
	void Start () 
	{
		if (GetComponent<AudioSource>() == null)
		{
			gameObject.AddComponent<AudioSource>();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void Record()
	{
		record = Microphone.Start("", false, Duration, SampleRate);
		GetComponent<AudioSource>().clip = record;
	}

	void Stop()
	{
		if (Microphone.IsRecording(""))
		{
			Microphone.End(null);
			SendAudioDataToServer();
		}
		else
		{
			if (GetComponent<AudioSource>().clip != null)
			{
				GetComponent<AudioSource>().Stop ();
			}
		}
	}

	void SendAudioDataToServer()
	{
		//AudioSource audio = GetComponent<AudioSource>();
		//float[] data = new float[audio.clip.samples];
		//audio.clip.GetData(data, 0);
		//Debug.Log ("send " + data.Length + " samples of audio data");
		NetworkSync ns = GetComponent<NetworkSync>();
		if (ns)
		{
			ns.ServerSyncAudio();
		}
	}
	void Play()
	{
		if (Microphone.IsRecording(""))
		{
			Microphone.End(null);
			SendAudioDataToServer();
		}
		if (GetComponent<AudioSource>().clip != null)
		{
			GetComponent<AudioSource>().Play ();
		}
	}


	void OnGUI()
	{
		if (bShowGUI)
		{
			Vector3 pos2d = Camera.main.WorldToViewportPoint(transform.position);
			if (pos2d.z > 0)
			{
				GUI.color = Color.white;
				string strDisplay = name;
				int x = (int)(pos2d.x*Screen.width);
				int y = (int)(Screen.height-pos2d.y*Screen.height);
				GUI.Box (new Rect(x,y,250,60), "AudioTrunk");
				GUI.color = GetComponent<AudioSource>().isPlaying ? Color.green : Color.white;
				if (GUI.Button (new Rect(x,y+30,50,30), "play"))
				{
					Play();
				}
				GUI.color = Color.white;
				if (GUI.Button (new Rect(x+50,y+30,50,30), "stop"))
				{
					Stop();
				}
				GUI.color = Microphone.IsRecording("") ? Color.red : Color.white;
				if (GUI.Button (new Rect(x+100,y+30,50,30), "rec"))
				{
					Record();
				}

				GUI.color = GetComponent<AudioSource>().loop ? Color.red : Color.white;
				if (GUI.Button (new Rect(x+150,y+30,50,30), "loop"))
				{
					GetComponent<AudioSource>().loop = !GetComponent<AudioSource>().loop;
				}

				if (GUI.Button (new Rect(x+200,y+30,50,30), "sync"))
				{
					SendAudioDataToServer();
                }

			}
		}
	}






}
