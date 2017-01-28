using UnityEngine;
using System.Collections;

public class Rectacontador : MonoBehaviour {
    private GameObject manager;
    private RectaGameplay scriptConteo;
    void Start(){
        manager = GameObject.FindGameObjectWithTag("Manager");
        scriptConteo = manager.GetComponent<RectaGameplay>();
    }

    void OnTriggerEnter(Collider player){
        if (player.tag == "Player")
        {
            scriptConteo.conteo = scriptConteo.conteo + 1;
            scriptConteo.masTiempo = true;
            Destroy(this.gameObject);
        }
    }
}
