using UnityEngine;
using System.Collections;

public class NAAudioSynthGranulate : NAObjectBase 
{
	//public bool 		bShowGUI = true;
	public AudioSource 	inputAudioSource;
	public float 		pos;
	public float		duration;


	// Use this for initialization
	void Start () 
	{
	}
	

	// Update is called once per frame
	void Update () 
	{
	}




	//put your logic inside this function
	public override void ExtendedControl()
	{
		//contrôles custom
		pos += Time.deltaTime*NAInput.GetAxis(NAControl.MoveHorizontal);
		duration += Time.deltaTime*NAInput.GetAxis(NAControl.MoveVertical);

		pos = Mathf.Clamp(pos, 0f, 1f);
		duration = Mathf.Clamp(duration, 0.0001f, 1f);
		if (NAInput.GetControlDown(NAControl.Jump))
		{
			Generate();
		}
		if (NAInput.GetControlDown(NAControl.Camera))
		{
			Randomize();
		}
	}

	public override void DrawSimpleGUI(Vector3 pos2d)
	{
		GUI.color = Color.white;
		string strDisplay = name;
		int x = (int)(pos2d.x*Screen.width);
		int y = (int)(Screen.height-pos2d.y*Screen.height);
		GUI.Box (new Rect(x,y,100,30), "Granulator");

	}


	public override void DrawExtendedGUI(Vector3 pos2d)
	{
		GUI.color = Color.white;
		string strDisplay = name;
		int x = (int)(pos2d.x*Screen.width);
		int y = (int)(Screen.height-pos2d.y*Screen.height);
		GUI.Box (new Rect(x,y,200,100), "Granulator");
		GUI.Label(new Rect(x,y+30,50,20), "pos");
		GUI.Label(new Rect(x,y+60,50,20), "duration");
		pos = GUI.HorizontalSlider(new Rect(x+50,y+30,150,20), pos, 0f, 1f);
		duration = GUI.HorizontalSlider(new Rect(x+50,y+60,150,20), duration, 0f, 1f);
		if (GUI.Button (new Rect(x,y+80,100,20), "Generate (X)"))
		{
			Generate();
		}
		if (GUI.Button (new Rect(x+100,y+80,100,20), "Randomize (∆)"))
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


	private void Generate ()
	{
		AudioSource audio = GetComponent<AudioSource>();
		if (audio)
		{
			//float duration = inputAudioSource.clip.channels * inputAudioSource.clip.length;

			float source_duration 	= inputAudioSource.clip.length;
			int source_samplerate 	= inputAudioSource.clip.frequency;
			int source_channels 	= inputAudioSource.clip.channels;

			//get source data
			int source_samplecount	= inputAudioSource.clip.samples;
			float[] source_data 	= new float[source_samplecount];
			inputAudioSource.clip.GetData(source_data, 0);

			float extract_pos = pos * source_duration;
			float extract_duration = duration*source_duration;

			if (extract_duration == 0)
				extract_duration = 0.01f;
			//compute extract coordinates
			int start = (int)((extract_pos-extract_duration/2f)*(float)source_samplerate);
			start = Mathf.Max(0,start);

			int end = (int)((extract_pos+extract_duration/2f)*(float)source_samplerate);
			end = Mathf.Min(inputAudioSource.clip.samples-1,end);

			extract_duration = (float)(end-start)/(float)source_samplerate;
			Debug.Log("start=" + start + " end="+end);

			int samplecount = end-start;
			float[] data = new float[samplecount];

			//copy extract
			for (int i=0;i<samplecount;++i)
			{
				data[i] = 	source_data[i+start];
			}
				
			audio.clip = AudioClip.Create("NAAudioSynthGranulate", samplecount, source_channels, source_samplerate, false);

			audio.clip.SetData(data, 0);
			audio.loop = true;
			audio.Play();
		}
	}

	void Play()
	{
		if (GetComponent<AudioSource>().clip != null)
		{
			GetComponent<AudioSource>().Play ();
		}
	}


}
