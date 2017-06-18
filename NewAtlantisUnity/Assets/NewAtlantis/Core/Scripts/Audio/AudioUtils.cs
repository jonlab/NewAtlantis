using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUtils 
{
	public static Texture2D ComputeWaveForm(AudioClip clip, int width, int height)
	{
		Texture2D 		tex;
		Color[] 		colors;

		int count = clip.samples;
		colors = new Color[width*height];
		tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
		for (int i=0;i<width*height;++i)
		{
			colors[i] = Color.red;
		}
		tex.SetPixels(colors);
		tex.Apply();

		//Debug.Log("samples = " + count);
		float[] data = new float[count*clip.channels];
		clip.GetData(data, 0);
		for (int i=0;i<width;+++i)
		{
			int n1 = i*count*clip.channels/width;
			int n2 = (i+1)*count*clip.channels/width;
			//n1/=clip.channels;
			//n2/=clip.channels;
			float ave = 0f;
			for (int k=n1;k<n2;k+=1)
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

		return tex;
	}
}
