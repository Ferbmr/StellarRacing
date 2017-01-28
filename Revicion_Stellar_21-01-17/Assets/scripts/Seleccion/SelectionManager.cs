using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour {

    //NAVES
    public GameObject NavesGUI; //GUI de seleccion de naves
    private GameObject[] navesList; //lista de naves
    public int naveNum; //Nave seleccionada
    public GameObject lasNaves; //objeto que contiene todas las naves
    //PISTAS    
    public GameObject PistasGUI; //GUI de seleccion de Pistas
    private GameObject[] pistasList; //lista de pistas
    public int pistaNum; //pista seleccionada
    public int pista; //escena que se debe cargar
    public GameObject LasPistas; // objeto que contiene todas las pistas
	public AudioSource audio;

	void Start () {
        //Inicia solo con el GUI de naves visible, oculta el GUI de pistas
        NavesGUI.SetActive(true);
        PistasGUI.SetActive(false);

        //Indica que la lista de naves, seran los hijos del objeto que las contine
        navesList = new GameObject[lasNaves.transform.childCount];//+++++++++++++++++¿LINEA DE MAS?++++++++++++++++++++
        //llena el arreglo con los hijos del objeto que incluye las naves
        for (int i = 0; i < navesList.Length; i ++)
            navesList[i] = lasNaves.transform.GetChild(i).gameObject;
        //Oculta todas las naves menos la que esta en seleccion
        foreach (GameObject noSeleccionado in navesList)
            noSeleccionado.SetActive(false);
        if (navesList[naveNum])
            navesList[naveNum].SetActive(true);
        
        //Indica que la lista de pistas, seran los hijos del objeto que las contiene
        pistasList = new GameObject[LasPistas.transform.childCount];//+++++++++++++++++¿LINEA DE MAS?++++++++++++++++++++
        //llena el arreglo con los hijos del objeto que incluye las pistas
        for (int j = 0; j < pistasList.Length; j++)
            pistasList[j] = LasPistas.transform.GetChild(j).gameObject;
        //oculta todas las pistas, menos la que esta en seleccion
        foreach (GameObject noSelected in pistasList)
            noSelected.SetActive(false);
        if (pistasList[pistaNum])
            pistasList[pistaNum].SetActive(true);
	
	}
	
	void Update () {
        //Mantiene girando las naves
        lasNaves.GetComponent<Transform>().RotateAround(Vector3.zero, Vector3.up, 360.0f * Time.deltaTime/10.0f);
	}
    //+++++++++++++++++++++++++++++++++++
    //++++++++++++BOTONES NAVES++++++++++
    //+++++++++++++++++++++++++++++++++++
    //Flecha izquierda de seleccion, naves
    public void ToggleLeftNaves()
    {
        //desactiva la nave actual para pasar a la siguiente
        navesList[naveNum].SetActive(false);
        //cambia de objeto en la lista, por ende, cambia de nave
        naveNum--;
        //Indica que si se llega al inicio de la lista, se vaya al final de la misma. creando un loop.
        if (naveNum < 0)
            naveNum = navesList.Length - 1;
        //activa la nave siguiente
        navesList[naveNum].SetActive(true);
    }
    //Flecha derecha de seleccion, naves
    public void ToggleRightNaves()
    {
        //desactiva la nave actual para pasar a la siguiente
        navesList[naveNum].SetActive(false);
        //cambia de objeto en la lista, por ende, cambia de nave
        naveNum++;
        //Indica que si se llega al final de la lista, se vaya al inicio de la misma. creando un loop.
        if (naveNum == navesList.Length)
            naveNum = 0;
        //activa la nave siguiente
        navesList[naveNum].SetActive(true);
    }
    //Boton de confirmacion, naves
    public void ConfirmButtonNaves()
    {
		audio.Play ();
		//Desactiva el GUI de las naves, y activa el de pistas
		NavesGUI.SetActive (false);
		PistasGUI.SetActive (true);
		//Guarda el valor de la nave seleccionada
		PlayerPrefs.SetInt ("miNave", naveNum);
    }
    //Boton volver, naves
    public void BackButtonNaves(){
		audio.Play ();
        //Carga la escena de inicio
        SceneManager.LoadScene("Intro");
    }
    //Boton de opciones
    public void OptionsButton(){
		audio.Play ();
        //Carga la escena de opciones
        SceneManager.LoadScene("menu_opciones");
    }

    //++++++++++++++++++++++++++++++++++++
    //++++++++++++BOTONES PISTAS++++++++++
    //++++++++++++++++++++++++++++++++++++
    //Flecha izquierda de seleccion, pistas
    public void ToggleLeftPistas()
    {
        //desactiva la pista actual para pasar a la siguiente
        pistasList[pistaNum].SetActive(false);
        //cambia de objeto en la lista, por ende, cambia de pista
        pistaNum--;
        //Indica que si se llega al inicio de la lista, se vaya al final de la misma. creando un loop.
        if (pistaNum < 0)
            pistaNum = pistasList.Length - 1;
        //activa la pista siguiente
        pistasList[pistaNum].SetActive(true);
    }
    //Flecha derecha de seleccion, pistas
    public void ToggleRightPistas()
    {
        //desactiva la pista actual para pasar a la siguiente
        pistasList[pistaNum].SetActive(false);
        //cambia de objeto en la lista, por ende, cambia de pista
        pistaNum++;
        //Indica que si se llega al final de la lista, se vaya al inicio de la misma. creando un loop.
        if (pistaNum == pistasList.Length)
            pistaNum = 0;
        //activa la pista siguiente
        pistasList[pistaNum].SetActive(true);
    }
    //Boton de confirmacion, pistas
    public void ConfirmButtonPistas()
    {
		audio.Play ();
        //almacena la pista a cargar. 
        pista = pistaNum + 2;
        //guarda el valor de la pista en las preferencias
        PlayerPrefs.SetInt("Pista", pista);
        //Carga la escena de la pantalla de carga
        SceneManager.LoadScene("LoadingScreen");
    }
    //Boton de volver, pistas
    public void BackButtonPistas(){
		audio.Play ();
        //Desactiva el GUI de pistas y activa el de naves
        PistasGUI.SetActive(false);
        NavesGUI.SetActive(true);
    }
}
