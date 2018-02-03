﻿using UnityEngine;
using System.Collections;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using System;

namespace DP
{
[ExecuteInEditMode]
public class DMX : MonoBehaviour {

	private const int DMX_INDEX_OFFSET = 5;
	private const int DMX_MESSAGE_OVERHEAD = 6;

	private const int N_DMX_CHANNELS = 512;
	public int nChannels
	{
		get {return N_DMX_CHANNELS;}
	}

	private const byte DMX_PRO_HEADER_SIZE = 4;
	private const byte DMX_PRO_START_MSG = 0x7E;
	private const byte DMX_PRO_LABEL_DMX = 6;
	private const byte DMX_PRO_LABEL_SERIAL_NUMBER = 10;
	private const byte DMX_PRO_START_CODE = 0;
	private const byte DMX_PRO_START_CODE_SIZE = 1;
	private const byte DMX_PRO_END_MSG = 0xE7;
	private const byte DMX_PRO_END_MSG_SIZE = 1;


	private const int TX_BUFFER_LENGTH = DMX_MESSAGE_OVERHEAD + N_DMX_CHANNELS;
	private Thread dmxThread;
	private bool updateDMX;

	private static SerialPort serialPort;
	public List<string> serialPorts;
	public int serialPortIdx;

	private byte[] DMXLevels = new byte[N_DMX_CHANNELS];
	private byte[] TxBuffer = new byte[DMX_MESSAGE_OVERHEAD + N_DMX_CHANNELS];

	void Start () 
	{	
		GetPortNames();
		if (serialPortIdx > 0) OpenSerialPort();

		initTXBuffer();

		dmxThread = new Thread(ThreadedIO);
		dmxThread.Start();

		updateDMX = true;
	}

	public int this[int index]
	{
		get
		{
			if (index < 1 || index > N_DMX_CHANNELS)
			{
				throw new UnityException("Channel out fo range: " +index);
			}
			else
			{
				return (int)DMXLevels[index-1];
			}
		}
		set
		{
			if (index < 1 || index > N_DMX_CHANNELS)
			{
				throw new UnityException("Channel out fo range: " +index);
			}
			else
			{
				if (value < 0 || value > 255)
				{
					throw new UnityException("Level out fo range");
				}
				DMXLevels[index-1] = (byte)Mathf.Clamp(value,0,255);
				updateDMX = true;
			}
		}
	}

	private void ThreadedIO()
	{
	Debug.Log("Thread Start");
		while(true)
		{
			if (updateDMX)
			{
				updateDMX = false;
				Buffer.BlockCopy(DMXLevels,0,TxBuffer,DMX_INDEX_OFFSET,N_DMX_CHANNELS);
				if (serialPort != null && serialPort.IsOpen) {serialPort.Write(TxBuffer, 0, TX_BUFFER_LENGTH); };
			}

			//if (serialPort.BytesToRead > 0)
		}
	}

	private void GetPortNames()
	{
		int p = (int)System.Environment.OSVersion.Platform;
		serialPorts = new List<string>();
		serialPorts.Add("");
		
		if(p == 4 || p == 128 || p == 6)
		{
			string[] ttys = Directory.GetFiles("/dev/", "tty.*");
			foreach(string dev in ttys)
			{
				serialPorts.Add(dev.Replace("/", "\\")); //Replace forward slash to play nicely with gui.		
			}
		}
	}

	public void OpenSerialPort()
	{
		if (serialPort != null)
		{
			serialPort.Close();
			serialPort.Dispose();
		}

		serialPort = new SerialPort(serialPorts[serialPortIdx].Replace("\\", "/"), 57600, Parity.None, 8, StopBits.One);

		try 
		{
			serialPort.Open();
			serialPort.ReadTimeout = 50;
			updateDMX = true;
		}
		catch (System.Exception e) 
		{
			Debug.LogException(e);
			serialPortIdx = 0;
		}
	}

	private void initTXBuffer()
	{
		for(int i=0; i<TX_BUFFER_LENGTH; i++) TxBuffer[i] = (byte)255;

		TxBuffer[000] = DMX_PRO_START_MSG;
		TxBuffer[001] = DMX_PRO_LABEL_DMX;
		TxBuffer[002] = (byte)(N_DMX_CHANNELS+1 & 255);; 
		TxBuffer[003] = (byte)((N_DMX_CHANNELS+1 >> 8) & 255);
		TxBuffer[004] = DMX_PRO_START_CODE;
		TxBuffer[517] = DMX_PRO_END_MSG;
	}

	void OnApplicationQuit()
	{
		//Reset DMX levels to 0;
		for(int i=0; i< N_DMX_CHANNELS; i++) DMXLevels[i] = (byte)0x00;
		updateDMX = true;

		//Clean up
		dmxThread.Abort();
		if (serialPort != null)
		{
			serialPort.Close();
			serialPort.Dispose();
		}
	}
}
}

