using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour {

	[SerializeField] private PlayerStats playerStats;
	[SerializeField] private Image headIcon;
	[SerializeField] private Image bodyIcon;
	[SerializeField] private Image ringIcon;
	[SerializeField] private Image legIcon;
	[SerializeField] private Image weaponIcon;
	[SerializeField] private Text characterName;
	[SerializeField] private GameObject characterStatsPanel;
	[SerializeField] private GameObject field;
	[SerializeField] private GameObject fieldValue;

	private List <GameObject> characterStatsObjects;

	private void Awake () {
		characterStatsObjects = new List<GameObject> ();
	}

	private void OnEnable () {
		ShowCharacterStats ();
	}

	private void ShowCharacterStats () {
		for (int i = 0; i < playerStats.stats.Length; i++) {
			characterStatsObjects.Add (Instantiate (field, characterStatsPanel.transform));
			characterStatsObjects [characterStatsObjects.Count - 1].transform.GetComponentInChildren <Text> ().text = playerStats.stats [i].statName;
			characterStatsObjects.Add (Instantiate (fieldValue, characterStatsPanel.transform));
			characterStatsObjects [characterStatsObjects.Count - 1].transform.GetComponentInChildren <Text> ().text = playerStats.stats [i].GetValue ().ToString ();
		}
	}

}
