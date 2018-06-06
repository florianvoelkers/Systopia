using UnityEditor;
using UnityEngine;

[CustomEditor(typeof (Stat))]
public class StatsEditor : Editor {

	SerializedProperty baseValueProperty;
	SerializedProperty statNameProperty;
	[TextArea (3, 10)] SerializedProperty statDescriptionsProperty;

	private bool showStat;
	private const float statButtonWidth = 30f;
	private const float toggleOffset = 30f;
	private const string baseValuePropName = "baseValue";
	private const string statNamePropName = "statName";
	private const string statDescriptionPropName = "statDescription";

	private Stat stat;

	private void OnEnable () {
		stat = (Stat)target;

		if (target == null) {
			DestroyImmediate (this);
			return;
		}

		showStat = true;
		baseValueProperty = serializedObject.FindProperty (baseValuePropName);
		statNameProperty = serializedObject.FindProperty (statNamePropName);
		statDescriptionsProperty = serializedObject.FindProperty (statDescriptionPropName);
	}

	public override void OnInspectorGUI (){
		serializedObject.Update ();

		EditorGUILayout.BeginVertical (GUI.skin.box);
		EditorGUILayout.BeginHorizontal ();
		EditorGUI.indentLevel++;

		showStat = EditorGUILayout.Foldout (showStat, stat.name);

		EditorGUI.indentLevel--;
		EditorGUILayout.EndHorizontal ();

		if (showStat) {
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField (statNameProperty);
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Description");
			statDescriptionsProperty.stringValue = GUILayout.TextArea (statDescriptionsProperty.stringValue, GUILayout.Height (75f));
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.PropertyField (baseValueProperty);
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndVertical ();
		serializedObject.ApplyModifiedProperties ();
	}

	public static Stat CreateStat (string name) {
		Stat newStat = CreateInstance <Stat> ();
		newStat.name = name;
		newStat.statBonus = 0;
		return newStat;
	}

}
