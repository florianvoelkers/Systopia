using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (AllLocations))]
public class AllLocationsEditor : Editor {

	private LocationEditor[] locationEditors;
	private AllLocations allLocations;
	private string newLocationName = "newLocation";
	private const float buttonWidth = 30f;

	private void OnEnable () {
		allLocations = (AllLocations)target;
		if (allLocations.locations == null)
			allLocations.locations = new Location[0];

		if (locationEditors == null) {
			CreateEditors ();
		}
	}

	private void OnDisable () {
		for (int i = 0; i < locationEditors.Length; i++) {
			DestroyImmediate (locationEditors [i]);
		}
		locationEditors = null;
	}

	public override void OnInspectorGUI () {
		serializedObject.Update ();

		if (locationEditors.Length != allLocations.locations.Length) {
			for (int i = 0; i < locationEditors.Length; i++) {
				DestroyImmediate (locationEditors [i]);
			}

			CreateEditors ();
		}

		for (int i = 0; i < locationEditors.Length; i++) {
			EditorGUILayout.BeginHorizontal ();
			locationEditors [i].OnInspectorGUI ();
			EditorGUILayout.EndHorizontal ();
		}

		if (allLocations.locations.Length > 0) {
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
		}

		EditorGUILayout.BeginHorizontal ();

		newLocationName = EditorGUILayout.TextField (GUIContent.none, newLocationName);
		if (GUILayout.Button ("+", GUILayout.Width (buttonWidth))) {
			AddLocation (newLocationName);
			newLocationName = "newLocation";
		}

		EditorGUILayout.EndHorizontal ();

		serializedObject.ApplyModifiedProperties ();
	}

	private void CreateEditors () {
		locationEditors = new LocationEditor[allLocations.locations.Length];
		for (int i = 0; i < locationEditors.Length; i++) {
			Debug.Log ("location editor created");
			locationEditors [i] = CreateEditor (allLocations.locations[i]) as LocationEditor;
		}
	}

	private void AddLocation (string name) {
		Location newLocation = LocationEditor.CreateLocation (name);
		Undo.RecordObject (newLocation, "Created new location");
		AssetDatabase.AddObjectToAsset (newLocation, allLocations);
		AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (newLocation));
		ArrayUtility.Add (ref allLocations.locations, newLocation);
		EditorUtility.SetDirty (allLocations);
	}

	public static void RemoveLocation (Location location) {

	}
}
