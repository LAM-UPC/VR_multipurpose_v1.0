using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using System;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class andar : MonoBehaviour {

    
    //Objetos externos
    Animator anim;
    public GameObject personaje;
    public GameObject iluminacion;
    GameObject PaseoInterface;
    bool cerrando = false;

    //MejorasPascual

    public Text veloc;
    public bool delay;
    public string old;
    public string variable;
    public int counter=0;
    public Boolean ini=false;
    public Boolean reset = false;
    public Boolean inirut=false;
    public Image brainimag;
    public Sprite bwait;
    public Sprite btrue;
    public Sprite bfalse;
    public Sprite bneut;
    public GameObject white;
    public GameObject green;
    public GameObject red;
    public GameObject yellow;
    public bool delayml=false;
    public bool aviso = false;
    public bool avisosc = false;
    public bool primertiempo = false;
    public int j=0;
    public string nuevo;
    public GameObject explain;
    public string filename = "C:/VR_instruction/result.txt";
    public string nfsignal="";
    public string vel="";
    
   

    //Posicion inicial personaje
    float posX = 112;
    float posZ = 0f;
    float angulo = 0;
    float radio = 112;

    float vAnimDer = 0;
    float vAnimIzq = 0;

    //Variables de velocidad de avance
    float v = 0;
    public float vAnim=0f;
   

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

        //StartCoroutine(espera(3));
        old = "0";
        white.SetActive(true);
        green.SetActive(false);
        yellow.SetActive(false);
        red.SetActive(false);
        explain.SetActive(true);

       
        

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

    /* LECTURA DE ARDUINOS
     * SOLO SIRVE PARA APLICACIÓN JOSEP LLORENTE
     * O PARA INCORPORAR MÓDULOS ARDUINO
     * NO AUMENTA CONSUMO DE RAM NI DE CPU, NO QUITAR
     * */


    bool arduinosConectats = false;
 

    public float maxCalibreDer = 0;
    public float maxCalibreIzq = 0;
    
    //Lectura asincrona de arduinos (LEE SI HAY ALGÚN MÓDULO ARDUINO_NO QUITAR)  
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

    /*
     * 
     * 
     * 
     * */

    void FixedUpdate()
    {
        acelerometros = false;
        MRI = true;
        
        
        if (MRI)
        {
            
            string filename = ("C:/VR_instruction/NF_signal.txt"); //Ubicación archivo de texto para leer
            try {
                string line;
                StreamReader Reader = new StreamReader(filename);

                using (Reader)
                {
                    //Lectura de las dos primeras lineas
                    variable = Reader.ReadLine();
                    line = Reader.ReadLine();
                    Reader.Close();
                    
                    //Salida de la escena
                    if (variable=="5")
                    {
                        
                        PlayerPrefs.SetString("velocityresult", vel);
                        PlayerPrefs.SetString("signalresult", nfsignal);
                        PlayerPrefs.SetString("distresult", PaseoInterface.GetComponent<paseoInterface>().distancia.ToString());
                        SceneManager.LoadScene("exit");
                    }
                    
                    if (old != line) {
                        
                        if (primertiempo == false)
                        {
                            vAnim = 2f;
                            primertiempo = true;
                            explain.SetActive(false);
                        }
                        if (ini == false)
                        {
                            ini = true;
                            inirut = true;
                            StartCoroutine(espera(90));
                        }
                        white.SetActive(true);
                        green.SetActive(false);
                        yellow.SetActive(false);
                        red.SetActive(false);
                                                

                        if (avisosc == false)
                        {
                            aviso = true;
                            avisosc = true;
                            //Se inicia la temporización de 0.5s (simula el tiempo de adquisición de datos)
                            StartCoroutine(braindelay(0.5f));
                        }
                      
                    }

                    //Pasado el retardo de adquisición leemos el feedback
                    if (delayml==true)
                    {
                        if (variable == "0")
                        {
                                                        
                            white.SetActive(false);
                            green.SetActive(false);
                            yellow.SetActive(true);
                            red.SetActive(false);
                            nfsignal = nfsignal + "0" + ",";
                            vel = vel +  (vAnim * 25).ToString("F0") + "% , ";

                        }

                        if (variable == "1")
                        {
                            white.SetActive(false);
                            green.SetActive(true);
                            yellow.SetActive(false);
                            red.SetActive(false);
                            nfsignal = nfsignal + "1" + ",";


                            if (vAnim < 4f)
                            {
                                vAnim += 0.2f;
                                
                            }
                            vel = vel + (vAnim * 25).ToString("F0") + "% , ";


                        }
                        if (variable == "-1" )
                        {
                                                       
                            white.SetActive(false);
                            green.SetActive(false);
                            yellow.SetActive(false);
                            red.SetActive(true);
                            nfsignal = nfsignal + "-1" + ",";


                            if (vAnim > 0.4f)
                            {
                                vAnim -= 0.2f;
                            }
                            vel = vel +  (vAnim * 25).ToString("F0") + "% , ";

                        }
                        delayml = false;
                                               



                    }

                    old = line;
                    // Al llegar a los 90 segundos de la primera lectura se reinicia y vuelve a empezar

                    if (reset == true)
                    {
                        vAnim = 2f;
                        reset = false;
                    }

                }

             }
                

            catch (System.Exception)
            {

            }

           
            
            
            delay = false;
            
        }

        if (primertiempo == true)
        {
            veloc.text = "Velocidad: " + (vAnim * 25).ToString("F0") + "%";
           
        }
        
       
                /*DESCRIPCIÓN DEL MOVIMIENTO DEL PERSONAJE
                 * SOLO GENERA LA TRAYECTORIA E INCLINACIÓN DEL PERSONAJE
                 * 
                 * */
      
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
                
        
        /*INICIO ACELEROMETROS
         * NO QUITAR QUE ESTÁ VINCULADO A COMPONENTES 
         * DE TRAYECTORIA DEL PERSONAJE
         * ESTO DE MÁS NO AUMENTA EL CONSUMO
         * */
        
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
               
               // vAnim = (vAnimDer > vAnimIzq ? vAnimDer : vAnimIzq);
            }
            else {
                //vAnim = 0;
            }
            
        }
        else {
            //vAnim = 1;
        }
        
        //Control avance personaje
        //v =vAnim/ 23;
        //El personaje describe una circumferencia, la variable angulo representa el avance angular
        //angulo += v;
        int nVueltas = (int)angulo / 360;//Contamos el numero de vuelta para poder restar nVueltas*360 para mantenernos siempre en valores de entre 0-359
        //Definimos el arco k representa el puente y el arco que representa tierra para controlar el sonido
        if ((angulo - nVueltas * 360 > 14) & (angulo - nVueltas * 360 < 28)) { pisada.clip = madera; } else { pisada.clip = tierra; }

        //posicion cartesiana respecto del angulo
        PaseoInterface.GetComponent<paseoInterface>().distancia += vAnim*3/100;
        Debug.Log(PaseoInterface.GetComponent<paseoInterface>().distancia + "m");
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

        /*
         * 
         * 
         * 
         * 
         * 
         * */


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
    
    //TEMPORITZADOR
    IEnumerator espera(int esp)
    {
        while (inirut==true)
        {
            yield return new WaitForSecondsRealtime(esp);
            print(Time.time);
            ini = false;
            reset = true;
            inirut = false;
         }

    }

    IEnumerator braindelay(float wait)
    {
        while (aviso==true)
        {
            Debug.Log("dentro rutina");
            yield return new WaitForSecondsRealtime(wait);
            Debug.Log("sale rutina");
            delayml = true;
            
            avisosc = false;
            Debug.Log(Time.time);
            aviso = false;
        }
    }

}
