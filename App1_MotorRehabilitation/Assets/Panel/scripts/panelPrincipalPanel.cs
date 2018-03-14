using UnityEngine;
using System.Collections;

public class panelPrincipalPanel : MonoBehaviour {

    //Variables de relacion de aspecto 
    int h = Screen.width;
    int v = Screen.height;
    int vy = Screen.height / 15;
    int idioma = 0;

    Texture2D grisOscuro;
    Texture2D sombreado;
    Texture2D blanco;
    Texture2D azul;
    Texture2D transparente;
    public Texture2D sombra;
    public Texture2D sombraVolumen;
    public Texture2D botonGris;
    public Texture2D[] colaboradores;

    //GameObject panelPrincipal;
    public GameObject crearEstudio;
    public GameObject veureEstudios;
    public GameObject gestioDePacients;
    public GameObject gestioDePersonal;

    string usuario = "";
    string organizacion = "";

    void Start()
    {
        //Texturas
        grisOscuro = new Texture2D(1, 1);
        grisOscuro.SetPixel(0, 0, new Color(0.7f, 0.7f, 0.7f, 0.9f));
        grisOscuro.Apply();

        sombreado = new Texture2D(1, 1);
        sombreado.SetPixel(0, 0, new Color(0, 0, 0, 0.6f));
        sombreado.Apply();

        blanco = new Texture2D(1, 1);
        blanco.SetPixel(0, 0, new Color(0, 0, 0, 0.4f));
        blanco.Apply();

        transparente = new Texture2D(1, 1);
        transparente.SetPixel(0, 0, new Color(1, 1, 1, 0));
        transparente.Apply();

        azul = new Texture2D(1, 1);
        azul.SetPixel(0, 0, new Color(0, 0.6f, 0.7f, 1));
        azul.Apply();
        
        usuario = PlayerPrefs.GetString("Usuario");
        organizacion = PlayerPrefs.GetString("Organizacion");
        idioma = PlayerPrefs.GetInt("idioma");

        if (gameObject.transform.childCount == 0)
        {
            GameObject crearObj = (GameObject)Instantiate(crearEstudio, transform.position, Quaternion.identity);
            crearObj.transform.parent = this.transform;
        }
        
    }

    //Interface
    void OnGUI()
    {
        GUI.depth = 14;

        GUI.DrawTexture(new Rect(0, 0, h, v), grisOscuro);
        GUI.DrawTexture(new Rect(0, 0, h, v), sombra);

        //Fondo barra tareas
        int anchoPanel = 7 * vy;
        GUI.DrawTexture(new Rect(0,0,anchoPanel,v),blanco);
        GUI.DrawTexture(new Rect(0, 0, anchoPanel, v), sombra);
        
        GUI.DrawTexture(new Rect(0,0,anchoPanel,2*vy),azul);
        GUI.DrawTexture(new Rect(0, 1.9f*vy, anchoPanel, 0.1f * vy), sombreado);
        GUI.DrawTexture(new Rect(0, 0, anchoPanel, 2 * vy), sombra);
        GUI.DrawTexture(new Rect(0, 0, anchoPanel, 0.3f * vy), sombraVolumen);
        GUI.DrawTexture(new Rect(0, 2*vy, anchoPanel, 0.15f * vy), sombraVolumen);

        //Nombre
        GUI.skin.label.fontSize = (int)(0.45*vy);
        GUI.skin.label.normal.textColor = Color.white;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        GUI.Label(new Rect(0.25f*vy,0,anchoPanel-0.5f*vy,1.5f*vy),usuario);
        //Organizacion
        GUI.skin.label.fontSize = (int)(0.35 * vy);
        GUI.skin.label.fontStyle = FontStyle.BoldAndItalic;
        GUI.Label(new Rect(0.25f * vy, 1.3f * vy, anchoPanel - 0.5f * vy, 0.5f * vy), organizacion);

        //Nou estudi
        GUI.skin.button.normal.background = transparente;
        GUI.skin.button.hover.background = sombreado;
        GUI.skin.button.active.background = sombreado;
        GUI.skin.button.normal.textColor = Color.white;
        GUI.skin.button.hover.textColor = Color.cyan;
        GUI.skin.button.active.textColor = Color.cyan;
        GUI.skin.button.alignment = TextAnchor.MiddleLeft;
        GUI.skin.button.fontSize = (int)(0.5f*vy);

        int pos_y = 2 * vy;
        string txt = "";
        GUI.DrawTexture(new Rect(0, pos_y, anchoPanel, vy), sombra);
        GUI.DrawTexture(new Rect(0,pos_y+0.95f*vy,anchoPanel,0.05f*vy),sombreado);
        switch (idioma) { case 0: txt = "Nou estudi"; break; case 1: txt = "Nuevo estudio"; break; case 2: txt = "New project"; break; }
        if (GUI.Button(new Rect(0, pos_y, anchoPanel, vy), txt))
        {
            if (gameObject.transform.childCount>0) {
                Destroy(gameObject.transform.GetChild(0).gameObject);
            }
            
            GameObject crearObj = (GameObject)Instantiate(crearEstudio, transform.position, Quaternion.identity);
            crearObj.transform.parent = this.transform;
        }
        pos_y += vy;

        GUI.DrawTexture(new Rect(0, pos_y, anchoPanel, vy), sombra);
        GUI.DrawTexture(new Rect(0, pos_y + 0.95f * vy, anchoPanel, 0.05f * vy), sombreado);
        switch (idioma) { case 0: txt = "Veure estudis"; break; case 1: txt = "Ver estudios"; break; case 2: txt = "Load projects"; break; }
        if (GUI.Button(new Rect(0, pos_y, anchoPanel, vy), txt))
        {
            if (gameObject.transform.childCount > 0)
            {
                Destroy(gameObject.transform.GetChild(0).gameObject);
            }
            GameObject crearObj = (GameObject)Instantiate(veureEstudios, transform.position, Quaternion.identity);
            crearObj.transform.parent = this.transform;
        }
        pos_y += vy;

        GUI.DrawTexture(new Rect(0, pos_y, anchoPanel, vy), sombra);
        GUI.DrawTexture(new Rect(0, pos_y + 0.95f * vy, anchoPanel, 0.05f * vy), sombreado);
        switch (idioma) { case 0: txt = "Gestió de pacients"; break; case 1: txt = "Gestión de pacientes"; break; case 2: txt = "Patient management"; break; }
        if (GUI.Button(new Rect(0, pos_y, anchoPanel, vy),txt))
        {
            if (gameObject.transform.childCount > 0)
            {
                Destroy(gameObject.transform.GetChild(0).gameObject);
            }
            GameObject crearObj = (GameObject)Instantiate(gestioDePacients, transform.position, Quaternion.identity);
            crearObj.transform.parent = this.transform;
        }
        pos_y += vy;

        GUI.DrawTexture(new Rect(0, pos_y, anchoPanel, vy), sombra);
        GUI.DrawTexture(new Rect(0, pos_y + 0.95f * vy, anchoPanel, 0.05f * vy), sombreado);
        switch (idioma) { case 0: txt = "Sortir"; break; case 1: txt = "Salir"; break; case 2: txt = "Exit"; break; }
        if (GUI.Button(new Rect(0, pos_y, anchoPanel, vy), txt))
        {
            Application.Quit();
        }
        
       pos_y += vy;

        GUI.skin.button.alignment = TextAnchor.MiddleCenter;
    }
}
