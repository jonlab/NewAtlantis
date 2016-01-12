using UnityEngine;
using System.Collections;


public class MixSettings
{
	public float position;
	public float gain;
}

public class DSP
{

	public static void GenerateSilence(float[] data)
	{
		for (int i = 0; i < data.Length; i++) 
		{
			data[i] = 0;
		}
	}

	public static void GenerateNoise(float[] data)
	{
		for (int i = 0; i < data.Length; i++) 
		{
			data[i] += Random.value*2f-1f;
		}
	}

	public static void GenerateSinus(float[] data, float samplerate, float frequency)
	{
		for (int i = 0; i < data.Length; i++) 
		{
			float angle = (float)i*frequency/samplerate*Mathf.PI*2;
			data[i] += Mathf.Sin(angle);
		}
	}

	public static void GenerateCosinus(float[] data, float samplerate, float frequency)
	{
		for (int i = 0; i < data.Length; i++) 
		{
			float angle = (float)i*frequency/samplerate*Mathf.PI*2;
			data[i] += Mathf.Cos(angle);
		}
	}

	public static void GenerateSquare(float[] data, float samplerate, float frequency)
	{
		for (int i = 0; i < data.Length; i++) 
		{
			float val = (float)i*frequency/samplerate*2;
			data[i] += (int)val%2*2-1;
		}
	}

	public static void GenerateSawtooth(float[] data, float samplerate, float frequency)
	{
		for (int i = 0; i < data.Length; i++) 
		{
			float val = (float)i*frequency/samplerate*2;
			data[i] += (int)val%2*2-1;
			
		}
	}





	public static void Normalize(float[] data, float val)
	{
		//recherche du max (Abs)
		//int count = clip.samples;
		float max = 0f;
		for (int i = 0; i < data.Length; i++) 
		{
			float s = data[i];
			if (max < s)
				max = s;
		}
		//norm
		if (max != 0.0f)
		{
			for (int i = 0; i < data.Length; i++) 
			{
				data[i] = data[i]*val/max;
			}
		}
	}
	
	public static float[] AutoTrim(float[] data, float min)
	{
		int start = 0;
		int end = data.Length-1;
		for (start = 0; start < data.Length; start++) 
		{
			float a = Mathf.Abs(data[start]);
			if (a > min)
				break;
		}
		for (end = data.Length-1; end >= 0; end--) 
		{
			float a = Mathf.Abs(data[end]);
			if (a > min)
				break;
		}
		if (end>start)
		{
			float[] output = new float[end-start];
			for (int i=0;i<end-start;++i)
			{
				output[i] = data[i+start];
			}
			return output;
		}
		return null;
	}
	
	
	public static AudioClip Mix(AudioClip[] clips, MixSettings[] mixSettings)
	{
		//channels 2 44100
		
		//recherche de la longueur max
		int max = 0;
		for (int i=0;i<clips.Length;++i)
		{
			if (clips[i] == null)
				continue;
			int start = (int)(mixSettings[i].position*44100f);
			int len = clips[i].samples+start;
			if (max<len)
				max = len;
		}
		
		AudioClip mix = AudioClip.Create("mix", max, 2, 44100, false, false);
		float[] datamix = new float[max*2];
		for (int i=0;i<max;++i)
		{
			datamix[i] = 0f;
		}
		for (int i=0;i<clips.Length;++i)
		{
			if (clips[i] == null)
				continue;
			int start = (int)(mixSettings[i].position*44100f);
			Debug.Log ("clip == " + clips[i]);
			
			float[] data = new float[clips[i].samples*clips[i].channels];
			clips[i].GetData(data, 0);
			if (clips[i].channels == 2)
			{
				for (int j=0;j<clips[i].samples*clips[i].channels;++j)
				{
					datamix[j+start*2] += data[j] * mixSettings[i].gain;
				}
			}
			else
			{
				for (int j=0;j<clips[i].samples*clips[i].channels;++j)
				{
					datamix[(j+start)*2+0] += data[j]* mixSettings[i].gain;
					datamix[(j+start)*2+1] += data[j]* mixSettings[i].gain;
				}
			}
			data = null;
		}
		
		mix.SetData(datamix, 0);
		return mix;
		
	}
	
	
	public static AudioClip Granulate(AudioClip[] clips, MixSettings[] mixSettings, float position, float masterduration = 1f)
	{
		int max = 0;
		for (int i=0;i<clips.Length;++i)
		{
			if (clips[i] == null)
				continue;
			int start = (int)(mixSettings[i].position*44100f);
			int len = clips[i].samples*2; //par sécurité on prend le double
			if (max<len)
				max = len;
		}
		
		max = (int)(masterduration*44100f);
		AudioClip mix = AudioClip.Create("granulate", max, 2, 44100, false, false);
		float[] datamix = new float[max*2];
		for (int i=0;i<max;++i)
		{
			datamix[i] = 0f;
		}
		for (int i=0;i<clips.Length;++i)
		{
			if (clips[i] == null)
				continue;
			if (mixSettings[i].gain == 0)
				continue;
			int start = (int)(mixSettings[i].position*44100f);
			Debug.Log ("clip == " + clips[i]);
			
			float[] data = new float[clips[i].samples*clips[i].channels];
			clips[i].GetData(data, 0);
			
			for (int k=0;k<200;++k)
			{
				//extraction et copie d'un grain
				int duration = 10000; //15000
				//int src = (int)(Random.value*clips[i].samples);
				int src = (int)(clips[i].samples*position);
				if (src>clips[i].samples-duration)
					src -= duration*2;
				
				int dst = (int)(Random.value*mix.samples);
				//if (dst>mix.samples-duration)
				//	dst -= duration*2;
				dst = Mathf.Max(0,dst);
				src = Mathf.Max(0,src);
				//Debug.Log("SRC="+src+" DST="+dst);
				//float d = 0.1f;
				//float s = Random.value*clips[i].length-d;
			
				
				//int samples = (int)(d*44100f);
				//int samples_start = (int)(s*44100f);
				
				//la sortie est obligatoirement stéréo
				if (clips[i].channels == 2)
				{
					for (int j=0;j<duration;++j)
					{
						//source mono
						int n = (j+dst)%mix.samples;
						
						datamix[n*2+0] += data[(j+src)*2+0]* mixSettings[i].gain;
						datamix[n*2+1] += data[(j+src)*2+1]* mixSettings[i].gain;
						
						//datamix[j+dst] += data[j+src] * mixSettings[i].gain;
					}
				}
				else
				{
					for (int j=0;j<duration;++j)
					{
						//source mono
						int n = (j+dst)%mix.samples;
						
						datamix[n*2+0] += data[j+src]* mixSettings[i].gain;
						datamix[n*2+1] += data[j+src]* mixSettings[i].gain;
					}
				}
			}
			data = null;
		}
		
		mix.SetData(datamix, 0);
		return mix;
		
	}
	
	public static float[] Convolve(float[] s1, float[] s2)
	{
		int size = s1.Length+s2.Length;
		float[] output = new float[size];
		for (int i=0;i<size;++i)
		{
			output[i] = 0;
		}
		//time domain convolve
		for (int i=0;i<s1.Length;++i)
		{
			for (int j=0;j<s2.Length;++j)
			{
				output[i+j] += s1[i]*s2[j];
			}
		}
		return output;
	}


	public static float[] Add(float[] s1, float[] s2)
	{
		int size = Mathf.Max(s1.Length,s2.Length);

		float[] output = new float[size];
		float[] in1 = new float[size];
		float[] in2 = new float[size];
		for (int i=0;i<s1.Length;++i)
		{
			in1[i] = s1[i];
		}
		for (int i=0;i<s2.Length;++i)
		{
			in2[i] = s2[i];
		}
		for (int i=0;i<size;++i)
		{
			output[i] = in1[i]+in2[i];
		}
		return output;
	}

	
	public static AudioClip Concatenate(AudioClip[] clips)
	{
		
		//recherche de la longueur totale
		int total = 0;
		for (int i=0;i<clips.Length;++i)
		{
			if (clips[i] == null)
				continue;
			int len = clips[i].samples;
			total += len;
		}
		total *= 2;
		AudioClip mix = AudioClip.Create("concatenation", total, 2, 44100, false, false);
		float[] datamix = new float[total*2];
		for (int i=0;i<total*2;++i)
		{
			datamix[i] = 0f;
		}
		int current = 0;
		for (int i=0;i<clips.Length;++i)
		{
			Debug.Log ("clip == " + clips[i]);
			if (clips[i] == null)
				continue;
			float[] data = new float[clips[i].samples*clips[i].channels];
			clips[i].GetData(data, 0);
			if (clips[i].channels == 2)
			{
				for (int j=0;j<clips[i].samples*clips[i].channels;++j)
				{
					datamix[j+current] += data[j];
				}
			}
			else
			{
				for (int j=0;j<clips[i].samples*clips[i].channels;++j)
				{
					//float sample = data[j];
					datamix[(j+current)*2+0] += data[j];
					datamix[(j+current)*2+1] += data[j];
				}
			}
			int interval = 44100*1; //1
			//current += clips[i].samples*2;
			current += interval;
			data = null;
		}
		
		mix.SetData(datamix, 0);
		return mix;
		
	}

	public static float[] Extract(AudioClip audioClip, float pos, float duration)
	{
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

		return data;
	}
	
}
