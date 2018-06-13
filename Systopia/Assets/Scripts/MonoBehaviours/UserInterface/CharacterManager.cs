using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour {

	[SerializeField] private PlayerStats playerStats;
	[SerializeField] private PlayerEquipment playerEquipment;
	[SerializeField] private Image headIcon;
	[SerializeField] private Image bodyIcon;
	[SerializeField] private Image ringIcon;
	[SerializeField] private Image legIcon;
	[SerializeField] private Image weaponIcon;
	[SerializeField] private Text characterName;
	[SerializeField] private GameObject characterStatsPanel;
	[SerializeField] private GameObject field;
	[SerializeField] private GameObject fieldValue;

	private List <GameObject> characterStatsObjects = new List<GameObject> ();

	private void OnEnable () {
		ShowCharacterStats ();
	}

	private void OnDisable () {
		RemoveCharacterStats ();
	}

	private void ShowCharacterStats () {
		characterName.text = playerStats.playerName;
		if (playerEquipment.headItem) {
			headIcon.enabled = true;
			headIcon.sprite = playerEquipment.headItem.itemSprite;
		}
		if (playerEquipment.bodyItem) {
			bodyIcon.enabled = true;
			bodyIcon.sprite = playerEquipment.bodyItem.itemSprite;
		}
		if (playerEquipment.fingerItem) {
			ringIcon.enabled = true;
			ringIcon.sprite = playerEquipment.fingerItem.itemSprite;
		}
		if (playerEquipment.legsItem) {
				legIcon.enabled = true;
			legIcon.sprite = playerEquipment.legsItem.itemSprite;
		}
		if (playerEquipment.weapon) {
			weaponIcon.enabled = true;
			weaponIcon.sprite = playerEquipment.weapon.itemSprite;
		}
		for (int i = 0; i < playerStats.stats.Length; i++) {
			characterStatsObjects.Add (Instantiate (field, characterStatsPanel.transform));
			characterStatsObjects [characterStatsObjects.Count - 1].transform.GetComponentInChildren <Text> ().text = playerStats.stats [i].statName;
			characterStatsObjects.Add (Instantiate (fieldValue, characterStatsPanel.transform));
			characterStatsObjects [characterStatsObjects.Count - 1].transform.GetComponentInChildren <Text> ().text = playerStats.stats [i].GetValue ().ToString ();
		}
	}

	private void RemoveCharacterStats () {
		characterName.text = "";
		for (int i = 0; i < characterStatsPanel.transform.childCount; i++) {
			Destroy (characterStatsPanel.transform.GetChild (i).gameObject);
		}
		characterStatsObjects.Clear ();
	}

}
