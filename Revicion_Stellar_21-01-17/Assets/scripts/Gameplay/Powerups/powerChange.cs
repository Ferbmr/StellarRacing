using UnityEngine;
using System.Collections;

public class powerChange : MonoBehaviour {
	public GameObject pUp;
	public GameObject pDown;
	public float tiempo;
	public int valor;

	void Update(){
		tiempo += Time.deltaTime;
		valor = Random.Range (1, 2);
		if (tiempo >= 4) {
			if (valor == 1) {
				powerup ();
				tiempo = 0;
			} else if (valor == 2) {
				Instantiate (pDown);
				tiempo = 0;
			}
		}
	}

	IEnumerator powerup(){
		GameObject uno = Instantiate (pUp);
		yield return new WaitForSeconds (4);
		Destroy (uno);
	}
}
