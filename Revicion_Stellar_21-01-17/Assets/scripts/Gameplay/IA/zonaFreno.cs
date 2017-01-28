using UnityEngine;
using System.Collections;

public class zonaFreno : MonoBehaviour {

	public float frenoTorque;
	//public float velocidadMin;

	void OnTriggerStay( Collider other){
		if (other.tag == "IA" && other.GetComponent<IA>()!=null){
			other.GetComponent<IA>().enZona = true;
			other.GetComponent<IA>().llantaDI.brakeTorque = frenoTorque*100;
			other.GetComponent<IA>().llantaTD.brakeTorque = frenoTorque*100;


		}
		
	}
	void OnTriggerExit( Collider other){
		if (other.tag == "IA" && other.GetComponent<IA>()!=null){
			other.GetComponent<IA>().enZona = false;
            other.GetComponent<IA>().llantaDI.brakeTorque = 0;
            other.GetComponent<IA>().llantaTD.brakeTorque = 0;

		}

	}
}
