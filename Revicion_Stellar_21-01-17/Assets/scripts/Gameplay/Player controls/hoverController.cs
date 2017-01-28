using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class hoverController : MonoBehaviour {
    //+++++++++++++++++++++++++++++++++++++++
    //+++++++++++++++++Player++++++++++++++++
    //+++++++++++++++++++++++++++++++++++++++
    private Rigidbody hover;
	public GameObject manager;
    //+++++++++++++++++++++++++++++++++++++++
    //++++++++++++++++Controles++++++++++++++
    //+++++++++++++++++++++++++++++++++++++++
	public bool giro;
	private float valorGiro;
	public bool move;
	private float valorMove;
	public float aceleracion;
	public float rotationRate;
	public float turnRotationAngle;
	public float turnRotationSeekSpeed;
    public float VelocityZ;
	public float VelocityX;
    private Vector3 ForwardForce; 
	//Efecto de movimiento
	public float Fuerza;
	public float Distancia = 4;
    public float DistanciaEstabilizador = 5f;
    public Transform[] Estabilizadores = new Transform[4];
    private Vector3 downwardForce;
	public float miVelocidad;
	public Image fill;
	private float vel = 40;
	public Transform currentRespawn;
	public AudioSource acelerar_audio;
	public AudioSource choque_audio;

    void Start () {
        hover = this.gameObject.GetComponent <Rigidbody> ();
	}
	void Update(){
		if (this.GetComponent<HoverCheckpoints> ().currentCheckPoint == 0) {
			currentRespawn = this.GetComponent<HoverCheckpoints> ().checkPointArray [this.GetComponent<HoverCheckpoints> ().currentCheckPoint].transform;
		}else
			currentRespawn = this.GetComponent<HoverCheckpoints> ().checkPointArray [this.GetComponent<HoverCheckpoints> ().currentCheckPoint-1].transform;
		fill.fillAmount = 1 / vel * ForwardForce.magnitude;
        if (manager.name == "Manager_Recta")
        {
            giro = manager.GetComponent<RectaGameplay>().controlG;
            move = manager.GetComponent<RectaGameplay>().controlM;
        }else
        {
            giro = manager.GetComponent<Gameplay_Manager> ().controlG;
            move = manager.GetComponent<Gameplay_Manager> ().controlM;
        }

		if (giro)
			return;
		else
			valorGiro = Input.GetAxis ("Horizontal");
		if (move)
			return;
		else
			valorMove = Input.GetAxis ("Vertical");
		miVelocidad = hover.velocity.magnitude * 3600 / 1000;

	}

	void FixedUpdate(){
		if (Input.GetKeyDown (KeyCode.R)){
			transform.position = currentRespawn.transform.position;
			transform.rotation = currentRespawn.transform.rotation;
		}
		if (Physics.Raycast (transform.position, transform.up * -1, Distancia)) { // Si no estoy a mas de 4 unidades del piso 
			ForwardForce = transform.forward * -aceleracion * valorMove;
			ForwardForce = ForwardForce * Time.deltaTime * hover.mass;
			if (ForwardForce != Vector3.zero) {
				hover.drag = 1f;
			} else {
				hover.drag = 2f;
			}
            hover.AddForce (-ForwardForce, ForceMode.Acceleration);
		}
		if (Input.GetAxis ("Horizontal") > 0 || Input.GetKeyDown(KeyCode.W)) {
			acelerar_audio.Play ();
		}
        RaycastHit hit;
        foreach (Transform thruster in Estabilizadores)
        {
            float distancePercentage;
            if (Physics.Raycast(thruster.position, thruster.up * -1, out hit, DistanciaEstabilizador))
            {
				Debug.DrawLine(thruster.position, hit.point, Color.cyan);

				distancePercentage = 1 - (hit.distance /DistanciaEstabilizador);
				downwardForce = thruster.up * Fuerza * distancePercentage;
				downwardForce = downwardForce * Time.deltaTime;
				hover.AddForceAtPosition(downwardForce, thruster.position, ForceMode.VelocityChange);
            }
            else
            {
                downwardForce = Vector3.zero;

            }
        }


        Vector3 newRotation = this.transform.eulerAngles;
		if (valorGiro != 0) {
			newRotation.z = Mathf.SmoothDampAngle (newRotation.z, valorGiro * -turnRotationAngle, ref VelocityZ, turnRotationSeekSpeed);
		}else newRotation.z = Mathf.SmoothDampAngle (newRotation.z, 0, ref VelocityZ, turnRotationSeekSpeed);


        transform.eulerAngles = newRotation;
		Vector3 turnTorque = Vector3.up * rotationRate;

		turnTorque = turnTorque * Time.deltaTime;
		if (valorGiro != 0) {
			hover.angularDrag = 7f;
                hover.AddTorque (0, turnTorque.magnitude * valorGiro, 0, ForceMode.VelocityChange);

		} else {
			hover.angularDrag = 7f;
		}
        
    }

	void OnCollisionEnter(Collision other){
		choque_audio.Play ();
	}
		
}
