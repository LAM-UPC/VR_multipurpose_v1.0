using UnityEngine;
using System.Collections;

public class portaCamaras : MonoBehaviour {

   //Lectura del objeto camara
   public GameObject Camara;
	
	void FixedUpdate () {
        float altura = 14.8f + (4-Camara.transform.localPosition.y);
        transform.position = new Vector3(transform.position.x,altura,transform.position.z);
	}
}
