/*AUTOR: DAVID PRADOS
 * VERSIÓN:1
 * DESCRIPCIÓN: GENERA EL ARCHIVO DE TEXTO CON LOS DATOS DEL EXPERIMENTO
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class txtfile : MonoBehaviour {

    public string filename = "C:/VR_instruction/result.txt";
    public string vel;
    public string nfsignal;
    public string dist;
   

    void Start () {

        vel = PlayerPrefs.GetString("velocityresult");
        nfsignal = PlayerPrefs.GetString("signalresult");
        dist = PlayerPrefs.GetString("distresult");

        //Creación del fichero de texto y escritura
        var sr = File.CreateText(filename);
        sr.WriteLine("Velocity: " + vel + "\r\n NFSignal: " + nfsignal + "\r\n Distance: " + dist + "m");
        sr.Close();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
