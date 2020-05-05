// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;

// [CustomEditor(typeof(Shape))]
// [CanEditMultipleObjects]
// public class ShapeEditor : Editor
// {
//     SerializedProperty blocks;

//     void OnEnable()
//     {
//         blocks = serializedObject.FindProperty("blocks");
//     }

//     public override void OnInspectorGUI()
//     {
//         serializedObject.Update();
//         EditorGUILayout.PropertyField(blocks);
//         serializedObject.ApplyModifiedProperties();

//     }
// }
