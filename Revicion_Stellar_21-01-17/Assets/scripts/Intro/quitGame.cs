using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class quitGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void QuitGame () {
		Application.Quit();
	}
}
