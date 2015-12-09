using UnityEngine;
using System.Collections;

public class NAReflectionsFX : MonoBehaviour 
{

	private const int tap_count = 100;
	public float delaytime = 1f;
	float[] buffer = new float[200000];
	int readposition = 0;
	int writeposition = 0;
	
	public float feedback = 0.5f;
	public float[] reflections = new float[tap_count];

	private int framecount = 0;
	//public int tap1 = 20000;

	//texture de préview de la IR
	Texture2D 		tex;
	Color[] 		colors;
	int width 		= 256;
	int height 		= 64;


	// Use this for initialization
	void Start () 
	{
		for (int i=0; i<200000; ++i)
			buffer [i] = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		framecount++;
		if (framecount % 20 == 0)
		{
			UpdateTexture();
		}
		float distanceToListener =(NA.listener.transform.position - transform.position).magnitude;
		if (distanceToListener > 10)
		{
		//	return;
		}
		//compute reflections 

		SphereDistribution d = new SphereDistribution ();
		d.CreateSpiral (tap_count);
		for (int i=0;i<tap_count;++i)
		{
			Ray ray = new Ray(gameObject.transform.position, d.positions[i]);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				//we hit something, we add a tap to the delay
				float distance = (hit.point-gameObject.transform.position).magnitude;
				float wall2listener = (hit.point-NA.listener.transform.position).magnitude;
				float time = (distance+wall2listener)/340f*1f;
				if (time < 2f)
				{
					reflections[i] = time;
				}
			}
			else
			{
				//nothing here
				reflections[i] = 0;
			}
		}

		//tap1 = (int)(delaytime * 44100f);
	}
	
	void OnGUI()
	{
		if (!NA.app.bGUI)
			return;
		/*float average_first = 0f;
		for (int t=0;t<tap_count;++t)
		{
			average_first += reflections[t];
		}
		average_first /= tap_count;
		GUI.Label (new Rect (0, 0, 300, 30), "average first reflections=" + average_first * 1000 + " ms");
		*/
		GUI.color = new Color(1,1,1,0.5f);
		if (tex != null)
		{
			GUI.DrawTexture(new Rect(Screen.width-256, Screen.height-64, 256, 64), tex);
		}
	}
	void OnAudioFilterRead(float[] data, int channels)
	{
		//return;
		/*for (int i=0;i<data.Length;++i)
		{
			int writepos = writeposition+i;
			buffer[writepos%200000] = 0;
		}
		*/
		//write tap

		/*
		for (int i=0;i<data.Length;++i)
		{
			//buffer[writepos%200000] = 0;
			for (int t=0;t<tap_count;++t)
			{
				if (reflections[t] != 0)
				{
					//we have a reflection
					int delay = (int)(reflections[t]*44100f);
					int writepos = writeposition+i+delay;
					//float delay_gain = 0.5f;//1f/(reflections[t]*340f/2f);
					float delay_gain = 1f/(reflections[t]*340f);
					delay_gain = Mathf.Clamp(delay_gain, 0, 1);
					buffer[writepos%200000] += data[i] * delay_gain;
				}
			}

		}
		*/
		//float delay_gain = 1f/(float)tap_count; //NEW
		float delay_gain = 1f;
		for (int t=0;t<tap_count;++t)
		{
			int delay = (int)(reflections[t]*44100f);
			//float delay_gain = 1f/(reflections[t]*340f);

			delay_gain = Mathf.Clamp(delay_gain, 0, 1);
			if (t%2==0)
				delay_gain *= -1f;
			if (reflections[t] != 0) //we have a reflection
			{
				for (int i=0;i<data.Length;++i)
				{
					int writepos = writeposition+i+delay;
					buffer[writepos%200000] += data[i] * delay_gain;
				}
			}
			
		}




		writeposition += data.Length;


		//read tap
		
		for (int i=0; i<data.Length; ++i) 
		{
			int readpos = readposition+i;
			data[i] += buffer[readpos%200000];
			buffer[readpos%200000] = 0;

		}
		
		readposition += data.Length;
		
		
	}


	public void UpdateTexture()
	{
		if (colors == null)
			colors = new Color[width*height];
		if (tex == null)
			tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
		for (int i=0;i<width*height;++i)
		{
			colors[i] = Color.black;
		}

		float sign = 1f;
		for (int x=0;x<width;++x)
		{
			colors[(height/2)*width+x] = Color.blue;
		}
		float duration = 2;
		foreach (float t in reflections)
		{
			sign *= -1f;
			float delay_gain = 1f/(t*340f);
			float db = (20*Mathf.Log10(delay_gain)+60f)/60f;
			db = Mathf.Clamp(db, 0, 1);
			db*= sign; //fake
			int x = (int)(t*width/duration);
			int a = (int)(height/2f+height/2f*db);
			x = Mathf.Clamp(x, 0, width-1);
			int b = height/2;
			int y1 = 0;
			int y2 = 0;
			if (b > a)
			{
				y1 = a;
				y2 = b;
			}
			else
			{
				y1 = b;
				y2 = a;
			}
			
			for (int y=y1;y<y2;++y)
			{
				colors[y*width+x] = Color.white;
			}

			//reflection drawing


		}

		tex.SetPixels(colors);
		tex.Apply();
	}

}
