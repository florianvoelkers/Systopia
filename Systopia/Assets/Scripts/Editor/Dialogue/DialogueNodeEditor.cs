using UnityEngine;
using UnityEditor;

//[CustomEditor (typeof (DialogueNode))]
public class DialogueNodeEditor : Editor {

	private SerializedProperty dialogueNodeIdProperty;
	private SerializedProperty dialogueTextProperty;
	private SerializedProperty dialogueOptionsProperty;

	private const string dialogueNodeIdPropName = "nodeId";
	private const string dialogueTextPropName = "text";
	private const string dialogueOptionsPropName = "options";

	private void OnEnable () {
		dialogueNodeIdProperty = serializedObject.FindProperty (dialogueNodeIdPropName);
		dialogueTextProperty = serializedObject.FindProperty (dialogueTextPropName);
		dialogueOptionsProperty = serializedObject.FindProperty (dialogueOptionsPropName);
	}

	public override void OnInspectorGUI () {
		serializedObject.Update ();

		EditorGUILayout.BeginVertical (GUI.skin.box);
		EditorGUI.indentLevel++;

		EditorGUILayout.PropertyField (dialogueNodeIdProperty);
		dialogueTextProperty.stringValue = GUILayout.TextArea (dialogueTextProperty.stringValue, GUILayout.Height (75f));
		EditorGUILayout.PropertyField (dialogueOptionsProperty, true);

		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical ();

		serializedObject.ApplyModifiedProperties ();
	}
}
