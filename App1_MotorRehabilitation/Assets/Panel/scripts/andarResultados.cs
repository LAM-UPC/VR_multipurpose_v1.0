using UnityEngine;
using System.Collections;

public class andarResultados : MonoBehaviour {

    //Medidas escalado
    int vy = Screen.height / 15;

    //Idioma
    int idioma = 0; // 0. Català 1.Castellano 2.English 

    //Texturas
    Texture2D sombreado;
    Texture2D fondoGris;
    Texture2D azul;
    Texture2D blanco;
    Texture2D verde;
    Texture2D rojo;
    Texture2D transparente;
    public Texture2D cuadricula;
    public Texture2D cuadricula2;

    //Config. graficas
    float[] datosGF;
    float[,] datosGF_Global;
    int[] maxValueGlobal;
    int valorElegido = 1;
    int datosEnGf = 0;
    bool datosLeidos = false;

    //Datos
    float[,] estudiDatsPerCent;
    float[,] estudiDats;
    string[,] otherDats;
    float[] maxEscale;
    float[] minEscale;
    float[] rangeEscale;
    string newTxt = "";

    //Scroll
    Vector2 scroll = Vector2.zero;

    void Start () {


        //Crear texturas
        sombreado = new Texture2D(1,1);
        sombreado.SetPixel(0,0,new Color(0,0,0,0.6f));
        sombreado.Apply();

        fondoGris = new Texture2D(1, 1);
        fondoGris.SetPixel(0, 0, new Color(0, 0, 0, 0.6f));
        fondoGris.Apply();

        azul = new Texture2D(1, 1);
        azul.SetPixel(0, 0, new Color(0, 1, 1, 0.6f));
        azul.Apply();

        blanco = new Texture2D(1, 1);
        blanco.SetPixel(0, 0, new Color(0, 1, 1, 0.6f));
        blanco.Apply();


        rojo = new Texture2D(1, 1);
        rojo.SetPixel(0, 0, new Color(1, 0, 0, 0.6f));
        rojo.Apply();

        verde = new Texture2D(1, 1);
        verde.SetPixel(0, 0, new Color(0, 1, 0, 0.6f));
        verde.Apply();

        transparente = new Texture2D(1,1);
        transparente.SetPixel(0,0,new Color(0,0,0,0));
        transparente.Apply();

        StartCoroutine( leerResultados() );

    }

    //Interface
    void OnGUI()
    {
        
        GUI.depth = 13;

        string txt = "";
        //Valores comparativos
        GUI.skin.label.normal.textColor = Color.white;
        GUI.skin.label.fontSize = (int)(0.35f * vy);
        GUI.skin.label.fontStyle = FontStyle.BoldAndItalic;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        switch (idioma) { case 0: txt = "Resultats: (Comparativa)"; break; case 1: txt = "Resultados: (Comparativa)"; break; case 2: txt = "Results: (Comparative)"; break; }
        GUI.Label(new Rect(7.5f * vy, 0.5f * vy, 6 * vy, 0.5f * vy), txt);
        GUI.DrawTexture(new Rect(7.5f * vy, 1 * vy, 9.1f * vy, 5.5f * vy), sombreado);

        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.skin.label.fontSize = (int)(0.3f*vy);
        GUI.Label(new Rect(7.6f * vy, 1.1f * vy, 2.9f * vy, 0.5f * vy),"Dades");
        GUI.Label(new Rect(10.6f * vy, 1.1f * vy, 1.9f * vy, 0.5f * vy), "Seleccionat");
        GUI.Label(new Rect(12.6f * vy, 1.1f * vy, 1.9f * vy, 0.5f * vy), "Últim");
        GUI.Label(new Rect(14.6f * vy, 1.1f * vy, 1.9f * vy, 0.5f * vy), "Millora");

        string[] nombres = {"Puntuació","Distancia","Asimetria","Amplitut Dre.","Amplitut Esq.","Amplitut Dre.Inf.","Amplitut Esq.Inf." };

        for (int i = 0; i<7; i++) {
            if (datosLeidos)
            {
                GUI.skin.label.normal.textColor = Color.white;
                GUI.DrawTexture(new Rect(7.6f * vy, 1.6f * vy + i * 0.7f * vy, 2.9f * vy, 0.6f * vy), sombreado);
                GUI.Label(new Rect(7.6f * vy, 1.6f * vy + i * 0.7f * vy, 2.9f * vy, 0.6f * vy), nombres[i]);

                GUI.DrawTexture(new Rect(10.6f * vy, 1.6f * vy + i * 0.7f * vy, 1.9f * vy, 0.6f * vy), sombreado);
                string valor = "";
                switch (i) {
                    case 0: valor = (estudiDats.Length > 12 ? estudiDats[valorElegido, i].ToString() : "-" );
                        break;
                    case 1:
                        valor = (estudiDats.Length > 12 ? estudiDats[valorElegido, i].ToString()+" m" : "-");
                        break;
                    case 2:
                        valor = (estudiDats.Length > 12 ? estudiDats[valorElegido, i].ToString()+" %" : "-");
                        break;
                    case 3:
                        valor = (estudiDats.Length > 12 ? estudiDats[valorElegido, i].ToString()+" º" : "-");
                        break;
                    case 4:
                        valor = (estudiDats.Length > 12 ? estudiDats[valorElegido, i].ToString()+" º" : "-");
                        break;
                    case 5:
                        valor = (estudiDats.Length > 12 ? estudiDats[valorElegido, i].ToString()+" º" : "-");
                        break;
                    case 6:
                        valor = (estudiDats.Length > 12 ? estudiDats[valorElegido, i].ToString()+" º" : "-");
                        break;
                }
                GUI.Label(new Rect(10.6f * vy, 1.6f * vy + i * 0.7f * vy, 1.9f * vy, 0.6f * vy), valor );

                GUI.DrawTexture(new Rect(12.6f * vy, 1.6f * vy + i * 0.7f * vy, 1.9f * vy, 0.6f * vy), sombreado);
                switch (i)
                {
                    case 0:
                        valor = estudiDats[0, i].ToString();
                        break;
                    case 1:
                        valor = estudiDats[0, i].ToString() + " m";
                        break;
                    case 2:
                        valor = estudiDats[0, i].ToString() + " %";
                        break;
                    case 3:
                        valor = estudiDats[0, i].ToString() + " º";
                        break;
                    case 4:
                        valor = estudiDats[0, i].ToString() + " º";
                        break;
                    case 5:
                        valor = estudiDats[0, i].ToString() + " º";
                        break;
                    case 6:
                        valor = estudiDats[0, i].ToString() + " º";
                        break;
                }
                GUI.Label(new Rect(12.6f * vy, 1.6f * vy + i * 0.7f * vy, 1.9f * vy, 0.6f * vy), valor);

                GUI.DrawTexture(new Rect(14.6f * vy, 1.6f * vy + i * 0.7f * vy, 1.9f * vy, 0.6f * vy), sombreado);
                if (estudiDats.Length > 12)
                {
                    if (i == 2)
                    {
                        GUI.skin.label.normal.textColor = (estudiDats[0, i] > estudiDats[valorElegido, i] ? Color.red : Color.green);
                    }
                    else {
                        GUI.skin.label.normal.textColor = (estudiDats[0, i] > estudiDats[valorElegido, i] ? Color.green : Color.red);
                    }
                    
                    GUI.Label(new Rect(14.6f * vy, 1.6f * vy + i * 0.7f * vy, 1.9f * vy, 0.6f * vy), (estudiDats[0, i] > estudiDats[valorElegido, i] ? ((((estudiDats[0, i] - estudiDats[valorElegido, i]) / estudiDats[0, i])) * 100).ToString("F2") : ((((estudiDats[0, i] - estudiDats[valorElegido, i]) / estudiDats[valorElegido, i])) * 100).ToString("F2")) + " %");
                }
                else {
                    GUI.Label(new Rect(14.6f * vy, 1.6f * vy + i * 0.7f * vy, 1.9f * vy, 0.6f * vy),"-");
                }

            }
        }

        
        //Grafica resultados        
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUI.skin.label.normal.textColor = Color.white;

        if (datosLeidos)
        {
            valorElegido = (estudiDats.Length > 12 ? valorElegido : 0);

            GUI.DrawTexture(new Rect(7.5f * vy, 8 * vy, 19 * vy, 5.5f * vy), sombreado);
            GUI.Label(new Rect(7.5f * vy, 7.5f * vy, 19 * vy, 0.5f * vy), "Dades de: " + otherDats[valorElegido, 1]);
            GUI.skin.label.fontSize = (int)(0.25f * vy);


            GUI.Label(new Rect(9.5f * vy, 8.1f * vy, 19 * vy, 0.5f * vy), "Dirigit per: " + otherDats[valorElegido, 2] + ", " + otherDats[valorElegido, 3] + "  (" + otherDats[valorElegido, 4] + ")");
            GUI.Label(new Rect(9.5f * vy, 8.5f * vy, 19 * vy, 0.5f * vy), "Resultats: ");
            GUI.skin.label.fontSize = (int)(0.22f * vy);
            valorElegido = (estudiDats.Length > 7 ? valorElegido : 0);
            GUI.Label(new Rect(9.5f * vy, 8.85f * vy, 19 * vy, 0.5f * vy), "Puntuació: " + estudiDats[valorElegido, 0] + "    " + "Distancia: " + estudiDats[valorElegido, 1] + "m    " + "Asimetria: " + estudiDats[valorElegido, 2] + "%    " + "Amplitut Dre.: " + estudiDats[valorElegido, 3] + "º    " + "Amplitut Esq.: " + estudiDats[valorElegido, 4] + "º    " + "Amplitut Dre.Inf.: " + estudiDats[valorElegido, 5] + "º    " + "Amplitut Esq.Inf: " + estudiDats[valorElegido, 6] + "º");




            float sl = 9.52f * vy;
            float altura = 0.5f * vy;
            GUI.skin.button.fontSize = (int)(0.3f * vy);

            GUI.DrawTexture(new Rect(9.5f * vy, sl - 0.02f * vy, 16.55f * vy, 3.95f * vy), blanco);


            GUI.skin.button.hover.background = blanco;
            GUI.skin.button.active.background = blanco;

            GUI.skin.button.normal.background = (datosEnGf == 0 ? blanco : sombreado);
            if (GUI.Button(new Rect(23.5f * vy, sl, 2.5f * vy, altura - 0.02f * vy), "Puntuació")) { datosEnGf = 0; }; sl += altura;

            GUI.skin.button.normal.background = (datosEnGf == 1 ? blanco : sombreado);
            if (GUI.Button(new Rect(23.5f * vy, sl, 2.5f * vy, altura - 0.02f * vy), "Distancia")) { datosEnGf = 1; }; sl += altura;

            GUI.skin.button.normal.background = (datosEnGf == 2 ? blanco : sombreado);
            if (GUI.Button(new Rect(23.5f * vy, sl, 2.5f * vy, altura - 0.02f * vy), "Asimetria")) { datosEnGf = 2; }; sl += altura;

            GUI.skin.button.normal.background = (datosEnGf == 3 ? blanco : sombreado);
            if (GUI.Button(new Rect(23.5f * vy, sl, 2.5f * vy, altura - 0.02f * vy), "Amplitud D.")) { datosEnGf = 3; }; sl += altura;

            GUI.skin.button.normal.background = (datosEnGf == 4 ? blanco : sombreado);
            if (GUI.Button(new Rect(23.5f * vy, sl, 2.5f * vy, altura - 0.02f * vy), "Amplitud E.")) { datosEnGf = 4; }; sl += altura;

            GUI.skin.button.normal.background = (datosEnGf == 5 ? blanco : sombreado);
            if (GUI.Button(new Rect(23.5f * vy, sl, 2.5f * vy, altura - 0.02f * vy), "Amplitud D.Inf")) { datosEnGf = 5; }; sl += altura;

            GUI.skin.button.normal.background = (datosEnGf == 6 ? blanco : sombreado);
            if (GUI.Button(new Rect(23.5f * vy, sl, 2.5f * vy, altura - 0.02f * vy), "Amplitud E.Inf")) { datosEnGf = 6; }; sl += altura;

            GUI.skin.label.alignment = TextAnchor.MiddleRight;
            GUI.Label(new Rect(8.3f * vy, 9.68f * vy, vy, 0.38f * vy), rojo);
            GUI.Label(new Rect(8.3f * vy, 9.68f * vy, vy, 0.38f * vy), maxEscale[datosEnGf].ToString("F0"));
            float incremento = (maxEscale[datosEnGf] - minEscale[datosEnGf]);
            GUI.Label(new Rect(8.3f * vy, 10.08f * vy, vy, 0.38f * vy), (maxEscale[datosEnGf] - (incremento / 7)).ToString("F0"));
            GUI.Label(new Rect(8.3f * vy, 10.48f * vy, vy, 0.38f * vy), (maxEscale[datosEnGf] - 2 * (incremento / 7)).ToString("F0"));
            GUI.Label(new Rect(8.3f * vy, 10.88f * vy, vy, 0.38f * vy), (maxEscale[datosEnGf] - 3 * (incremento / 7)).ToString("F0"));
            GUI.Label(new Rect(8.3f * vy, 11.28f * vy, vy, 0.38f * vy), (maxEscale[datosEnGf] - 4 * (incremento / 7)).ToString("F0"));
            GUI.Label(new Rect(8.3f * vy, 11.68f * vy, vy, 0.38f * vy), (maxEscale[datosEnGf] - 5 * (incremento / 7)).ToString("F0"));
            GUI.Label(new Rect(8.3f * vy, 12.08f * vy, vy, 0.38f * vy), (maxEscale[datosEnGf] - 6 * (incremento / 7)).ToString("F0"));
            GUI.Label(new Rect(8.3f * vy, 12.48f * vy, vy, 0.38f * vy), (maxEscale[datosEnGf] - 7 * (incremento / 7)).ToString("F0"));
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;

            int barras = (estudiDats.Length / 12 > 28 ? estudiDats.Length / 12 : 29);
            scroll = GUI.BeginScrollView(new Rect(9.5f * vy, 9.5f * vy, 14 * vy, 3.9f * vy), scroll, new Rect(9.5f * vy, 9.5f * vy, (barras - 1) * (3.5f / 7) * vy + 3 * vy, 3.5f * vy));



            for (int i = 0; i < barras / 7 + 1; i++)
            {
                GUI.DrawTexture(new Rect((9.5f + i * 3.5f) * vy, 9.5f * vy, 3.5f * vy, 3.5f * vy), cuadricula2);
            }

            float valorRef = 0;
            int maxValorRef = 0;
            if (datosLeidos)
            {

                
                GUI.skin.button.normal.background = transparente; GUI.skin.button.hover.background = azul; GUI.skin.button.active.background = azul;
                for (int i = 0; i < estudiDatsPerCent.Length / 7; i++)
                {
                    GUI.DrawTexture(new Rect(9.65f * vy + (barras - 1) * (3.5f / 7) * vy - (i + 1) * 7.5f * vy / 15, 13 * vy - estudiDatsPerCent[i, datosEnGf] * 3.1f * vy, 7.5f * vy / 30, estudiDatsPerCent[i, datosEnGf] * 3.1f * vy), (i == 0 ? blanco : azul));
                    if (i == valorElegido)
                    {
                        GUI.DrawTexture(new Rect(9.65f * vy + (barras - 1) * (3.5f / 7) * vy - (i + 1) * 7.5f * vy / 15, 13 * vy - estudiDatsPerCent[i, datosEnGf] * 3.1f * vy, 7.5f * vy / 30, estudiDatsPerCent[i, datosEnGf] * 3.1f * vy), (i == 0 ? blanco : azul));
                    }


                    if (GUI.Button(new Rect(9.65f * vy + (barras - 1) * (3.5f / 7) * vy - (i + 1) * 7.5f * vy / 15, 10 * vy, 7.5f * vy / 30, 3.5f * vy), ""))
                    {
                        valorElegido = i;
                    }
                    maxValorRef = (i != 0 & valorRef < estudiDatsPerCent[i, datosEnGf] ? i : maxValorRef);
                    valorRef = (i != 0 & valorRef < estudiDatsPerCent[i, datosEnGf] ? estudiDatsPerCent[i, datosEnGf] : valorRef);

                }
            }

            //Linea maximo
            GUI.DrawTexture(new Rect(9.5f * vy, 13 * vy - estudiDatsPerCent[maxValorRef, datosEnGf] * 3.1f * vy, (barras - 1) * (3.5f / 7) * vy, 0.05f * vy), rojo);
            
            if (datosEnGf == 2)
            {
                GUI.DrawTexture(new Rect(9.65f * vy + (barras - 1) * (3.5f / 7) * vy - (0 + 1) * 7.5f * vy / 15, 13 * vy - estudiDatsPerCent[maxValorRef, datosEnGf] * 3.1f * vy, 7.5f * vy / 30, (estudiDatsPerCent[maxValorRef, datosEnGf] - estudiDatsPerCent[0, datosEnGf]) * 3.1f * vy), (estudiDatsPerCent[maxValorRef, datosEnGf] > estudiDatsPerCent[0, datosEnGf] ? verde : rojo));
                GUI.DrawTexture(new Rect(9.65f * vy + (barras - 1) * (3.5f / 7) * vy - (0 + 1) * 7.5f * vy / 15, 13 * vy - estudiDatsPerCent[0, datosEnGf] * 3.1f * vy, 2 * vy, 0.05f * vy), (estudiDatsPerCent[maxValorRef, datosEnGf] > estudiDatsPerCent[0, datosEnGf] ? verde : rojo));

            }
            else {
                GUI.DrawTexture(new Rect(9.65f * vy + (barras - 1) * (3.5f / 7) * vy - (0 + 1) * 7.5f * vy / 15, 13 * vy - estudiDatsPerCent[maxValorRef, datosEnGf] * 3.1f * vy, 7.5f * vy / 30, (estudiDatsPerCent[maxValorRef, datosEnGf] - estudiDatsPerCent[0, datosEnGf]) * 3.1f * vy), (estudiDatsPerCent[maxValorRef, datosEnGf] > estudiDatsPerCent[0, datosEnGf] ? rojo : verde));
                GUI.DrawTexture(new Rect(9.65f * vy + (barras - 1) * (3.5f / 7) * vy - (0 + 1) * 7.5f * vy / 15, 13 * vy - estudiDatsPerCent[0, datosEnGf] * 3.1f * vy, 2 * vy, 0.05f * vy), (estudiDatsPerCent[maxValorRef, datosEnGf] > estudiDatsPerCent[0, datosEnGf] ? rojo : verde));

            }

            GUI.skin.label.alignment = TextAnchor.LowerRight;
            if (datosEnGf == 2)
            {
                GUI.skin.label.normal.textColor = (estudiDatsPerCent[maxValorRef, datosEnGf] > estudiDatsPerCent[0, datosEnGf] ? Color.green : Color.red);

            }
            else {
                GUI.skin.label.normal.textColor = (estudiDatsPerCent[maxValorRef, datosEnGf] > estudiDatsPerCent[0, datosEnGf] ? Color.red : Color.green);

            }
            GUI.Label(new Rect(9.65f * vy + (barras - 1) * (3.5f / 7) * vy - (0 + 1) * 7.5f * vy / 15, 12 * vy - estudiDatsPerCent[0, datosEnGf] * 3.1f * vy, 2 * vy, vy), (((estudiDats[0, datosEnGf] - estudiDats[maxValorRef, datosEnGf]) / maxEscale[datosEnGf]) * 100).ToString("F2") + "%");
            GUI.skin.label.normal.textColor = Color.white;

         //   Debug.Log((((estudiDats[0, datosEnGf] - estudiDats[maxValorRef, datosEnGf]) / maxEscale[datosEnGf]) * 100).ToString("F2"));


            GUI.EndScrollView();

        }

    }

    //Leer datos del servidor
    IEnumerator leerResultados() {
      
       
        string resultados = "";
        WWWForm form = new WWWForm();
        form.AddField("filtro", PlayerPrefs.GetString("UsuarioEstudio"));
        WWW w = new WWW("http://vrproyect.esy.es/buscaEstudios.php", form);
        yield return w;
        if (w.error == null)
        {

            if (w.text != "No hay resultados")
            {
                subProcesado(w.text);
                Debug.Log(resultados);
            }
            else
            {
                //sinResultados = true;
            }

        }
        else
        {
            // errorDeCarga = true;
        }
        calcularDatosGlobales(newTxt);

    }

    //Procesar info descargada
    void subProcesado(string txt)
    {

        string[] estudiosThis = txt.Split("|"[0]);
        newTxt = "";
        otherDats = new string[estudiosThis.Length,5];
        for (int i = 0; i < estudiosThis.Length; i++) {
            string[] subDats = estudiosThis[i].Split(":"[0]);
            otherDats[i, 0] = subDats[0];  //ID
            otherDats[i, 1] = subDats[1];  //Pacient
            otherDats[i, 2] = subDats[2];  //Resp
            otherDats[i, 3] = subDats[3];  //Centre
            otherDats[i, 4] = subDats[4];  //Data
            newTxt +=( newTxt!="" ?  "," + subDats[5] + "|" + subDats[7] + "|" + subDats[6] + "|" + subDats[8] + "|" + subDats[9] + "|" + subDats[10] + "|" + subDats[11] : subDats[5] + "|" + subDats[7] + "|" + subDats[6] + "|" + subDats[8] + "|" + subDats[9] + "|" + subDats[10] + "|" + subDats[11]);
        }
    }

    //Calculo con info descargada
    void calcularDatosGlobales(string datos) {

        maxEscale = new float[7];
        minEscale = new float[7];
        rangeEscale = new float[7];

        string[] estudis = datos.Split(","[0]);
        estudiDats = new float[estudis.Length,12];
        estudiDatsPerCent = new float[estudis.Length, 7]; 
        for (int i = 0; i< estudis.Length; i++) {

            Debug.Log("Grupo "+(i+1).ToString()+": "+estudis[i]);
            string[] dats = estudis[i].Split("|"[0]);

            for (int j = 0; j<7; j++) {
               
                estudiDats[i, j] = float.Parse(dats[j]);

                maxEscale[j] = (maxEscale[j] < estudiDats[i, j] ? estudiDats[i, j] : maxEscale[j]);
                minEscale[j] = (minEscale[j] == 0 | minEscale[j] > estudiDats[i,j] ? estudiDats[i,j] : minEscale[j]);
               

            }

        }

        for (int i = 0; i<7; i++) {
            rangeEscale[i] = maxEscale[i] - minEscale[i]; 
        }

        Debug.Log("Maximos: " + maxEscale[0].ToString() + "  " + maxEscale[1].ToString() + "  " + maxEscale[2].ToString() + "  " + maxEscale[3].ToString() + "  " + maxEscale[4].ToString() + "  " + maxEscale[5].ToString() + "  " + maxEscale[6].ToString());
        Debug.Log("Minimos: " + minEscale[0].ToString() + "  " + minEscale[1].ToString() + "  " + minEscale[2].ToString() + "  " + minEscale[3].ToString() + "  " + minEscale[4].ToString() + "  " + minEscale[5].ToString() + "  " + minEscale[6].ToString());


        //Valor porcentual de 0.2 a 1
        for (int i = 0; i < estudis.Length; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                estudiDatsPerCent[i, j] = ( rangeEscale[j] != 0 ? (estudiDats[i, j] - minEscale[j]) / rangeEscale[j] * 0.9f + 0.1f : 1 );
            }

        }

        scroll = new Vector2((estudiDatsPerCent.Length > 28 ? estudiDatsPerCent.Length : 29) * 3.5f * vy, 0);
        datosLeidos = true;
    }

}
