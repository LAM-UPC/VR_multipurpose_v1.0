/*AUTOR: DAVID PRADOS
 * VERSIÓN:1
 * DESCRIPCIÓN: CUENTA 12 SEGUNDOS HASTA EJECUTAR LA SIGUIENTE ESCENA
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class timing : MonoBehaviour {

    public int tiempo;
    
	void Start () {
        tiempo = 12;
        StartCoroutine(espera(tiempo));
		
	}

    IEnumerator espera(int esp)
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(esp);
            SceneManager.LoadScene("paseoPorElLago");
            
        }

    }

}
