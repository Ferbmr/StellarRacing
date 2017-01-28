using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IA : MonoBehaviour {
    //+++++++++Trayetoria++++++++
    public Transform path;
    private List<Transform> nodos;
    private int nodoActual = 0;
    public float distCamino = 10000;
    //+++++++++movimiento++++++++
    private float anguloDir= 45f;
    public WheelCollider llantaDI;
    public WheelCollider llantaDD;
    public WheelCollider llantaTI;
    public WheelCollider llantaTD;
    public Vector3 centroMasa;
    private Rigidbody rival;
    public float maxTorque;
    public float velocidadActual;
    public float velocidadTope = 150;
    public float desaceleracion = 20;
    //++++++++++++Freno++++++++++
    public bool frenando;
	public bool enZona;
    //++++++++++Sensores+++++++++
    public float distSensor = 3;
    public float inicioFrontal = 0f;
    public float distFrontales = 0.5f;
    public float anguloFrontal = 30;
    public float distLateral = 3;
    private int flag = 0;
    public float velocidadEvasion = 20;
    //++++++++++Reversa+++++++++
	public bool reversing = false;
	public float revercounter = 0;
	public float waitToReverse = 2.0f;
    public float reverFor = 1.5f;
    public float respawnTime = 5;
    public float respawnContador = 0;
    public float resGiro;

    void Start () {
        //crea una lista de los hijos del objeto "Path" para determinar la trayectoria
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>(); 
        nodos = new List<Transform>(); //lista vacia de nodos
        for (int i = 0; i < pathTransforms.Length; i++) { //llena la lista nodos a seguir
            if (pathTransforms [i] != path.transform) {
                nodos.Add (pathTransforms [i]);
            }
        }
        rival = GetComponent<Rigidbody> ();//obtiene el rigidbody del vehiculo
        rival.centerOfMass = centroMasa; //determina el nuevo centro de masa del vehiculo
       
    }

    void FixedUpdate () {
        if (flag == 0)
            ApplySteer(); 
        Move();
        if (gameObject.name == "IA_Callo")
        {
            cashoSensores();
        }else if (gameObject.name == "IA_Ferb")
        {
            ferbSensores();
        } else if (gameObject.name == "IA_Panhios")
        {
            panhiosSensores();
        }else if (gameObject.name == "IA_Tito")
        {
            fitoSensores();
        } else if (gameObject.name == "IA_Chamir")
        {
            chamirSensores();
        } else if (gameObject.name == "IA_Andres")
        {
            pandresSensores();
        }
        Respawn();
    }

    void ApplySteer(){ //direccion
        Vector3 vectorRelativo = transform.InverseTransformPoint(nodos[nodoActual].position.x, transform.position.y, nodos[nodoActual].position.z);
        float nuevaDir = (vectorRelativo.x / vectorRelativo.magnitude) * anguloDir;  // angulo de la llanta
        llantaDI.steerAngle = nuevaDir;//gira las llantas
        llantaDD.steerAngle = nuevaDir;
        if (vectorRelativo.magnitude <= distCamino) {
            nodoActual++;
            if (nodoActual >= nodos.Count) {
                nodoActual = 0;
            }
        }
    }

    void Move(){ //movimiento
		velocidadActual = rival.velocity.magnitude*3600/1000; //2*Mathf.PI * llantaTI.radius * llantaTI.rpm * 60 /1000 ; //El factor se da debido a que la velocidad se dara en kilometros por hora (es un factor de conversion)
        velocidadActual = Mathf.Round (velocidadActual);
		if (velocidadActual <= velocidadTope) {
			if (!reversing) {
				if (!enZona) {
					llantaTI.brakeTorque = 0;
					llantaTD.brakeTorque = 0;
					llantaTI.motorTorque = maxTorque;
					llantaTD.motorTorque = maxTorque;
				} else {
					llantaTI.motorTorque = 0;
					llantaTD.motorTorque = 0;
					llantaTI.brakeTorque = desaceleracion;
					llantaTD.brakeTorque = desaceleracion;
				}
			} else {
				/*if (rival.velocity.magnitude < 2) {
					llantaTI.brakeTorque = maxTorque;
					llantaTD.brakeTorque = maxTorque;
				}*/
				llantaTI.motorTorque = -maxTorque;
				llantaTD.motorTorque = -maxTorque;
			}
	
		} else {
			llantaTI.motorTorque = 0;
			llantaTD.motorTorque = 0;
			llantaTI.brakeTorque = desaceleracion;
			llantaTD.brakeTorque = desaceleracion;
		}

    }

    void Respawn(){
        if (rival.velocity.magnitude < 1.5f){
            float lado = Random.Range(-4f, 5f);
            respawnContador += Time.deltaTime;
            if (respawnContador >= respawnTime){
                if (nodoActual == 0) {
                    transform.position = new Vector3(nodos[0].position.x, nodos[0].position.y, nodos[0].position.z + lado);
                    transform.rotation = nodos[0].transform.rotation;
                } else {
                    transform.position = new Vector3(nodos[nodoActual-1].position.x, nodos[nodoActual-1].position.y, nodos[nodoActual-1].position.z + lado);
                    transform.rotation = nodos[nodoActual - 1].transform.rotation;
                }
                respawnContador = 0;
                //resGiro = transform.localEulerAngles.z;
                resGiro = 0;
            }
        }
    }
    void OnTriggerEnter(Collider col){
        if (col.tag == "Caida")
        {
            Respawn();
        }
    }

    void evitarDireccion(float sensible){
        llantaDI.steerAngle = velocidadEvasion * sensible;
        llantaDD.steerAngle = velocidadEvasion * sensible;
    }

    void pandresSensores(){
        flag = 0;
        float sensibilidad = 0;
        RaycastHit hit;
        Vector3 pos = transform.position;
        Vector3 anguloDerecha = Quaternion.AngleAxis (anguloFrontal, transform.up) * transform.forward;
        Vector3 anguloIzquierda = Quaternion.AngleAxis (-anguloFrontal, transform.up) * transform.forward;
        pos += transform.forward;
        //Sensor de frenos
        if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                llantaTI.brakeTorque = desaceleracion;
                llantaTD.brakeTorque = desaceleracion;
                Debug.DrawLine (pos, hit.point, Color.magenta);
            }
        } else {
            llantaTI.brakeTorque = 0;
            llantaTD.brakeTorque = 0;
        }
        //Sensor frontal derecho
        pos += transform.right * distFrontales/15;
        if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad -= 1;
                Debug.DrawLine (pos, hit.point, Color.magenta);
            }
        }else if (Physics.Raycast (pos, anguloDerecha, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad -= 0.5f;
                Debug.DrawLine (pos, hit.point, Color.magenta);
            }
        }

        //Sensor frontal izquierdo
        pos = transform.position;
        pos += transform.forward;
        pos -= transform.right * distFrontales/15;
        if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad += 1;
                Debug.DrawLine (pos, hit.point, Color.magenta);
            }
        }else if (Physics.Raycast (pos, anguloIzquierda, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad += 0.5f;
                Debug.DrawLine (pos, hit.point, Color.magenta);
            }
        }

        //Sensor Lateral Derecho
        if (Physics.Raycast (transform.position, transform.right, out hit, distLateral)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad -= 0.5f;
                Debug.DrawLine (transform.position, hit.point, Color.magenta);
            }
        }

        //Sensor Lateral Izquierdo
        if (Physics.Raycast (transform.position, -transform.right, out hit, distLateral)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad += 0.5f;
                Debug.DrawLine (transform.position, hit.point, Color.magenta);
            }
        }
        pos = transform.position;
        pos += transform.forward * inicioFrontal;

        //Sensor frontal Central 
        if (sensibilidad == 0) {
            if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
                if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                    if (hit.normal.x < 0) {
                        sensibilidad = -1;
                    } else {
                        sensibilidad = 1;
                    }
                    Debug.DrawLine (pos, hit.point, Color.magenta);
                }
            }
        }

		if (rival.velocity.magnitude < 2 && !reversing){
			revercounter += Time.deltaTime;
			if (revercounter >= waitToReverse){
				revercounter = 0;
				reversing = true;
			}
		}else if (rival.velocity.magnitude >= 2 &&!reversing){ 
			revercounter = 0;
		}

		if (reversing){
			sensibilidad *= -1;
			revercounter += Time.deltaTime;
			if (revercounter >= reverFor){
				revercounter = 0;
				reversing = false;
			}
		}

        if (flag != 0) {
            evitarDireccion (sensibilidad);
        }
    }
    void chamirSensores(){
        flag = 0;
        float sensibilidad = 0;
        RaycastHit hit;
        Vector3 pos = transform.position;
        Vector3 anguloDerecha = Quaternion.AngleAxis (anguloFrontal, transform.up) * transform.forward;
        Vector3 anguloIzquierda = Quaternion.AngleAxis (-anguloFrontal, transform.up) * transform.forward;
        pos += transform.forward;
        //Sensor de frenos
        if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                llantaTI.brakeTorque = desaceleracion;
                llantaTD.brakeTorque = desaceleracion;
                Debug.DrawLine (pos, hit.point, Color.white);
            }
        } else {
            llantaTI.brakeTorque = 0;
            llantaTD.brakeTorque = 0;
        }
        //Sensor frontal derecho
        pos += transform.right * distFrontales/40;
        if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad -= 1;
                Debug.DrawLine (pos, hit.point, Color.white);
            }
        }else if (Physics.Raycast (pos, anguloDerecha, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad -= 0.5f;
                Debug.DrawLine (pos, hit.point, Color.white);
            }
        }

        //Sensor frontal izquierdo
        pos = transform.position;
        pos += transform.forward;
        pos -= transform.right * distFrontales/40;
        if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad += 1;
                Debug.DrawLine (pos, hit.point, Color.white);
            }
        }else if (Physics.Raycast (pos, anguloIzquierda, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad += 0.5f;
                Debug.DrawLine (pos, hit.point, Color.white);
            }
        }

        //Sensor Lateral Derecho
        if (Physics.Raycast (transform.position, transform.right, out hit, distLateral)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad -= 0.5f;
                Debug.DrawLine (transform.position, hit.point, Color.white);
            }
        }

        //Sensor Lateral Izquierdo
        if (Physics.Raycast (transform.position, -transform.right, out hit, distLateral)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad += 0.5f;
                Debug.DrawLine (transform.position, hit.point, Color.white);
            }
        }
        pos = transform.position;
        pos += transform.forward * inicioFrontal;

        //Sensor frontal Central 
        if (sensibilidad == 0) {
            if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
                if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                    if (hit.normal.x < 0) {
                        sensibilidad = -1;
                    } else {
                        sensibilidad = 1;
                    }
                    Debug.DrawLine (pos, hit.point, Color.white);
                }
            }
        }
		if (rival.velocity.magnitude < 2 && !reversing){
			revercounter += Time.deltaTime;
			if (revercounter >= waitToReverse){
				revercounter = 0;
				reversing = true;
			}
		}else if (rival.velocity.magnitude >= 2 &&!reversing){ 
			revercounter = 0;
		}

		if (reversing){
			sensibilidad *= -1;
			revercounter += Time.deltaTime;
			if (revercounter >= reverFor){
				revercounter = 0;
				reversing = false;
			}
		}

        if (flag != 0) {
            evitarDireccion (sensibilidad);
        }
    }
    void ferbSensores(){
        Color naranja = new Color(1f, .27f, 0f);
        flag = 0;
        float sensibilidad = 0;
        RaycastHit hit;
        Vector3 pos = transform.position;
        Vector3 anguloDerecha = Quaternion.AngleAxis (anguloFrontal, transform.up) * transform.forward;
        Vector3 anguloIzquierda = Quaternion.AngleAxis (-anguloFrontal, transform.up) * transform.forward;
        pos += transform.forward;
        //Sensor de frenos
        if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                llantaTI.brakeTorque = desaceleracion;
                llantaTD.brakeTorque = desaceleracion;
                Debug.DrawLine (pos, hit.point, naranja);
            }
        } else {
            llantaTI.brakeTorque = 0;
            llantaTD.brakeTorque = 0;
        }
        //Sensor frontal derecho
        pos += transform.right * distFrontales/50;
        if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad -= 1;
                Debug.DrawLine (pos, hit.point, naranja);
            }
        }else if (Physics.Raycast (pos, anguloDerecha, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad -= 0.5f;
                Debug.DrawLine (pos, hit.point, naranja);
            }
        }

        //Sensor frontal izquierdo
        pos = transform.position;
        pos += transform.forward;
        pos -= transform.right * distFrontales/50;
        if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad += 1;
                Debug.DrawLine (pos, hit.point, naranja);
            }
        }else if (Physics.Raycast (pos, anguloIzquierda, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad += 0.5f;
                Debug.DrawLine (pos, hit.point, naranja);
            }
        }

        //Sensor Lateral Derecho
        if (Physics.Raycast (transform.position, transform.right, out hit, distLateral)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad -= 0.5f;
                Debug.DrawLine (transform.position, hit.point, naranja);
            }
        }

        //Sensor Lateral Izquierdo
        if (Physics.Raycast (transform.position, -transform.right, out hit, distLateral)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad += 0.5f;
                Debug.DrawLine (transform.position, hit.point, naranja);
            }
        }
        pos = transform.position;
        pos += transform.forward * inicioFrontal;

        //Sensor frontal Central 
        if (sensibilidad == 0) {
            if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
                if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                    if (hit.normal.x < 0) {
                        sensibilidad = -1;
                    } else {
                        sensibilidad = 1;
                    }
                    Debug.DrawLine (pos, hit.point, naranja);
                }
            }
        }

		if (rival.velocity.magnitude < 2 && !reversing){
			revercounter += Time.deltaTime;
			if (revercounter >= waitToReverse){
				revercounter = 0;
				reversing = true;
			}
		}else if (rival.velocity.magnitude >= 2 &&!reversing){ 
			revercounter = 0;
		}

		if (reversing){
			sensibilidad *= -1;
			revercounter += Time.deltaTime;
			if (revercounter >= reverFor){
				revercounter = 0;
				reversing = false;
			}
		}

        if (flag != 0) {
            evitarDireccion (sensibilidad);
        }
    }
    void panhiosSensores(){
        flag = 0;
        float sensibilidad = 0;
        RaycastHit hit;
        Vector3 pos = transform.position;
        Vector3 anguloDerecha = Quaternion.AngleAxis (anguloFrontal, transform.up) * transform.forward;
        Vector3 anguloIzquierda = Quaternion.AngleAxis (-anguloFrontal, transform.up) * transform.forward;
        pos += transform.forward;
        //Sensor de frenos
        if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                llantaTI.brakeTorque = desaceleracion;
                llantaTD.brakeTorque = desaceleracion;
                Debug.DrawLine (pos, hit.point, Color.red);
            }
        } else {
            llantaTI.brakeTorque = 0;
            llantaTD.brakeTorque = 0;
        }
        //Sensor frontal derecho
        pos += transform.right * distFrontales/30;
        if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad -= 1;
                Debug.DrawLine (pos, hit.point, Color.red);
            }
        }else if (Physics.Raycast (pos, anguloDerecha, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad -= 0.5f;
                Debug.DrawLine (pos, hit.point, Color.red);
            }
        }

        //Sensor frontal izquierdo
        pos = transform.position;
        pos += transform.forward;
        pos -= transform.right * distFrontales/30;
        if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad += 1;
                Debug.DrawLine (pos, hit.point, Color.red);
            }
        }else if (Physics.Raycast (pos, anguloIzquierda, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad += 0.5f;
                Debug.DrawLine (pos, hit.point, Color.red);
            }
        }

        //Sensor Lateral Derecho
        if (Physics.Raycast (transform.position, transform.right, out hit, distLateral)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad -= 0.5f;
                Debug.DrawLine (transform.position, hit.point, Color.red);
            }
        }

        //Sensor Lateral Izquierdo
        if (Physics.Raycast (transform.position, -transform.right, out hit, distLateral)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad += 0.5f;
                Debug.DrawLine (transform.position, hit.point, Color.red);
            }
        }
        pos = transform.position;
        pos += transform.forward * inicioFrontal;

        //Sensor frontal Central 
        if (sensibilidad == 0) {
            if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
                if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                    if (hit.normal.x < 0) {
                        sensibilidad = -1;
                    } else {
                        sensibilidad = 1;
                    }
                    Debug.DrawLine (pos, hit.point, Color.red);
                }
            }
        }

		if (rival.velocity.magnitude < 2 && !reversing){
			revercounter += Time.deltaTime;
			if (revercounter >= waitToReverse){
				revercounter = 0;
				reversing = true;
			}
		}else if (rival.velocity.magnitude >= 2 &&!reversing){ 
			revercounter = 0;
		}

		if (reversing){
			sensibilidad *= -1;
			revercounter += Time.deltaTime;
			if (revercounter >= reverFor){
				revercounter = 0;
				reversing = false;
			}
		}


        if (flag != 0) {
            evitarDireccion (sensibilidad);
        }
    }
    void fitoSensores(){
        flag = 0;
        float sensibilidad = 0;
        RaycastHit hit;
        Vector3 pos = transform.position;
        Vector3 anguloDerecha = Quaternion.AngleAxis (anguloFrontal, transform.up) * transform.forward;
        Vector3 anguloIzquierda = Quaternion.AngleAxis (-anguloFrontal, transform.up) * transform.forward;
        pos += transform.forward;
        //Sensor de frenos
        if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                llantaTI.brakeTorque = desaceleracion;
                llantaTD.brakeTorque = desaceleracion;
                Debug.DrawLine (pos, hit.point, Color.green);
            }
        } else {
            llantaTI.brakeTorque = 0;
            llantaTD.brakeTorque = 0;
        }
        //Sensor frontal derecho
        pos += transform.right * distFrontales/50;
        if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad -= 1;
                Debug.DrawLine (pos, hit.point, Color.green);
            }
        }else if (Physics.Raycast (pos, anguloDerecha, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad -= 0.5f;
                Debug.DrawLine (pos, hit.point, Color.green);
            }
        }

        //Sensor frontal izquierdo
        pos = transform.position;
        pos += transform.forward;
        pos -= transform.right * distFrontales/50;
        if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad += 1;
                Debug.DrawLine (pos, hit.point, Color.green);
            }
        }else if (Physics.Raycast (pos, anguloIzquierda, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad += 0.5f;
                Debug.DrawLine (pos, hit.point, Color.green);
            }
        }

        //Sensor Lateral Derecho
        if (Physics.Raycast (transform.position, transform.right, out hit, distLateral)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad -= 0.5f;
                Debug.DrawLine (transform.position, hit.point, Color.green);
            }
        }

        //Sensor Lateral Izquierdo
        if (Physics.Raycast (transform.position, -transform.right, out hit, distLateral)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad += 0.5f;
                Debug.DrawLine (transform.position, hit.point, Color.green);
            }
        }
        pos = transform.position;
        pos += transform.forward * inicioFrontal;

        //Sensor frontal Central 
        if (sensibilidad == 0) {
            if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
                if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                    if (hit.normal.x < 0) {
                        sensibilidad = -1;
                    } else {
                        sensibilidad = 1;
                    }
                    Debug.DrawLine (pos, hit.point, Color.green);
                }
            }
        }

		if (rival.velocity.magnitude < 2 && !reversing){
			revercounter += Time.deltaTime;
			if (revercounter >= waitToReverse){
				revercounter = 0;
				reversing = true;
			}
		}else if (rival.velocity.magnitude >= 2 &&!reversing){ 
			revercounter = 0;
		}

		if (reversing){
			sensibilidad *= -1;
			revercounter += Time.deltaTime;
			if (revercounter >= reverFor){
				revercounter = 0;
				reversing = false;
			}
		}

        if (flag != 0) {
            evitarDireccion (sensibilidad);
        }
    }
    void cashoSensores(){
        flag = 0;
        float sensibilidad = 0;
        RaycastHit hit;
        Vector3 pos = transform.position;
        Vector3 anguloDerecha = Quaternion.AngleAxis (anguloFrontal, transform.up) * transform.forward;
        Vector3 anguloIzquierda = Quaternion.AngleAxis (-anguloFrontal, transform.up) * transform.forward;
        pos += transform.forward;
        //Sensor de frenos
        if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                llantaTI.brakeTorque = desaceleracion;
                llantaTD.brakeTorque = desaceleracion;
                Debug.DrawLine (pos, hit.point, Color.yellow);
            }
        } else {
            llantaTI.brakeTorque = 0;
            llantaTD.brakeTorque = 0;
        }
        //Sensor frontal derecho
        pos += transform.right * distFrontales/50;
        if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad -= 1;
                Debug.DrawLine (pos, hit.point, Color.yellow);
            }
        }else if (Physics.Raycast (pos, anguloDerecha, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad -= 0.5f;
                Debug.DrawLine (pos, hit.point, Color.yellow);
            }
        }

        //Sensor frontal izquierdo
        pos = transform.position;
        pos += transform.forward;
        pos -= transform.right * distFrontales/50;
        if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad += 1;
                Debug.DrawLine (pos, hit.point, Color.yellow);
            }
        }else if (Physics.Raycast (pos, anguloIzquierda, out hit, distSensor)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad += 0.5f;
                Debug.DrawLine (pos, hit.point, Color.yellow);
            }
        }

        //Sensor Lateral Derecho
        if (Physics.Raycast (transform.position, transform.right, out hit, distLateral)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad -= 0.5f;
                Debug.DrawLine (transform.position, hit.point, Color.yellow);
            }
        }

        //Sensor Lateral Izquierdo
        if (Physics.Raycast (transform.position, -transform.right, out hit, distLateral)) {
            if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                flag++;
                sensibilidad += 0.5f;
                Debug.DrawLine (transform.position, hit.point, Color.yellow);
            }
        }
        pos = transform.position;
        pos += transform.forward * inicioFrontal;

        //Sensor frontal Central 
        if (sensibilidad == 0) {
            if (Physics.Raycast (pos, transform.forward, out hit, distSensor)) {
                if (hit.transform.tag != "Pista" && hit.transform.tag != "breakZone") {
                    if (hit.normal.x < 0) {
                        sensibilidad = -1;
                    } else {
                        sensibilidad = 1;
                    }
                    Debug.DrawLine (pos, hit.point, Color.yellow);
                }
            }
        }

		if (rival.velocity.magnitude < 2 && !reversing){
			revercounter += Time.deltaTime;
			if (revercounter >= waitToReverse){
				revercounter = 0;
				reversing = true;
			}
		}else if (rival.velocity.magnitude >= 2 &&!reversing){ 
			revercounter = 0;
		}

		if (reversing){
			sensibilidad *= -1;
			revercounter += Time.deltaTime;
			if (revercounter >= reverFor){
				revercounter = 0;
				reversing = false;
			}
		}

        if (flag != 0) {
            evitarDireccion (sensibilidad);
        }
    }
}
