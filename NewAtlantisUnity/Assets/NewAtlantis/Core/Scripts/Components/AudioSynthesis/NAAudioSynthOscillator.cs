using UnityEngine;
using System.Collections;



public enum WaveForm
{
	Sin,
	Cos,
	Square,
	Triangle,
	Sawtooth
}


public class NAAudioSynthOscillator : NAObjectBase 
{
	public float 	duration 	= 10f;
	public float 	frequency	= 440;
	public WaveForm waveform 	= WaveForm.Sin;
	private int 	samplerate 	= 44100;
	//private bool 	bShowGUI	= true;

	// Use this for initialization
	void Awake () 
	{
		Generate();
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}


	void Generate()
	{
		AudioSource audio = GetComponent<AudioSource>();
		if (audio)
		{
			int samplecount = (int)(samplerate*duration);
			//if (audio.clip == null)
			audio.clip = AudioClip.Create("NAAudioSynthSin", samplecount, 1, samplerate, false);
			float[] data = new float[samplecount];
			audio.clip.GetData(data, 0);
			DSP.GenerateSilence(data);
			if (waveform == WaveForm.Cos)
			{
				DSP.GenerateCosinus(data, samplerate, frequency);
			}
			else if (waveform == WaveForm.Sin)
			{
				DSP.GenerateSinus(data, samplerate, frequency);
			}
			else if (waveform == WaveForm.Square)
			{
				DSP.GenerateSquare(data, samplerate, frequency);
			}
			//FIXME : manque certaines formes d'ondes
			audio.clip.SetData(data, 0);
			//audio.Play();
		}
		Debug.Log ("Generate Oscillator");

	}




	public override void DrawSimpleGUI(Vector3 pos2d)
	{
		GUI.color = Color.white;
		string strDisplay = name;
		int x = (int)(pos2d.x*Screen.width);
		int y = (int)(Screen.height-pos2d.y*Screen.height);
		GUI.Box (new Rect(x,y,100,30), "Oscillator");

	}
	public override void DrawExtendedGUI(Vector3 pos2d)
	{
		GUI.color = Color.white;
		string strDisplay = name;
		int x = (int)(pos2d.x*Screen.width);
		int y = (int)(Screen.height-pos2d.y*Screen.height);
		GUI.Box (new Rect(x,y,200,100), "Oscillator");

		string[] waveforms = {"sin", "cos", "squ", "tri", "saw"};
		frequency = GUIParamEdit(new Rect(x, y+20, 200, 20), "frequency", "Hz", frequency);
		duration = GUIParamEdit(new Rect(x, y+40, 200, 20), "duration", "s", duration);

		//frequency = Mathf.Floor(frequency);
		//duration = Mathf.Floor(frequency);
		waveform = (WaveForm)GUI.SelectionGrid(new Rect(x,y+60,200,20), (int)waveform, waveforms, waveforms.Length);

		if (GUI.Button (new Rect(x,y+80,100,20), "Generate (X)"))
		{
			Generate();
		}
		/*if (GUI.Button (new Rect(x+100,y+80,100,20), "Play (∆)"))
		{
			Play();
		}*/
		/*if (GUI.Button (new Rect(x+100,y+60,100,20), "Stop"))
				{
					Stop();
				}*/

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

		frequency += x1*dt*100f;
		duration += y1*dt;

		duration = Mathf.Clamp(duration, 0.1f, 10);


		if (buttonJump)
		{
			Generate();
			//Play();
		}

		float padx = NAInput.GetAxis(NAControl.PadHorizontal);
		float pady = NAInput.GetAxis(NAControl.PadVertical);

		if (NAInput.PadHorizontalPressed && padx > 0)
		{
			waveform = waveform++;
		}
		else if (NAInput.PadHorizontalPressed && padx < 0)
		{
			waveform = waveform--;
		}
	}
	/*
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
				GUI.Box (new Rect(x,y,200,100), "Oscillator");

				string[] waveforms = {"sin", "cos", "squ", "tri", "saw"};
				frequency = GUIParamEdit(new Rect(x, y+20, 200, 20), "frequency", "Hz", frequency);
				duration = GUIParamEdit(new Rect(x, y+40, 200, 20), "duration", "s", duration);
				waveform = (WaveForm)GUI.SelectionGrid(new Rect(x,y+60,200,20), (int)waveform, waveforms, waveforms.Length);

				if (GUI.Button (new Rect(x,y+80,100,20), "Generate"))
				{
					Generate();
				}
				if (GUI.Button (new Rect(x+100,y+80,100,20), "Play"))
				{
					Play();
				}

				
			}
		}
	}
	*/
	void Play()
	{
		if (GetComponent<AudioSource>().clip != null)
		{
			GetComponent<AudioSource>().Play ();
		}
	}
}
