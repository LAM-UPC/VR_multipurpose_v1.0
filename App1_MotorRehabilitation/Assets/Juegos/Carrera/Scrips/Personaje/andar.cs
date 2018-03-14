using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using System;
using System.IO;

public class andar : MonoBehaviour {

    //Objetos externos
    Animator anim;
    public GameObject personaje;
    public GameObject iluminacion;
    GameObject PaseoInterface;
    bool cerrando = false;

    //Posicion inicial personaje
    float posX = 112;
    float posZ = 0f;
    float angulo = 0;
    float radio = 112;

    float vAnimDer = 0;
    float vAnimIzq = 0;

    //Variables de velocidad de avance
    float v = 0;
    public float vAnim=0.5f;
   

    //Modos de juego
    bool automatico = false;
    bool acelerometros = false;
    bool MRI = false;

    //Sonidos 
    AudioSource pisada;
    public AudioClip tierra;
    public AudioClip madera;

    //Conexion con arduino
    public SerialPort[] arduino;

    //Variables de arduino
    int n_Arduinos = 4; //Numero de arduinos conectados

    //Ultimo valor de angulo registrado
    float yDer = 1;  //Angulo der sup
    float yIzq = 1;  //Angulo izq sup
    float yDer1 = 1; //Angulo der inf
    float yIzq1 = 1; //Angulo izq inf

    //Recogido dato inicial
    bool iniciarDer = false;
    bool iniciarIzq = false;
    bool iniciarDer1 = false;
    bool iniciarIzq1 = false;

    //Control lectura arduino
    bool[] leerArduino;
    bool dispositivosCargados = false;

    //Angulo resultante
    public float anguloDer = 0;
    public float anguloIzq = 0;
    public float anguloDer1 = 0;
    public float anguloIzq1 = 0;
    string[] nombresCom;
    float vAnimAcumuladoDer = 0;
    float vAnimAcumuladoIzq = 0;
    public Thread thread;

    GameObject acelerometrosObj;

void Start()
    {

        //Obtener componente animator de personaje  
        anim = personaje.GetComponent<Animator>();
        
        PaseoInterface = GameObject.Find("paseoPorElLagoInterface");

        //Acceder a el componente audio source
        pisada = GetComponent<AudioSource>();

        //Definir la iluminacion del juego
        iluminacionJuego(PlayerPrefs.GetInt("AmbienteJuegoAndar"));

        //Definir modo de juego
        modoDeJuego(PlayerPrefs.GetInt("ModoJuegoAndar"));

        if (acelerometros) {
    
            acelerometrosObj = GameObject.Find("Acelerometros(Clone)");

        }
    }

   void leerDispositivos()
    {
        while (true) {
            //Lectura continua
            if (dispositivosCargados)
            {
                if (!cerrando)
                {
                    for (int i = 0; i < n_Arduinos; i++)
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
                else {

                    for (int i = 0; i< n_Arduinos; i++) {
                        arduino[i].Close();

                    }


                }
                

            }

            if (!dispositivosCargados)
            {
                //Definir puerto de arduinos
                arduino[0] = new SerialPort(nombresCom[0], 9600);
                arduino[1] = new SerialPort(nombresCom[1], 9600);
                arduino[2] = new SerialPort(nombresCom[2], 9600);
                arduino[3] = new SerialPort(nombresCom[3], 9600);

                //Abrir arduinos, definir timeout y iniciar leerArduino en false
                for (int i = 0; i < n_Arduinos; i++)
                {
                    arduino[i].Open();
                    arduino[i].ReadTimeout = 5;
                    leerArduino[i] = true;
                }

                dispositivosCargados = true;
                
            }
           
        }
                
    }

    
        
   
    bool arduinosConectats = false;
 

    public float maxCalibreDer = 0;
    public float maxCalibreIzq = 0;
    //Lectura asincrona de arduinos     

    void lecturaArduino(int n) {
        try
        {    
            string arduinoRead = arduino[n].ReadLine();
            string[] arduinoReadLine = arduinoRead.Split(","[0]);

            if (arduinoReadLine[0] == "Der")
            {
                if (!iniciarDer) { yDer = (float.Parse(arduinoReadLine[1])-85.8f); iniciarDer = true; }
                
                float incremento = 1.05f * (yDer - (float.Parse(arduinoReadLine[1])-85.8f));
                anguloDer += incremento;
                if (maxCalibreDer!=0 ) {
                    vAnimAcumuladoDer = Mathf.Abs(incremento);
                }
                
                yDer = float.Parse(arduinoReadLine[1])-85.8f;
                
            }

            if (arduinoReadLine[0] == "Izq") {
                if (!iniciarIzq) { yIzq = (float.Parse(arduinoReadLine[1])-51); iniciarIzq = true; }
                float incremento = 1.8f * (yIzq - (float.Parse(arduinoReadLine[1])-51));
                anguloIzq += incremento;
                if (maxCalibreIzq != 0)
                {
                    vAnimAcumuladoIzq = Mathf.Abs(incremento);
                }

                yIzq = float.Parse(arduinoReadLine[1])-51;
            }

            if (arduinoReadLine[0] == "Der1")
            {
                if (!iniciarDer1) { yDer1 = float.Parse(arduinoReadLine[1]) + 73.5f ; iniciarDer1 = true; }
                anguloDer1 += 1.26f * (yDer1 - (float.Parse(arduinoReadLine[1])+73.5f));
                yDer1 = float.Parse(arduinoReadLine[1])+73.5f;
                

            }

            if (arduinoReadLine[0] == "Izq1")
            {
                if (!iniciarIzq1) { yIzq1 = float.Parse(arduinoReadLine[1])-82.2f; iniciarIzq1= true; }
                anguloIzq1 += 1.11f * (yIzq1 - (float.Parse(arduinoReadLine[1])-82.5f));
                yIzq1 = float.Parse(arduinoReadLine[1])-82.5f;
            }


        }
        catch (System.Exception)
        {
            
        }
       
        leerArduino[n] = true;
    }

    int cntReBluetooth = 0;
    int tryInt = 0;
    float deltaTimerDer = 0;
    float deltaTimerIzq = 0;
    void FixedUpdate()
    {
        if (MRI)
        {

			string filename = ("C:\\Users\\VISYON\\Desktop\\davidhtc\\NF_signal.txt");
            try {
                string line;
                StreamReader Reader = new StreamReader(filename);

                using (Reader)
                {
                    line = Reader.ReadLine();
                    if (line == "0")
                    {
						vAnim = vAnim;
                    }
                    if (line == "1" && vAnim<4)
                    {
						vAnim = vAnim+0.2f;
                    }
					if (line == "-1" && vAnim>0)
                    {
						vAnim = vAnim-1;
                    }
                    Reader.Close();
                    
                }
            }

            catch (System.Exception)
            {

            }

                //vAnim =0 ;
                v = vAnim / 23;
                angulo += v;
                int numVueltas = (int)angulo / 360;
                if((angulo-numVueltas*360>14)&(angulo-numVueltas*360<28))
                { pisada.clip = madera; }
                else { pisada.clip = tierra; }

                PaseoInterface.GetComponent<paseoInterface>().distancia += vAnim * 3 / 100;
                posX = Mathf.Sin(angulo * Mathf.Deg2Rad) * radio;
                posZ = Mathf.Cos(angulo * Mathf.Deg2Rad) * radio;
                transform.position = new Vector3(transform.eulerAngles.x, angulo + 90, transform.eulerAngles.z);
                anim.speed = vAnim;
                
        }
        

        if (acelerometros)
        {
            if (maxCalibreDer != 0 & maxCalibreIzq != 0)
            {
                deltaTimerDer += 0.02f;
                deltaTimerIzq += 0.02f;

                if (acelerometrosObj.GetComponent<acelerometrosScript>().okDer > 0) {
                    vAnimDer = 0.7f * acelerometrosObj.GetComponent<acelerometrosScript>().incrementoDer / ( 2*maxCalibreDer / (1/deltaTimerDer) );
                    deltaTimerDer = 0;
                    acelerometrosObj.GetComponent<acelerometrosScript>().okDer = 0;
                }
                if (acelerometrosObj.GetComponent<acelerometrosScript>().okIzq > 0)
                {
                    vAnimIzq = 0.7f * acelerometrosObj.GetComponent<acelerometrosScript>().incrementoIzq / (2*maxCalibreIzq / (1 / deltaTimerIzq));
                    deltaTimerIzq = 0;
                    acelerometrosObj.GetComponent<acelerometrosScript>().okIzq = 0;
                }
               
                vAnim = (vAnimDer > vAnimIzq ? vAnimDer : vAnimIzq);
            }
            else {
                vAnim = 0;
            }
            
        }
        else {
            vAnim = 1;
        }
        
        //Control avance personaje
        v =vAnim/ 23;
        //El personaje describe una circumferencia, la variable angulo representa el avance angular
        angulo += v;
        int nVueltas = (int)angulo / 360;//Contamos el numero de vuelta para poder restar nVueltas*360 para mantenernos siempre en valores de entre 0-359
        //Definimos el arco k representa el puente y el arco que representa tierra para controlar el sonido
        if ((angulo - nVueltas * 360 > 14) & (angulo - nVueltas * 360 < 28)) { pisada.clip = madera; } else { pisada.clip = tierra; }

        //posicion cartesiana respecto del angulo
        PaseoInterface.GetComponent<paseoInterface>().distancia += vAnim*3/100;
        posX = Mathf.Sin(angulo * Mathf.Deg2Rad) * radio;
        posZ = Mathf.Cos(angulo * Mathf.Deg2Rad) * radio;
        transform.position = new Vector3(posX, 15, posZ);
        //Mantenemos la orientacion del personaje tangente a la trayectoria
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, angulo + 90, transform.eulerAngles.z);

        //velocidad de animacion
        anim.speed =vAnim;

        if (!arduinosConectats)
        {
            PaseoInterface.GetComponent<paseoInterface>().conectantArduinos = !dispositivosCargados;
            arduinosConectats = dispositivosCargados;
        }


    }

    //Deteccion pisada pie derecho
    public void pisadaDerecha(int n)
    {
        if (n==0) { pisada.Play(); }
        
    }

    //Detecion pisada pie izquierdo
    public void pisadaIzquierda(int n)
    {
         
        if (n == 0) { pisada.Play(); }
    }


    void modoDeJuego(int modo)
    {
        switch (modo)
        {
            case 0:
                automatico = true;
                break;
            case 1:
                acelerometros = true;
                break;
            case 2:
                automatico = true;
                break;
            case 3:
                automatico = true;
                break;
            case 4:
                automatico = true;
                break;
            case 5:
                MRI = true;
                break;
                
        }
    }


    //Control de iluminacion
    void iluminacionJuego(int i)
    {
        switch (i)
        {
            case 1:
                iluminacion.transform.position = new Vector3(-37, 14.2f, -297.6f);
                iluminacion.transform.eulerAngles = new Vector3(16, 2, 12.3f);
                iluminacion.GetComponent<Light>().intensity = 0.93f;
                iluminacion.GetComponent<Light>().color = new Color(1, 0.4f, 0, 1);
                break;
        }
    }

    void detenerArduinos()
    {
        if (!dispositivosCargados) {
            thread.Abort();
            for (int i = 0; i < arduino.Length; i++)
            {
                if (arduino[i].IsOpen)
                {
                    arduino[i].Close();
                }
               
                thread = new Thread(leerDispositivos); thread.Start();
            }
            Debug.Log("Nuevo intento");
            Invoke("detenerArduinos", 18);
        }
        
    }

}
