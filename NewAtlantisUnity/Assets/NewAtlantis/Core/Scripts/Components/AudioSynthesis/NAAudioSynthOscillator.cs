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


public class NAAudioSynthOscillator : MonoBehaviour 
{
	public float 	duration 	= 10f;
	public float 	frequency	= 440;
	public WaveForm waveform 	= WaveForm.Sin;
	private int 	samplerate 	= 44100;
	private bool 	bShowGUI	= true;

	// Use this for initialization
	void Awake () 
	{
		//Generate();
		
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
			
			audio.clip.SetData(data, 0);
			audio.Play();
		}
		Debug.Log ("Generate Oscillator");

	}


	int GUIParamEdit(Rect rect, string caption, string unit, int value)
	{
		GUI.Label (new Rect(rect.x, rect.y, rect.width/3, rect.height), caption);
		string strVal = ""+value;
		strVal = GUI.TextField(new Rect(rect.x+rect.width/3, rect.y, rect.width/3, rect.height), strVal); 
		GUI.Label (new Rect(rect.x+2*rect.width/3, rect.y, rect.width/3, rect.height), unit);
		int.TryParse(strVal, out value);
		return value;
	}

	float GUIParamEdit(Rect rect, string caption, string unit, float value)
	{
		GUI.Label (new Rect(rect.x, rect.y, rect.width/3, rect.height), caption);
		string strVal = ""+value;
		strVal = GUI.TextField(new Rect(rect.x+rect.width/3, rect.y, rect.width/3, rect.height), strVal); 
		GUI.Label (new Rect(rect.x+2*rect.width/3, rect.y, rect.width/3, rect.height), unit);
		float.TryParse(strVal, out value);
		return value;
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
				/*if (GUI.Button (new Rect(x+100,y+60,100,20), "Stop"))
				{
					Stop();
				}*/
				
			}
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
