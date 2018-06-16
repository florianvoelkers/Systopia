using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour {

	[Header ("Description")]
	[SerializeField] private Text locationTitle;
	[SerializeField] private Text locationDescription;
	[Header ("Locations")]
	[SerializeField] private AllLocations locations;

	private void OnEnable () {

	}

	private void OnDisable () {
		locationTitle.text = "";
		locationDescription.text = "";
	}

	public void SelectLocation (Location location) {
		Debug.Log (location.locationTitle + " " + location.locationDiscovered);
		if (location.locationDiscovered) {
			locationTitle.text = location.locationTitle;
			locationDescription.text = location.locationDescription;
		} else {
			locationTitle.text = "?";
			locationDescription.text = "";
		}

	}
}
