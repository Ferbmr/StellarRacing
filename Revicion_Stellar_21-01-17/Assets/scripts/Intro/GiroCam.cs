using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GiroCam : MonoBehaviour {
	
	void Update () {
        transform.Rotate(Vector3.up, Time.deltaTime*5.0f);
		if (Input.anyKeyDown)
        {
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
				return;
			
            SceneManager.LoadScene("carSelection");
        }
	
	}
	public void QuitGame () {
		Application.Quit();
	}
}
