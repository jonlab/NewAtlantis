using UnityEngine;
using System.Collections;

public class NAAudioSynthConvolutionLooper : NAObjectBase 
{
	public AudioSource 	target;
    public AudioClip    audioClip1;
    public AudioClip    audioClip2;
	public float 		pos1;
    public float        pos2;
	public float		duration1;
    public float        duration2;

	// Use this for initialization
	void Start () 
	{
		if (target == null)
			target = GetComponent<AudioSource>();
		//auto generate
		//Randomize();
		//Generate();
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
		/*pos += Time.deltaTime*NAInput.GetAxis(NAControl.MoveHorizontal);
		duration += Time.deltaTime*NAInput.GetAxis(NAControl.MoveVertical);

		pos = Mathf.Clamp(pos, 0f, 1f);
		duration = Mathf.Clamp(duration, 0.0001f, 1f);
		if (NAInput.GetControlDown(NAControl.Jump))
		{
			Generate();
		}
		if (NAInput.GetControlDown(NAControl.Menu))
		{
			Randomize();
		}
  */      
	}

	public override void DrawSimpleGUI(Vector3 pos2d)
	{
		GUI.color = Color.white;
		string strDisplay = name;
		int x = (int)(pos2d.x*Screen.width);
		int y = (int)(Screen.height-pos2d.y*Screen.height);
		GUI.Box (new Rect(x,y,100,30), "ConvolutionLooper");

	}


	public override void DrawExtendedGUI(Vector3 pos2d)
	{
        /*
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
		}
		if (GUI.Button (new Rect(x+100,y+80,100,20), "Randomize (O)"))
		{
			Randomize();
		}
  */      
		/*if (GUI.Button (new Rect(x+100,y+80,100,20), "Play"))
				{
					Play();
				}*/
	}
		

	private void Randomize()
	{
		pos1 = Random.value;
        pos2 = Random.value;
		duration1 = Random.value * 0.05f;
        duration2 = Random.value * 0.05f;
	}


	public void Generate ()
	{
		AudioSource audio = target;//GetComponent<AudioSource>();
        if (audio && audioClip1 && audioClip2)
		{
			pos1 = Mathf.Clamp(pos1, 0, 1);
			pos2 = Mathf.Clamp(pos2, 0, 1);
			float[] data1 = DSP.Extract(audioClip1, pos1, duration1);
			float[] data2 = DSP.Extract(audioClip2, pos2, duration2);
            //convolve
			//float[] convolved = DSP.Convolve(data1, data2);
			float[] convolved = DSP.Add(data1, data2);

			int source_samplerate = audioClip1.frequency;
			audio.clip = AudioClip.Create("NAAudioSynthConvolutionLooper", convolved.Length, 1, source_samplerate, false);
			audio.clip.SetData(convolved, 0);
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
