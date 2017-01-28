using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class Gameplay_Manager : MonoBehaviour {
	//+++++++++++++++++++++++++++++++++++++++
	//++++++++++¿Cual es el Player?++++++++++
	//+++++++++++++++++++++++++++++++++++++++
	public GameObject naves; //Objeto en escena que contiene las naves
	private Transform[] player; //Arreglo de naves
	private int miVehiculo; //valor que se toma de los Playerprefs, de la seleccion de nave	
	private int wait = 0;
	private Vector3 positionVelocity; 

	//+++++++++++++++++++++++++++++++++++++++
	//++++++++++¿Cuales son las IAs?+++++++++
	//+++++++++++++++++++++++++++++++++++++++
	public GameObject IAs; //Objeto en escena que contiene las IAs
    private GameObject[] rivales; //Arreglos
    private IA IAscript;
    private int i;
	//+++++++++++++++++++++++++++++++++++++++
	//+++++++++Posiciones Iniciales++++++++++
	//+++++++++++++++++++++++++++++++++++++++
	private GameObject[] corredores;
	public GameObject lugares;
	private Transform[] listaLugares;
	public bool controlG;
	public bool controlM;
	private bool inicio = false;
	public Vector4[] valores;
	private float[] ranking;
    public Vector3 v;
    public int nave = 0;
	public string[] nombre;
	//+++++++++++++++++++++++++++++++++++++++
	//++++++++++Sprites Posiciones+++++++++++
	//+++++++++++++++++++++++++++++++++++++++
	private SpritePackingRotation render;
	public Image canvasLap;
	public Sprite[] laps;
	public Image canvasPos;
	public Sprite[] pos;
	public Transform target;
	public float distanceUp; //distancia hacia arriba de la camara
	public float distanceBack; //distancia hacia atras de la camara
	public float minimumHeight; 
	public float focal;
	public Camera cam;
    public Image canvasWin;
	public Image canvasLose;
	public float camST = 0.18f;
	public float contadorVignette=0;
	public bool resp;
    public int vueltas;
	public Canvas botonWin;
	//public Text cosas;
	void Awake(){
		//+++++++++++++++++++++++++++++++++++++++
		//++++Determina cual nave es el player+++
		//+++++++++y toma su transformada++++++++
		//+++++++++++++++++++++++++++++++++++++++
		miVehiculo = PlayerPrefs.GetInt ("miNave"); //toma el valor de la nave seleccionada
		player = new Transform[naves.transform.childCount]; //++++++++++++LINEA DE MAS?+++++++++
		for (int l = 0; l < player.Length; l++) { //llena el arreglo
			player [l] = naves.transform.GetChild (l).transform;
		}
			
		//+++++++++++++++++++++++++++++++++++++++
		//++Nuevo arreglo que incluye al player++
		//+++++++++++++++++++++++++++++++++++++++
		corredores = new GameObject[naves.transform.childCount];
		for (int m = 0; m < player.Length; m++) {
			corredores [m] = naves.transform.GetChild (m).gameObject;
		}
		foreach (GameObject noPlayer in corredores)
			noPlayer.SetActive (false);
		if (corredores [miVehiculo])
			corredores [miVehiculo].SetActive (true);
		//+++++++++++++++++++++++++++++++++++++++
		//++++++++Determina las posiciones+++++++
		//+++++++++++++++++++++++++++++++++++++++
		//int i;
		int j;
		int k;
		rivales = new GameObject[IAs.transform.childCount];//++++++++++++LINEA DE MAS?+++++++++
		listaLugares = new Transform[lugares.transform.childCount];//++++++++++++LINEA DE MAS?+++++++++
		for (i = 0; i < rivales.Length; i++) { 
			rivales[i] = IAs.transform.GetChild (i).gameObject;
		}
		rivales [miVehiculo] = corredores [miVehiculo];
		IAs.transform.GetChild (miVehiculo).gameObject.SetActive (false);
		for (j = 0; j < listaLugares.Length; j++) {
			listaLugares [j] = lugares.transform.GetChild (j).transform;
		}

		for (k = 0; k < rivales.Length; k++)
		{
			rivales[k].transform.position = listaLugares[k].transform.position;
		}

	}
    void Start(){
        valores = new Vector4[rivales.Length];
		v = new Vector4 (0, 0, 0, 0);
		nombre = new string[rivales.Length];
		rivales [miVehiculo] = corredores [miVehiculo];
		target = rivales [miVehiculo].transform;

	}

	void Update () {
		if (Input.GetKey (KeyCode.R)) {
			resp = true;
		}
		if (resp) {
			cam.GetComponent<VignetteAndChromaticAberration> ().intensity = 1;
			contadorVignette += Time.deltaTime;
			if (contadorVignette >= .4f) {
				cam.GetComponent<VignetteAndChromaticAberration> ().intensity = 0;
				resp = false;
				contadorVignette = 0;
			}
		}
			
		Vector3 newPosition = target.position + (target.forward * -distanceBack);
		newPosition.y = Mathf.Max (newPosition.y + distanceUp, minimumHeight);
		cam.transform.position = Vector3.SmoothDamp (cam.transform.position, newPosition, ref positionVelocity, camST);
		Vector3 focalPoint = target.position + (target.forward * -focal);
		cam.transform.LookAt (focalPoint);
        
		if (!inicio) {
			StartCoroutine (espera (3));
			if (wait <= 3f) {
				controlG = true;
				controlM = true;
				wait = 4;
			} else if (wait > 3) {
				controlG = false;
				controlM = false;
				for (int a = 0; a < rivales.Length; a++) { 
					rivales [a] = IAs.transform.GetChild (a).gameObject;
					IAscript = IAs.transform.GetChild (a).GetComponent<IA> ();
					IAscript.enabled = true;
				}

			}
		}
		rivales [miVehiculo] = corredores [miVehiculo];

		int n;
		for (n = 0; n < rivales.Length; n++) { 
			rivales [miVehiculo] = corredores [miVehiculo]; //necesario, sino toma la nave IA desactivada
			v = new Vector4 (rivales [n].GetComponent<HoverCheckpoints> ().currentLap, rivales [n].GetComponent<HoverCheckpoints> ().currentCheckPoint, rivales [n].GetComponent<HoverCheckpoints> ().currentTime,(float) n);
			valores [n] = v;
			nombre [n] = rivales [n].name;
		} 

		for (int a = valores.Length - 1; a > 0; a--) {
			for (int b = 0; b <= a - 1; b++) {
				float total1 = valores [b].x * 100 + valores [b].y*0.5f; //suma checks mas lap, donde cada lap vale 100 y cada check 1
				float total2 = valores [b + 1].x * 100 + valores [b + 1].y*0.5f;
				float tiempo1 = valores [b].z; // valor del tiempo
				float tiempo2 = valores [b + 1].z;
				if (total1 > total2) {
					Vector4 highValue = valores [b];
					string texto = nombre [b];
					valores [b] = valores [b + 1];
					nombre [b] = nombre [b + 1];
					valores [b + 1] = highValue;
					nombre [b + 1] = texto;
				}else if (total1 == total2) {
					if (tiempo1 < tiempo2) {
						Vector4 highValue2 = valores [b];
						string texto2 = nombre [b];
						valores [b] = valores [b + 1];
						nombre [b] = nombre [b + 1];
						valores [b + 1] = highValue2;
						nombre [b + 1] = texto2;
					} 
				}
				for (int yo = nombre.Length -1 ; yo >= 0; yo--) {
					if (nombre [yo] == rivales[miVehiculo].name) {
						canvasPos.sprite = pos [yo];
						canvasLap.sprite = laps [(int) valores [yo].x];
						if ((int)valores [yo].x == vueltas) {
							rivales [miVehiculo].GetComponent<hoverController> ().enabled = false;
							IAscript.enabled = false;
							Debug.Log ("wtf");
							if (valores [yo] == valores [5] || valores [yo] == valores [4] || valores [yo] == valores [3]) {
								canvasWin.enabled = true;
								botonWin.enabled = true;
							} else {
								canvasLose.enabled = true;
								botonWin.enabled = true;
							}
                                
						} else {
							canvasWin.enabled = false;
							canvasLose.enabled = false;
							botonWin.enabled = false;
						}
					}
				}
			}
		}








	}

	IEnumerator espera(int seconds){
		inicio = true;
		yield return new WaitForSeconds (seconds);
		wait++;
		inicio = false;
	}
        
}
