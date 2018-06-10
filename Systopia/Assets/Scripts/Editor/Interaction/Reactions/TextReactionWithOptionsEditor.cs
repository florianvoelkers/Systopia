using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (TextWithOptionsReaction))]
public class TextReactionWithOptionsEditor : ReactionEditor {

	private SerializedProperty messageProperty;
	private SerializedProperty textColorProperty;
	private SerializedProperty delayProperty;
	private SerializedProperty optionsProperty;

	private const float messageGUILines = 3f;
	private const float areaWidthOffset = 19f;
	private const string textReactionPropMessageName = "message";
	private const string textReactionPropTextColorName = "textColor";
	private const string textReactionPropDelayName = "delay";
	private const string textReactionPropOptionsName = "options";

	protected override void Init () {
		messageProperty = serializedObject.FindProperty (textReactionPropMessageName);
		textColorProperty = serializedObject.FindProperty (textReactionPropTextColorName);
		delayProperty = serializedObject.FindProperty (textReactionPropDelayName);
		optionsProperty = serializedObject.FindProperty (textReactionPropOptionsName);
	}

	protected override void DrawReaction () {
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Message", GUILayout.Width (EditorGUIUtility.labelWidth - areaWidthOffset));
		messageProperty.stringValue = EditorGUILayout.TextArea (messageProperty.stringValue, GUILayout.Height (EditorGUIUtility.singleLineHeight * messageGUILines));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.PropertyField (textColorProperty);
		EditorGUILayout.PropertyField (delayProperty);

		EditorGUILayout.PropertyField (optionsProperty, true);
	}

	protected override string GetFoldoutLabel () {
		return "OptionText Reaction";
	}
}
