using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoController : MonoBehaviour
{
	private SerialPort stream;
    private PlayerController _privateController;
    public int port = 3;

    private void Awake()
    {
        _privateController = GetComponent<PlayerController>();
    }

    void Start () {
		
        stream = new SerialPort("COM"+port.ToString(), 9600);
        stream.ReadTimeout = 50;
        stream.Open();
        //stream.WriteLine(ping);
	}
	
	// Update is called once per frame
	void Update ()
	{
        SetVectorAngles();
	}

    void SetVectorAngles()
    {
        string date = ReadFromArduino();
        if (date == null)
        {
            return;
        }
        string[] dates = date.Split('_');
        if (dates.Length < 4)
        {
            return;
        }
        string axis = dates[1];

        if (axis == "X")
        {
            _privateController.anglesArduino.Set(float.Parse(dates[3]), _privateController.anglesArduino.y, _privateController.anglesArduino.z);
        }
        else if (axis == "Y")
        {
            _privateController.anglesArduino.Set(_privateController.anglesArduino.x, float.Parse(dates[3]), _privateController.anglesArduino.z);
        }
    }

	public string ReadFromArduino(int timeout = 1)
	{
		stream.ReadTimeout = timeout;
		try
		{
			string streamReadline = stream.ReadLine();
			return streamReadline;

		}
		catch (TimeoutException e)
		{
			return null;
		}
	}
}
