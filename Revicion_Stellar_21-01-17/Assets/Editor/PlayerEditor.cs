
/*
using UnityEditor;
using UnityEngine;
using System.Collections;
[CustomEditor (typeof(hoverController))]
[CanEditMultipleObjects]
public class PlayerEditor : Editor {
	SerializedProperty mainManagerProp;
	SerializedProperty aceleracionProp;
	SerializedProperty rotationRateProp;
	SerializedProperty rotationAngleProp;
	SerializedProperty distEstProp;
	SerializedProperty estabProp;

	void OnEnable(){
		aceleracionProp = serializedObject.FindProperty ("aceleracion");
		mainManagerProp = serializedObject.FindProperty ("manager");
		rotationRateProp = serializedObject.FindProperty ("rotationRate");
		rotationAngleProp = serializedObject.FindProperty ("turnRotationAngle");
		distEstProp = serializedObject.FindProperty ("DistanciaEstabilizador");
		estabProp = serializedObject.FindProperty ("Estabilizadores");
	}

	public override void OnInspectorGUI() {
		serializedObject.Update ();
		EditorGUILayout.PropertyField (mainManagerProp, new GUIContent ("Manager"));
		EditorGUILayout.Slider (aceleracionProp, 10, 20, new GUIContent ("Aceleracion"));
		EditorGUILayout.Slider (rotationRateProp, 5, 15, new GUIContent ("Rango Giro"));
		EditorGUILayout.Slider (rotationAngleProp, 10, 40, new GUIContent ("AnguloGiro"));
		EditorGUILayout.Slider (distEstProp, 0.5f, 1, new GUIContent ("Altura"));
		EditorGUILayout.Space ();
		EditorGUILayout.PropertyField (estabProp, true);
		serializedObject.ApplyModifiedProperties ();
	}
		
}*/