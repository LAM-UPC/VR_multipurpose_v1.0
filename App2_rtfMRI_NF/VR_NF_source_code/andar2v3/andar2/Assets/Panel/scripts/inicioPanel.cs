using UnityEngine;
using System.Collections;

public class inicioPanel : MonoBehaviour
{
    //variables resolicion pantalla
    int h = Screen.width;
    int v = Screen.height;
    int vy = Screen.height / 15;
    int idioma = 0;

    //Texturas
    Texture2D grisOscuro;
    Texture2D azul;
    public Texture2D sombra;
    public Texture2D botonGris;
    public Texture2D[] colaboradores;

    //Referencia a otros objetos
    GameObject panelPrincipal;
    public GameObject inicioSesion;
    

    //Escena
    bool cargado = false;
    float loaded = 0;

    void Start()
    {
        //Texturas
        grisOscuro = new Texture2D(1, 1);
        grisOscuro.SetPixel(0, 0, new Color(0.3f, 0.3f, 0.3f, 0.9f));
        grisOscuro.Apply();

        azul = new Texture2D(1, 1);
        azul.SetPixel(0, 0, new Color(0, 0.7f, 0.7f, 0.9f));
        azul.Apply();

        panelPrincipal = GameObject.Find("panelDeControl");

        if (!PlayerPrefs.HasKey("idioma")) {
            PlayerPrefs.SetInt("idioma",0);
        }
        idioma = PlayerPrefs.GetInt("idioma");

        PlayerPrefs.SetString("COM1","");
        PlayerPrefs.SetString("COM2", "");
        PlayerPrefs.SetString("COM3", "");
        PlayerPrefs.SetString("COM4", "");
    }

    void OnGUI()
    {
        GUI.depth = 14;

        //fondo
        GUI.DrawTexture(new Rect(0, 0, h, v), grisOscuro);
        GUI.DrawTexture(new Rect(0, 0, h, v), sombra);

        if (cargado)
        {
            //Coloboradores
            float colaboradores_pos_x = 0.5f * vy;
            for (int i = 0; i < colaboradores.Length; i++)
            {
                float Ra = colaboradores[i].width / colaboradores[i].height;
                colaboradores_pos_x += Ra * 1.5f * vy;
                GUI.DrawTexture(new Rect(h - colaboradores_pos_x, v - 2.5f * vy, Ra * 1.5f * vy, 1.5f * vy), colaboradores[i]);
                colaboradores_pos_x += 0.5f * vy;
            }

            //Titulo
            GUI.skin.label.normal.textColor = Color.white;
            GUI.skin.label.fontSize = (int)(1.3f * vy);
            GUI.skin.label.fontStyle = FontStyle.BoldAndItalic;

            string txt = "";
            switch (idioma) { case 0: txt = "Benvingut,"; break; case 1: txt = "Bienvenido,"; break; case 2: txt = "Welcome,"; break; }
            GUI.Label(new Rect(2 * vy, 2 * vy, h - 4 * vy, 2 * vy), txt);

            GUI.skin.label.normal.textColor = Color.cyan;
            GUI.skin.label.fontSize = (int)(1.6f * vy);

            //Boton inicio sesion
            GUI.skin.button.fontSize = (int)(0.7f * vy);
            GUI.skin.button.normal.textColor = Color.white;
            GUI.skin.button.hover.textColor = Color.cyan;
            GUI.skin.button.active.textColor = Color.cyan;
            GUI.skin.button.active.background = botonGris;
            GUI.skin.button.hover.background = botonGris;
            GUI.skin.button.normal.background = botonGris;
            GUI.skin.button.fontStyle = FontStyle.BoldAndItalic;

            if (GUI.Button(new Rect((h - 16 * vy) / 3, 7 * vy, 8 * vy, 2.5f * vy), "Iniciar sesión"))
            {
                GameObject crearObj = (GameObject)Instantiate(inicioSesion, transform.position, Quaternion.identity);
                crearObj.transform.parent = panelPrincipal.transform;
                Destroy(this.gameObject);
            }

            //Boton juego libre
            if (GUI.Button(new Rect(2 * (h - 16 * vy) / 3 + 8 * vy, 7 * vy, 8 * vy, 2.5f * vy), "Juego libre"))
            {
            }
        }
        else {
            GUI.DrawTexture(new Rect(0.5f*h-7*vy,4*vy,14*vy,14*vy/2.8f),colaboradores[0]);
            GUI.DrawTexture(new Rect(0.5f * h - 7 * vy, 9 * vy,loaded* 14 * vy, 0.4f * vy ),azul);
            GUI.DrawTexture(new Rect(0.5f * h - 7 * vy, 9 * vy, loaded*14 * vy, 0.4f * vy), sombra);
        }
    }

    void FixedUpdate() {
        if (loaded < 1 & !cargado)
        {
            loaded += 0.01f;
        }
        else {
            cargado = true;
            GameObject crearObj = (GameObject)Instantiate(inicioSesion, transform.position, Quaternion.identity);
            crearObj.transform.parent = panelPrincipal.transform;
            Destroy(this.gameObject);
        }       
    }   

}