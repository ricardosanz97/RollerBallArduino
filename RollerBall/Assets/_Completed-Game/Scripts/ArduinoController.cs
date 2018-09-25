using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoController : MonoBehaviour
{
	private SerialPort stream;
	private string ping = "PING";
	private string pong = "PONG";
	
	void Start () {
		
        stream = new SerialPort("COM9", 9600);
        stream.ReadTimeout = 50;
        stream.Open();
        //stream.WriteLine(ping);
	}
	
	// Update is called once per frame
	void Update ()
	{

		print(ReadFromArduino());
	}

	public string ReadFromArduino(int timeout = 1)
	{
		stream.ReadTimeout = timeout;
		try
		{
			string streamReadline = stream.ReadLine();
			print(streamReadline);
			return streamReadline;

		}
		catch (TimeoutException e)
		{
			return null;
		}
	}
}
