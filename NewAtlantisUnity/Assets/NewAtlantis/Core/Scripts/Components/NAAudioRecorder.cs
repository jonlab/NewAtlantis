using UnityEngine;
using System.Collections;
using System.IO;


public class NAAudioRecorder : NAObjectBase 
{
	AudioClip record 			= null;
	public int SampleRate 		= 22050;
	public int Duration 		= 10;
	private bool bShowGUI		= true;
	private  FileInfo[] 		info = null;
	private static int 			index = 0;
	public bool 				AutoLoad = false;
	public bool 				AutoPlay = false;
	public string 				directory = "SoundFiles";
	// Use this for initialization
	void Start () 
	{
		if (GetComponent<AudioSource>() == null)
		{
			gameObject.AddComponent<AudioSource>();
		}
		RefreshSoundfiles();

		if (AutoLoad)
		{
			LoadSoundFileAtIndex(index);
			GetComponent<AudioSource>().loop = true;
			index++;
		}

		if (AutoPlay)
		{
			GetComponent<AudioSource> ().Play ();
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

			UpdateWaveform();
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


	void UpdateWaveform()
	{
		AudioSource source = GetComponent<AudioSource>();
		Texture2D texWaveform = AudioUtils.ComputeWaveForm(source.clip, 512,256);
		Renderer renderer = gameObject.GetComponent<Renderer>();
		if (renderer != null)
		{
			renderer.material.mainTexture = texWaveform;
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
		GUI.color = Color.white;
		if (GUI.Button (new Rect(x,y+60,60,20), "rand"))
		{
			float[] src = GetAudioData();
			DSP.AddNoise(src, 0.001f);
			UpdateAudioData(src);
		}

		if (GUI.Button (new Rect(x+60,y+60,60,20), "rev"))
		{
			float[] src = GetAudioData();
			DSP.Reverse(src);
			UpdateAudioData(src);
		}

		if (GUI.Button (new Rect(x+120,y+60,60,20), "lp"))
		{
			float[] src = GetAudioData();
			DSP.PoorManFIR(src);
			UpdateAudioData(src);
		}


		if (GUI.Button (new Rect(x+180,y+60,60,20), "load"))
		{
			string strFile = "boulez_end.wav";
			//string strFile = "synths/beats_at_110_bpm_matching/(110) Declare War.wav";
			byte[] data = System.IO.File.ReadAllBytes(directory+"/"+strFile);
			AudioSource audio = GetComponent<AudioSource>();
			audio.clip = WavUtility.ToAudioClip(data, 0, "wav");
			UpdateWaveform();
			SendAudioDataToServer();
		}

		if (info != null)
		{
			foreach (FileInfo f in info) 
			{
				GUILayout.BeginHorizontal();
				if (GUILayout.Button(f.Name, GUILayout.Width(200)))
				{
					string strFile = f.Name;
					byte[] data = System.IO.File.ReadAllBytes(directory+"/"+strFile);
					AudioSource audio = GetComponent<AudioSource>();
					audio.clip = WavUtility.ToAudioClip(data, 0, "wav");
					UpdateWaveform();
					SendAudioDataToServer();
				}
				GUILayout.EndHorizontal();
			}
		}

		/*
		if (GUI.Button (new Rect(x+200,y+30,50,30), "sync"))
		{
			SendAudioDataToServer();
		}
		*/

	}

	void LoadSoundFileAtIndex(int i)
	{
		i = i%info.Length;
		FileInfo f = info[i];
		string strFile = f.Name;
		byte[] data = System.IO.File.ReadAllBytes(directory+"/"+strFile);
		AudioSource audio = GetComponent<AudioSource>();
		audio.clip = WavUtility.ToAudioClip(data, 0, "wav");
		UpdateWaveform();
		SendAudioDataToServer();
	}


	float[] GetAudioData()
	{
		AudioSource audio = GetComponent<AudioSource>();
		float[] src = new float[audio.clip.samples];
		audio.clip.GetData(src, 0);
		return src;
	}

	void UpdateAudioData(float[] data)
	{
		AudioSource audio = GetComponent<AudioSource>();
		audio.clip.SetData(data, 0);
		UpdateWaveform();
		SendAudioDataToServer();
	}

	public  void RefreshSoundfiles()
	{
		try
		{
			#if UNITY_WEBPLAYER
			#else
			DirectoryInfo dir = new DirectoryInfo(directory);
			info = dir.GetFiles("*.*");
			Debug.Log("info = " + info.Length);
			#endif
		}
		catch (System.Exception e)
		{

		}
	}



}
