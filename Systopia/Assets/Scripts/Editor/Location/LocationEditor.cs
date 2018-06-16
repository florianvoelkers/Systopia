using UnityEditor;
using UnityEngine;

[CustomEditor(typeof (Location))]
public class LocationEditor : Editor {

	private SerializedProperty locationTitleProperty;
	[TextArea (3, 10)] private SerializedProperty locationDescriptionProperty;
	private SerializedProperty locationDiscoveredProperty;

	private bool showLocation = true;
	private const float locationButtonWidth = 30f;
	private const float toggleOffset = 30f;
	private const string locationTitlePropName = "locationTitle";
	private const string locationDescriptionPropName = "locationDescription";
	private const string locationDiscoveredPropName = "locationDiscovered";

	private Location location;

	private void OnEnable () {
		location = (Location)target;

		if (target == null) {
			DestroyImmediate (this);
			return;
		}
			
		locationTitleProperty = serializedObject.FindProperty (locationTitlePropName);
		locationDescriptionProperty = serializedObject.FindProperty (locationDescriptionPropName);
		locationDiscoveredProperty = serializedObject.FindProperty (locationDiscoveredPropName);
	}

	public override void OnInspectorGUI () {
		Debug.Log ("create editor gui");
		serializedObject.Update ();

		EditorGUILayout.BeginVertical (GUI.skin.box);
		EditorGUILayout.BeginHorizontal ();
		EditorGUI.indentLevel++;

		showLocation = EditorGUILayout.Foldout (showLocation, location.name);
		if (GUILayout.Button ("-", GUILayout.Width (locationButtonWidth)))
			AllLocationsEditor.RemoveLocation (location);
		
		EditorGUI.indentLevel--;
		EditorGUILayout.EndHorizontal ();

		if (showLocation) {
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField (locationTitleProperty);
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Description");
			locationDescriptionProperty.stringValue = GUILayout.TextArea (locationDescriptionProperty.stringValue, GUILayout.Height (100f));
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.PropertyField (locationDiscoveredProperty);
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndVertical ();
		serializedObject.ApplyModifiedProperties ();
	}

	public static Location CreateLocation (string name) {
		Location newLocation = CreateInstance <Location> ();
		newLocation.name = name;
		return newLocation;
	}
}
