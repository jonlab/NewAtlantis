using UnityEngine;
using System.Collections;

public class AudioWidget 
{

	Texture2D 		tex;
	Color[] 		colors;
	
	AudioClip 		record = null;
	//AudioClip 		adpcm = null;
	
	int width 		= 512;
	int height 		= 128;
	int samplerate 	= 44100;
	
	//gérer la sélection + la position de lecture et d'enregistrement

	public void Init()
	{
		//Microphone.devices
		colors = new Color[width*height];
		tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
		for (int i=0;i<width*height;++i)
		{
			colors[i] = Color.red;
		}
		tex.SetPixels(colors);
		tex.Apply();
		
	}
	
	public AudioClip GetRecordAudioClip()
	{
		return record;
	}
	
	public void SetRecordAudioClip(AudioClip clip)
	{
		record = clip;
	}


	/*public AudioClip GetADPCMAudioClip()
	{
		return adpcm;
	}
	*/
	
	public void Process()
	{
		if (record)
		{
			ComputeWaveForm(record);
		}
	}
	
	public void DrawGUI(Rect r)
	{
		GUI.DrawTexture(r, tex);
	}
	
	
	public void Play()
	{
		
		
	}
	
	public void Record()
	{
		record = Microphone.Start(null, false, 10, samplerate);
		//adpcm = AudioClip.Create("adpcm", samplerate*10, 1, samplerate, false, false);
		
	}
	
	public void Stop()
	{
		if (record)
		{
			Microphone.End(null);
			ComputeWaveForm(record);
			//EncodeToADPCM(record, adpcm);
		}
	}
	
	public void Pause()
	{
		
	}
	
	public void ComputeWaveForm(AudioClip clip)
	{
		int count = clip.samples;
		
		//Debug.Log("samples = " + count);
		float[] data = new float[count*clip.channels];
		clip.GetData(data, 0);
		for (int i=0;i<width;+++i)
		{
			int n1 = i*count*clip.channels/width;
			int n2 = (i+1)*count*clip.channels/width;
			
			float ave = 0f;
			for (int k=n1;k<n2;++k)
			{
				ave += Mathf.Abs(data[k]);
			}
			ave /= (n2-n1);
			
			float a = ave;
			//Debug.Log ("a="+a);
			int a1 = 0;
			int a2 = 0;
			
			a1 = (int)(height/2+(ave)*(float)height/2f);
			a2 = (int)(height/2-(ave)*(float)height/2f);
			/*
			if (a > 0)
			{
				a1 = (int)((a+1f)*(float)height/2);
				a2 = height/2;
			}
			else if (a<0)
			{
				a1 = height/2;
				a2 = (int)((a+1)*height/2);
			}
			*/
			for (int j=0;j<height;++j)
			{
				if (j>a1 || j<a2)
					colors[j*width+i] = Color.white;	
				else
					colors[j*width+i] = Color.blue;	
			}
			
			colors[(height/2)*width+i] = Color.red; //ligne 0
		}
		
		tex.SetPixels(colors);
		tex.Apply();
		
		data = null;
	}
	
	
	public void Normalize(AudioClip clip, float val)
	{
		float[] data = new float[clip.samples*clip.channels];
		clip.GetData(data, 0);
		DSP.Normalize(data, val);
		//reaffectation
		clip.SetData(data, 0);
		data = null;
	}


	public AudioClip AutoTrim(AudioClip clip, float min)
	{
		float[] data = new float[clip.samples*clip.channels];
		clip.GetData(data, 0);
		float[] output = DSP.AutoTrim(data, min);
		
		//reaffectation
		//clip.SetData(output, 0);
		AudioClip c = AudioClip.Create("trim", data.Length, 1, samplerate, false, false);
		c.SetData(output, 0);
		data = null;
		return c;
	}
	
	
	
	/*
	public void EncodeToADPCM(AudioClip source, AudioClip dest)
	{
		float[] data = new float[source.samples*source.channels];
		source.GetData(data, 0);
		//float -> 16 -> ADPCM -> 16 -> float
		IMAADPCM.ADPCMState state = new IMAADPCM.ADPCMState();
		int count = source.samples;
		byte[] bytes = new byte[count/2];
		for (int i = 0; i < count/2; i++) 
		{
			short i1 = (short)(data[i*2+0]*32768f);
			short i2 = (short)(data[i*2+1]*32768f);
			byte b1 = IMAADPCM.encodeADPCM(i1, ref state);
			byte b2 = IMAADPCM.encodeADPCM(i2, ref state);
			bytes[i] = (byte)(b1*16+b2);
        }
		
		//bytes contient le ADPCM
		//adpcm.SetData(	
		state = new IMAADPCM.ADPCMState();
		for (int i = 0; i < count/2; i++) 
		{
			byte b = bytes[i];
			byte b1 = (byte)((b/16)&15);
			byte b2 = (byte)(b&15);
			short i1 = IMAADPCM.decodeADPCM(b1, ref state);
			short i2 = IMAADPCM.decodeADPCM(b2, ref state);
			data[i*2+0] = (float)i1/32768f;
			data[i*2+1] = (float)i2/32768f;
		}
		dest.SetData(data, 0);
		Debug.Log ("ADPCM data bytes = " + bytes.Length);
		
	}
	*/
	
}
