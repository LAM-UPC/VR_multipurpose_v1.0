using UnityEngine;
using System.Collections;

public class gestioDePacients : MonoBehaviour {

    int h = Screen.width;
   // int v = Screen.height;
    int vy = Screen.height / 15;
    int cntBuscar = 0;
    int idEliminar = 0;
    int id = 0;

    Texture2D Sombra;
    Texture2D blanco;
    Texture2D transparente;
    public Texture2D buscarIco;
    public Texture2D boton;
    public Texture2D boton1x1;
    public Texture2D boton1x8;

    public GameObject historial;

    Vector2 scrollView = Vector2.zero;

    string[,] info;

    bool infoProcesada = false;
    bool errorDeCarga = false;
    bool sinResultados = false;
    bool principal = true;
    bool eliminarConfirm = false;
    bool eliminando = false;
    bool mesInfo = false;
    bool editarPacient = false; 
    bool cargandoNuevoPaciente = false;
    bool eliminarConfirmPaciente = false;
    bool eliminandoPaciente = false;
    bool newPaciente = false;
    bool cargandoNuevoPacienteDos = false;

    string filtro = "";
    string nuevoNombre = "";
    string nuevoApellidos = "";
    string nuevaFecha = "";
    string sexo = "";
    string nuevoCorreo = "";
    string nuevoCentroDeEstudio = "";
    string nombreResp = "";
    string fechaDeCreacion="";

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

        //0_ID  1_Nombre 2_Apellidos  3_Edad  4_Sexo  5_Correo  6_Centro  7_Direccion 8_Responsable  9_Fecha
        //string msg = "MiID:Josep:Marco:Visyon:17/06/09:1950:150:23:24:26:27:28|MiID:Josep2:Marco2:Visyon:17/06/09:1950:150:23:24:26:27:28";
        StartCoroutine(leerInfo());

    }

    void OnGUI() {
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.skin.label.fontSize = (int)(0.65f * vy);
        GUI.Label(new Rect(8.5f * vy, 0.25f * vy, 8 * vy, vy), "Llistat de pacients");
        GUI.DrawTexture(new Rect(8.5f * vy, 1.25f * vy, h - 7.5f * vy, 0.05f * vy), blanco);
        GUI.skin.label.fontStyle = FontStyle.Bold;
        GUI.skin.label.fontSize = (int)(0.4f * vy);

        //Pantalla principal
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
            GUI.DrawTexture(new Rect(8 * vy + refx, 4 * vy + refy, h - 9 * vy, 11 * vy), Sombra);
            scrollView = GUI.BeginScrollView(new Rect(8 * vy + refx, 4 * vy + refy, h - 9 * vy, 11 * vy), scrollView, new Rect(8 * vy + refx, 4 * vy + refy, h - 9.5f * vy,( infoProcesada ? ((info.Length/10) * 2.5f * vy + 3*vy) : 9*vy)));


            GUI.skin.label.normal.textColor = new Color(0, 0, 0, 1);
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            if (infoProcesada)
            {
                for (int i = 0; i < info.Length / 10; i++)
                {
                    GUI.DrawTexture(new Rect(8.5f * vy + refx, 4.5f * vy + refy + i * 2.5f * vy, h - 10 * vy, 2 * vy), blanco);
                    GUI.skin.label.fontSize = (int)(0.4f * vy);
                    GUI.Label(new Rect(8.75f * vy + refx, 4.5f * vy + refy + i * 2.5f * vy, 14 * vy, 0.75f * vy), "Pacient: " + info[i, 1]+" "+ info[i, 2]);
                    GUI.Label(new Rect(8.75f * vy + refx, 5 * vy + refy + i * 2.5f * vy, 14 * vy, 0.75f * vy), "Responsable: " + info[i, 8] + ", " + info[i, 6]);
                    GUI.skin.label.fontSize = (int)(0.35f * vy);
                    GUI.Label(new Rect(8.75f * vy + refx, 5.75f * vy + refy + i * 2.5f * vy, 17 * vy, 0.75f * vy), "("+ info[i, 5] +")");

                    GUI.skin.button.normal.background = transparente; GUI.skin.button.hover.background = transparente; GUI.skin.button.active.background = transparente;
                    GUI.DrawTexture(new Rect(22 * vy + refx, 4.7f * vy + refy + i * 2.5f * vy, 3 * vy, 0.75f * vy), boton);
                    if (GUI.Button(new Rect(22 * vy + refx, 4.7f * vy + refy + i * 2.5f * vy, 3 * vy, 0.75f * vy), "Informació")) { principal = false; id = i; mesInfo = true; }
                    GUI.DrawTexture(new Rect(22 * vy + refx, 5.7f * vy + refy + i * 2.5f * vy, 3 * vy, 0.75f * vy), boton);
                    if (GUI.Button(new Rect(22 * vy + refx, 5.7f * vy + refy + i * 2.5f * vy, 3 * vy, 0.75f * vy), "Historial")) { PlayerPrefs.SetString("UsuarioEstudio", info[i, 1] +" "+info[i,2] ); abrirHistorial(); }

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
            GUI.DrawTexture(new Rect(14 * vy, 14f * vy, 8 * vy, vy),boton1x8);
            GUI.skin.button.alignment = TextAnchor.MiddleCenter;
            GUI.skin.button.fontSize = (int)(0.5f*vy);
            if (GUI.Button(new Rect(14*vy,14f*vy,8*vy,vy),"Afergir nou pacient")) {
                newPaciente = true;
                principal = false;
                natejarFormulari();
            }
        }

        //Pantalla nuevo paciente
        if (newPaciente) {
            int idioma = 0;
            string txt = "";
            GUI.DrawTexture(new Rect(9 * vy, 1.5f*vy, 16 * vy, 12.5f * vy), Sombra);
            if (!cargandoNuevoPacienteDos)
            {
                float sl = 1.5f * vy;
                GUI.skin.label.fontSize = (int)(0.45f * vy);
                GUI.skin.label.alignment = TextAnchor.MiddleLeft;
              
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
                GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.5f * vy), txt); sl += 0.5f * vy;
                nuevoCorreo = GUI.TextField(new Rect(9.5f * vy, sl, 5.5f * vy, 0.6f * vy), nuevoCorreo); sl += 0.8f * vy;

                switch (idioma) { case 0: txt = "Centre d'estudi:"; break; case 1: txt = "Centro de estudio:"; break; case 2: txt = "Study Center:"; break; }
                GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.5f * vy), txt); sl += 0.5f * vy;
                nuevoCentroDeEstudio = GUI.TextField(new Rect(9.5f * vy, sl, 5.5f * vy, 0.6f * vy), nuevoCentroDeEstudio); sl += 0.8f * vy;

                GUI.skin.label.fontSize = (int)(0.45f * vy);
                GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                switch (idioma) { case 0: txt = "Responsable:"; break; case 1: txt = "Responsable:"; break; case 2: txt = "Manager:"; break; }
                GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.7f * vy), txt);
                GUI.DrawTexture(new Rect(9.5f * vy, sl + 0.7f * vy, 7 * vy, 0.05f * vy), blanco);
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


                GUI.skin.button.normal.background = transparente; GUI.skin.button.hover.background = transparente; GUI.skin.button.active.background = transparente;
                switch (idioma) { case 0: txt = "Afegir"; break; case 1: txt = "Agregar"; break; case 2: txt = "Add"; break; }
                GUI.DrawTexture(new Rect(9.5f * vy, sl, 4 * vy, vy),boton);
                if (GUI.Button(new Rect(9.5f * vy, sl, 4 * vy,  vy), txt))
                {
                    StartCoroutine(subirPaciente());
                }
                GUI.DrawTexture(new Rect(h - 5.25f * vy, 14.25f * vy, 3 * vy, 0.75f * vy), boton);
                switch (idioma) { case 0: txt = "Tornar"; break; case 1: txt = "Volver"; break; case 2: txt = "Back"; break; }
                if (GUI.Button(new Rect(h - 5.25f * vy, 14.25f * vy, 3 * vy, 0.75f * vy), txt))
                {
                    principal = true;
                    newPaciente = false;
                }
            }
            else
            {
                switch (idioma) { case 0: txt = "Carregant..."; break; case 1: txt = "Cargando..."; break; case 2: txt = "Loading..."; break; }
                GUI.Label(new Rect(15.5f * vy, 6.5f * vy, 5 * vy, vy), txt);
            }
        }

        //Pantalla confirmar eliminacion
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


        if (eliminarConfirmPaciente)
        {
            GUI.DrawTexture(new Rect(12 * vy, 3 * vy, 10 * vy, 5 * vy), Sombra);
            if (!eliminandoPaciente)
            {
                GUI.skin.label.fontSize = (int)(0.55f * vy);
                GUI.Label(new Rect(12.5f * vy, 3 * vy, 9 * vy, 2 * vy), "Si continua eliminarà totes les dades d'aquest pacient!");
                GUI.skin.button.normal.background = transparente; GUI.skin.button.hover.background = transparente; GUI.skin.button.active.background = transparente;
                GUI.DrawTexture(new Rect(12.5f * vy, 6.5f * vy, 3 * vy, 0.75f * vy), boton);
                if (GUI.Button(new Rect(12.5f * vy, 6.5f * vy, 3 * vy, 0.75f * vy), "Eliminar")) { StartCoroutine( eliminarPaciente()); }
                GUI.DrawTexture(new Rect(18.5f * vy, 6.5f * vy, 3 * vy, 0.75f * vy), boton);
                if (GUI.Button(new Rect(18.5f * vy, 6.5f * vy, 3 * vy, 0.75f * vy), "Cancel·lar")) { mesInfo = true; eliminarConfirmPaciente = false; }

            }
            else
            {
                GUI.Label(new Rect(12.5f * vy, 3 * vy, 9 * vy, 2 * vy), "Eliminant...");
            }
        }

        //Abrir información extra
        if (mesInfo) {
            int idioma = 0;
            string txt = "";
            float sl = 1.5f * vy;
            GUI.DrawTexture(new Rect(9 * vy, 2 * vy, 14 * vy, 11.5f * vy), Sombra);
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            GUI.skin.label.fontSize = (int)(0.4f * vy);
            sl += vy;
            switch (idioma) { case 0: txt = "Nom:"; break; case 1: txt = "Nombre:"; break; case 2: txt = "Name:"; break; }
            GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.5f * vy), txt);
            switch (idioma) { case 0: txt = "Cognom:"; break; case 1: txt = "Apellido:"; break; case 2: txt = "Last name:"; break; }
            GUI.Label(new Rect(15.5f * vy, sl, 5 * vy, 0.5f * vy), txt);
            sl += 0.5f * vy;
            GUI.Label(new Rect(9.5f * vy, sl, 5.5f * vy, 0.6f * vy), info[id,1]);
            GUI.Label(new Rect(15.5f * vy, sl, 7.5f * vy, 0.6f * vy), info[id,2]);
            sl += 0.8f * vy;
            switch (idioma) { case 0: txt = "Data de naixement:"; break; case 1: txt = "Fecha de nacimiento:"; break; case 2: txt = "Birthdate"; break; }
            GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.5f * vy), txt);
            switch (idioma) { case 0: txt = "Gènere:"; break; case 1: txt = "Género:"; break; case 2: txt = "Gender:"; break; }
            GUI.Label(new Rect(15.5f * vy, sl, 5 * vy, 0.5f * vy), txt);
            sl += 0.5f * vy;
            GUI.Label(new Rect(9.5f * vy, sl, 2.5f * vy, 0.6f * vy), info[id,3]);
            GUI.Label(new Rect(15.5f * vy, sl, 2.5f * vy, 0.6f * vy), info[id,4]);
            sl += 0.8f * vy;

            switch (idioma) { case 0: txt = "Correu:"; break; case 1: txt = "Correo:"; break; case 2: txt = "Email:"; break; }
            GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.5f * vy), txt); sl += 0.5f * vy;
            GUI.Label(new Rect(9.5f * vy, sl, 5.5f * vy, 0.6f * vy), info[id,5]); sl += 0.8f * vy;

            switch (idioma) { case 0: txt = "Centre d'estudi:"; break; case 1: txt = "Centro de estudio:"; break; case 2: txt = "Study Center:"; break; }
            GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.5f * vy), txt); sl += 0.5f * vy;
            GUI.Label(new Rect(9.5f * vy, sl, 5.5f * vy, 0.6f * vy), info[id,6]); sl += 0.8f * vy;

            GUI.skin.label.fontSize = (int)(0.45f * vy);
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            switch (idioma) { case 0: txt = "Responsable:"; break; case 1: txt = "Responsable:"; break; case 2: txt = "Manager:"; break; }
            GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.7f * vy), txt);
            GUI.DrawTexture(new Rect(9.5f * vy, sl + 0.7f * vy, 7 * vy, 0.05f * vy), blanco);
            GUI.skin.label.fontSize = (int)(0.35f * vy);
            sl += vy;
            switch (idioma) { case 0: txt = "Nom i cognom:"; break; case 1: txt = "Nombre y apallidos:"; break; case 2: txt = "Name and last name:"; break; }
            GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.5f * vy), txt);
            sl += 0.5f * vy;
            GUI.Label(new Rect(9.5f * vy, sl, 5.5f * vy, 0.6f * vy), info[id,8]);
            sl += 0.8f * vy;
            switch (idioma) { case 0: txt = "Data:"; break; case 1: txt = "Fecha:"; break; case 2: txt = "Date:"; break; }
            GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.5f * vy), txt);
            sl += 0.5f * vy;
            GUI.Label(new Rect(9.5f * vy, sl, 5.5f * vy, 0.6f * vy), info[id,9]);
            sl += 1.5f * vy;

          
            switch (idioma) { case 0: txt = "Tornar"; break; case 1: txt = "Volver"; break; case 2: txt = "Back"; break; }
            GUI.skin.button.normal.background = transparente; GUI.skin.button.hover.background = transparente; GUI.skin.button.active.background = transparente;
            GUI.DrawTexture(new Rect(h - 3.5f * vy, 14 * vy, 3 * vy, 0.75f * vy), boton);
            if (GUI.Button(new Rect(h - 3.5f * vy, 14 * vy, 3 * vy, 0.75f * vy), txt))
            {
                mesInfo = false;
                principal = true;
            }

            switch (idioma) { case 0: txt = "Eliminar"; break; case 1: txt = "Eliminar"; break; case 2: txt = "Delete"; break; }
            GUI.skin.button.normal.background = transparente; GUI.skin.button.hover.background = transparente; GUI.skin.button.active.background = transparente;
            GUI.DrawTexture(new Rect(h - 3.5f * vy, 3 * vy, 3 * vy, 0.75f * vy), boton);
            if (GUI.Button(new Rect(h - 3.5f * vy, 3 * vy, 3 * vy, 0.75f * vy), txt))
            {
                mesInfo = false;
                eliminarConfirmPaciente = true;
            }

            switch (idioma) { case 0: txt = "Modificar"; break; case 1: txt = "Editar"; break; case 2: txt = "Edit"; break; }
            GUI.skin.button.normal.background = transparente; GUI.skin.button.hover.background = transparente; GUI.skin.button.active.background = transparente;
            GUI.DrawTexture(new Rect(h - 3.5f * vy, 2 * vy, 3 * vy, 0.75f * vy), boton);
            if (GUI.Button(new Rect(h - 3.5f * vy, 2 * vy, 3 * vy, 0.75f * vy), txt))
            {
                carregarDades();
                mesInfo = false;
                editarPacient = true;
            }
        }

        //Editar informacion de paciente
        if (editarPacient) {
            int idioma = 0;
            string txt = "";


            GUI.DrawTexture(new Rect(9 * vy, 2 * vy, 14 * vy, 11.5f * vy), Sombra);
            if (!cargandoNuevoPaciente)
            {
                float sl = 1.5f * vy;
                GUI.skin.label.alignment = TextAnchor.MiddleLeft;

                GUI.skin.label.fontSize = (int)(0.4f * vy);
                sl += vy;
                switch (idioma) { case 0: txt = "Nom:"; break; case 1: txt = "Nombre:"; break; case 2: txt = "Name:"; break; }
                GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.5f * vy), txt);
                switch (idioma) { case 0: txt = "Cognom:"; break; case 1: txt = "Apellido:"; break; case 2: txt = "Last name:"; break; }
                GUI.Label(new Rect(15.5f * vy, sl, 5 * vy, 0.5f * vy), txt);
                sl += 0.5f * vy;
                nuevoNombre = GUI.TextField(new Rect(9.5f * vy, sl, 5.5f * vy, 0.6f * vy), nuevoNombre);
                nuevoApellidos = GUI.TextField(new Rect(15.5f * vy, sl, 7 * vy, 0.6f * vy), nuevoApellidos);
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
                GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.5f * vy), txt); sl += 0.5f * vy;
                nuevoCorreo = GUI.TextField(new Rect(9.5f * vy, sl, 5.5f * vy, 0.6f * vy), nuevoCorreo); sl += 0.8f * vy;

                switch (idioma) { case 0: txt = "Centre d'estudi:"; break; case 1: txt = "Centro de estudio:"; break; case 2: txt = "Study Center:"; break; }
                GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.5f * vy), txt); sl += 0.5f * vy;
                nuevoCentroDeEstudio = GUI.TextField(new Rect(9.5f * vy, sl, 5.5f * vy, 0.6f * vy), nuevoCentroDeEstudio); sl += 0.8f * vy;

                GUI.skin.label.fontSize = (int)(0.45f * vy);
                GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                switch (idioma) { case 0: txt = "Responsable:"; break; case 1: txt = "Responsable:"; break; case 2: txt = "Manager:"; break; }
                GUI.Label(new Rect(9.5f * vy, sl, 5 * vy, 0.7f * vy), txt);
                GUI.DrawTexture(new Rect(9.5f * vy, sl + 0.7f * vy, 7 * vy, 0.05f * vy), blanco);
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
                
                switch (idioma) { case 0: txt = "Modificar"; break; case 1: txt = "Editar"; break; case 2: txt = "Edit"; break; }
                GUI.skin.button.normal.background = transparente; GUI.skin.button.hover.background = transparente; GUI.skin.button.active.background = transparente;
                GUI.DrawTexture(new Rect(h - 7.5f * vy, 12.5f * vy, 3 * vy, 0.75f * vy), boton);
                if (GUI.Button(new Rect(h - 7.5f * vy, 12.5f * vy, 3 * vy, 0.75f * vy), txt))
                {
                    StartCoroutine(editarPacienteConect());
                }


                switch (idioma) { case 0: txt = "Tornar"; break; case 1: txt = "Volver"; break; case 2: txt = "Back"; break; }
                GUI.skin.button.normal.background = transparente; GUI.skin.button.hover.background = transparente; GUI.skin.button.active.background = transparente;
                GUI.DrawTexture(new Rect(h - 3.5f * vy, 14 * vy, 3 * vy, 0.75f * vy), boton);
                if (GUI.Button(new Rect(h - 3.5f * vy, 14 * vy, 3 * vy, 0.75f * vy), txt))
                {
                    mesInfo = true;
                    editarPacient = false;
                }
            }

            else
            {
                switch (idioma) { case 0: txt = "Carregant..."; break; case 1: txt = "Cargando..."; break; case 2: txt = "Loading..."; break; }
                GUI.Label(new Rect(15.5f * vy, 6.5f * vy, 5 * vy, vy), txt);
            }

            
        }
        

    }

    //Cargar pantalla historial
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

    //Subir nuevo paciente el servidor
    IEnumerator subirPaciente()
    {
        cargandoNuevoPacienteDos = true;
        WWWForm form = new WWWForm();
        form.AddField("Nombre", nuevoNombre);
        form.AddField("Apellidos", nuevoApellidos);
        form.AddField("FechaNacimiento", nuevaFecha);
        form.AddField("Correo", nuevoCorreo);
        form.AddField("Instituto", nuevoCentroDeEstudio);
        form.AddField("Responsable", nombreResp);
        form.AddField("Fecha", fechaDeCreacion);
        form.AddField("Sexo", sexo);
        WWW w = new WWW("http://vrproyect.esy.es/nuevoPaciente.php", form);
        yield return w;
        cargandoNuevoPacienteDos = false;
        if (w.error == null)
        {
            newPaciente = false;
            principal = true;
            StartCoroutine(leerInfo());
        }
        else
        {

        }
    }

    //Eliminar paciente del servidor
    IEnumerator eliminarPaciente()
    {
        eliminandoPaciente = true;
        WWWForm form = new WWWForm();
        form.AddField("Id", info[id,0]);
        WWW w = new WWW("http://vrproyect.esy.es/eliminarPacientes.php", form);
        yield return w;
        eliminandoPaciente = false;
        if (w.error == null)
        {
           principal = true;
           eliminarConfirmPaciente = false;
            StartCoroutine(leerInfo());

        }
        else
        {


        }
    }

    //Editar paciente del servidor
    IEnumerator editarPacienteConect()
    {
        cargandoNuevoPaciente = true;
        WWWForm form = new WWWForm();
        form.AddField("Id", info[id,0]);
        form.AddField("Nombre",nuevoNombre);
        form.AddField("Apellidos", nuevoApellidos);
        form.AddField("Edad", nuevaFecha);
        form.AddField("Sexo", sexo);
        form.AddField("Correo", nuevoCorreo);
        form.AddField("Centro", nuevoCentroDeEstudio);
        form.AddField("Responsable", nombreResp);
        form.AddField("Fecha", fechaDeCreacion);
        WWW w = new WWW("http://vrproyect.esy.es/editarPaciente.php", form);
        yield return w;
        cargandoNuevoPaciente = false;
        if (w.error == null)
        {
            mesInfo = true;
            editarPacient = false;
            StartCoroutine(leerInfo());

        }
        else
        {


        }
    }

    //Ler informacion de paciente
    IEnumerator leerInfo()
    {
        infoProcesada = false;
        sinResultados = false;
        WWWForm form = new WWWForm(); 
        form.AddField("filtro", filtro);
        WWW w = new WWW("http://vrproyect.esy.es/buscaPacientes2.php", form);
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

    //Resetear formulario
    void natejarFormulari() {
        nuevoNombre = "";
        nuevoApellidos = "";
        nuevaFecha = "dd/mm/aaaa";
        sexo = "";
        nuevoCorreo = "";
        nuevoCentroDeEstudio = PlayerPrefs.GetString("Organizacion");
        nombreResp = PlayerPrefs.GetString("Usuario") ;
        fechaDeCreacion = System.DateTime.Now.Day +"/"+System.DateTime.Now.Month+"/"+System.DateTime.Now.Year;

    }

    //Cargar datos en el formulario
    void carregarDades() {
        nuevoNombre = info[id,1];
        nuevoApellidos = info[id, 2];
        nuevaFecha = info[id,3];
        sexo = info[id,4];
        nuevoCorreo = info[id, 5];
        nuevoCentroDeEstudio = info[id,6];
        nombreResp = info[id,8];
        fechaDeCreacion = info[id, 9];
        //0_ID  1_Nombre 2_Apellidos  3_Edad  4_Sexo  5_Correo  6_Centro  7_Direccion 8_Responsable  9_Fecha
    }

    //Procesar información descargada
    void procesarInfo(string data) {
        string[] data1 = data.Split("|"[0]);
        info = new string[data1.Length,10];
        for (int i = 0; i<data1.Length; i++) {
            string[] data2 = data1[i].Split(":"[0]);
            for (int j = 0; j<10; j++) {
                info[i, j] = data2[j];
            }           
        }
        infoProcesada = true;
    }

    //Actualizacion de buscqueda
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
 