using UnityEngine;
using System.Collections;

public class PowerDown : MonoBehaviour {
    private GameObject manager;
    private GameObject player;
    private hoverController playerController;
    private GameObject NPC;
    private IA iaController;
    public bool arre;
    public float conteoP;
    public float conteoE;
    private float aceleracionCache;
    public bool enemy;
    private float torqueCache;
    private GameObject[] rivales;

    void OnTriggerEnter(Collider other){
        if (other.tag == "Player")
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerController = player.GetComponent<hoverController>();
            aceleracionCache = playerController.aceleracion;
            conteoP = 1;
            arre = true;
        }
        else if (other.tag == "IA")
        {
            StartCoroutine(Power(other.gameObject));
        }
    }

    IEnumerator Power (GameObject rival){
        IA iaScript = rival.gameObject.GetComponent<IA>();
        float torqueCache = iaScript.maxTorque;
        iaScript.maxTorque = 1000;
        yield return new WaitForSeconds(1.5f);
        iaScript.maxTorque = torqueCache;
    }

    void Update(){
        if (arre)
        {
            conteoP -= Time.deltaTime;
            if (conteoP >= 0)
                playerController.aceleracion = 5;
            else
            {
                playerController.aceleracion = aceleracionCache;
                arre = false;
            }
        }

    }
}
