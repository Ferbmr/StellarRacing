using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class opciones : MonoBehaviour {
	public GameObject creditos;
	public GameObject menuOpciones;


	// Use this for initialization
	void Start () {
		creditos.SetActive (false);
		menuOpciones.SetActive (true);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void GraficosAlto(){
		QualitySettings.currentLevel = QualityLevel.Fantastic;
		
	}
	public void GraficosMedio(){
		QualitySettings.currentLevel = QualityLevel.Good;
	}
	public void GraficosBajo(){
		QualitySettings.currentLevel = QualityLevel.Fastest;
	}
	public void RegresarSelection(){
		SceneManager.LoadScene ("carSelection");
	}
	public void Creditos(){
		creditos.SetActive (true);
		menuOpciones.SetActive (false);
	}
	public void Regresar(){
		creditos.SetActive (false);
		menuOpciones.SetActive (true);
	}
}
