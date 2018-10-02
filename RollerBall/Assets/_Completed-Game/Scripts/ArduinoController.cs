using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoController : MonoBehaviour
{
	private SerialPort stream;
    private PlayerController _playerController;
    public int port = 3;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    void Start () {
		
        stream = new SerialPort("COM"+port.ToString(), 9600); //configuramos el puerto por donde vamos a leer datos
        stream.ReadTimeout = 50;
        stream.Open(); //lo abrimos
	}

	void Update ()
	{
        SetVectorAngles();
	}

    void SetVectorAngles() //esta función lee los datos y los envia al player controller
    {
        string date = ReadFromArduino(); //almacenamos en una string lo que leemos del serial port
        if (date == null) //si es una linea vacia lo que leemos, la pasamos.
        {
            return;
        }
        string[] dates = date.Split('_'); //dividimos la string en una lista
        if (dates.Length < 4) //si tiene menos de los datos que necesitamos, la pasamos.
        {
            return;
        }
        string axis = dates[1]; //nos quedamos con el eje para poder saber a cual corresponde. axis será X o Y.


        //modificamos el vector anglesArduino de la clase PlayerController pasandole el valor leído. El cual se almacena en dates[3]
        if (axis == "X")
        {
            _playerController.anglesArduino.Set(float.Parse(dates[3]), _playerController.anglesArduino.y, _playerController.anglesArduino.z);
        }
        else if (axis == "Y")
        {
            _playerController.anglesArduino.Set(_playerController.anglesArduino.x, float.Parse(dates[3]), _playerController.anglesArduino.z);
        }
    }

	public string ReadFromArduino(int timeout = 1)
	{
		stream.ReadTimeout = timeout;
		try
		{
			string streamReadline = stream.ReadLine();//lee una linea del serial port
			return streamReadline;// y la devuelve

		}
		catch (TimeoutException e)
		{
			return null;
		}
	}
}
