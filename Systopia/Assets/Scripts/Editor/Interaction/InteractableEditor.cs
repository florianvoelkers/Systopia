﻿using UnityEditor;
using UnityEngine;

[CustomEditor(typeof (Interactable))]
public class InteractableEditor : EditorWithSubEditors <ConditionCollectionEditor, ConditionCollection> {

	private Interactable interactable;
	private SerializedProperty interactionCursorProperty;
	private SerializedProperty interactionNameProperty;
	private SerializedProperty interactionLocationProperty;
	private SerializedProperty collectionsProperty;
	private SerializedProperty defaultReactionCollectionProperty;

	private const float collectionButtonWidth = 125f;
	private const string interactablePropInteractionCursorName = "interactionCursor";
	private const string interactablePropInteractionNameName = "interactionName";
	private const string interactablePropInteractionLocationName = "interactionLocation";
	private const string interactablePropConditionCollectionsName = "conditionCollections";
	private const string interactactablePropDefaultReactionCollectionName = "defaultReactionCollection";

	private void OnEnable () {
		interactable = (Interactable)target;
		interactionCursorProperty = serializedObject.FindProperty (interactablePropInteractionCursorName);
		interactionNameProperty = serializedObject.FindProperty (interactablePropInteractionNameName);
		collectionsProperty = serializedObject.FindProperty (interactablePropConditionCollectionsName);
		interactionLocationProperty = serializedObject.FindProperty (interactablePropInteractionLocationName);
		defaultReactionCollectionProperty = serializedObject.FindProperty (interactactablePropDefaultReactionCollectionName);

		CheckAndCreateSubEditors (interactable.conditionCollections);
	}

	private void OnDisable () {
		CleanupEditors ();
	}

	protected override void SubEditorSetup (ConditionCollectionEditor editor) {
		editor.collectionsProperty = collectionsProperty;
	}

	public override void OnInspectorGUI () {
		serializedObject.Update ();

		CheckAndCreateSubEditors (interactable.conditionCollections);
		EditorGUILayout.PropertyField (interactionCursorProperty);
		EditorGUILayout.PropertyField (interactionNameProperty);
		EditorGUILayout.PropertyField (interactionLocationProperty);
		for (int i = 0; i < subEditors.Length; i++) {
			subEditors [i].OnInspectorGUI ();
			EditorGUILayout.Space ();
		}

		EditorGUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("Add Collection", GUILayout.Width (collectionButtonWidth))) {
			ConditionCollection newCollection = ConditionCollectionEditor.CreateConditionCollection ();
			collectionsProperty.AddToObjectArray (newCollection);
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.Space ();
		EditorGUILayout.PropertyField (defaultReactionCollectionProperty);
		serializedObject.ApplyModifiedProperties ();
	}
}
