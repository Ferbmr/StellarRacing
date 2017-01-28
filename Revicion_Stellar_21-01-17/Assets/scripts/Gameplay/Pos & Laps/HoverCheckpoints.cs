using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HoverCheckpoints : MonoBehaviour {
    public Transform puntos;
    public Transform[] checkPointArray;
    public int currentCheckPoint = 0;
    public int currentLap = 0;
	public float currentTime = 0;
    public Vector3 startPos;
	public float currentDist = 0;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
        checkPointArray = new Transform[puntos.transform.childCount];
        for (int i = 0; i < checkPointArray.Length; i++)
        {
            checkPointArray[i] = puntos.transform.GetChild(i).transform;
        }
	}
}
