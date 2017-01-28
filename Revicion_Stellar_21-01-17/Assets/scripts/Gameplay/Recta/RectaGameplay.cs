 using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class RectaGameplay : MonoBehaviour {
    public GameObject naves;
    private GameObject[] player;
    private int miVehiculo; 
    private int wait = 0;
    private Vector3 positionVelocity; 
    public bool controlG;
    public bool controlM;
    private bool inicio = false;
    public Transform target;
    public float distanceUp; //distancia hacia arriba de la camara
    public float distanceBack; //distancia hacia atras de la camara
    public float minimumHeight; 
    public float focal;
    public Camera cam;
    public float camST = 0.18f;
    public float contadorVignette=0;
    public bool resp;
    public GameObject objetos;
    public GameObject[] cosas;
    public int conteo;
    public Text contadorObj;
    public Text masTiempoText;
    public float cuentaRegresiva;
    public bool masTiempo;
    public float aguarda = 2;
    public bool meta;
	public Image canvasWin;
	public Image canvasLose;
	public Canvas botonWin;

    void Awake(){
        miVehiculo = PlayerPrefs.GetInt ("miNave"); //toma el valor de la nave seleccionada
        player = new GameObject[naves.transform.childCount]; //++++++++++++LINEA DE MAS?+++++++++
        for (int l = 0; l < player.Length; l++) { //llena el arreglo
            player [l] = naves.transform.GetChild (l).gameObject;
        }

        foreach (GameObject noPlayer in player)
            noPlayer.SetActive (false);
        if (player [miVehiculo])
            player[miVehiculo].SetActive (true);
    }


    void Start(){
		botonWin.enabled = false;
        target = player [miVehiculo].transform;
        cosas = new GameObject[objetos.transform.childCount];
        for (int i = 0; i < cosas.Length; i++)
        {
            cosas[i] = objetos.transform.GetChild(i).gameObject;
        }

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
            }
        }
        cuentaRegresiva -= Time.deltaTime;
        contadorObj.text = ("Esferas: " + conteo + "/" + cosas.Length + "\n" + "Tiempo: " + cuentaRegresiva.ToString("0.0"));
        if (masTiempo)
        {
            masTiempoText.text = ("+5!");
            cuentaRegresiva = cuentaRegresiva + 5;
            masTiempo = false;
            aguarda = 1.5f;
        }
        else
        {
            aguarda -= Time.deltaTime;
            if (aguarda <= 0)
            {
                masTiempoText.text = ("");
            }
        }

		if (meta && cuentaRegresiva > 0) {
			cuentaRegresiva = cuentaRegresiva;
			player [miVehiculo].GetComponent<hoverController> ().enabled = false;
			canvasWin.enabled = true;
			botonWin.enabled = true;
		} else if (cuentaRegresiva <= 0) {
			cuentaRegresiva = 0;
			player [miVehiculo].GetComponent<hoverController> ().enabled = false;
			canvasLose.enabled = true;
			botonWin.enabled = true;
		}
    }

    IEnumerator espera(int seconds){
        inicio = true;
        yield return new WaitForSeconds (seconds);
        wait++;
        inicio = false;
    }
}
