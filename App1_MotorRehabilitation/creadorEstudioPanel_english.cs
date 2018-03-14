using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Threading;

public class creadorEstudioPanel : MonoBehaviour {

    int h = Screen.width;
    int vy = Screen.height / 15;
    int cntFiltro = 0;
    int idioma = 0;

    Thread thread;
    float bluetoothCarga = 0;
    string detalls = "Loading...";

    Texture2D grisOscuro;
    Texture2D sombreado;
    Texture2D blanco;
    Texture2D azul;    
    Texture2D transparente;
    public Texture2D sombra;
    public Texture2D sombraVolumen;
    public Texture2D botonGris;
    public Texture2D botonGris1x8;
    public Texture2D[] colaboradores;
    public Texture2D select;
    public Texture2D noSelect;
    public Texture2D buscarIcono;
    public Texture2D carga;
    public Texture2D[] imgJuegos;


   // GameObject panelPrincipal;
    public GameObject crearEstudio;
    bool yaleidos = false;
    //Puertos
    SerialPort[] arduino;
    string[] nombreCOM;
    string[] nombrePuerto;

    string usuario = "";
   // string organizacion = "";
    string paciente = "None";
    string fecha = "";
    string actividad = "None";
    string filtro = "";
    string filtroBefore = "";
    string[] parametros;
    string[] nombres;
    string[] apellidos;
    string com = "COM3";
    
    public string[] nombreJuegos;
    bool cargando = false;

    string nuevoNombre = "";
    string nuevoApellidos = "";
    string nuevaFecha = "dd/mm/yyyy";
    string nuevoCorreo = "";
    string nuevoCentroDeEstudio = "";
    string nombreResp = "";
    string fechaDeCreacion = "";
    string sexo = "";

    bool creador = true;
    bool selectorPaciente = false;
    bool selectorActividad = false;
    bool pacientesCargados = false;
    bool[] activarParametros;
    bool nuevoPaciente = false;
    bool bluetoothActivados = false;
    bool buscant = false;
    int[] parametrosInt;


    Vector2 scroll = Vector2.zero;

    int bluetoothStep = 0;

    bool bluetoothDer = false;
    bool bluetoothIzq = false;
    bool bluetoothDer1 = false;
    bool bluetoothIzq1 = false;
    bool scaneigComplet = false;
    public bool errorBluetooth = false;
    string puntitos = "...";


    GameObject misAcelerometros;
    public GameObject publicAcelerometro;

    void Start()
    {

        //Texturas
        grisOscuro = new Texture2D(1, 1);
        grisOscuro.SetPixel(0, 0, new Color(0.1f, 0.1f, 0.1f, 1));
        grisOscuro.Apply();

        carga = new Texture2D(1, 1);
        carga.SetPixel(0, 0, new Color(0f, 0.7f, 0.7f, 1));
        carga.Apply();

        sombreado = new Texture2D(1, 1);
        sombreado.SetPixel(0, 0, new Color(0, 0, 0, 0.6f));
        sombreado.Apply();

        blanco = new Texture2D(1, 1);
        blanco.SetPixel(0, 0, new Color(1, 1, 1, 1));
        blanco.Apply();

        transparente = new Texture2D(1, 1);
        transparente.SetPixel(0, 0, new Color(1, 1, 1, 0));
        transparente.Apply();


        azul = new Texture2D(1, 1);
        azul.SetPixel(0, 0, new Color(1, 1, 1, 1));
        azul.Apply();

        //Leer nombre del responsable
        usuario = PlayerPrefs.GetString("User");
        nombreResp = usuario;

        //Leer idioma
        idioma = PlayerPrefs.GetInt("language");
        
        //Leer fecha
        fecha = System.DateTime.Now.Day.ToString() + "/" + System.DateTime.Now.Month.ToString() + "/" + System.DateTime.Now.Year;
        fechaDeCreacion = fecha;

        //Leer centro de estudiso
        nuevoCentroDeEstudio = PlayerPrefs.GetString("Company");

        //Inicializar arrays
        parametros = new string[10];
        activarParametros = new bool[10];
        parametrosInt = new int[10];
        for (int i = 0; i<parametros.Length; i++) {
            parametros[i] = "";
            activarParametros[i] = true;
        }

        //Verificar si s'han iniciat els acceleromtres
        nombrePuerto = new string[4];
        if (PlayerPrefs.GetString("COM1")!="") {
            bluetoothActivados = true;
            nombrePuerto[0] = PlayerPrefs.GetString("COM1");
            nombrePuerto[1] = PlayerPrefs.GetString("COM2");
            nombrePuerto[2] = PlayerPrefs.GetString("COM3");
            nombrePuerto[3] = PlayerPrefs.GetString("COM4");
        }
        
        //Descargar lista la usuarios
        StartCoroutine(buscar());
    }

    //Interface
    void OnGUI()
    {

        GUI.depth = 12;
        
        GUI.skin.label.fontSize = (int)(0.45f * vy);
        GUI.skin.label.normal.textColor = Color.white;
        GUI.skin.label.fontStyle = FontStyle.BoldAndItalic;
        GUI.skin.button.normal.background = botonGris;
        GUI.skin.button.active.background = botonGris;
        GUI.skin.button.hover.background = botonGris;
        GUI.skin.button.fontSize = (int)(0.35f * vy);
        GUI.skin.button.normal.textColor = Color.white;
        GUI.skin.button.active.textColor = Color.cyan;
        GUI.skin.button.hover.textColor = Color.cyan;

        string txt = "";
        if (creador) {//Pantalla de creación de un nuevo proyecto

            GUI.skin.label.fontSize = (int)(0.65f*vy);
            switch (idioma) { case 0: txt ="Generador d'estudis"; break; case 1: txt = "Generador de estudios"; break; case 2: txt = "Projects assistant"; break; }
            GUI.Label(new Rect(8.5f*vy,0.25f*vy,8*vy,vy),txt);
            GUI.DrawTexture(new Rect(8.5f*vy,1.25f*vy,h-7.5f*vy,0.05f*vy),blanco);

            GUI.skin.label.fontStyle = FontStyle.Bold;
            GUI.skin.label.fontSize = (int)(0.4f * vy);
            GUI.DrawTexture(new Rect(7.5f * vy, 1.5f*vy, h - 8 * vy, 1.5f * vy), sombreado);

            switch (idioma) { case 0: txt = "Responsable de l'estudi:"; break; case 1: txt = "Responsable del estudio:"; break; case 2: txt = "Projects manager:"; break; }
            GUI.Label(new Rect(8 * vy, 1.5f*vy, h, vy), txt);

            GUI.skin.label.fontSize = (int)(0.35f * vy);
            GUI.skin.label.fontStyle = FontStyle.BoldAndItalic;
            GUI.Label(new Rect(8 * vy, 2f * vy, h, vy), usuario);
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(h - 4 * vy, 1.7f * vy, 3.5f * vy, 0.75f * vy), fecha);
            GUI.Label(new Rect(h - 4 * vy, 2.25f * vy, 3.5f * vy, 0.75f * vy), System.DateTime.Now.Hour.ToString() + ":" + (System.DateTime.Now.Minute < 10 ? "0" : "") + System.DateTime.Now.Minute.ToString());
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;

            GUI.skin.label.fontSize = (int)(0.45f * vy);
            GUI.DrawTexture(new Rect(7.5f * vy, 3.75f * vy, h - 12 * vy, 1.25f*vy), sombreado);

            switch (idioma) { case 0: txt = "Pacient:"; break; case 1: txt = "Paciente:"; break; case 2: txt = "Patient:"; break; }
            GUI.Label(new Rect(7.5f * vy, 3 * vy, h, vy), txt);
            switch (idioma) { case 0: txt = "Seleccionar"; break; case 1: txt = "Seleccionar"; break; case 2: txt = "Select"; break; }
            if (GUI.Button(new Rect(8 * vy, 4 * vy, 3 * vy, 0.75f * vy), txt))
            {
                creador = false;
                selectorPaciente = true;
               
                
            }
            GUI.skin.label.fontSize = (int)(0.4f * vy);
            switch (idioma) { case 0: txt = "No seleccionat"; break; case 1: txt = "No seleccionado"; break; case 2: txt = "Not selected"; break; }
            GUI.Label(new Rect(11.5f * vy, 4 * vy, h, vy), (paciente != "Ninguno" ? paciente : txt));

            GUI.skin.label.fontSize = (int)(0.45f * vy);
            GUI.DrawTexture(new Rect(7.5f * vy, 5.75f * vy, h - 12 * vy, 1.25f * vy), sombreado);
            switch (idioma) { case 0: txt = "Activitat:"; break; case 1: txt = "Actividad"; break; case 2: txt = "Activity:"; break; }
            GUI.Label(new Rect(7.5f * vy, 5 * vy, h, vy), txt);
            switch (idioma) { case 0: txt = "Seleccionar"; break; case 1: txt = "Seleccionar"; break; case 2: txt = "Select"; break; }
            if (GUI.Button(new Rect(8 * vy, 6 * vy, 3 * vy, 0.75f * vy), txt))
            {
                selectorActividad = true;
                creador = false;
            }
            GUI.skin.label.fontSize = (int)(0.4f * vy);
            switch (idioma) { case 0: txt = "No seleccionada"; break; case 1: txt = "No seleccionada"; break; case 2: txt = "Not selected"; break; }
            GUI.Label(new Rect(11.5f * vy, 6 * vy, h, vy), (actividad!="Ninguna" ? actividad : txt));

            GUI.skin.button.alignment = TextAnchor.MiddleCenter;

            if (actividad != "Ninguna")
            {
                switch (idioma) { case 0: txt = "Paràmetres"; break; case 1: txt = "Parámetros"; break; case 2: txt = "Settings"; break; }
                GUI.Label(new Rect(7.8f * vy, 7.3f * vy, 8 * vy, 0.5f * vy), txt);
                
                GUI.skin.textField.fontSize = (int)(0.4f * vy);
                GUI.skin.label.alignment = TextAnchor.LowerLeft;
                if (actividad == "Paseo por el lago")
                {

                    GUI.skin.label.normal.textColor = (activarParametros[0] ? Color.white : Color.gray);
                    GUI.skin.button.normal.background = (activarParametros[0] ? select : noSelect);
                    GUI.skin.button.active.background = (activarParametros[0] ? select : noSelect);
                    GUI.skin.button.hover.background = (activarParametros[0] ? select : noSelect);
                    
                    //Tipo de ejercicio
                    GUI.DrawTexture(new Rect(8 * vy, 8.5f * vy, 6.3f * vy, 6.5f * vy), sombreado);
                    switch (idioma) { case 0: txt = "Tipus d'exercici"; break; case 1: txt = "Tipo de ejercicio"; break; case 2: txt = "Type of exercise"; break; }
                    GUI.Label(new Rect(8 * vy, 8f * vy, 6.3f * vy, 0.5f * vy), txt);
                    GUI.skin.button.normal.background = (parametrosInt[0]==0 ? select : noSelect);
                    GUI.skin.button.active.background = (parametrosInt[0] == 0 ? select : noSelect);
                    GUI.skin.button.hover.background = (parametrosInt[0] == 0 ? select : noSelect);
                    if (GUI.Button(new Rect(8.3f * vy, 8.9f * vy, 0.7f * vy, 0.7f * vy), ""))
                    {
                        parametrosInt[0] = 0;
                    }
                    switch (idioma) { case 0: txt = "Automàtic"; break; case 1: txt = "Automático"; break; case 2: txt = "Automatic"; break; }
                    GUI.Label(new Rect(9.2f * vy, 9 * vy, 4 * vy, 0.5f * vy), txt);


                    GUI.skin.button.normal.background = (parametrosInt[0] == 1 ? select : noSelect);
                    GUI.skin.button.active.background = (parametrosInt[0] == 1 ? select : noSelect);
                    GUI.skin.button.hover.background = (parametrosInt[0] == 1 ? select : noSelect);
                    if (GUI.Button(new Rect(8.3f * vy, 9.9f * vy, 0.7f * vy, 0.7f * vy), ""))
                    {
                        parametrosInt[0] = 1;
                        if (PlayerPrefs.GetString("COM1") != "")
                        {
                            scaneigComplet = true;
                        }
                        parametrosInt[0] = 1;
                    }
                    
                    switch (idioma) { case 0: txt = "Cinemàtica"; break; case 1: txt = "Cinemática"; break; case 2: txt = "Kinematics"; break; }
                    GUI.Label(new Rect(9.2f * vy, 10 * vy, 4 * vy, 0.5f * vy), txt);
                    if (parametrosInt[0] == 1)
                    {
                        GUI.skin.textField.fontSize = (int)(0.35f*vy);
                        GUI.skin.label.fontSize =(int)(0.35f*vy);
                        GUI.skin.label.fontStyle = FontStyle.Normal;
                        GUI.skin.label.fontSize = (int)(0.4f * vy);
                        GUI.skin.label.fontStyle = FontStyle.BoldAndItalic;
                    }

                    GUI.skin.button.normal.background = (parametrosInt[0] == 2 ? select : noSelect);
                    GUI.skin.button.active.background = (parametrosInt[0] == 2 ? select : noSelect);
                    GUI.skin.button.hover.background = (parametrosInt[0] == 2 ? select : noSelect);
                    if (GUI.Button(new Rect(8.3f * vy, 10.9f * vy, 0.7f * vy, 0.7f * vy), ""))
                    {
                       // parametrosInt[0] = 2;
                    }

                    
                    GUI.Label(new Rect(9.2f * vy, 11f * vy, 4 * vy, 0.5f * vy), "Brain sensor");

                    GUI.skin.button.normal.background = (parametrosInt[0] == 3 ? select : noSelect);
                    GUI.skin.button.active.background = (parametrosInt[0] == 3 ? select : noSelect);
                    GUI.skin.button.hover.background = (parametrosInt[0] == 3 ? select : noSelect);
                    if (GUI.Button(new Rect(8.3f * vy, 11.9f * vy, 0.7f * vy, 0.7f * vy), ""))
                    {
                        //parametrosInt[0] = 3;
                    }

                    GUI.skin.button.normal.background = (parametrosInt[0] == 5 ? select : noSelect);
                    GUI.skin.button.active.background = (parametrosInt[0] == 5 ? select : noSelect);
                    GUI.skin.button.hover.background = (parametrosInt[0] == 5 ? select : noSelect);
                    if (GUI.Button(new Rect(8.3f * vy, 12.9f * vy, 0.7f * vy, 0.7f * vy), ""))
                    {
                        parametrosInt[0] = 5;
                    }
                    switch (idioma) {case 0: txt = "fMRI"; break; case 1: txt = "fMRI"; break; case 2: txt = "fMRI"; break; }
                    GUI.Label(new Rect(9.2f* vy, 13f * vy, 6.3f * vy, 0.5f * vy), txt);
                

                    switch (idioma) { case 0: txt = "Contrast"; break; case 1: txt = "Contraste"; break; case 2: txt = "Comparative"; break; }
                    GUI.Label(new Rect(9.2f * vy, 12f * vy, 4 * vy, 0.5f * vy), txt);

                    //Ambiente
                    GUI.DrawTexture(new Rect(16 * vy, 8.5f * vy, 6.3f * vy, 2.5f * vy), sombreado);
                    switch (idioma) { case 0: txt = "Ambient"; break; case 1: txt = "Ambiente"; break; case 2: txt = "Environment"; break; }
                    GUI.Label(new Rect(16 * vy, 8 * vy, 6.3f * vy, 0.5f * vy),txt);
                    GUI.skin.button.normal.background = (parametrosInt[1] == 0 ? select : noSelect);
                    GUI.skin.button.active.background = (parametrosInt[1] == 0 ? select : noSelect);
                    GUI.skin.button.hover.background = (parametrosInt[1] == 0 ? select : noSelect);
                    if (GUI.Button(new Rect(16.3f * vy, 8.9f * vy, 0.7f * vy, 0.7f * vy), ""))
                    {
                        parametrosInt[1] = 0;
                    }

                    switch (idioma) { case 0: txt = "Mig dia"; break; case 1: txt = "Medio día"; break; case 2: txt = "Midday"; break; }
                    GUI.Label(new Rect(17.2f * vy, 9 * vy, 4 * vy, 0.5f * vy), txt);

                    GUI.skin.button.normal.background = (parametrosInt[1] == 1 ? select : noSelect);
                    GUI.skin.button.active.background = (parametrosInt[1] == 1 ? select : noSelect);
                    GUI.skin.button.hover.background = (parametrosInt[1] == 1 ? select : noSelect);
                    if (GUI.Button(new Rect(16.3f * vy, 9.9f * vy, 0.7f * vy, 0.7f * vy), ""))
                    {
                        
                       

                        parametrosInt[1] = 1;
                    }
                    switch (idioma) { case 0: txt = "Capvespre"; break; case 1: txt = "Atardecer"; break; case 2: txt = "Sunset"; break; }
                    GUI.Label(new Rect(17.2f * vy, 10 * vy, 4 * vy, 0.5f * vy), txt);
                }

                GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            }

            GUI.skin.button.normal.background = botonGris;
            GUI.skin.button.active.background = botonGris;
            GUI.skin.button.hover.background = botonGris;
            switch (idioma) { case 0: txt = "Començar"; break; case 1: txt = "Empezar"; break; case 2: txt = "Start"; break; }
            if (GUI.Button(new Rect(8 * vy, 14f * vy, 3 * vy, 0.75f * vy), txt))
            {
                PlayerPrefs.SetString("Paciente",paciente);
                PlayerPrefs.SetInt("ModoJuegoAndar",parametrosInt[0]);
                PlayerPrefs.SetInt("AmbienteJuegoAndar",parametrosInt[1]);
                if (parametros[0]=="") { parametros[0] = "0"; }
                PlayerPrefs.SetInt("MinutosAndar",5);
                if (parametros[1] == "") { parametros[1] = "0"; }
                PlayerPrefs.SetInt("DistanciaAndar", int.Parse(parametros[1]));

          

                if (actividad == "Paseo por el lago") {
                    SceneManager.LoadScene("paseoPorElLago");                   
                }
                Destroy(this.transform.parent.gameObject);
            }
        }

        if (nuevoPaciente) { //Pantalla crear un nuevo paciente
            GUI.DrawTexture(new Rect(9*vy,vy,16*vy,12.5f*vy),sombreado);
            if (!cargando)
            {
                float sl = 1.5f * vy;
                GUI.skin.label.fontSize = (int)(0.45f * vy);
                GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                switch (idioma) { case 0: txt = "Nou pacient"; break; case 1: txt = "Nuevo paciente"; break; case 2: txt = "New patient"; break; }
                GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.7f * vy), txt);
                GUI.DrawTexture(new Rect(9.5f * vy, sl + 0.7f * vy, 7 * vy, 0.05f * vy), azul);
                GUI.skin.label.fontSize = (int)(0.35f * vy);
                sl += vy;
                switch (idioma) { case 0: txt = "Nom:"; break; case 1: txt = "Nombre:"; break; case 2: txt = "Name:"; break; }
                GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.5f * vy), txt);
                switch (idioma) { case 0: txt = "Cognom:"; break; case 1: txt = "Apellido:"; break; case 2: txt = "Last name:"; break; }
                GUI.Label(new Rect(15.5f * vy, sl, 5 * vy, 0.5f * vy), txt);
                sl += 0.5f * vy;
                nuevoNombre = GUI.TextField(new Rect(9.5f * vy, sl, 5.5f * vy, 0.6f * vy), nuevoNombre);
                nuevoApellidos = GUI.TextField(new Rect(15.5f * vy, sl, 7.5f * vy, 0.6f * vy), nuevoApellidos);
                sl += 0.8f * vy;
                switch (idioma) { case 0: txt = "Data de naixement:"; break; case 1: txt = "Fecha de nacimiento:"; break; case 2: txt = "Birthdate"; break; }
                GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.5f * vy), txt);
                switch (idioma) { case 0: txt = "Gènere:"; break; case 1: txt = "Género:"; break; case 2: txt = "Gender:"; break; }
                GUI.Label(new Rect(15.5f * vy, sl, 5 * vy, 0.5f * vy), txt);
                sl += 0.5f * vy;
                nuevaFecha = GUI.TextField(new Rect(9.5f * vy, sl, 2.5f * vy, 0.6f * vy), nuevaFecha);
                sexo = GUI.TextField(new Rect(15.5f * vy, sl, 2.5f * vy, 0.6f * vy), sexo);
                sl += 0.8f * vy;

                switch (idioma) { case 0: txt = "Correu:"; break; case 1: txt = "Correo:"; break; case 2: txt = "Email:"; break; }
                GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.5f * vy),txt); sl += 0.5f * vy;
                nuevoCorreo = GUI.TextField(new Rect(9.5f * vy, sl, 5.5f * vy, 0.6f * vy), nuevoCorreo); sl += 0.8f * vy;

                switch (idioma) { case 0: txt = "Centre d'estudi:"; break; case 1: txt = "Centro de estudio:"; break; case 2: txt = "Study Center:"; break; }
                GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.5f * vy), txt); sl += 0.5f * vy;
                nuevoCentroDeEstudio = GUI.TextField(new Rect(9.5f * vy, sl, 5.5f * vy, 0.6f * vy), nuevoCentroDeEstudio); sl += 0.8f * vy;

                GUI.skin.label.fontSize = (int)(0.45f * vy);
                GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                switch (idioma) { case 0: txt = "Responsable:"; break; case 1: txt = "Responsable:"; break; case 2: txt = "Manager:"; break; }
                GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.7f * vy), txt);
                GUI.DrawTexture(new Rect(9.5f * vy, sl + 0.7f * vy, 7 * vy, 0.05f * vy), azul);
                GUI.skin.label.fontSize = (int)(0.35f * vy);
                sl += vy;
                switch (idioma) { case 0: txt = "Nom i cognom:"; break; case 1: txt = "Nombre y apallidos:"; break; case 2: txt = "Name and last name:"; break; }
                GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.5f * vy), txt);
                sl += 0.5f * vy;
                nombreResp = GUI.TextField(new Rect(9.5f * vy, sl, 5.5f * vy, 0.6f * vy), nombreResp);
                sl += 0.8f * vy;
                switch (idioma) { case 0: txt = "Data:"; break; case 1: txt = "Fecha:"; break; case 2: txt = "Date:"; break; }
                GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.5f * vy), txt);
                sl += 0.5f * vy;
                fechaDeCreacion = GUI.TextField(new Rect(9.5f * vy, sl, 5.5f * vy, 0.6f * vy), fechaDeCreacion);
                sl += 1.5f * vy;

                switch (idioma) { case 0: txt = "Afegir"; break; case 1: txt = "Agregar"; break; case 2: txt = "Add"; break; }
                if (GUI.Button(new Rect(9.5f * vy, sl, 3 * vy, 0.9f * vy), txt))
                {
                    cargando = true;
                    StartCoroutine( subirPaciente());
                }
                switch (idioma) { case 0: txt = "Tornar"; break; case 1: txt = "Volver"; break; case 2: txt = "Back"; break; }
                if (GUI.Button(new Rect(h - 3.5f * vy, 14 * vy, 3 * vy, 0.8f * vy),txt))
                {
                    selectorPaciente = true;
                    nuevoPaciente = false;
                }
            }
            else {
                switch (idioma) { case 0: txt = "Carregant..."; break; case 1: txt = "Cargando..."; break; case 2: txt = "Loading..."; break; }
                GUI.Label(new Rect(15.5f * vy, 6.5f*vy, 5 * vy, vy ), txt);
            }
        }

        if (selectorPaciente)//Pantalla seleccion de pacientes
        {
            GUI.DrawTexture(new Rect(11.8f * vy, 2.5f * vy, 10 * vy, 11.8f * vy), sombreado);
            GUI.DrawTexture(new Rect(11.8f * vy, 2.5f * vy, 10 * vy, vy), sombreado);
            GUI.DrawTexture(new Rect(11.95f * vy, 2.65f * vy, 0.7f * vy, 0.7f * vy), buscarIcono);
            GUI.skin.textField.fontSize = (int)(0.4f * vy);
            GUI.skin.textField.alignment = TextAnchor.MiddleLeft;
            filtroBefore = filtro;
            filtro = GUI.TextField(new Rect(12.8f * vy, 2.65f * vy, 7 * vy, 0.7f * vy), filtro, 20);
            if (filtro!=filtroBefore) { cntFiltro = 25; }
            switch (idioma) { case 0: txt = "Seleccionar pacient"; break; case 1: txt = "Seleccionar paciente"; break; case 2: txt = "Select patient"; break; }
            GUI.Label(new Rect(11.8f*vy,1.8f*vy,6*vy,0.5f*vy),txt);
            GUI.skin.verticalScrollbar.fixedWidth = 0.3f * vy;
            GUI.skin.verticalSliderThumb.fixedWidth = 0.3f * vy;
            switch (idioma) { case 0: txt = "Tornar"; break; case 1: txt = "Volver"; break; case 2: txt = "Back"; break; }
            if (GUI.Button(new Rect(h-3.5f*vy,14*vy,3*vy,0.8f*vy),txt)) {
                selectorPaciente = false;
                creador = true;
            }
            switch (idioma) { case 0: txt = "+Afegir"; break; case 1: txt = "+Agregar"; break; case 2: txt = "+Add"; break; }
            if (GUI.Button(new Rect(h - 4.5f * vy,2.5f * vy, 3 * vy, 0.8f * vy), txt))
            {
                selectorPaciente = false;
                nuevoPaciente = true;
            }

            if (pacientesCargados)
            {
                scroll = GUI.BeginScrollView(new Rect(11.8f * vy, 3.5f * vy, 10 * vy, 10.8f * vy), scroll, new Rect(11.8f * vy, 3.5f * vy, 9.6f * vy, 30 * vy));
                int n = nombres.Length;
                GUI.skin.label.normal.textColor = Color.black;
                GUI.skin.label.fontStyle = FontStyle.Bold;
                GUI.skin.button.fontSize = (int)(0.35f * vy);
                GUI.skin.button.normal.background = botonGris;
                GUI.skin.button.active.background = botonGris;
                GUI.skin.button.hover.background = botonGris;
                for (int i = 0; i < n; i++)
                {
                    GUI.DrawTexture(new Rect(11.9f * vy, 3.5f * vy + i * vy, 9.5f * vy, 0.95f * vy), blanco);
                    GUI.DrawTexture(new Rect(11.9f * vy, 3.5f * vy + i * vy, 9.5f * vy, 0.95f * vy), sombra);
                    GUI.Label(new Rect(12.4f * vy, 3.5f * vy + i * vy, 9.5f * vy, 0.95f * vy), nombres[i] + " " + apellidos[i]);
                    switch (idioma) { case 0: txt = "+Seleccionar"; break; case 1: txt = "+Seleccionar"; break; case 2: txt = "+Select"; break; }
                    if (GUI.Button(new Rect(18.5f * vy, 3.65f * vy + i * vy, 2.8f * vy, 0.7f * vy), txt))
                    {
                        paciente = nombres[i] + " " + apellidos[i];
                        selectorPaciente = false;
                        creador = true;
                    }
                }

                GUI.EndScrollView();
            }
            else {
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                switch (idioma) { case 0: txt = "Carregant..."; break; case 1: txt = "Cargando..."; break; case 2: txt = "Loading..."; break; }
                GUI.Label(new Rect(11.9f * vy, 3.5f * vy, 9.5f * vy, 0.95f * vy),txt);
                GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            }

        }

        if (selectorActividad) { //Pantalla seleccionar actividad
            GUI.skin.label.fontSize = (int)(0.4f*vy);
            switch (idioma) { case 0: txt = "Seleccionar activitat:"; break; case 1: txt = "Seleccionar actividad:"; break; case 2: txt = "Add activity:"; break; }
            GUI.Label(new Rect(8*vy,1.8f*vy,10*vy,0.8f*vy),txt);
            GUI.DrawTexture(new Rect(8*vy,2.5f*vy,18*vy,11.3f*vy),sombreado);
            GUI.skin.label.fontSize = (int)(0.3f*vy);
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            for (int i =0; i < imgJuegos.Length; i++) {
                int j = i / 5;
                if (GUI.Button(new Rect(8.5f * vy + (i - j * 5) * 3.5f * vy, 3f * vy + j * 3.5f * vy, 3 * vy, 3 * vy),"")) {
                    selectorActividad = false;
                    creador = true;
                    actividad = nombreJuegos[i];
                }
                GUI.DrawTexture(new Rect(8.5f*vy+(i-j*5)*3.5f*vy,3f*vy+j*3.5f*vy,3*vy,3*vy),imgJuegos[i]);
                GUI.Label(new Rect(8.5f * vy+(i-j*5)*3.5f*vy, 5.7f * vy+j*3.5f*vy, 3 * vy, vy), nombreJuegos[i]);
            }
            switch (idioma) { case 0: txt = "Tornar"; break; case 1: txt = "Volver"; break; case 2: txt = "Back"; break; }
            if (GUI.Button(new Rect(h - 3.5f * vy, 14 * vy, 3 * vy, 0.8f * vy), txt))
            {
                creador = true;
                selectorActividad = false;
            }
        }

        if (scaneigComplet ) { creador = true; }

        if (!scaneigComplet & parametrosInt[0] == 1 ) {
            selectorActividad = false;
            selectorPaciente = false;
            nuevoPaciente = false;
            creador = false;

            GUI.DrawTexture(new Rect(11.75f * vy, 3 * vy, 10.5f * vy, 6 * vy), sombreado);
            GUI.DrawTexture(new Rect(11.75f * vy, 3 * vy, 10.5f * vy, 6 * vy), sombra);
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            GUI.skin.label.fontStyle = FontStyle.Bold;
            GUI.skin.label.fontSize = (int)(0.5f * vy);
            GUI.Label(new Rect(11.75f * vy, 2.25f * vy, 12 * vy, 0.75f * vy), "Detector de dispositius bluetooth");

            if (!errorBluetooth)
            {
                if (buscant) {
                    detalls = misAcelerometros.GetComponent<acelerometrosScript>().mensaje;
                    bluetoothCarga = misAcelerometros.GetComponent<acelerometrosScript>().carga;
                    bluetoothDer = misAcelerometros.GetComponent<acelerometrosScript>().bluetoothDer;
                    bluetoothIzq = misAcelerometros.GetComponent<acelerometrosScript>().bluetoothIzq;
                    bluetoothDer1 = misAcelerometros.GetComponent<acelerometrosScript>().bluetoothDer1;
                    bluetoothIzq1 = misAcelerometros.GetComponent<acelerometrosScript>().bluetoothIzq1;
                    scaneigComplet = misAcelerometros.GetComponent<acelerometrosScript>().bluetoothComplete;
                }



                GUI.skin.label.fontSize = (int)(0.35f * vy);
                GUI.Label(new Rect(12 * vy, 3.5f* vy, 8 * vy, 0.5f * vy), "Procés:");
                GUI.DrawTexture(new Rect(12 * vy, 4 * vy, 10 * vy, 0.5f * vy), sombreado);
                GUI.DrawTexture(new Rect(12.05f * vy, 4.05f * vy,bluetoothCarga * 9.9f * vy, 0.4f * vy), carga);
                GUI.DrawTexture(new Rect(12.05f * vy, 4.05f * vy, bluetoothCarga *  9.9f * vy, 0.4f * vy), sombra);
                GUI.skin.label.fontStyle = FontStyle.Normal;
                GUI.skin.label.fontSize = (int)(0.25f*vy);
                GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                GUI.Label(new Rect(12 * vy, 4.5f * vy, 8 * vy, 0.45f * vy), "Detalls: ( "+detalls+" )");


                GUI.DrawTexture(new Rect(12 * vy, 5.5f * vy, 10 * vy, 3.25f * vy), sombreado);
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                GUI.skin.label.fontStyle = FontStyle.BoldAndItalic;
                GUI.skin.label.fontSize = (int)(0.35f*vy);
                GUI.Label(new Rect(12 * vy, 5.5f * vy, 6 * vy, 0.5f * vy), "Dispositiu");
                GUI.Label(new Rect(18 * vy, 5.5f * vy, 4 * vy, 0.5f * vy), "Estat");
                GUI.DrawTexture(new Rect(12.25f * vy, 6 * vy, 5.625f * vy, 2.5f * vy), sombreado);
                GUI.DrawTexture(new Rect(18.125f * vy, 6 * vy, 3.625f * vy, 2.5f * vy), sombreado);

                GUI.skin.label.fontStyle = FontStyle.Bold;
                GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                GUI.skin.label.fontSize = (int)(0.35f * vy);
                GUI.Label(new Rect(12.5f * vy, 6.25f * vy, 12 * vy,0.5f* vy), "Cama dreta superior:    ");
                GUI.Label(new Rect(12.5f * vy, 6.75f * vy, 12 * vy,0.5f* vy), "Cama esquerra inferior: ");
                GUI.Label(new Rect(12.5f * vy, 7.25f * vy, 12 * vy, 0.5f*vy), "Cama dreta inferior:    ");
                GUI.Label(new Rect(12.5f * vy, 7.75f * vy, 12 * vy, 0.5f*vy), "Cama esquerra superior: ");

                GUI.skin.label.normal.textColor = (bluetoothDer ? Color.green : Color.white);
                GUI.Label(new Rect(18.5f * vy, 6.25f * vy, 12 * vy, 0.5f*vy), (bluetoothDer ? "Connectat" : "Escanejant" + puntitos));

                GUI.skin.label.normal.textColor = (bluetoothIzq ? Color.green : Color.white);
                GUI.Label(new Rect(18.5f * vy, 6.75f * vy, 12 * vy, 0.5f*vy), (bluetoothIzq ? "Connectat" : "Escanejant" + puntitos));

                GUI.skin.label.normal.textColor = (bluetoothDer1 ? Color.green : Color.white);
                GUI.Label(new Rect(18.5f * vy, 7.25f * vy, 12 * vy, 0.5f*vy), (bluetoothDer1 ? "Connectat" : "Escanejant" + puntitos));

                GUI.skin.label.normal.textColor = (bluetoothIzq1 ? Color.green : Color.white);
                GUI.Label(new Rect(18.5f * vy, 7.75f * vy, 12 * vy, 0.5f*vy), (bluetoothIzq1 ? "Connectat" : "Escanejant" + puntitos));

                GUI.skin.label.normal.textColor = Color.white;
                if (!buscant) {
                    buscant = true;/* thread = new Thread(leerDispositivos); thread.Start();*/
                    misAcelerometros = Instantiate(publicAcelerometro, transform.position, Quaternion.identity) as GameObject;
                }
            }
            else {
                buscant = false;
                GUI.skin.label.fontSize = (int)(0.4f * vy);
                GUI.skin.label.alignment = TextAnchor.UpperLeft;
                GUI.Label(new Rect(12 * vy, 4 * vy, 12 * vy,6* vy), "No s'ha trobat:\n" +(bluetoothDer ? "" : "  Cama dreta superior\n") + (bluetoothIzq ? "" : "  Cama esquerra superior\n") + (bluetoothDer1 ? "" : "  Cama dreta inferior\n") + (bluetoothIzq1 ? "" : "  Cama esquerra inferior\n"));

                GUI.skin.button.normal.background = transparente; GUI.skin.button.active.background = transparente; GUI.skin.button.hover.background = transparente;
                GUI.DrawTexture(new Rect(18.5f * vy, 8 * vy, 3 * vy, 0.75f * vy),botonGris);
                if (GUI.Button(new Rect(18.5f*vy,8*vy,3*vy,0.75f*vy),"Reintentar")) {
                   // bluetoothDer = false; bluetoothDer1 = false; bluetoothIzq = false; bluetoothIzq1 = false;
                   // cntErrorSemaforo = 0;
                    
                 //   detenerArduinos();
                    errorBluetooth = false;


                }
                GUI.DrawTexture(new Rect(14.5f * vy, 8 * vy, 3 * vy, 0.75f * vy), botonGris);
                if (GUI.Button(new Rect(14.5f * vy, 8 * vy, 3 * vy, 0.75f * vy), "Tornar"))
                {
                   // detenerArduinos();
                    creador = true;
                    parametrosInt[0] = 0;
                    errorBluetooth = false;
                    bluetoothDer = false; bluetoothDer1 = false; bluetoothIzq = false; bluetoothIzq1 = false;
                    //cntErrorSemaforo = 0;

                }

            }
                               
        }

    }


    bool[] arduinoDisp;
    bool arduinosAbiertos = false;
    void leerDispositivos() {
        detalls = "Buscant dispositius...";
        nombreCOM = SerialPort.GetPortNames();

        bluetoothCarga = 0;
        Debug.Log("Hello");
        for (int i = 0; i<nombreCOM.Length; i++) {
            detalls = "Detectat PORT: " + nombreCOM[i];
            Debug.Log(detalls);
        }
        bluetoothCarga = 0.2f;

        arduino = new SerialPort[nombreCOM.Length];
        arduinoDisp = new bool[nombreCOM.Length];
        

        for (int i = 0; i< nombreCOM.Length; i++) {
            detalls = "Obrint PORT: " + nombreCOM[i] + "...";
            bluetoothCarga += 0.4f / nombreCOM.Length;
            string[] verify = nombreCOM[i].Split("M"[0]);

            if (true)
            {
                try
                {
                    arduino[i] = new SerialPort("\\\\.\\"+nombreCOM[i], 9600);
                    arduino[i].Open();
                    arduino[i].ReadTimeout = 5;
                    arduinoDisp[i] = true;
                    Debug.Log(nombreCOM[i] + ": OK");
                }
                catch (System.Exception)
                {
                    Debug.Log(nombreCOM[i] + ": Error");
                    arduinoDisp[i] = false;
                }



            }
        }
        
        for (int i = 0; i<nombreCOM.Length; i++) {
            detalls = "Emparellant...";
            bluetoothCarga += 0.4f / nombreCOM.Length;
            int intentos = 0;
            Thread.Sleep(1000);
            while (intentos<10) {
                if (arduinoDisp[i])
                {
                    Debug.Log(nombreCOM[i]+": "+intentos.ToString());
                    intentos++;
                    try
                    {
                        string arduinoRead = arduino[i].ReadLine();

                        string[] desc = arduinoRead.Split(","[0]);
                        if (desc[0] == "Izq")
                        {
                            Debug.Log("Izq: " + nombreCOM[i]);
                            bluetoothIzq = true;
                            intentos = 10;
                            nombrePuerto[0] = nombreCOM[i];
                        }
                        else
                        {
                            if (desc[0] == "Der")
                            {
                                Debug.Log("Der: " + nombreCOM[i]);
                                bluetoothDer = true;
                                nombrePuerto[1] = nombreCOM[i];
                                intentos = 10;
                            }
                            else
                            {
                                if (desc[0] == "Izq1")
                                {
                                    Debug.Log("Izq1: " + nombreCOM[i]);
                                    bluetoothIzq1 = true;
                                    intentos = 10;
                                    nombrePuerto[2] = nombreCOM[i];
                                }
                                else
                                {
                                    if (desc[0] == "Der1")
                                    {
                                        Debug.Log("Der1: " + nombreCOM[i]);
                                        bluetoothDer1 = true;
                                        intentos = 10;
                                        nombrePuerto[3] = nombreCOM[i];
                                    }
                                    else
                                    {
                                        intentos++;
                                    }
                                }
                            }
                        }
                    }
                    catch (System.Exception)
                    {
                        intentos++;
                        Debug.Log(nombreCOM[i] + ": Error");
                    }

                    
                }
                else
                {
                    intentos = 10;
                }

            }          
        }

        if (bluetoothDer & bluetoothDer1 & bluetoothIzq & bluetoothIzq1)
        {
            bluetoothActivados = true;
            creador = true;
            for (int i = 0; i<nombreCOM.Length;i++) {
                if (arduinoDisp[i]) {
                    if (arduino[i].IsOpen)
                    {
                        arduino[i].Close();
                    }
                }               
            }
        }
        else
        {
            errorBluetooth = true;
        }

        thread.Abort();
    }

    void guardarPuerto(int i, string nombre) {
        PlayerPrefs.SetString("COM"+i.ToString(),nombre);
    }
   

    IEnumerator asignarArduino(int i , int intentos ) {
        

        
        yield return true;

        Debug.Log(nombreCOM[i]+": Intento"+intentos.ToString());

        if (intentos == 10) { intentos = 0; i++; }
        if (i < nombreCOM.Length - 1)
        {            
            StartCoroutine(asignarArduino(i, intentos));
        }
        else
        {
            
        }


    }

    void abrirArduinos() {
        arduinosAbiertos = true;
    }

    void Update() {
      /*  if (!bluetoothActivados & parametrosInt[0]==1 & arduinosAbiertos) {
            for (int i = 0; i < nombreCOM.Length; i++)
            {
                string[] verify = nombreCOM[i].Split("M"[0]);
                if (int.Parse(verify[1]) < 9 & int.Parse(verify[1]) >1)
                {
                    Debug.Log(nombreCOM[i]+"  2");
                    if (arduino[i].IsOpen)
                    {
                        if (arduinoDisp[i])
                        {
                            arduinoDisp[i] = false;
                            StartCoroutine(lecturaArduino(i));
                        }
                    }
            }
            }
        }*/
    }

    IEnumerator lecturaArduino(int i) {
       

        yield return true;

    }

    /* IEnumerator validarBluetooth( int i, int intento ) {
    cntErrorSemaforo = 0;
    string[] verify = nombreCOM[i].Split("M"[0]);
    if (int.Parse(verify[1]) < 10 )
    {
        if (intento == 0) {
            arduino = new SerialPort(nombreCOM[i], 9600);
            arduino.Open();
            arduino.ReadTimeout = 10;
        }
        try
        {
            string arduinoRead = arduino.ReadLine();
         //   Debug.Log(nombreCOM[i] + ": " + arduinoRead);

            string[] desc = arduinoRead.Split(","[0]);
            if (desc[0] == "Izq")
            {
                Debug.Log("Izq: "+nombreCOM[i]);
                bluetoothIzq = true;
                i++; intento = 0;
            }
            else {
                if (desc[0] == "Der")
                {
                    Debug.Log("Der: " + nombreCOM[i]);
                    bluetoothDer = true;
                    i++; intento = 0;
                }
                else
                {
                    if (desc[0] == "Izq1")
                    {
                        Debug.Log("Izq1: " + nombreCOM[i]);
                        bluetoothIzq1 = true;
                        i++; intento = 0;
                    }
                    else
                    {
                        if (desc[0] == "Der1")
                        {
                            Debug.Log("Der1: " + nombreCOM[i]);
                            bluetoothDer1 = true;
                            i++; intento = 0;
                        }
                        else
                        {
                            intento++;
                        }
                    }
                }
            }
        }
        catch (System.Exception)
        {

            Debug.Log(nombreCOM[i] + ": Error");
            intento++;
        }
    }
    else {
        intento = 10;
    }


    yield return true;


    if (intento>9) { intento = 0; i++; }
    if (i < nombreCOM.Length - 1)
    {
        StartCoroutine(validarBluetooth(i, intento));
    }
    else {
        if (bluetoothDer & bluetoothDer1 & bluetoothIzq & bluetoothIzq1)
        {
            bluetoothActivados = true;
            creador = true;
        }
        else {
            errorBluetooth = true;
        }
    }


}*/


    //Descgar base de datos de pacientes
    IEnumerator buscar() {
        WWWForm form = new WWWForm();
        form.AddField("filtro", filtro);
        WWW w = new WWW("http://vrproyect.esy.es/buscarPacientes.php", form);
        yield return w;
        string json = w.text;
        if (w.error == null)
        {
            leerJson(json);
            pacientesCargados = true;
        }
        else
        {
            

        }
    }

    //Añadir nueco paciente a la base de datos
    IEnumerator subirPaciente()
    {
        WWWForm form = new WWWForm();
        form.AddField("Name", nuevoNombre);
        form.AddField("Surname", nuevoApellidos);
        form.AddField("Birth",nuevaFecha);
        form.AddField("Mail",nuevoCorreo);
        form.AddField("Company", nuevoCentroDeEstudio);
        form.AddField("Leader",nombreResp);
        form.AddField("Date", fechaDeCreacion);
        form.AddField("Gender", sexo);
        WWW w = new WWW("http://vrproyect.esy.es/nuevoPaciente.php", form);
        yield return w;
        Debug.Log(w.text);
        if (w.error == null)
        {
            creador = true;
            nuevoPaciente = false;
            paciente = nuevoNombre + " " + nuevoApellidos;
            cargando = false;
        }
        else
        {
            cargando = false;

        }
    }

    //Leer formato Json
    void leerJson(string json) {
        string[] grupos = json.Split("}"[0]);
        nombres = new string[grupos.Length-1];
        apellidos = new string[grupos.Length-1];
        for (int i = 0; i < grupos.Length-1; i++) {
            string[] elementos = grupos[i].Split(","[0]);
            for (int j = 0; j<elementos.Length; j++)
            {
                string[] palabras = elementos[j].Split("\""[0]);
                for (int k = 0; k<palabras.Length; k++) {
                    if (palabras[k]=="name") { nombres[i] = palabras[k + 2]; }
                    if (palabras[k] == "surname") { apellidos[i] = palabras[k + 2]; }
                }
            }
        }
        for (int i = 0; i<nombres.Length; i++) {
        }
    }

    //Controlar actulización de base de datos
    int cntPuntitos = 0;
    int cntErrorSemaforo = 0;
    void FixedUpdate() {
        if (cntFiltro>0) {
            cntFiltro -= 1;
            if (cntFiltro==0) {
                pacientesCargados = false;
                StartCoroutine(buscar());
            }
        }

        if (cntPuntitos<=10) { puntitos = "."; }
        if (cntPuntitos > 10 & cntPuntitos <= 20) { puntitos = ".."; }
        if (cntPuntitos > 20 & cntPuntitos <= 30) { puntitos = "..."; if (cntPuntitos==30) { cntPuntitos = 0; } }

        cntPuntitos++;

        if (cntErrorSemaforo < 1000 & !bluetoothActivados & parametrosInt[0] == 1)
        {
          //  Debug.Log(cntErrorSemaforo.ToString());
           // cntErrorSemaforo++;
        }
        else {
          //  errorBluetooth = true;
        }
        
    }

    void detenerArduinos()
    {
        thread.Abort();
        for (int i = 0; i < nombreCOM.Length; i++)
        {
            if (arduinoDisp[i])
            {
                if (arduino[i].IsOpen)
                {
                    arduino[i].Close();
                }
            }
        }

        thread = new Thread(leerDispositivos); thread.Start();
    }

}
