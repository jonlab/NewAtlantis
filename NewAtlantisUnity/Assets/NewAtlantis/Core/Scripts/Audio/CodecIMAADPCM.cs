using UnityEngine;
using System.Collections;

public class CodecIMAADPCM 
{


	public static byte[] EncodeToADPCM(AudioClip source, IMAADPCM.ADPCMState[] states = null)
	{
		Debug.Log ("channels = " + source.channels);
		float[] data = new float[source.samples*source.channels];
		source.GetData(data, 0);
		IMAADPCM.ADPCMState state = new IMAADPCM.ADPCMState();
		state.valprev = 0;
		
		int count = source.samples;
		byte[] bytes = new byte[count/2];
		for (int i = 0; i < count/2; i++) 
		{
			int m1 = i*2;
			int m2 = i*2+1;
			
			//samples en entrée
			short i1 = (short)(data[i*2+0]*32768f);
			short i2 = (short)(data[i*2+1]*32768f);
			
			if (m1%1017 == 0 && states != null)
			{
				Debug.Log ("modulo1");
				int stateindex = m1/1017;
				states[stateindex].valprev = state.valprev;
				states[stateindex].index = state.index;
			}
			byte b1 = IMAADPCM.encodeADPCM(i1, ref state);
			
			if (m2%1017 == 0 && states != null)
			{
				Debug.Log ("modulo2");
				int stateindex = m2/1017;
				states[stateindex].valprev = state.valprev;
				states[stateindex].index = state.index;
			}
			byte b2 = IMAADPCM.encodeADPCM(i2, ref state);
			
			//Debug.Log (""+b1+" " + b2);
			//bytes[i] = (byte)(b1*16+b2);
			
			bytes[i] = (byte)(b1*16+b2);
		}
		return bytes;
	}
	
	
	/*
	public void EncodeToADPCM(AudioClip source, AudioClip dest)
	{
		float[] data = new float[source.samples*source.channels];
		//float -> 16 -> ADPCM -> 16 -> float
		byte[] bytes = EncodeToADPCM(source);
		int count = source.samples;
		//bytes contient le ADPCM
		//adpcm.SetData(	
		IMAADPCM.ADPCMState state = new IMAADPCM.ADPCMState();
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
	
	
	public static byte[] GetADPCMData(AudioClip clip)
	{
		return EncodeToADPCM(clip);
	}

	
	public static byte[] GetADPCMWAVData(AudioClip clip, int maxsamples)
	{
		Debug.Log ("maxsamples="+maxsamples);
		int headersize = 60;
		float[] fdata = new float[clip.samples*clip.channels];
		int rawdatasize = (clip.samples*clip.channels)/2;
		if (rawdatasize>maxsamples/2)
			rawdatasize = maxsamples/2;
		clip.GetData(fdata, 0);

		int 	SampleRate 			= 22050;
		int 	ByteRate 			= 11100;

		int blockalign = 1024; //taille d'un header ADPCM complet
		int blockdata = blockalign-4;//252;
		int blocks = rawdatasize/blockdata;
		int datasize = blocks * blockalign;
		//IMAADPCM.ADPCMState[] states = new IMAADPCM.ADPCMState[blocks+1];
		//for (int i=0;i<blocks;++i)
		//{
		//	states[i] = new IMAADPCM.ADPCMState();
		//}
		//byte[] data = EncodeToADPCM(record, states);
		
		
		
		
		uint totalsize = (uint)(datasize+headersize);
		Debug.Log ("total size = " + totalsize);
		byte[] total = new byte[totalsize];
		
		byte[] b = null;
		
		
		b = System.Text.Encoding.UTF8.GetBytes("RIFF");
		b.CopyTo(total, 0);
		
		b = System.BitConverter.GetBytes(total.Length-8);
		b.CopyTo(total, 4);
		
		b = System.Text.Encoding.UTF8.GetBytes("WAVE");
		b.CopyTo(total, 8);
		
		//fmt CHUNK =======================
		b = System.Text.Encoding.UTF8.GetBytes("fmt ");
		b.CopyTo(total, 12);
		
		//4
		b = System.BitConverter.GetBytes(20); //taille du block fmt
		b.CopyTo(total, 16);
		
		//2
		b = System.BitConverter.GetBytes((ushort)17); //FORMAT 0x11=INA ADPCM (17) OK !!!
		//b = System.BitConverter.GetBytes((ushort)57); 
		//b = System.BitConverter.GetBytes((ushort)259); //FORMAT 0x11=INA ADPCM (17)
		b.CopyTo(total, 20);
		
		b = System.BitConverter.GetBytes((ushort)1); //CHANNELS OK !!!
		b.CopyTo(total, 22);
		
		b = System.BitConverter.GetBytes((uint)SampleRate); //SR
		b.CopyTo(total, 24);
		
		//byte rate = SR/2 + overhead
		//b = System.BitConverter.GetBytes((uint)(22050*1/2)); //byte rate //SR*channels*2
		b = System.BitConverter.GetBytes((uint)(ByteRate)); //byte rate //SR*channels*2
		b.CopyTo(total, 28);
		
		//block align
		b = System.BitConverter.GetBytes((ushort)blockalign); //2 : 16 bits mono 512 dans le fichier Audacity
		b.CopyTo(total, 32);
		
		b = System.BitConverter.GetBytes((ushort)4); //bits per samples, 4
		b.CopyTo(total, 34);
		
		//taille de la zone extended
		b = System.BitConverter.GetBytes((ushort)2); 
		b.CopyTo(total, 36);
		
		int samplesperblock = (blockalign-4)*2+1;
		b = System.BitConverter.GetBytes((ushort)samplesperblock);  //nombre de samples par bloc : (512-4)*2+1
		b.CopyTo(total, 38);
		
		
		
		//FACT CHUNK
		b = System.Text.Encoding.UTF8.GetBytes("fact");
		b.CopyTo(total, 40);
		
		b = System.BitConverter.GetBytes(4); //taille du block fact : 4 octets
		b.CopyTo(total, 44);
		
		b = System.BitConverter.GetBytes((uint)datasize*2);  //nombre de samples au total
		b.CopyTo(total, 48);
		
		
		//DATA CHUNK
		b = System.Text.Encoding.UTF8.GetBytes("data");
		b.CopyTo(total, 52);
		
		b = System.BitConverter.GetBytes((uint)totalsize);
		b.CopyTo(total, 56);
		
		
		//on commence à header size
		
		
		//compression IMA ADPCM
		
		
		//Debug.Log("Samples per blocks = " + samplesperblock);
		
		IMAADPCM.ADPCMState state = new IMAADPCM.ADPCMState();
		state.valprev = 0;
		state.index = 0;
		int count = (blocks-1) * samplesperblock;
		int dest = 0;
		bool first = true;
		
		
		for (int s=0;s<count;++s) //pour tous les samples en entrée
		{
			short input = (short)(fdata[s]*32768f);
			int m = s%samplesperblock;
			byte adpcm = IMAADPCM.encodeADPCM(input, ref state);
			
			if (m == 0)
			{
				//premier sample du bloc
				//il faut écrire un header mais ne pas émettre normalement le sample
				//s+=1;
				//byte adpcm = IMAADPCM.encodeADPCM(input, ref state);
				//Debug.Log ("write adpcm header at dest " + dest + " first="+first + " vals " + state.valprev + " " + state.index);
				b = System.BitConverter.GetBytes(state.valprev);
				total[headersize+dest+0] = b[0];
				total[headersize+dest+1] = b[1];
				total[headersize+dest+2] = state.index;
				total[headersize+dest+3] = 0;//adpcm; //hack pour avoir quand même le nible dans le header NON ! semble poser un problème sur la box
				
				dest += 4;
				first = true;
				//byte adpcm = IMAADPCM.encodeADPCM(input, ref state);
			}
			else
			{
				//byte adpcm = IMAADPCM.encodeADPCM(input, ref state);
				//cas normal, on écrit l'échantillon
				//byte adpcm = IMAADPCM.encodeADPCM(input, ref state);
				if (first)
				{
					total[headersize+dest] = (byte)(adpcm);
					first = false;
				}
				else
				{
					total[headersize+dest] += (byte)(adpcm*16);
					dest ++;
					first = true;
				}
			}
		}
		
		return total;
	}

	
	//=============================================
	//=============================================
	public static AudioClip SetADPCMData(byte[] bytes)
	{
		
		//lire le header 
		//reconstituer le buffer ADPCM
		//décoder
		int headersize = 60; //hardcoded for ADPCM app encoded
		if (bytes[0] == 'R' && bytes[1] == 'I' && bytes[2] == 'F' && bytes[3] == 'F')
		{
			Debug.Log ("ok, this is a RIFF file");
			
			//48
			ushort format = System.BitConverter.ToUInt16(bytes, 20); //FORMAT
			if (format != 17)
			{
				Debug.Log ("ERROR, bad format : " + format + " - should be IMA ADPCM");
				return null;
			}
			uint totalsamples = System.BitConverter.ToUInt32(bytes, 48);
			ushort blockalign = System.BitConverter.ToUInt16(bytes, 32); //alignement de bloc en octets
			uint SampleRate = System.BitConverter.ToUInt32(bytes, 24);
			Debug.Log ("total samples=" + totalsamples);
			//totalsamples = (uint)(bytes.Length-60); //test
			Debug.Log ("block align=" + blockalign);
			
			int datasize = bytes.Length-headersize;
			int blocks = datasize/blockalign;
			int estimated_samples = blocks*((blockalign-4)*2+1);
			Debug.Log ("estimated samples=" + estimated_samples);
			if (totalsamples >= estimated_samples)
			{
				Debug.Log ("bad totalsamples!");
				totalsamples = (uint)estimated_samples;
			}
			//return null;
			AudioClip dest = AudioClip.Create("adpcm_downloaded", (int)totalsamples, 1, (int)SampleRate, false, false);
			float[] data = new float[totalsamples];
			for (int k=0;k<totalsamples;++k)
				data[k] = 0f;
			IMAADPCM.ADPCMState state = new IMAADPCM.ADPCMState(); 
			state.valprev = 0;
			state.index = 0;
			int j = 0; //destination index
			int i = 0; //source index
			while (i<totalsamples/2)
			{
				if (i%blockalign == 0)
				{
					//Debug.Log ("ADPCM BLOCK (i=" + i + ") = " + bytes[headersize+i+0] + " " + bytes[headersize+i+1] + " " + bytes[headersize+i+2] + " " + bytes[headersize+i+3]);
					//on est sur le premier byte du header
					//Debug.Log ("Current ADPCM state = " + state.valprev + " " + state.index);
					//cas particulier : 4 octets header, on met à jour le state et on écrit un échantillon
					//bytes[headersize+i+1] = 0;
					byte[] b = new byte[2];
					b[0] = bytes[headersize+i+0];
					b[1] = bytes[headersize+i+1];
					short sample = System.BitConverter.ToInt16(b, 0); //sample de référence
					//Debug.Log ("sample=" + sample);
					byte index = bytes[headersize+i+2]; //index de référence
					//le 4e octet ne contient rien de significatif
					//data[j] = ((float)state.valprev)/32767f;
					//short i1 = IMAADPCM.decodeADPCM(bytes[headersize+i+3], ref state); //hack
					//data[j] = (float)(i1)/32768f;
					//if (i>0)
					//	IMAADPCM.encodeADPCM(sample, ref state);
					//if (i>0)
					//{
					//ADPCM state update
					state.valprev = sample;
					state.index = index;
					data[j] = (float)(sample)/32768f;
					//}
					//IMAADPCM.encodeADPCM(sample, ref state); //post encode 
					//IMAADPCM.encodeADPCM(sample, ref state); //post encode 
					
					//byte b2 = (byte)(state.valprev&15);
					//short i1 = IMAADPCM.decodeADPCM(b2, ref state);
					//Debug.Log ("predicted=" + i1 + " sample=" + sample);
					//byte adpcm = IMAADPCM.encodeADPCM(sample, ref state);
					//state.valprev = System.BitConverter.ToInt16(bytes, headersize+i);
					//
					//sample = i1;
					
					//data[j] = ((float)state.valprev)/32768f;
					//data[j] = (float)((double)sample)/32768f;
					//data[j] = (float)(sample)/32768f;
					//data[j] = 0;
					/*if (j>0)
					{
						Debug.Log ("previous="+data[j-1] + " current=" + data[j]);
					}*/
					//j++;
					
					//state.valprev = System.BitConverter.ToInt16(b, 0);
					//state.index = bytes[headersize+i+2];
					//Debug.Log ("Corrected ADPCM state = " + state.valprev + " " + state.index);
					//i+=3;
					//if (j>10)
					j++;
					i+=4; //on avance de 4 pour passer le header
				}
				else
				{
					//cas normal
					byte b = bytes[headersize+i];
					byte b1 = (byte)((b/16)&15); //nibble 1
					byte b2 = (byte)(b&15); //nibble 2
					short i1 = IMAADPCM.decodeADPCM(b2, ref state);
					short i2 = IMAADPCM.decodeADPCM(b1, ref state);
					data[j] = ((float)i1)/32768f;
					j++;
					data[j] = ((float)i2)/32768f;
					j++;
					i++; //next input byte
				}
			}
			
			//patch 
			
			/*for (int k=0;k<totalsamples;++k)
			{
				if (k>0 && k%(blockalign*2) == 0)
				{
					Debug.Log ("before patch " + data[k-1] + " " + data[k] + " " + data[k+1]);
					//data[k] = (data[k-1]+data[k+1])/2f;
					//Debug.Log ("after patch " + data[k-1] + " " + data[k] + " " + data[k+1]);
				}
			}*/
			
			/*
			FileStream fs = new FileStream("adpcm.raw", FileMode.Create);
			for (int u=0;u<data.Length;++u)
			{
				byte[] b = System.BitConverter.GetBytes(data[u]); 
				fs.Write(b, 0, 4);
			}
			*/
			//
			//fs.Close();
			//rampin and out
			
			/*for (int u=0;u<2000;++u)
			{
				float k = (float)u/2000f;
				data[u] *= k;
				data[totalsamples-1-u] *= k;
			}
			*/
			dest.SetData(data, 0);
			return dest;
		}
		else
		{
			Debug.Log ("ERROR, this is not a RIFF file");
		}
		
		return null;
	}


	

}
