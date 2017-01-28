using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Path : MonoBehaviour {

	public Color colorLinea; //Color de la linea guia
	private List<Transform> nodos = new List<Transform>(); //Lista de nodos

	void OnDrawGizmos(){
		Gizmos.color = colorLinea; //le da color a la linea
		Transform[] pathTransforms = GetComponentsInChildren<Transform>(); //crea un arreglo de los hijos del objeto "Path"
		nodos = new List<Transform>(); //lista vacia de nodos

		for (int i = 0; i < pathTransforms.Length; i++) { //llena la lista nodos
			if (pathTransforms [i] != transform) {
				nodos.Add (pathTransforms [i]);
			}
		}

		for (int i = 0; i < nodos.Count; i++) { //crea la linea entre nodos
			Vector3 nodoActual = nodos [i].position; 
			Vector3 nodoAnterior = Vector3.zero;
			if (i > 0) { //determina cual nodo va antes, si es el primero (nodo 0), toma el ultimo nodo como anterior creando un circuito
				nodoAnterior = nodos [i - 1].position;
			} else if (i == 0 && nodos.Count > 1) {
				nodoAnterior = nodos [nodos.Count - 1].position;
			}
			Gizmos.DrawLine (nodoAnterior, nodoActual); //dibuja la linea
			Gizmos.DrawWireSphere (nodoActual, 0.3f); //dibuja una esfera en cada nodo
		}
	}
}
