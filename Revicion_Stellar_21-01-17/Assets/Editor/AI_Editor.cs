using UnityEditor;
using UnityEngine;
using System.Collections;
/*
[CustomEditor (typeof(IA))]
[CanEditMultipleObjects]
public class AI_Editor : Editor {
	SerializedProperty PathProp;
	SerializedProperty distCaminoProp;
	SerializedProperty llantaDIProp;
	SerializedProperty llantaDDProp;
	SerializedProperty llantaTIProp;
	SerializedProperty llantaTDProp;
	SerializedProperty centroMasaProp;


	void OnEnable(){
		PathProp = serializedObject.FindProperty ("path");
		distCaminoProp = serializedObject.FindProperty ("distCamino");
		llantaDIProp = serializedObject.FindProperty ("llantaDI");
		llantaDDProp = serializedObject.FindProperty ("llantaDD");
		llantaTIProp = serializedObject.FindProperty ("llantaTI");
		llantaTDProp = serializedObject.FindProperty ("llantaTD");
	}

	public override void OnInspectorGUI() {
		serializedObject.Update ();
		EditorGUILayout.LabelField ("Trayectoria");
		EditorGUILayout.PropertyField (PathProp, new GUIContent ("Path"));
		EditorGUILayout.Slider (distCaminoProp, 1500, 2000, new GUIContent ("Distancia Camino"));
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField ("Llantas");
		EditorGUILayout.PropertyField (llantaDIProp);
		EditorGUILayout.PropertyField (llantaDDProp);
		EditorGUILayout.PropertyField (llantaTIProp);
		EditorGUILayout.PropertyField (llantaTDProp);
		EditorGUILayout.Vector3Field ("centro de masa", centroMasaProp);
		EditorGUILayout.Space ();
		serializedObject.ApplyModifiedProperties ();
	}

}*/
