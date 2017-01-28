using UnityEngine;
using System.Collections;

public class Estabilizar : MonoBehaviour {

    private float Fuerza = 100;
    public float Distancia;
    public Transform[] Estabilizadores;
    public Rigidbody rigid;
	public Vector3 downwardForce;

    void Start(){
        rigid = GetComponent<Rigidbody> ();
    }
        
    void FixedUpdate () {
        RaycastHit hit;
        foreach (Transform thruster in Estabilizadores) {
            
            float distancePercentage;
			if (Physics.Raycast (thruster.position, thruster.up * -1, out hit, Distancia)) {
				Debug.DrawRay (thruster.localPosition, -thruster.up, Color.yellow);
				distancePercentage = 1 - (hit.distance / Distancia);
				downwardForce = thruster.up * Fuerza * distancePercentage;
				downwardForce = downwardForce * Time.deltaTime;
				//rigid.AddForceAtPosition(downwardForce, thruster.position);
				rigid.AddForce (0, downwardForce.magnitude, 0, ForceMode.VelocityChange);
			} else {
				downwardForce = Vector3.zero;

			}
        }

    }
}
