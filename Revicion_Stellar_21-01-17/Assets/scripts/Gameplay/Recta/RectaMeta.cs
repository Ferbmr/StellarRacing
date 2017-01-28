using UnityEngine;
using System.Collections;

public class RectaMeta : MonoBehaviour {
    private GameObject manager;
    private RectaGameplay scriptConteo;
    void Start(){
        manager = GameObject.FindGameObjectWithTag("Manager");
        scriptConteo = manager.GetComponent<RectaGameplay>();
    }
    void OnTriggerEnter(Collider player){
        if (player.tag == "Player")
        {
            scriptConteo.meta = true;
        }
    }
}
