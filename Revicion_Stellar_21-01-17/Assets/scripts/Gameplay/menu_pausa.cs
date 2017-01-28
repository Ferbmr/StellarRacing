using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class menu_pausa : MonoBehaviour {
	public GameObject PauseUi;
	public bool paused;

	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
		PauseUi.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) && !paused) {
			Time.timeScale = 0;
			PauseUi.SetActive (true);
		} else if(Input.GetKeyDown (KeyCode.Escape) && paused){
			Time.timeScale = 1;
			PauseUi.SetActive (false);
		}
	}

	public void Reanudar(){
		PauseUi.SetActive (false);
		Time.timeScale = 1;
		paused = false;

	}


	public void Reiniciar(){
		if (SceneManager.GetActiveScene().name == "pista01")
		{
			SceneManager.LoadScene ("pista01");
		}
		if (SceneManager.GetActiveScene().name == "pista02")
		{
			SceneManager.LoadScene ("pista02");
		}
		if (SceneManager.GetActiveScene().name == "pista03")
		{
			SceneManager.LoadScene ("pista03");
		}
		if (SceneManager.GetActiveScene().name == "pista04")
		{
			SceneManager.LoadScene ("pista04");
		}

	}
	public void Salir(){
		Time.timeScale = 1;
		SceneManager.LoadScene ("carSelection");
	}
}
