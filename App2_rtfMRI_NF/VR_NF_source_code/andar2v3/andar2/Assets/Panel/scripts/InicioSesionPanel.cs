using UnityEngine;
using System.Collections;
#if UNITY_METRO
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
#else
using System.Text;
using System.Security.Cryptography;
#endif

public class InicioSesionPanel : MonoBehaviour {

    int h = Screen.width;
    int v = Screen.height;
    int vy = Screen.height / 15;
    int idioma = 0;

    Texture2D grisOscuro;
    Texture2D sombreado;
    Texture2D azul;
    public Texture2D sombra;
    public Texture2D botonGris;

    GameObject panelPrincipal;
    public GameObject menu;
    bool conectando = false;
    bool errorConexion = false;
    bool usuarioError = false;

    string usuario = "";
    string contraseña = "";

    void Start()
    {

        //Texturas
        grisOscuro = new Texture2D(1, 1);
        grisOscuro.SetPixel(0, 0, new Color(0.7f, 0.7f, 0.7f, 0.9f));
        grisOscuro.Apply();

        sombreado = new Texture2D(1, 1);
        sombreado.SetPixel(0, 0, new Color(0, 0, 0, 0.4f));
        sombreado.Apply();

        azul = new Texture2D(1, 1);
        azul.SetPixel(0, 0, new Color(0, 0.6f, 0.7f, 1));
        azul.Apply();

        panelPrincipal = GameObject.Find("panelDeControl");

        if (!PlayerPrefs.HasKey("idioma"))
        {
            PlayerPrefs.SetInt("idioma", 0);
        }
        idioma = PlayerPrefs.GetInt("idioma");

    }

    //Interface
    void OnGUI()
    {

        GUI.depth = 14;

        GUI.DrawTexture(new Rect(0, 0, h, v), grisOscuro);
        GUI.DrawTexture(new Rect(0, 0, h, v), sombra);

        string txt = "";
        GUI.skin.label.normal.textColor = Color.white;
        GUI.skin.label.fontSize = (int)( vy);
        GUI.skin.label.fontStyle = FontStyle.BoldAndItalic;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        switch (idioma) { case 0: txt = "Benvingut,"; break; case 1: txt = "Bienvenido,"; break; case 2: txt = "Welcome,"; break; }
        GUI.Label(new Rect(4 * vy, 2 * vy, h - 4 * vy, 2 * vy), txt);

        GUI.skin.label.normal.textColor = Color.white;
        GUI.skin.label.fontSize = (int)(0.5f * vy);
        GUI.skin.label.fontStyle = FontStyle.Bold;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;

        switch (idioma) { case 0: txt = "Iniciar sessió"; break; case 1: txt = "Inicio de sesión"; break; case 2: txt = "Log in"; break; }
        GUI.Label(new Rect(0.5f*h-4.5f*vy,3.7f*vy,h,0.8f*vy),txt);
        
        GUI.skin.label.fontSize = (int)(0.7f * vy);
        GUI.skin.label.fontStyle = FontStyle.BoldAndItalic;
        GUI.skin.textField.fontSize = (int)(0.6f*vy);
        GUI.skin.textField.alignment = TextAnchor.MiddleCenter;

        GUI.DrawTexture(new Rect(0.5f*h-4.5f*vy,4.5f*vy,9*vy,8.5f*vy),sombreado);

        switch (idioma) { case 0: txt = "Usuari"; break; case 1: txt = "Usuario"; break; case 2: txt = "User"; break; }
        GUI.Label(new Rect(0.5f*h-4 * vy, 5 * vy, 8 * vy, vy), txt);
        usuario = GUI.TextField(new Rect(0.5f*h-4*vy, 6 * vy,  8 * vy, vy),usuario,40);

        switch (idioma) { case 0: txt = "Contrasenya"; break; case 1: txt = "Contraseña"; break; case 2: txt = "Password"; break; }
        GUI.Label(new Rect(0.5f*h-4 * vy, 7.5f * vy, h - 6 * vy, vy), txt);
        contraseña = GUI.PasswordField(new Rect(0.5f * h - 4 * vy, 8.5f * vy, 8 * vy, vy), contraseña, "*"[0], 25);



        GUI.skin.button.fontSize = (int)(0.4f * vy);
        GUI.skin.button.normal.textColor = Color.white;
        GUI.skin.button.hover.textColor = Color.cyan;
        GUI.skin.button.active.textColor = Color.cyan;
        GUI.skin.button.active.background = botonGris;
        GUI.skin.button.hover.background = botonGris;
        GUI.skin.button.normal.background = botonGris;
        GUI.skin.button.fontStyle = FontStyle.BoldAndItalic;

        switch (idioma) { case 0: txt = "Entrar"; break; case 1: txt = "Entrar"; break; case 2: txt = "Enter"; break; }
        if (!conectando) {
            if (GUI.Button(new Rect(0.5f * h - 2 * vy, 10.5f * vy, 4 * vy, vy), txt))
            {
                conectando = true;
                StartCoroutine(autenticar(usuario, contraseña));
            }
        }else
        {
            switch (idioma) { case 0: txt = "Autenticant..."; break; case 1: txt = "Autenticando..."; break; case 2: txt = "Authenticating..."; break; }
            GUI.skin.label.fontSize = (int)(0.45f * vy);
            GUI.Label(new Rect(0.5f * h - 2 * vy, 10.5f * vy, 4 * vy, vy), txt);
        }
        GUI.skin.label.fontSize = (int)(0.35f*vy);
        switch (idioma) { case 0: txt = "Error de conexió"; break; case 1: txt = "Error de conexión"; break; case 2: txt = "Connection error"; break; }
        if (errorConexion) { GUI.Label(new Rect(0.5f * h - 4 * vy, 12 * vy, 9 * vy, vy), txt); }
        else {
            switch (idioma) { case 0: txt = "Les dades no són correctes"; break; case 1: txt = "Los datos no son correctos"; break; case 2: txt = "Wrong data"; break; }
            if (usuarioError) { GUI.Label(new Rect(0.5f * h - 4 * vy, 12 * vy, 9 * vy, vy), txt); }
        }

      /*  if (GUI.Button(new Rect(h-2*vy,v-2*vy,vy,vy),"a")) {
            GameObject crearObj = (GameObject)Instantiate(menu, transform.position, Quaternion.identity);
            crearObj.transform.parent = panelPrincipal.transform;
            Destroy(this.gameObject);
        }*/

    }

    //Autenticar
    IEnumerator autenticar(string usuario, string contraseña)
    {
        bool autenticacion = false;
        WWWForm form = new WWWForm();
        form.AddField("usuario", usuario);
        contraseña = Md5Sum(contraseña);
        form.AddField("contra", contraseña);
        WWW w = new WWW("http://vrproyect.esy.es/autenticar.php", form);
        yield return w;
        if (w.error == null)
        {
           
            if (w.text != "Error")
            {
                string[] data = w.text.Split("|"[0]);
                PlayerPrefs.SetString("Usuario", data[0]+" "+data[1]);
                PlayerPrefs.SetString("Organizacion", data[2]);
                autenticacion = true;
            }
            else {
                usuarioError = true;
                conectando = false;
            }
            
        }
        else
        {
            errorConexion = true;
            conectando = false;
        }


            yield return autenticacion;

        if (autenticacion)
        {     
            GameObject crearObj = (GameObject)Instantiate(menu, transform.position, Quaternion.identity);
            crearObj.transform.parent = panelPrincipal.transform;
            Destroy(this.gameObject);
        }

    }

    public static string Md5Sum(string strToEncrypt)
    {
#if UNITY_METRO

        // Convert the message string to binary data.
        IBuffer buffUtf8Msg = CryptographicBuffer.ConvertStringToBinary(strToEncrypt, BinaryStringEncoding.Utf8);


        // Create a HashAlgorithmProvider object.
        HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);

        // Demonstrate how to retrieve the name of the hashing algorithm.
        string strAlgNameUsed = objAlgProv.AlgorithmName;

        // Hash the message.
        IBuffer buffHash = objAlgProv.HashData(buffUtf8Msg);

        // Verify that the hash length equals the length specified for the algorithm.
        if (buffHash.Length != objAlgProv.HashLength)
            return null;

        // Convert the hash to a string (for display).
        string strHashBase64 = CryptographicBuffer.EncodeToBase64String(buffHash);

        // Return the encoded string
        return strHashBase64.PadLeft(32, '0');
#else
        UTF8Encoding ue = new UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
#endif
    }

}
