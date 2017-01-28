using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {
    public int pista;
    AsyncOperation async;

	void Start(){
		pista = PlayerPrefs.GetInt("Pista"); 
		StartCoroutine(LoadNewScene()); 
	}
        
    IEnumerator LoadNewScene() {
		yield return new WaitForSeconds (5);
        async = SceneManager.LoadSceneAsync(pista); 
        async.allowSceneActivation = false; 
        if (async.progress <= 0.9f) 
        {
            //permite la carga de la escena.
            async.allowSceneActivation = true; 
            yield return async; 
        }
    }
}