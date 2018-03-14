using UnityEngine;
using System.Collections;

public class veureEstudis : MonoBehaviour {

    //Relacion de aspecto
    int h = Screen.width;
    int vy = Screen.height / 15;
    int cntBuscar = 0;

    //Id seleccionada
    int idEliminar = 0;

    //Texturas
    Texture2D Sombra;
    Texture2D blanco;
    Texture2D transparente;
    public Texture2D buscarIco;
    public Texture2D boton;
    public Texture2D boton1x1;

    //Objeto exterior
    public GameObject historial;

    //Vector posicion de scroll
    Vector2 scrollView = Vector2.zero;

    //Filtro de busqueda
    string filtro = "";

    //Informacion descargada
    string[,] info;

    //Booleanos
    bool infoProcesada = false;
    bool errorDeCarga = false;
    bool sinResultados = false;
    bool principal = true;
    bool eliminarConfirm = false;
    bool eliminando = false;

    

	void Start () {

        Sombra = new Texture2D(1,1);
        Sombra.SetPixel(0, 0, new Color(0, 0, 0, 0.7f));
        Sombra.Apply();

        blanco = new Texture2D(1,1);
        blanco.SetPixel(0, 0, new Color(1,1,1,1));
        blanco.Apply();

        transparente = new Texture2D(1, 1);
        transparente.SetPixel(0, 0, new Color(1, 1, 1, 0));
        transparente.Apply();

        //0_ID  1_Nombre Paciente  2_Nombre Responsable  3_Organizacion 4_Fecha  5_Puntuacion 6_Asimetrio 7_distancia 8_ADS 9_AIS 10_ADI 11_AII
        //string msg = "MiID:Josep:Marco:Visyon:17/06/09:1950:150:23:24:26:27:28|MiID:Josep2:Marco2:Visyon:17/06/09:1950:150:23:24:26:27:28";
        StartCoroutine(leerInfo());

    }

    //Interface
    void OnGUI() {
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.skin.label.fontSize = (int)(0.65f * vy);
        GUI.Label(new Rect(8.5f * vy, 0.25f * vy, 8 * vy, vy), "Llistat d'estudis");
        GUI.DrawTexture(new Rect(8.5f * vy, 1.25f * vy, h - 7.5f * vy, 0.05f * vy), blanco);

        GUI.skin.label.fontStyle = FontStyle.Bold;
        GUI.skin.label.fontSize = (int)(0.4f * vy);

        if (principal) {
            //Buscador
            GUI.DrawTexture(new Rect(8 * vy, 1.5f * vy, h - 9 * vy, vy), Sombra);
            GUI.DrawTexture(new Rect(8.25f * vy, 1.75f * vy, 0.5f * vy, 0.5f * vy), buscarIco);
            GUI.skin.textField.fontSize = (int)(0.4f * vy);
            GUI.skin.textField.alignment = TextAnchor.MiddleLeft;

            string verificar = filtro;
            filtro = GUI.TextField(new Rect(9 * vy, 1.6f * vy, h - 10.1f * vy, 0.8f * vy), filtro);
            if (verificar != filtro) { cntBuscar = 50; }

            //Listado
            int refx = 0;
            float refy = -1.4f * vy;
            GUI.DrawTexture(new Rect(8 * vy + refx, 4 * vy + refy, h - 9 * vy, 11.5f * vy), Sombra);
            scrollView = GUI.BeginScrollView(new Rect(8 * vy + refx, 4 * vy + refy, h - 9 * vy, 11 * vy), scrollView, new Rect(8 * vy + refx, 4 * vy + refy, h - 9.5f * vy, (infoProcesada ? ((info.Length / 12) * 2.5f * vy + 3 * vy) : 9 * vy)));


            GUI.skin.label.normal.textColor = new Color(0, 0, 0, 1);
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            if (infoProcesada)
            {
                for (int i = 0; i < info.Length / 12; i++)
                {
                    GUI.DrawTexture(new Rect(8.5f * vy + refx, 4.5f * vy + refy + i * 2.5f * vy, h - 10 * vy, 2 * vy), blanco);
                    GUI.skin.label.fontSize = (int)(0.4f * vy);
                    GUI.Label(new Rect(8.75f * vy + refx, 4.5f * vy + refy + i * 2.5f * vy, 14 * vy, 0.75f * vy), "Pacient: " + info[i, 1]);
                    GUI.Label(new Rect(18.5f * vy + refx, 4.5f * vy + refy + i * 2.5f * vy, 3 * vy, 0.75f * vy), info[i, 4]);
                    GUI.Label(new Rect(8.75f * vy + refx, 5 * vy + refy + i * 2.5f * vy, 14 * vy, 0.75f * vy), "Responsable: " + info[i, 2] + ", " + info[i, 3]);
                    GUI.skin.label.fontSize = (int)(0.3f * vy);
                    GUI.Label(new Rect(8.75f * vy + refx, 5.75f * vy + refy + i * 2.5f * vy, 17 * vy, 0.75f * vy), "Puntuació: " + info[i, 5] + "  Asimetria: " + info[i, 6] + "%  Distància: " + info[i, 7] + "m  Dret.Sup: " + info[i, 8] + "º  Esq.Sup: " + info[i, 9] + "º  Dre.Inf: " + info[i, 10] + "º  Esq.Inf: " + info[i, 11] + "º");

                    GUI.skin.button.normal.background = transparente; GUI.skin.button.hover.background = transparente; GUI.skin.button.active.background = transparente;
                    GUI.DrawTexture(new Rect(21 * vy + refx, 4.7f * vy + refy + i * 2.5f * vy, 3 * vy, 0.75f * vy), boton);
                    if (GUI.Button(new Rect(21 * vy + refx, 4.7f * vy + refy + i * 2.5f * vy, 3 * vy, 0.75f * vy), "Historial")) { PlayerPrefs.SetString("UsuarioEstudio",info[i,1]) ; abrirHistorial(); }
                    GUI.DrawTexture(new Rect(24.25f * vy + refx, 4.7f * vy + refy + i * 2.5f * vy, 0.75f * vy, 0.75f * vy), boton1x1);
                    if (GUI.Button(new Rect(24.25f * vy + refx, 4.7f * vy + refy + i * 2.5f * vy, 0.75f * vy, 0.75f * vy), "X")) { principal = false; idEliminar = int.Parse(info[i,0]); eliminarConfirm = true; }

                }
            }
            else
            {
                if (!errorDeCarga)
                {
                    if (!sinResultados)
                    {
                        //Cargando
                        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                        GUI.Label(new Rect(8.5f * vy + refx, 4.5f * vy + refy, h - 10 * vy, 2 * vy), "Carregant...");
                    }
                    else
                    {
                        //No resultados
                        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                        GUI.Label(new Rect(8.5f * vy + refx, 4.5f * vy + refy, h - 10 * vy, 2 * vy), "No s'han trobat resultats");
                    }

                }
                else
                {
                    //Error al cargar
                    GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                    GUI.Label(new Rect(8.5f * vy + refx, 4.5f * vy + refy, h - 10 * vy, 2 * vy), "No s'ha pogut carregar la informació");
                }
            }

            GUI.EndScrollView();
        }

        if (eliminarConfirm) {
            GUI.DrawTexture(new Rect(12*vy,3*vy,10*vy,5*vy),Sombra);
            if (!eliminando)
            {
                GUI.skin.label.fontSize = (int)(0.55f * vy);
                GUI.Label(new Rect(12.5f * vy, 3 * vy, 9 * vy, 2 * vy), "Si elimina aquest estudi no el podrà recuperar!");
                GUI.skin.button.normal.background = transparente; GUI.skin.button.hover.background = transparente; GUI.skin.button.active.background = transparente;
                GUI.DrawTexture(new Rect(12.5f * vy, 6.5f * vy, 3 * vy, 0.75f * vy), boton);
                if (GUI.Button(new Rect(12.5f * vy, 6.5f * vy, 3 * vy, 0.75f * vy), "Eliminar")) { StartCoroutine(eliminarEstudio()); }
                GUI.DrawTexture(new Rect(18.5f * vy, 6.5f * vy, 3 * vy, 0.75f * vy), boton);
                if (GUI.Button(new Rect(18.5f * vy, 6.5f * vy, 3 * vy, 0.75f * vy), "Cancel·lar")) { principal = true; eliminarConfirm = false; }

            }
            else {
                GUI.Label(new Rect(12.5f * vy, 3 * vy, 9 * vy, 2 * vy), "Eliminant...");
            }

        }
        

    }

    //Abrir historial de paciente seleccionado
    void abrirHistorial() {
     GameObject crear =  Instantiate(historial,transform.position,Quaternion.identity) as GameObject;
        crear.transform.parent = this.transform.parent;
        Destroy(this.gameObject);
    }

    //Eliminar estudio del servidor
    IEnumerator eliminarEstudio() {
        eliminando = true;
        WWWForm form = new WWWForm();
        form.AddField("Id", idEliminar);
        WWW w = new WWW("http://vrproyect.esy.es/eliminarEstudios.php", form);
        yield return w;
        Debug.Log(w.text + "ID:"+idEliminar.ToString());
        eliminando = false;
        if (w.error == null)
        {
            principal = true;
            eliminarConfirm = false;
            StartCoroutine(leerInfo());

        }
        else
        {
           

        }
    }

    //Descargar datos del estudio
    IEnumerator leerInfo()
    {
        infoProcesada = false;
        sinResultados = false;
        WWWForm form = new WWWForm(); 
        form.AddField("filtro", filtro);
        WWW w = new WWW("http://vrproyect.esy.es/buscaEstudios.php", form);
        yield return w;
        if (w.error == null)
        {
            if (w.text != "No hay resultados")
            {
                procesarInfo(w.text);
            }
            else {
                sinResultados = true;
            }
            
        }
        else
        {
            errorDeCarga = true;

        }
    }

    //Procesado de la informacion descargada
    void procesarInfo(string data) {
        string[] data1 = data.Split("|"[0]);
        info = new string[data1.Length,12];
        for (int i = 0; i<data1.Length; i++) {
            string[] data2 = data1[i].Split(":"[0]);
            for (int j = 0; j<12; j++) {
                info[i, j] = data2[j];
            }           
        }
        infoProcesada = true;
    }

    //Control de update de busqueda
    void FixedUpdate() {
        if (cntBuscar>0) {
            cntBuscar -= 1;
            Debug.Log(cntBuscar.ToString());
            if (cntBuscar==0) {
                StartCoroutine(leerInfo());
            }
        }
    }

}
