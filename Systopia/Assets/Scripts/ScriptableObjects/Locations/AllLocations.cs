using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "AllLocations")]
public class AllLocations : ScriptableObject {

	public Location [] locations;

	public void ResetLocations () {
		for (int i = 0; i < locations.Length; i++) {
			locations[i].locationDiscovered = false;
		}
	}
}
