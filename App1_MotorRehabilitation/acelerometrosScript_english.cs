using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class acelerometrosScript : MonoBehaviour {

    SerialPort[] arduino;

    Thread thread;

    int proceso = 0; // 0 - Inicar arduinos 
                     // 1 - Actualizar arduinos
                     // 2 - Cerrar Arduinos


    string[] nombreCOM;
    string[] puertoAsignado;

    bool[] arduinoVerify;
    bool[] leerArduino;

    public bool bluetoothDer = false;
    public bool bluetoothIzq = false;
    public bool bluetoothDer1 = false;
    public bool bluetoothIzq1 = false;
    public bool bluetoothComplete = false;

    public int okDer = 0;
    public int okIzq = 0;
    public float incrementoDer = 1;
    public float incrementoIzq = 1;
    public float anguloDer = 0;
    public float anguloDer1 = 0;
    public float anguloIzq = 0;
    public float anguloIzq1 = 0;

    public float maxCalibreDer = 0;
    public float maxCalibreIzq = 0;

    bool iniciarDer = false;
    bool iniciarIzq = false;
    bool iniciarDer1 = false;
    bool iniciarIzq1 = false;

    //Datos para compartir
    public string mensaje = "";
    public float carga = 0;

    GameObject crearEstudio;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start () {

        crearEstudio = GameObject.Find("crearEstudioPanel(Clone)");

        thread = new Thread(procesosDeHilo); thread.Start();
    }

    void procesosDeHilo()
    {
        while (true) {
            switch (proceso)
            {
                case 0:
                    conectarArduino();
                    break;
                case 1:
                    actualizarArduino();
                    break;
                case 2:
                    cerrarArduino();
                    break;
            }
        }       
    }

    //:::::::::::::::::::::::::::::::   Conectar arduino  :::::::::::::::::::::::::::::::::
    void conectarArduino() {

        leerPuertosCOM();

        arduino = new SerialPort[nombreCOM.Length];
        arduinoVerify = new bool[nombreCOM.Length];
        puertoAsignado = new string[nombreCOM.Length];
        leerArduino = new bool[nombreCOM.Length];
        for (int i = 0; i < nombreCOM.Length; i++) { leerArduino[i] = true; }

        comprobarPuerto(nombreCOM.Length);

        asignarArduinos(nombreCOM.Length);

    }

    void leerPuertosCOM() {
        Debug.Log("Reading ports COM...");
        mensaje = "Reading ports COM...";
        nombreCOM = SerialPort.GetPortNames();
        carga = 0.25f;
    }

    void comprobarPuerto(int n) {
        for (int i = 0; i<n; i++) {
            Debug.Log("Testing port : " + nombreCOM[i]);
            mensaje = "Testing port: " + nombreCOM[i]+"...";
            try
            {
                arduino[i] = new SerialPort("\\\\.\\" + nombreCOM[i], 9600);
                arduino[i].Open();
                arduino[i].ReadTimeout = 5;
                arduinoVerify[i] = true;
                Debug.Log("Port "+nombreCOM[i]+ " OK.");
                mensaje = "Testing port: " + nombreCOM[i] + " obert.";
            }
            catch(System.Exception) {
                arduinoVerify[i] = false;
                Debug.Log("Port " + nombreCOM[i] + " Error.");
                mensaje = "Testing port: " + nombreCOM[i] + " access denied.";
            }
            carga += 0.4f / nombreCOM.Length;
            Thread.Sleep(1000);
        }
    }

    void asignarArduinos(int n) {

        for (int i = 0; i<n; i++)
        {
            if (arduinoVerify[i])
            {
                int intentos = 0;
                while (intentos < 15)
                {
                    Debug.Log("Assigning ports:");
                    mensaje = "Pairing port: " + "Scanning..."; 
                    try
                    {
                        string arduinoRead = arduino[i].ReadLine();

                        string[] desc = arduinoRead.Split(","[0]);
                        if (desc[0] == "Izq")
                        {
                            Debug.Log("upper left: " + nombreCOM[i]);
                            mensaje = "Pairing ports: " + "upper left leg -> "+nombreCOM[i]+".";
                            bluetoothIzq = true;
                            intentos = 15;
                            puertoAsignado[0] = nombreCOM[i];
                        }
                        else
                        {
                            if (desc[0] == "Der")
                            {
                                Debug.Log("upper right: " + nombreCOM[i]);
                                mensaje = "Pairing ports: " + "upper right leg -> " + nombreCOM[i] + ".";
                                bluetoothDer = true;
                                puertoAsignado[1] = nombreCOM[i];
                                intentos = 15;
                            }
                            else
                            {
                                if (desc[0] == "Izq1")
                                {
                                    Debug.Log("lower left: " + nombreCOM[i]);
                                    mensaje = "Pairing ports: " + "lower left leg -> " + nombreCOM[i] + ".";
                                    bluetoothIzq1 = true;
                                    intentos = 10;
                                    puertoAsignado[2] = nombreCOM[i];
                                }
                                else
                                {
                                    if (desc[0] == "Der1")
                                    {
                                        Debug.Log("Lower right: " + nombreCOM[i]);
                                        mensaje = "Pairing ports: " + "lower right leg -> " + nombreCOM[i] + ".";
                                        bluetoothDer1 = true;
                                        intentos = 10;
                                        puertoAsignado[3] = nombreCOM[i];
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                        }
                    }
                    catch (System.Exception)
                    {

                    }

                    intentos++;
                }
                
                Thread.Sleep(1000);
            }
            carga += 0.25f / nombreCOM.Length;
        }

        //Verificar y cerrar puertos no usados
        if (bluetoothDer & bluetoothDer1 & bluetoothIzq & bluetoothIzq1)
        {
            for (int i = 0; i < nombreCOM.Length; i++)
            {
                if (arduinoVerify[i])
                {
                    if (arduino[i].IsOpen & nombreCOM[i] != puertoAsignado[0] & nombreCOM[i] != puertoAsignado[1] & nombreCOM[i] != puertoAsignado[2] & nombreCOM[i] != puertoAsignado[3])
                    {
                        mensaje = "Closing ports: Closing " + nombreCOM[i] + "...";
                        Debug.Log("Closing port: " + nombreCOM[i] );
                        arduino[i].Close();
                        mensaje = "Closing ports: " + nombreCOM[i] + " closed.";
                        Thread.Sleep(500);
                    }
                }
                carga += 0.1f / nombreCOM.Length;
            }
            bluetoothComplete = true;
            
            proceso = 1;
        }
        else
        {
            
            proceso = 2;
        }

    }

    //:::::::::::::::::::::::::::::::   Actualizar arduino  :::::::::::::::::::::::::::::::::
    
    void actualizarArduino() {
        for(int i = 0; i < nombreCOM.Length; i++)
         {
            if (arduino[i].IsOpen)
            {
                if (leerArduino[i])
                {
                    leerArduino[i] = false;
                    lecturaArduino(i);
                }
            }
        }
        Thread.Sleep(10);
    }

    

    float yDer = 0;
    float yIzq = 0;
    float yDer1 = 0;
    float yIzq1 = 0;
  
    

    void lecturaArduino(int n)
    {
        try
        {
            string arduinoRead = arduino[n].ReadLine();
            string[] arduinoReadLine = arduinoRead.Split(","[0]);

            if (arduinoReadLine[0] == "Der")
            {
                if (!iniciarDer) { yDer = (float.Parse(arduinoReadLine[1]) - 85.8f); iniciarDer = true; }

                float incremento = 1.05f * (yDer - (float.Parse(arduinoReadLine[1]) - 85.8f));
                anguloDer += incremento;
                okDer++;
                if (maxCalibreDer != 0)
                {
                    incrementoDer = Mathf.Abs(incremento);
                }

                yDer = float.Parse(arduinoReadLine[1]) - 85.8f;

            }

            if (arduinoReadLine[0] == "Izq")
            {
                if (!iniciarIzq) { yIzq = (float.Parse(arduinoReadLine[1]) - 51); iniciarIzq = true; }
                float incremento = 1.8f * (yIzq - (float.Parse(arduinoReadLine[1]) - 51));
                anguloIzq += incremento;
                okIzq++;
                if (maxCalibreIzq != 0)
                {
                    incrementoIzq = Mathf.Abs(incremento);
                }

                yIzq = float.Parse(arduinoReadLine[1]) - 51;
            }

            if (arduinoReadLine[0] == "Der1")
            {
                if (!iniciarDer1) { yDer1 = float.Parse(arduinoReadLine[1]) + 73.5f; iniciarDer1 = true; }
                anguloDer1 += 1.26f * (yDer1 - (float.Parse(arduinoReadLine[1]) + 73.5f));
                yDer1 = float.Parse(arduinoReadLine[1]) + 73.5f;

            }

            if (arduinoReadLine[0] == "Izq1")
            {
                if (!iniciarIzq1) { yIzq1 = float.Parse(arduinoReadLine[1]) - 82.2f; iniciarIzq1 = true; }
                anguloIzq1 += 1.11f * (yIzq1 - (float.Parse(arduinoReadLine[1]) - 82.5f));
                yIzq1 = float.Parse(arduinoReadLine[1]) - 82.5f;
            }


        }
        catch (System.Exception)
        {

        }

        leerArduino[n] = true;

    }


    //:::::::::::::::::::::::::::::::   Cerrar arduino  :::::::::::::::::::::::::::::::::
    bool destruirBool = false;
    void cerrarArduino() {
        Debug.Log("::::::::::::  Closing arduinos  ::::::::::::::");
        for (int i = 0; i < nombreCOM.Length; i++) {
            try
            {
                if (arduino[i].IsOpen)
                {
                    Debug.Log("Closing: "+nombreCOM[i]);
                    arduino[i].Close();
                    Debug.Log(nombreCOM[i]+" closed.");
                }
            }
            catch(System.Exception) {

            }
            
        }

        destruirBool = true;

        thread.Abort();

    }

    void proceso3() {
        proceso = 2;
    }

    bool comprobador = false;
    void Update()
    {
        if (destruirBool) {
            Invoke("destruir",1);
            destruirBool = false;
        }

        if (bluetoothComplete & !comprobador) {
            Debug.Log("OK");
            PlayerPrefs.SetString("COM1", "OK");
            comprobador = true;
        }

    }

    void destruir() {
        crearEstudio.GetComponent<creadorEstudioPanel>().errorBluetooth = true;
        Destroy(this.gameObject);
    }

}
