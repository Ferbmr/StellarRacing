using UnityEngine;
using System.Collections;

public class contador_inicio : MonoBehaviour {
	public bool countDown = false;
	public float tiempo = 3.5f;
	public void Start(){
		tiempo = 3.5f;
		countDown = false;
		Time.timeScale = 0;
	}
	public void SetCountDownNow(){
		countDown = true;
		Time.timeScale = 1;
	}
	public void Update(){
	}
}
