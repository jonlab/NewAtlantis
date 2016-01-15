using UnityEngine;
using System.Collections;

public class NAAudioSynthLooper : NAObjectBase 
{
    public AudioClip    audioClip;
	public float 		pos;
	public float		duration;

	// Use this for initialization
	void Start () 
	{
		
		//auto generate
		Randomize();
		Generate();
	}
	

	// Update is called once per frame
	void Update () 
	{
	}




	//put your logic inside this function
	public override void ExtendedControl()
	{
		base.ExtendedControl();
		//contrôles custom
		pos += Time.deltaTime*NAInput.GetAxis(NAControl.MoveHorizontal);
		duration += Time.deltaTime*NAInput.GetAxis(NAControl.MoveVertical);

		pos = Mathf.Clamp(pos, 0f, 1f);
		duration = Mathf.Clamp(duration, 0.0001f, 1f);
		if (NAInput.GetControlDown(NAControl.Jump))
		{
			Generate();
			Sync();
			LooperPlay();
		}
		if (NAInput.GetControlDown(NAControl.Menu))
		{
			Randomize();
			//new
			Generate(); 
			Sync();
			LooperPlay();
		}
	}

	public override void DrawSimpleGUI(Vector3 pos2d)
	{
		GUI.color = Color.white;
		string strDisplay = name;
		int x = (int)(pos2d.x*Screen.width);
		int y = (int)(Screen.height-pos2d.y*Screen.height);
		GUI.Box (new Rect(x,y,100,30), "Looper");

	}
		

	public override void DrawExtendedGUI(Vector3 pos2d)
	{
		GUI.color = Color.white;
		string strDisplay = name;
		int x = (int)(pos2d.x*Screen.width);
		int y = (int)(Screen.height-pos2d.y*Screen.height);
		GUI.Box (new Rect(x,y,200,100), "Looper");
		GUI.Label(new Rect(x,y+30,50,20), "pos");
		GUI.Label(new Rect(x,y+60,50,20), "duration");
		pos = GUI.HorizontalSlider(new Rect(x+50,y+30,150,20), pos, 0f, 1f);
		duration = GUI.HorizontalSlider(new Rect(x+50,y+60,150,20), duration, 0f, 1f);
		if (GUI.Button (new Rect(x,y+80,100,20), "Generate (X)"))
		{
			Generate();
			Sync();
		}
		if (GUI.Button (new Rect(x+100,y+80,100,20), "Randomize (O)"))
		{
			Randomize();
		}
		/*if (GUI.Button (new Rect(x+100,y+80,100,20), "Play"))
				{
					Play();
				}*/
	}
		

	private void Randomize()
	{
		pos = Random.value;
		duration = Random.value * 0.05f;
	}


	public void Generate ()
	{
		AudioSource audio = GetComponent<AudioSource>();
        if (audio && audioClip)
		{
			//float duration = inputAudioSource.clip.channels * inputAudioSource.clip.length;

            float source_duration 	= audioClip.length;
            int source_samplerate 	= audioClip.frequency;
            int source_channels 	= audioClip.channels;

			//get source data
            int source_samplecount	= audioClip.samples;
			float[] source_data 	= new float[source_samplecount];
            audioClip.GetData(source_data, 0);

			float extract_pos = pos * source_duration;
			float extract_duration = duration*source_duration;

			if (extract_duration == 0)
				extract_duration = 0.01f;
			//compute extract coordinates
			int start = (int)((extract_pos-extract_duration/2f)*(float)source_samplerate);
			start = Mathf.Max(0,start);

			int end = (int)((extract_pos+extract_duration/2f)*(float)source_samplerate);
			end = Mathf.Min(audioClip.samples-1,end);

			extract_duration = (float)(end-start)/(float)source_samplerate;
			Debug.Log("start=" + start + " end="+end);

			int samplecount = end-start;
			float[] data = new float[samplecount];

			//copy extract
			for (int i=0;i<samplecount;++i)
			{
				data[i] = 	source_data[i+start];
			}
				
			audio.clip = AudioClip.Create("NAAudioSynthLooper", samplecount, source_channels, source_samplerate, false);

			audio.clip.SetData(data, 0);
			audio.loop = true;


		}
	}

	void LooperPlay()
	{
		AudioSource audio = GetComponent<AudioSource>();

		if (audio != null)
		{
			audio.Play();

			NetworkSync ns = GetComponent<NetworkSync>();
			if (ns == null)
				ns = gameObject.GetComponentInParent<NetworkSync>();
			if (ns == null)
			{
				LogManager.LogError("Looper sync NetworkSync not found");
			}
			if (ns)
			{
				ns.SyncAudioSource();
			}
		}
	}





	void Sync()
	{
		NetworkView nv = GetComponent<NetworkView>();
		if (nv == null)
			nv = gameObject.GetComponentInParent<NetworkView>();
		if (nv != null)
		{
			nv.RPC("SetAudioSynthLooperState", RPCMode.OthersBuffered, pos, duration);
		}
		
	}


}
