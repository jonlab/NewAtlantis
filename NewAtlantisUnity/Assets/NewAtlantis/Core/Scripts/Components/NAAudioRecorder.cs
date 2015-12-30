using UnityEngine;
using System.Collections;



public class NAAudioRecorder : NAObjectBase 
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


	public override void ExtendedControl()
	{
		float dt = Time.deltaTime;

		float x1 = NAInput.GetAxis(NAControl.MoveHorizontal);
		float y1 = NAInput.GetAxis(NAControl.MoveVertical);
		float x2 = NAInput.GetAxis(NAControl.ViewHorizontal);
		float y2 = NAInput.GetAxis(NAControl.ViewVertical);

		bool buttonAction 	= NAInput.GetControlDown(NAControl.Action); 
		bool buttonJump 	= NAInput.GetControlDown(NAControl.Jump); 
		bool buttonCamera 	= NAInput.GetControlDown(NAControl.Camera);
		bool buttonMenu 	= NAInput.GetControlDown(NAControl.Menu);

		if (buttonCamera)
		{
			Play();
		}
		if (buttonMenu)
		{
			Record();
		}
		if (buttonAction)
		{
			Stop();
		}

	}

	public override void DrawSimpleGUI(Vector3 pos2d)
	{
		GUI.color = Color.white;
		string strDisplay = name;
		int x = (int)(pos2d.x*Screen.width);
		int y = (int)(Screen.height-pos2d.y*Screen.height);
		GUI.Box (new Rect(x,y,100,30), "Trunk");

	}
	public override void DrawExtendedGUI(Vector3 pos2d)
	{
		GUI.color = Color.white;
		string strDisplay = name;
		int x = (int)(pos2d.x*Screen.width);
		int y = (int)(Screen.height-pos2d.y*Screen.height);
		GUI.Box (new Rect(x,y,320,60), "Trunk");
		GUI.color = GetComponent<AudioSource>().isPlaying ? Color.green : Color.white;
		if (GUI.Button (new Rect(x,y+30,80,30), "play (∆)"))
		{
			Play();
		}
		GUI.color = Color.white;
		if (GUI.Button (new Rect(x+80,y+30,80,30), "stop (□)"))
		{
			Stop();
		}
		GUI.color = Microphone.IsRecording("") ? Color.red : Color.white;
		if (GUI.Button (new Rect(x+160,y+30,80,30), "rec (o)"))
		{
			Record();
		}

		GUI.color = GetComponent<AudioSource>().loop ? Color.red : Color.white;
		if (GUI.Button (new Rect(x+240,y+30,80,30), "loop"))
		{
			GetComponent<AudioSource>().loop = !GetComponent<AudioSource>().loop;
		}

		/*
		if (GUI.Button (new Rect(x+200,y+30,50,30), "sync"))
		{
			SendAudioDataToServer();
		}
		*/

	}





}
