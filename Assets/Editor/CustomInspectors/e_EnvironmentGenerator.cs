﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(EnvironmentGenerator))]
public class e_EnvironmentGenerator : Editor
{
	private ReorderableList atlases;

	private void OnEnable()
	{
		SerializedProperty atlasesSerialized = serializedObject.FindProperty("layers");
		atlases = new ReorderableList(serializedObject, atlasesSerialized, true, true, true, true)
		{
			drawHeaderCallback = rect =>
			{
				EditorGUI.LabelField(rect, atlases.serializedProperty.displayName, EditorStyles.boldLabel);
			},
			onAddCallback = list =>
			{
				int index = list.index + 1;
				list.serializedProperty.InsertArrayElementAtIndex(index);
				list.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("count").intValue = 90;
			},
			onRemoveCallback = list =>
			{
				int index = list.index;
				list.serializedProperty.DeleteArrayElementAtIndex(index);
			},
			drawElementCallback = (rect, index, active, focused) =>
			{
				string label = null;
				
				if (index == 0)
					label = "Back";
				else if (index == atlases.serializedProperty.arraySize - 1)
					label = "Front";

				SerializedProperty layerProperty = atlases.serializedProperty.GetArrayElementAtIndex(index);
				SerializedProperty atlasProperty = layerProperty.FindPropertyRelative("atlas");
				SerializedProperty countProperty = layerProperty.FindPropertyRelative("count");
				SerializedProperty posOffProperty = layerProperty.FindPropertyRelative("positionOffset");
				SerializedProperty rangeOffProperty = layerProperty.FindPropertyRelative("rangeOffset");

				const float labelWidth = 40;
				const float countWidth = 40;
				const float countLabelWidth = 60;
				var labelRect = new Rect(rect.x, rect.y, labelWidth, EditorGUIUtility.singleLineHeight);
				var atlasRect = new Rect(rect.x + labelWidth, rect.y, rect.width - countWidth - labelWidth - countLabelWidth - 4, labelRect.height);
				var countRect = new Rect(rect.xMax - countWidth, rect.y, countWidth, labelRect.height);
				var countLabelRect = new Rect(countRect.x - countLabelWidth, rect.y, countLabelWidth, labelRect.height);
				var posOffRect = new Rect(rect.x + labelWidth, labelRect.yMax, rect.width - labelWidth, labelRect.height);
				var rangeOffRect = new Rect(rect.x + labelWidth, posOffRect.yMax, rect.width - labelWidth, labelRect.height);

				EditorGUI.LabelField(labelRect, label);
				EditorGUI.PropertyField(atlasRect, atlasProperty, GUIContent.none);
				EditorGUI.LabelField(countLabelRect, countProperty.displayName);
				EditorGUI.PropertyField(countRect, countProperty, GUIContent.none);
				EditorGUI.PropertyField(posOffRect, posOffProperty);
				EditorGUI.PropertyField(rangeOffRect, rangeOffProperty);
			},

			elementHeight = EditorGUIUtility.singleLineHeight * 3.4f,
		};

	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		serializedObject.Update();

		EditorGUILayout.Space();
		atlases.DoLayoutList();
		EditorGUILayout.Space();

		serializedObject.ApplyModifiedProperties();

		Rect buttonsRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, GUIStyle.none);
		var leftRect = new Rect(buttonsRect.x, buttonsRect.y, buttonsRect.width * 0.5f - 2, buttonsRect.height);
		var rightRect = new Rect(leftRect.xMax + 4, buttonsRect.y, leftRect.width, buttonsRect.height);

		if (GUI.Button(leftRect, "Generate"))
		{
			((EnvironmentGenerator)target).GenerateEnvironment();
		}

		if (GUI.Button(rightRect, "Clense"))
		{
			((EnvironmentGenerator)target).ClenseEnvironment();
		}

		EditorGUILayout.Space();
	}
}
