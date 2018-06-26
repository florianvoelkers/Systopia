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
	[SerializeField] private Text avaArtefacts;
	[SerializeField] private Text avaHQ;
	[SerializeField] private Text avaArmoury;
	[SerializeField] private Text technoAdministration;
	[SerializeField] private Text hawRuins;
	[SerializeField] private Text libraryRuins;
	[SerializeField] private Text technoArtefacts;
	[SerializeField] private Text avaHideout;
	[SerializeField] private Text technoArmoury;
	[SerializeField] private Text technoHQ;
	[SerializeField] private Text tavern;

	private void OnEnable () {
		for (int i = 0; i < locations.locations.Length; i++) {
			if (locations.locations [i].name == "AvantgardistenHQ") {
				avaHQ.enabled = locations.locations [i].locationDiscovered;
			}
			if (locations.locations [i].name == "AvantgardistenUnterschlupf") {
				avaHideout.enabled = locations.locations [i].locationDiscovered;
			}
			if (locations.locations [i].name == "AvantgardistenWaffenlager") {
				avaArmoury.enabled = locations.locations [i].locationDiscovered;
			}
			if (locations.locations [i].name == "Bibliotheksruine") {
				libraryRuins.enabled = locations.locations [i].locationDiscovered;
			}
			if (locations.locations [i].name == "HAWRuine") {
				hawRuins.enabled = locations.locations [i].locationDiscovered;
			}
			if (locations.locations [i].name == "LagerArtefakte") {
				technoArtefacts.enabled = locations.locations [i].locationDiscovered;
			}
			if (locations.locations [i].name == "Taverne") {
				tavern.enabled = locations.locations [i].locationDiscovered;
			}
			if (locations.locations [i].name == "TechnokratenHQ") {
				technoHQ.enabled = locations.locations [i].locationDiscovered;
			}
			if (locations.locations [i].name == "TechnokratenVerwaltung") {
				technoAdministration.enabled = locations.locations [i].locationDiscovered;
			}
			if (locations.locations [i].name == "TechnokratenWaffenlager") {
				technoArmoury.enabled = locations.locations [i].locationDiscovered;
			}
			if (locations.locations [i].name == "VersteckArtefakte") {
				avaArtefacts.enabled = locations.locations [i].locationDiscovered;
			}
		}
	}

	private void OnDisable () {
		locationTitle.text = "";
		locationDescription.text = "";
	}

	public void SelectLocation (Location location) {
		if (location.locationDiscovered) {
			locationTitle.text = location.locationTitle;
			locationDescription.text = location.locationDescription;
		} else {
			locationTitle.text = "?";
			locationDescription.text = "";
		}

	}
}
