using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Threading;

public class paseoInterface : MonoBehaviour {

    //Relacion de aspecto
    int h = Screen.width;
    int v = Screen.height;
    int vy = Screen.height / 15;

    //Tipo de actividad Automatico/Acelerometros
    int tipoActividad = 0;

    //Temporizador
    int minutosTop = 0;
    int min = 0;
    int seg = 0;
    //Variables de grafico
    int densidadDePuntos = 500;
    float slider = 50; //Posicion de slider
    float slider2 = 50; //Posicion de slider
    float escalaRF = 1;
    float escalaRF2 = 1;

    float bv1 = 0;
    float bv2 = 0;
    bool tipoSeñal = true;
    bool tipoSeñal2 = true;
    bool cerrando = false;

    //Contador de pasos por pierna
    int n_amplitudDer = 0;
    int n_amplitudIzq = 0;

    float cuentaAtras = 5;

    //Valores representativos
    float simetriaMedia = 0;
    float amplitudDer = 0;
    float amplitudIzq = 0;
    float amplitudDerInf = 0;
    float amplitudIzqInf = 0;
    float puntuacion = 0;
    float maxDer = 0;
    float maxIzq = 0;
    float maxDer2 = 0;
    float maxIzq2 = 0;
    float izqDatBefore = 0;
    float derDatBefore = 0;
    float[] DatosDer;
    float[] DatosIzq;
    float[] DatosDer2;
    float[] DatosIzq2;
    public float distancia = 0;
    float tiempoPaso = 0;
    float cnt = 90; //Angulo simulado

    //Calibraje
    float calibreDer = 0;
    float calibreIzq = 0;
    float maxCalibreDer = 1000;
    float maxCalibreIzq = 1000;
    
    //Strings
    string asimetriaResult = "Calculant...";
    string responsable = "";
    string paciente = "";

    //Direccion de las piernas
    bool subiendo1 = false;
    bool subiendo2 = false;

    //Registro de valores medios
    bool valoresMedios = false;

    //Calibreje realizado
    bool calibraje = false;

    //Pantalla principal activa
    bool principal = true;

    //Pantalla de guardado activa
    bool desant = false;

    float max1 = 0;
    float max2 = 0;
    float max12 = 0;
    float max22 = 0;

    //Texturas
    Texture2D grisOscuro;
    Texture2D sombreado;
    Texture2D azul;
    Texture2D blanco;
    Texture2D azulOscuro;
    Texture2D azulOscuros;
    Texture2D rojo;
    Texture2D azulUltra;
    Texture2D verdeUltra;
    public Texture2D sombra;
    public Texture2D botonGris;
    public Texture2D indi;
    public Texture2D indiBN;
    public Texture2D sola;
    public Texture2D solaBN;   
    public Texture2D cuadricula;    
    public Texture2D pNeutra;
    public Texture2D pDerecha;
    public Texture2D pIzquierda;
    public Texture2D[] colaboradores;
    public RenderTexture camaraOnBoard;
    public Texture botonImg;
    public Texture botonImg2;

    string puntuacioFinal = "";
    string distanciaFinal = "";
    string asimetriaFinal = "";
    string anguloDerInfFinal = "";
    string anguloIzqInfFinal = "";
    string anguloDerFinal = "";
    string anguloIzqFinal = "";

    //Lectura del objeto personaje
    GameObject personaje;
    GameObject acelerometrosObj;
    //Lectura de la animacion del personaje
    public GameObject animacion;

    public bool conectantArduinos = true;

    Texture2D transparente;

    void Start()
    {

        //Texturas
        grisOscuro = new Texture2D(1, 1);
        grisOscuro.SetPixel(0, 0, new Color(0.1f, 0.1f, 0.3f, 1));
        grisOscuro.Apply();

        sombreado = new Texture2D(1, 1);
        sombreado.SetPixel(0, 0, new Color(0.1f, 0.1f, 0.1f, 0.5f));
        sombreado.Apply();

        transparente = new Texture2D(1, 1);
        transparente.SetPixel(0, 0, new Color(0.1f, 0.1f, 0.1f, 0));
        transparente.Apply();

        azul = new Texture2D(1, 1);
        azul.SetPixel(0, 0, new Color(0, 0.6f, 0.7f, 1));
        azul.Apply();

        blanco = new Texture2D(1, 1);
        blanco.SetPixel(0, 0, new Color(1, 1, 1, 1));
        blanco.Apply();

        azulOscuro = new Texture2D(1, 1);
        azulOscuro.SetPixel(0, 0, new Color(0.7f, 0.7f, 0.7f, 0.95f));
        azulOscuro.Apply();

        azulOscuros = new Texture2D(1, 1);
        azulOscuros.SetPixel(0, 0, new Color(0, 0.15f, 0.175f, 1));
        azulOscuros.Apply();

        rojo = new Texture2D(1, 1);
        rojo.SetPixel(0, 0, new Color(1, 0, 0, 1));
        rojo.Apply();

        azulUltra = new Texture2D(1, 1);
        azulUltra.SetPixel(0, 0, new Color(0, 1, 1, 1));
        azulUltra.Apply();

        verdeUltra = new Texture2D(1, 1);
        verdeUltra.SetPixel(0, 0, new Color(0, 1, 0, 1));
        verdeUltra.Apply();

      
        personaje = GameObject.Find("Personaje");
        tipoActividad = PlayerPrefs.GetInt("ModoJuegoAndar");
        minutosTop = 20;//Tiempo de ejercicio
        responsable = PlayerPrefs.GetString("Usuario");
        paciente = PlayerPrefs.GetString("Paciente");

        if (tipoActividad == 0)
        {
            conectantArduinos = false;
        }
        else {
            acelerometrosObj = GameObject.Find("Acelerometros(Clone)");
        }
       


        DatosDer = new float[densidadDePuntos];
        DatosIzq = new float[densidadDePuntos];
        DatosDer2 = new float[densidadDePuntos];
        DatosIzq2 = new float[densidadDePuntos];

        slider = (int)(densidadDePuntos * 0.5f);
        slider2 = (int)(densidadDePuntos * 0.5f);

       
        
    }

    //::::::::::::::::::::::::::::::::::::::::::::  Interface ::::::::::::::::::::::::::::::::::::::::::::
    bool camaraBool = false;
    void OnGUI()
    {
        //Capa de renderizado
        GUI.depth = 12;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;

        //Fondo
        

        //Pantalla principal
        if (principal)
        {
            if (!camaraBool)
            {
                GUI.DrawTexture(new Rect(0, 0, h, v), azulOscuro);
                //Camara onboard
                GUI.DrawTexture(new Rect(0.5f * vy, 8 * vy, 16.5f * vy, 7f * vy), sombreado);

                GUI.DrawTexture(new Rect(vy, 13.7f * vy, 0.8f * vy, 0.8f * vy), botonImg);
                GUI.skin.button.normal.background = transparente;
                GUI.skin.button.active.background = transparente;
                GUI.skin.button.hover.background = transparente;
                if (GUI.Button(new Rect(vy, 13.7f * vy, 0.8f * vy, 0.8f * vy), ""))
                {
                    camaraBool = true;
                }

                GUI.skin.button.normal.background = botonGris;
                GUI.skin.button.active.background = botonGris;
                GUI.skin.button.hover.background = botonGris;

                if (calibraje)
                {
                    GUI.DrawTexture(new Rect(5.4f * vy, 8.5f * vy, 11.11f * vy, 6.25f * vy), camaraOnBoard);

                    if (cuentaAtras > 0)
                    {

                        GUI.DrawTexture(new Rect(5.4f * vy, 8.5f * vy, 11.11f * vy, 6.25f * vy), sombreado);
                        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                        GUI.skin.label.fontSize = (int)(2.5f * vy);
                        GUI.skin.label.normal.textColor = Color.white;
                        GUI.Label(new Rect(5.4f * vy, 8.5f * vy, 11.11f * vy, 6.25f * vy), ((int)(cuentaAtras)).ToString());

                    }

                }
                else

                //::::::::::::::::::::::  Calibraje de acelerometros  :::::::::::::::::::::::::::::

                {
                    GUI.DrawTexture(new Rect(5 * vy, 8.5f * vy, 11 * vy, 6 * vy), sombreado);
                    GUI.skin.label.fontSize = (int)(0.4f * vy);
                    if (calibreDer == 0 & calibreIzq == 0)
                    {
                        //Calibraje posicion neutra
                        GUI.Label(new Rect(5.5f * vy, 8.5f * vy, 11.11f * vy, vy), "Calibrar posició neutra");
                        GUI.DrawTexture(new Rect(12 * vy, 9 * vy, 3.33f * vy, 5 * vy), pNeutra);
                        if (GUI.Button(new Rect(5.5f * vy, 13 * vy, 3.5f * vy, vy), "Ok!"))
                        {
                            calibreDer = -DatosDer[densidadDePuntos - 1] * escalaRF;
                            calibreIzq = -DatosIzq[densidadDePuntos - 1] * escalaRF;
                        }
                    }
                    else
                    {
                        if (maxCalibreDer == 1000)
                        {
                            GUI.Label(new Rect(5.5f * vy, 8.5f * vy, 11.11f * vy, vy), "Calibrar cama detra");
                            GUI.DrawTexture(new Rect(12 * vy, 9 * vy, 3.33f * vy, 5 * vy), pDerecha);
                            if (GUI.Button(new Rect(5.5f * vy, 13 * vy, 3.5f * vy, vy), "Ok!"))
                            {
                                maxCalibreDer = Mathf.Abs(DatosDer[densidadDePuntos - 1] * escalaRF);
                                personaje.GetComponent<andar>().maxCalibreDer = maxCalibreDer;
                                if (tipoActividad == 1)
                                {
                                    acelerometrosObj.GetComponent<acelerometrosScript>().maxCalibreDer = maxCalibreDer;
                                }

                                Debug.Log("Derecha: " + maxCalibreDer.ToString());
                            }
                        }
                        else
                        {
                            GUI.Label(new Rect(5.5f * vy, 8.5f * vy, 11.11f * vy, vy), "Calibrar cama esquerra");
                            GUI.DrawTexture(new Rect(12 * vy, 9 * vy, 3.33f * vy, 5 * vy), pIzquierda);
                            if (GUI.Button(new Rect(8.5f * vy, 13 * vy, 3.5f * vy, vy), "Ok!"))
                            {
                                maxCalibreIzq = Mathf.Abs(DatosIzq[densidadDePuntos - 1] * escalaRF);
                                personaje.GetComponent<andar>().maxCalibreIzq = maxCalibreIzq;
                                if (tipoActividad == 1)
                                {
                                    acelerometrosObj.GetComponent<acelerometrosScript>().maxCalibreIzq = maxCalibreIzq;
                                }
                                calibraje = true;
                                Debug.Log("Izquierda: " + maxCalibreIzq.ToString());
                                min = 0;
                                seg = 0;

                            }
                        }
                    }
                }

                GUI.skin.label.alignment = TextAnchor.MiddleLeft;

                //Datos del estudio
                GUI.skin.label.fontSize = (int)(0.35f * vy);
                GUI.skin.label.fontSize = (int)(0.35f * vy);
                GUI.DrawTexture(new Rect(0.5f * vy, 0.25f * vy, 13f * vy, 1.1f * vy), sombreado);
                GUI.Label(new Rect(1 * vy, 0.05f * vy, 12 * vy, vy), "Responsable:  " + responsable);
                GUI.Label(new Rect(1 * vy, 0.55f * vy, 12 * vy, vy), "Pacient:           " + paciente);

                //Boton finazalizar proyecto
                if (GUI.Button(new Rect(23 * vy, v - vy, 3 * vy, 0.7f * vy), "Finalitzar"))
                {
                    SceneManager.LoadScene("principal");
                }

                //Titulo pagina
                GUI.skin.label.fontSize = (int)(0.75f * vy);
                GUI.Label(new Rect(h - 9.5f * vy, 0.25f * vy, 9 * vy, vy), "Cinemàtiques");
                GUI.DrawTexture(new Rect(h - 9.5f * vy, 1.25f * vy, 9 * vy, 0.05f * vy), blanco);
                float sl = 1.25f * vy;
                GUI.skin.label.fontSize = (int)(0.35f * vy);
                GUI.skin.label.normal.textColor = Color.white;

                //Información recorrido
                GUI.Label(new Rect(0.5f * vy, 1.6f * vy, 7f * vy, 0.5f * vy), "Recorregut:");
                GUI.DrawTexture(new Rect(0.5f * vy, 2.2f * vy, 7f * vy, 5f * vy), sombreado);
                if (minutosTop > 0)
                {
                    GUI.Label(new Rect(vy, 2.2f * vy, 6 * vy, vy), "Temps:");
                    GUI.Label(new Rect(vy, 2.7f * vy, 6 * vy, vy), min.ToString() + ":" + (seg < 10 ? "0" + seg.ToString() : seg.ToString()) + " de " + minutosTop.ToString() + " min.");
                    sl += 0.5f * vy;
                }

                GUI.Label(new Rect(vy, 3.5f * vy, 7.5f * vy, vy), "Distància:");
                GUI.Label(new Rect(vy, 4f * vy, 7.5f * vy, vy), distancia.ToString("F2") + "m");
                sl += 0.5f * vy;


                if (minutosTop > 0)
                {
                    GUI.Label(new Rect(vy, 4.9f * vy, 7.5f * vy, 0.75f * vy), "Progrés:");
                    float porcentaje1 = 0;
                    if (minutosTop > 0) { porcentaje1 = (min + seg / 60f) / minutosTop * 1f; }
                    GUI.DrawTexture(new Rect(vy, 5.7f * vy, 5.7f * vy, 0.45f * vy), azulOscuros);
                    GUI.DrawTexture(new Rect(1.05f * vy, 5.75f * vy, 5.6f * vy * porcentaje1, 0.35f * vy), azul);
                    sl += vy;
                    if (porcentaje1 == 1)
                    {
                        if (tipoActividad == 0)
                        {
                            SceneManager.LoadScene("principal");
                        }
                        else
                        {
                            salir();
                        }

                    }
                }

                sl = 2 * vy;
                if (tipoActividad == 1 | tipoActividad == 0)
                {
                    GUI.Label(new Rect(h - 9.5f * vy, sl - 0.7f * vy, 6.5f * vy, vy), "Dades amplituds inferiors:");
                    sl += 0.7f * vy;

                    //:::::::::::Grafica
                    float alturaCuadricula = 3 * vy;
                    GUI.DrawTexture(new Rect(h - 9.5f * vy, sl - 0.5f * vy, 9 * vy, 5 * vy), sombreado);
                    if (GUI.Button(new Rect(h - 9 * vy, sl + 3.3f * vy, 0.8f * vy, 0.8f * vy), "")) { tipoSeñal = false; }
                    GUI.DrawTexture(new Rect(h - 9 * vy, sl + 3.3f * vy, 0.8f * vy, 0.8f * vy), (tipoSeñal ? solaBN : sola));
                    if (GUI.Button(new Rect(h - 8 * vy, sl + 3.3f * vy, 0.8f * vy, 0.8f * vy), "")) { tipoSeñal = true; }
                    GUI.DrawTexture(new Rect(h - 8 * vy, sl + 3.3f * vy, 0.8f * vy, 0.8f * vy), (tipoSeñal ? indi : indiBN));

                    GUI.skin.horizontalSlider.fixedHeight = 0.3f * vy;
                    GUI.skin.horizontalSliderThumb.fixedHeight = 0.32f * vy;
                    GUI.skin.horizontalSliderThumb.fixedWidth = 0.3f * vy;
                    GUI.skin.label.fontSize = (int)(0.3f * vy);
                    GUI.Label(new Rect(h - 6.4f * vy, sl + 3.5f * vy, 4 * vy, 0.6f * vy), "Fase:");
                    slider = GUI.HorizontalSlider(new Rect(h - 5.5f * vy, sl + 3.7f * vy, 4 * vy, 0.6f * vy), slider, 0, densidadDePuntos * 0.5f);
                    int ajuste = (int)(densidadDePuntos * 0.5f) - (int)slider;

                    GUI.DrawTexture(new Rect(h - 9f * vy, sl, 7 * vy, 3 * vy), cuadricula);

                    GUI.skin.label.fontSize = (int)(1.2f * vy);
                    GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                    GUI.skin.label.normal.textColor = new Color(0, 1, 1, 0.15f);
                    GUI.Label(new Rect(h - 9f * vy, sl, 7 * vy, 1.5f * vy), "Dreta");
                    GUI.skin.label.normal.textColor = new Color(0, 1, 0, 0.15f);
                    GUI.Label(new Rect(h - 9f * vy, sl + 1.5f * vy, 7 * vy, 1.5f * vy), "Esquerra");


                    GUI.skin.label.fontSize = (int)(0.25f * vy);
                    GUI.skin.label.normal.textColor = Color.red;
                    GUI.skin.label.alignment = TextAnchor.MiddleRight;

                    float fc = 100f / densidadDePuntos;

                    if (tipoSeñal)
                    {
                        GUI.DrawTexture(new Rect(h - 9 * vy, sl + 0.5f * alturaCuadricula - DatosDer2[densidadDePuntos - 1] * alturaCuadricula * 0.4f, 8f * vy, 0.02f * vy), rojo);
                        GUI.Label(new Rect(h - 3 * vy, sl - 0.4f * vy + 0.5f * alturaCuadricula - DatosDer2[densidadDePuntos - 1] * alturaCuadricula * 0.4f, 2f * vy, 0.5f * vy), (DatosDer2[densidadDePuntos - 1] * escalaRF2).ToString("F2"));
                        GUI.DrawTexture(new Rect(h - 9 * vy, sl + alturaCuadricula - DatosIzq2[densidadDePuntos - ajuste - 1] * alturaCuadricula * 0.4f, 8 * vy, 0.02f * vy), rojo);
                        GUI.Label(new Rect(h - 3 * vy, sl - 0.4f * vy + alturaCuadricula - DatosIzq2[densidadDePuntos - ajuste - 1] * alturaCuadricula * 0.4f, 2f * vy, 0.5f * vy), (DatosIzq2[densidadDePuntos - ajuste - 1] * escalaRF2).ToString("F2"));
                        for (int i = 0; i < densidadDePuntos; i++)
                        {
                            GUI.DrawTexture(new Rect(h - 9 * vy + i * 0.07f * fc * vy, sl + 0.5f * alturaCuadricula - DatosDer2[i] * alturaCuadricula * 0.4f, 0.07f * vy, 0.07f * vy), azulUltra);
                            GUI.DrawTexture(new Rect(h - 9 * vy + i * 0.07f * fc * vy, sl + alturaCuadricula - (i - ajuste > 0 ? DatosIzq2[i - ajuste] * alturaCuadricula * 0.4f : 0), 0.07f * vy, 0.07f * vy), verdeUltra);
                        }
                    }
                    else
                    {
                        GUI.DrawTexture(new Rect(h - 9 * vy, sl + alturaCuadricula - DatosDer2[densidadDePuntos - 1] * alturaCuadricula * 0.95f, 8.3f * vy, 0.02f * vy), rojo);
                        GUI.Label(new Rect(h - 2.7f * vy, sl - 0.4f * vy + alturaCuadricula - DatosDer2[densidadDePuntos - 1] * alturaCuadricula * 0.95f, 2f * vy, 0.5f * vy), (DatosDer2[densidadDePuntos - 1] * escalaRF2).ToString("F2"));
                        GUI.DrawTexture(new Rect(h - 9 * vy, sl + alturaCuadricula - DatosIzq2[densidadDePuntos - ajuste - 1] * alturaCuadricula * 0.95f, 7.7f * vy, 0.02f * vy), rojo);
                        GUI.Label(new Rect(h - 3.3f * vy, sl - 0.4f * vy + alturaCuadricula - DatosIzq2[densidadDePuntos - ajuste - 1] * alturaCuadricula * 0.95f, 2f * vy, 0.5f * vy), (DatosIzq2[densidadDePuntos - ajuste - 1] * escalaRF2).ToString("F2"));
                        for (int i = 0; i < densidadDePuntos; i++)
                        {
                            GUI.DrawTexture(new Rect(h - 9 * vy + i * 0.07f * fc * vy, sl + alturaCuadricula - DatosDer2[i] * alturaCuadricula * 0.95f, 0.07f * vy, 0.07f * vy), azulUltra);
                            GUI.DrawTexture(new Rect(h - 9 * vy + i * 0.07f * fc * vy, sl + alturaCuadricula - (i - ajuste > 0 ? DatosIzq2[i - ajuste] * alturaCuadricula * 0.95f : 0), 0.07f * vy, 0.07f * vy), verdeUltra);
                        }
                    }

                    //:::::::::::Grafica
                    float move = 9.5f * vy;
                    float movey = 0 * vy;
                    GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                    GUI.skin.label.normal.textColor = Color.white;
                    GUI.skin.label.fontSize = (int)(0.35f * vy);
                    GUI.Label(new Rect(h - move - 9.5f * vy, sl + movey - 1.35f * vy, 6.5f * vy, vy), "Dades amplituds:");
                    GUI.DrawTexture(new Rect(-move + h - 9.5f * vy, sl + movey - 0.5f * vy, 9 * vy, 5 * vy), sombreado);
                    if (GUI.Button(new Rect(-move + h - 9 * vy, sl + movey + 3.3f * vy, 0.8f * vy, 0.8f * vy), "")) { tipoSeñal2 = false; }
                    GUI.DrawTexture(new Rect(-move + h - 9 * vy, sl + movey + 3.3f * vy, 0.8f * vy, 0.8f * vy), (tipoSeñal2 ? solaBN : sola));
                    if (GUI.Button(new Rect(-move + h - 8 * vy, sl + movey + 3.3f * vy, 0.8f * vy, 0.8f * vy), "")) { tipoSeñal2 = true; }
                    GUI.DrawTexture(new Rect(-move + h - 8 * vy, sl + movey + 3.3f * vy, 0.8f * vy, 0.8f * vy), (tipoSeñal2 ? indi : indiBN));

                    GUI.skin.horizontalSlider.fixedHeight = 0.3f * vy;
                    GUI.skin.horizontalSliderThumb.fixedHeight = 0.32f * vy;
                    GUI.skin.horizontalSliderThumb.fixedWidth = 0.3f * vy;

                    GUI.skin.label.fontSize = (int)(0.3f * vy);
                    GUI.Label(new Rect(-move + h - 6.4f * vy, sl + movey + 3.5f * vy, 4 * vy, 0.6f * vy), "Fase:");
                    slider2 = GUI.HorizontalSlider(new Rect(-move + h - 5.5f * vy, sl + movey + 3.7f * vy, 4 * vy, 0.6f * vy), slider2, 0, densidadDePuntos * 0.5f);
                    ajuste = (int)(densidadDePuntos * 0.5f) - (int)slider2;

                    GUI.DrawTexture(new Rect(-move + h - 9f * vy, sl + movey, 7 * vy, 3 * vy), cuadricula);

                    GUI.skin.label.fontSize = (int)(1.2f * vy);
                    GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                    GUI.skin.label.normal.textColor = new Color(0, 1, 1, 0.15f);
                    GUI.Label(new Rect(-move + h - 9f * vy, sl + movey, 7 * vy, 1.5f * vy), "Dreta");
                    GUI.skin.label.normal.textColor = new Color(0, 1, 0, 0.15f);
                    GUI.Label(new Rect(-move + h - 9f * vy, sl + movey + 1.5f * vy, 7 * vy, 1.5f * vy), "Esquerra");


                    GUI.skin.label.fontSize = (int)(0.25f * vy);
                    GUI.skin.label.normal.textColor = Color.red;
                    GUI.skin.label.alignment = TextAnchor.MiddleRight;


                    if (tipoSeñal2)
                    {
                        GUI.DrawTexture(new Rect(-move + h - 9 * vy, sl + movey + 0.5f * alturaCuadricula - DatosDer[densidadDePuntos - 1] * alturaCuadricula * 0.4f, 8f * vy, 0.02f * vy), rojo);
                        GUI.Label(new Rect(-move + h - 3 * vy, sl + movey - 0.4f * vy + 0.5f * alturaCuadricula - DatosDer[densidadDePuntos - 1] * alturaCuadricula * 0.4f, 2f * vy, 0.5f * vy), (DatosDer[densidadDePuntos - 1] * escalaRF).ToString("F2"));
                        GUI.DrawTexture(new Rect(-move + h - 9 * vy, sl + movey + alturaCuadricula - DatosIzq[densidadDePuntos - ajuste - 1] * alturaCuadricula * 0.4f, 8 * vy, 0.02f * vy), rojo);
                        GUI.Label(new Rect(-move + h - 3 * vy, sl + movey - 0.4f * vy + alturaCuadricula - DatosIzq[densidadDePuntos - ajuste - 1] * alturaCuadricula * 0.4f, 2f * vy, 0.5f * vy), (DatosIzq[densidadDePuntos - ajuste - 1] * escalaRF).ToString("F2"));
                        for (int i = 0; i < densidadDePuntos; i++)
                        {
                            GUI.DrawTexture(new Rect(-move + h - 9 * vy + i * 0.07f * fc * vy, sl + movey + 0.5f * alturaCuadricula - DatosDer[i] * alturaCuadricula * 0.4f, 0.07f * vy, 0.07f * vy), azulUltra);
                            GUI.DrawTexture(new Rect(-move + h - 9 * vy + i * 0.07f * fc * vy, sl + movey + alturaCuadricula - (i - ajuste > 0 ? DatosIzq[i - ajuste] * alturaCuadricula * 0.4f : 0), 0.07f * vy, 0.07f * vy), verdeUltra);
                        }
                    }
                    else
                    {
                        GUI.DrawTexture(new Rect(-move + h - 9 * vy, sl + movey + alturaCuadricula - DatosDer[densidadDePuntos - 1] * alturaCuadricula * 0.95f, 8.3f * vy, 0.02f * vy), rojo);
                        GUI.Label(new Rect(-move + h - 2.7f * vy, sl + movey - 0.4f * vy + alturaCuadricula - DatosDer[densidadDePuntos - 1] * alturaCuadricula * 0.95f, 2f * vy, 0.5f * vy), (DatosDer[densidadDePuntos - 1] * escalaRF).ToString("F2"));
                        GUI.DrawTexture(new Rect(-move + h - 9 * vy, sl + movey + alturaCuadricula - DatosIzq[densidadDePuntos - ajuste - 1] * alturaCuadricula * 0.95f, 7.7f * vy, 0.02f * vy), rojo);
                        GUI.Label(new Rect(-move + h - 3.3f * vy, sl + movey - 0.4f * vy + alturaCuadricula - DatosIzq[densidadDePuntos - ajuste - 1] * alturaCuadricula * 0.95f, 2f * vy, 0.5f * vy), (DatosIzq[densidadDePuntos - ajuste - 1] * escalaRF).ToString("F2"));
                        for (int i = 0; i < densidadDePuntos; i++)
                        {
                            GUI.DrawTexture(new Rect(-move + h - 9 * vy + i * 0.07f * fc * vy, sl + movey + alturaCuadricula - DatosDer[i] * alturaCuadricula * 0.95f, 0.07f * vy, 0.07f * vy), azulUltra);
                            GUI.DrawTexture(new Rect(-move + h - 9 * vy + i * 0.07f * fc * vy, sl + movey + alturaCuadricula - (i - ajuste > 0 ? DatosIzq[i - ajuste] * alturaCuadricula * 0.95f : 0), 0.07f * vy, 0.07f * vy), verdeUltra);
                        }
                    }

                    GUI.skin.label.fontSize = (int)(0.35f * vy);
                    GUI.skin.label.normal.textColor = Color.white;
                    GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                    sl += 5.3f * vy;

                    //:::::::::::Datos
                    GUI.Label(new Rect(h - 9.5f * vy, sl - 0.5f * vy, 6.5f * vy, 0.5f * vy), "Dades globals");
                    GUI.DrawTexture(new Rect(h - 9.5f * vy, sl, 9 * vy, 5f * vy), sombreado);
                    sl += 0.5f * vy;
                    GUI.Label(new Rect(h - 9 * vy, sl, 7.5f * vy, vy), "Puntuació:");
                    GUI.Label(new Rect(h - 5 * vy, sl, 7.5f * vy, vy), puntuacion.ToString("F2"));
                    sl += 0.7f * vy;
                    GUI.Label(new Rect(h - 9 * vy, sl, 7.5f * vy, vy), "Asimetria:");
                    GUI.Label(new Rect(h - 5 * vy, sl, 7.5f * vy, vy), asimetriaResult + " %");
                    sl += 0.7f * vy;
                    GUI.Label(new Rect(h - 9 * vy, sl, 7.5f * vy, vy), "Amplitud dreta:");
                    GUI.Label(new Rect(h - 5 * vy, sl, 7.5f * vy, vy), (amplitudDer).ToString("F2") + " º");
                    sl += 0.7f * vy;
                    GUI.Label(new Rect(h - 9 * vy, sl, 7.5f * vy, vy), "Amplitud esquerra:");
                    GUI.Label(new Rect(h - 5 * vy, sl, 7.5f * vy, vy), (amplitudIzq).ToString("F2") + " º");
                    sl += 0.7f * vy;
                    GUI.Label(new Rect(h - 9 * vy, sl, 7.5f * vy, vy), "Amplitud dreta inf.:");
                    GUI.Label(new Rect(h - 5 * vy, sl, 7.5f * vy, vy), (amplitudDerInf).ToString("F2") + " º");
                    sl += 0.7f * vy;
                    GUI.Label(new Rect(h - 9 * vy, sl, 7.5f * vy, vy), "Amplitud esquerra inf.:");
                    GUI.Label(new Rect(h - 5 * vy, sl, 7.5f * vy, vy), (amplitudIzqInf).ToString("F2") + " º");
                    sl += 0.5f * vy;
                }

                puntuacioFinal = puntuacion.ToString("F2");
                distanciaFinal = distancia.ToString("F2");
                asimetriaFinal = asimetriaResult;
                anguloDerFinal = (amplitudDer).ToString("F2");
                anguloIzqFinal = (amplitudIzq).ToString("F2");
                anguloDerInfFinal = (amplitudDerInf).ToString("F2");
                anguloIzqInfFinal = (amplitudIzqInf).ToString("F2");
            }
            else {
                GUI.DrawTexture(new Rect(vy, 13.7f * vy, 0.8f * vy, 0.8f * vy), botonImg2);
                GUI.skin.button.normal.background = transparente;
                GUI.skin.button.active.background = transparente;
                GUI.skin.button.hover.background = transparente;
                if (GUI.Button(new Rect(vy, 13.7f * vy, 0.8f * vy, 0.8f * vy), ""))
                {
                    camaraBool = false;
                }
            }
            


        }

        //::::::::::::::::::::::::::::::::::  Estudio acabado :::::::::::::::::::::::::::::
        else {
            GUI.DrawTexture(new Rect(0, 0, h, v), azulOscuro);
            GUI.DrawTexture(new Rect(0.5f * h - 12 * vy, 0.5f * v - 4 * vy, 24 * vy, 8 * vy), sombreado);
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                GUI.skin.label.fontSize = (int)(0.6f * vy);
                GUI.Label(new Rect(0.5f * h - 11.5f * vy, 0.5f * v - 3.5f * vy, 23 * vy, 1.5f * vy), "Estudi finalitzat correctament");
                GUI.skin.label.fontSize = (int)(0.4f * vy);
                if (desant)
                {
                    GUI.Label(new Rect(0.5f * h - 11.5f * vy, 0.5f * v - 2 * vy, 23 * vy, 4 * vy), "Desant...");
                }
                else
                {
                       
                        string result = "Resultats:\n\n" +
                           "Puntuació: " + puntuacioFinal + "        " +
                           "Distància: " + distanciaFinal + "m        " +
                           "Asimetria: " + asimetriaFinal + "%\n\n" +
                           "Amplitud Dre.Sup.: " + anguloDerFinal + "º      " +
                           "Amplitud Esq.Sup.: " + anguloIzqFinal + "º      " +
                           "Amplitud Dre.Inf.: " + anguloDerInfFinal + "º      " +
                           "Amplitud Esq.Inf.: " + anguloIzqInfFinal + "º";

                        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                        GUI.Label(new Rect(0.5f * h - 11.5f * vy, 0.5f * v - 2 * vy, 23 * vy, 4 * vy), result);

                        if (GUI.Button(new Rect(0.5f * h - 5 * vy, 0.5f * v + 2.5f * vy, 4 * vy, vy), "Eliminar"))
                        {
                            SceneManager.LoadScene("principal");
                        }
                        if (GUI.Button(new Rect(0.5f * h + vy, 0.5f * v + 2.5f * vy, 4 * vy, vy), "Desar"))
                        {
                            StartCoroutine(SubirDatos());
                            desant = true;
                        }
                    }
                             
        }
                
    }


 


    void procesarDatos()
    {
            //Dades rebudes per l'acceloremetre
            float derDat = 0;
            float izqDat = 0;
            float derDat2 = 0;
            float izqDat2 = 0;

            if (tipoActividad == 0)//Simulació de dades
            {
                derDat = (Mathf.Cos(cnt * Mathf.Deg2Rad) * 50 >= 0 ? Mathf.Cos(cnt * Mathf.Deg2Rad) * 50 : 0);
                izqDat = (Mathf.Cos((cnt + 180) * Mathf.Deg2Rad) * 50 >= 0 ? Mathf.Cos((cnt + 180) * Mathf.Deg2Rad) * 50 : 0);
                derDat2 = 0.8f * derDat;
                izqDat2 = 0.8f * izqDat;
                cnt += 6;
            }

            if (tipoActividad == 1)//Leer valores del acelerometro
            {
            //Debug.Log("Ieee");
                derDat = Mathf.Abs(acelerometrosObj.GetComponent<acelerometrosScript>().anguloDer - calibreDer);
                izqDat = Mathf.Abs(acelerometrosObj.GetComponent<acelerometrosScript>().anguloIzq - calibreIzq);
                derDat2 = Mathf.Abs(derDat - Mathf.Abs(acelerometrosObj.GetComponent<acelerometrosScript>().anguloDer1));
                izqDat2 = Mathf.Abs(izqDat - Mathf.Abs(acelerometrosObj.GetComponent<acelerometrosScript>().anguloIzq1));
           // Debug.Log(derDat.ToString());
        }

            //Guardar valores maximos   
            maxDer = (derDat > maxDer ? derDat : maxDer);
            maxIzq = (izqDat > maxIzq ? izqDat : maxIzq);
            maxDer2 = (derDat2 > maxDer2 ? derDat2 : maxDer2);
            maxIzq2 = (izqDat2 > maxIzq2 ? izqDat2 : maxIzq2);

            //Escala de graficos
            float scalaBefore = escalaRF;
            float scalaBefore2 = escalaRF2;
            escalaRF = (maxDer > maxIzq ? maxDer : maxIzq);
            escalaRF2 = (maxDer2 > maxIzq2 ? maxDer2 : maxIzq2);


            //Reescalar datos
            derDat = derDat / escalaRF;
            izqDat = izqDat / escalaRF;
            derDat2 = derDat2 / escalaRF2;
            izqDat2 = izqDat2 / escalaRF2;

            if (scalaBefore != escalaRF)
            {
                reescalarDatos((scalaBefore / escalaRF));
            }
            if (scalaBefore2 != escalaRF2)
            {
                reescalarDatos2((scalaBefore2 / escalaRF2));
            }

        int precisionAngulo = 10; //Numero de interpolaciones

        //Acciones una vez calibrado el sistema
        if (calibraje)
            {


            max1 = (max1 < Mathf.Abs(derDat * escalaRF) ? Mathf.Abs(derDat * escalaRF) : max1);
            max2 = (max2 < Mathf.Abs(izqDat * escalaRF) ? Mathf.Abs(izqDat * escalaRF) : max2);
            max12 = (max12 < derDat2 * escalaRF2 ? derDat2 * escalaRF2 : max12);
            max22 = (max22 < izqDat2 * escalaRF2 ? izqDat2 * escalaRF2 : max22);
            if (Mathf.Abs( derDat*escalaRF) < 20 & Mathf.Abs( max1 )>20) {
                
                if (valoresMedios)
                { //Calculo amplitud maxima media
                    amplitudDer = (n_amplitudDer * amplitudDer + max1) / (n_amplitudDer + 1);
                    amplitudDerInf = (n_amplitudDer * amplitudDerInf + max12) / (n_amplitudDer + 1);
                    n_amplitudDer++;
                    calculoSimetria();
                }

                max1 = 0;
                max12 = 0;
            }
            if (Mathf.Abs( izqDat*escalaRF) < 20 & Mathf.Abs(max2) > 20)
            {

                if (valoresMedios)
                { //Calculo amplitud maxima media
                    amplitudIzq = (n_amplitudIzq * amplitudIzq + max2) / (n_amplitudIzq + 1);
                    amplitudIzqInf = (n_amplitudIzq * amplitudIzqInf + max22) / (n_amplitudIzq + 1);
                    n_amplitudIzq++;
                    calculoSimetria();
                }

                max2 = 0;
                max22 = 0;
            }


            if (DatosDer[densidadDePuntos - precisionAngulo] < derDat) { subiendo1 = true;} else { subiendo1 = false;  }
                if (DatosIzq[densidadDePuntos - precisionAngulo] < izqDat) { subiendo2 = true; } else { subiendo2 = false; }


            }

            

            //Desplazar valores a n-1
            for (int i = 0; i < densidadDePuntos - precisionAngulo; i++)
            {
                DatosDer[i] = DatosDer[i + precisionAngulo];
                DatosIzq[i] = DatosIzq[i + precisionAngulo];
                DatosDer2[i] = DatosDer2[i + precisionAngulo];
                DatosIzq2[i] = DatosIzq2[i + precisionAngulo];
            }

            //Interpolacion angular
            interpolarAngulos(derDat, izqDat, precisionAngulo);
            interpolarAngulos2(derDat2, izqDat2, precisionAngulo);
               
    }

    void calculoSimetria() {

        simetriaMedia = (100-(amplitudDer*amplitudDerInf > amplitudIzq*amplitudIzqInf ? (amplitudIzq * amplitudIzqInf / (amplitudDer * amplitudDerInf)) * 100 : (amplitudDer * amplitudDerInf / (amplitudIzq * amplitudIzqInf))*100 ));
        asimetriaResult = simetriaMedia.ToString("F2");
        tiempoPaso = 0;

    }

    void reescalarDatos(float rescalado) {
        for (int i = 0; i<DatosDer.Length;i++)
        {
            DatosDer[i] = DatosDer[i] * rescalado;
            DatosIzq[i] = DatosIzq[i] * rescalado;
        }
    }

    void reescalarDatos2(float rescalado)
    {
        for (int i = 0; i < DatosDer.Length; i++)
        {
            DatosDer2[i] = DatosDer2[i] * rescalado;
            DatosIzq2[i] = DatosIzq2[i] * rescalado;
        }
    }

    void interpolarAngulos(float derDat, float izqDat, int precision) {
        for (int i = 2; i<=precision; i++) {
            DatosDer[densidadDePuntos - i] = DatosDer[densidadDePuntos - 1] + (precision+1-i) * (derDat - DatosDer[densidadDePuntos - 1]) / precision*1f;
            DatosIzq[densidadDePuntos - i] = DatosIzq[densidadDePuntos - 1] + (precision+1-i) * (izqDat - DatosIzq[densidadDePuntos - 1]) / precision*1f;
        }      
        DatosDer[densidadDePuntos - 1] = derDat;
        DatosIzq[densidadDePuntos - 1] = izqDat;
    }

    void interpolarAngulos2(float derDat2, float izqDat2, int precision)
    {
        for (int i = 2; i <= precision; i++)
        {
            DatosDer2[densidadDePuntos - i] = DatosDer2[densidadDePuntos - 1] + (precision + 1 - i) * (derDat2 - DatosDer2[densidadDePuntos - 1]) / precision * 1f;
            DatosIzq2[densidadDePuntos - i] = DatosIzq2[densidadDePuntos - 1] + (precision + 1 - i) * (izqDat2 - DatosIzq2[densidadDePuntos - 1]) / precision * 1f;
        }
        DatosDer2[densidadDePuntos - 1] = derDat2;
        DatosIzq2[densidadDePuntos - 1] = izqDat2;
    }

    int cntTimer = 0;
    int procesarDatosInt = 0;
    void FixedUpdate()
    {
        if (valoresMedios) {
            cntTimer += 1;
            if (cntTimer == 50)
            {
                seg += 1;
                if (seg == 60) { min += 1; seg = 0; }
                cntTimer = 0;
            }
           
                tiempoPaso += 0.02f;
            

            //Calculo puntuacion
            puntuacion = distancia * ((100 - simetriaMedia) / 100 + amplitudDer * escalaRF / 90 + amplitudIzq * escalaRF / 90 + amplitudDerInf * escalaRF2 / 90 + amplitudIzqInf * escalaRF2 / 90);

        }
        
        //Cuanta atras
        if (cuentaAtras>0 & calibraje) {
            cuentaAtras -= 0.02f;
            if ((int)cuentaAtras <= 0) {
                valoresMedios = true;
                distancia = 0;
            }
        }

        procesarDatos();
        
    }

    void salir()
    {
        calibraje = false;
        principal = false;
    }

    IEnumerator SubirDatos() {

        WWWForm form = new WWWForm();
        form.AddField("Paciente", paciente);
        form.AddField("Responsable", responsable);
        form.AddField("Institucion", PlayerPrefs.GetString("Organizacion"));
        form.AddField("Puntuacion", puntuacioFinal);
        form.AddField("Asimetria", asimetriaFinal);
        form.AddField("Distancia", distanciaFinal);
        form.AddField("AnguloDerSup", anguloDerFinal);
        form.AddField("AnguloIzqSup", anguloIzqFinal);
        form.AddField("AnguloDerInf", anguloDerInfFinal);
        form.AddField("AnguloIzqInf", anguloIzqInfFinal);
        form.AddField("Fecha", System.DateTime.Now.Day+"/"+System.DateTime.Now.Month+"/"+System.DateTime.Now.Year);
        WWW w = new WWW("http://vrproyect.esy.es/subirEstudio.php", form);
        yield return w;
        desant = false;
        if (w.error == null)
        {
            PlayerPrefs.SetString("UsuarioEstudio", paciente);           
            SceneManager.LoadScene("resultados");
        }
        else
        {
            
        }
    }

}
