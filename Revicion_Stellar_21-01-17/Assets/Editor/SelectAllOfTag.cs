using UnityEngine;
using System.Collections;
using UnityEditor;

public class SelectAllOfTag : ScriptableWizard {
	public string searchTag = "Escribe una Tag";

	[MenuItem ("Herramientas Ferb / Seleccionar Tag ...")]
	static void SelectAllOfTagWizard(){
		ScriptableWizard.DisplayWizard<SelectAllOfTag> ("Seleccionar objetos con Tag", "Seleccionar alv.");
	}
	void OnWizardCreate(){
		GameObject[] gameobjects = GameObject.FindGameObjectsWithTag (searchTag);
		Selection.objects = gameobjects;
	}
}
