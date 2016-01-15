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

	void RecorderStop()
	{
		if (Microphone.IsRecording(""))
		{
            int spos = Microphone.GetPosition("");
            float pos = (float)spos / (float)SampleRate;
            LogManager.Log("trunk stopped at " + pos + " s");
            Microphone.End(null);

            AudioClip clip = AudioClip.Create("TrunkClamped", spos, 1, SampleRate, false);
            float[] src = new float[record.samples];
            record.GetData(src, 0);

            float[] dst = new float[spos];

            for (int i=0;i<spos;++i)
            {
                dst[i] = src[i];
            }
            clip.SetData(dst, 0);

            GetComponent<AudioSource>().clip = clip;

			

            //clamp


			SendAudioDataToServer();



		}
		else
		{
			if (GetComponent<AudioSource>().clip != null)
			{
				GetComponent<AudioSource>().Stop ();
			}
		}

		NetworkSync ns = GetComponent<NetworkSync>();
		if (ns)
		{
			ns.SyncAudioSource();
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
	void RecorderPlay()
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

		NetworkSync ns = GetComponent<NetworkSync>();
		if (ns)
		{
			ns.SyncAudioSource();
		}
	}


	public override void ExtendedControl()
	{
		base.ExtendedControl();
		float dt = Time.deltaTime;

		float x1 = NAInput.GetAxis(NAControl.MoveHorizontal);
		float y1 = NAInput.GetAxis(NAControl.MoveVertical);
		float x2 = NAInput.GetAxis(NAControl.ViewHorizontal);
		float y2 = NAInput.GetAxis(NAControl.ViewVertical);

		bool buttonAction 	= NAInput.GetControlDown(NAControl.Action); 
		bool buttonJump 	= NAInput.GetControlDown(NAControl.Jump); 
		bool buttonCamera 	= NAInput.GetControlDown(NAControl.Camera);
		bool buttonMenu 	= NAInput.GetControlDown(NAControl.Menu);

		AudioSource audio = GetComponent<AudioSource>();

		if (buttonCamera)
		{
			RecorderPlay();
		}
		if (buttonMenu)
		{
			Record();
		}
		if (buttonAction)
		{
			RecorderStop();
		}

		if (buttonJump)
		{
			audio.loop = !audio.loop;
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
		GUI.Box (new Rect(x,y,240,60), "Trunk");
		GUI.color = GetComponent<AudioSource>().isPlaying ? Color.green : Color.white;
		if (GUI.Button (new Rect(x,y+30,60,20), "play (∆)"))
		{
			RecorderPlay();
		}
		GUI.color = Color.white;
		if (GUI.Button (new Rect(x+60,y+30,60,20), "stop (□)"))
		{
			RecorderStop();
		}
		GUI.color = Microphone.IsRecording("") ? Color.red : Color.white;
		if (GUI.Button (new Rect(x+120,y+30,60,20), "rec (o)"))
		{
			Record();
		}

		GUI.color = GetComponent<AudioSource>().loop ? Color.red : Color.white;
		if (GUI.Button (new Rect(x+180,y+30,60,20), "loop (x)"))
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
