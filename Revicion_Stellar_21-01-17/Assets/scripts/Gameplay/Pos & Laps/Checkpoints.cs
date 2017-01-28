using UnityEngine;
using System.Collections;

public class Checkpoints : MonoBehaviour {
    public Transform hoverTransform;
    public HoverCheckpoints hoverscript;
	public Vector3 relativeDist;
    void Awake(){
        
    }
	// Use this for initialization
	void Start () {
        /*player = GameObject.FindGameObjectWithTag("Player");
        if (player.activeInHierarchy)
        {
            hoverTransform = GameObject.FindGameObjectWithTag("Player").transform;
            hoverscript = hoverTransform.GetComponent<HoverCheckpoints>();
        }else
            player = GameObject.FindGameObjectWithTag("Player");
            */
	}
	
    void OnTriggerEnter(Collider other){
        if (other.tag == "Player" || other.tag == "IA")
        {
        
            hoverTransform = other.gameObject.transform;
            hoverscript = hoverTransform.GetComponent<HoverCheckpoints>();
            if (this.transform == hoverscript.checkPointArray[hoverscript.currentCheckPoint].transform)
            {
				if (hoverscript.currentCheckPoint + 1 < hoverscript.checkPointArray.Length) {
					/*if (hoverscript.currentCheckPoint == 0) {
						hoverscript.currentLap++;
					}*/
					hoverscript.currentCheckPoint++;
					hoverscript.currentTime = Time.timeSinceLevelLoad;
				} else {
					hoverscript.currentCheckPoint = 0;
					hoverscript.currentLap++;

				}
            }
        }
        else
            return;
            
    }

    void OnTriggerExit(Collider other){
        hoverTransform = null;
        hoverscript = null;
    }

}
