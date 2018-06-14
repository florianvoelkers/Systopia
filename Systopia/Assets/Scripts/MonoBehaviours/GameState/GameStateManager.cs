using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/*
 * What needs saving:
 * 		- All Conditions
 * 		- Player: HP, Inventory, Equipment, Experience, Money, Quests, Stats
 */
public class GameStateManager : MonoBehaviour {

	[Header ("Save Data")]
	[SerializeField] private AllConditions allConditions;
	[SerializeField] private PlayerEquipment playerEquipment;
	[SerializeField] private PlayerExperience playerExperience;
	[SerializeField] private IntVariable playerHP;
	[SerializeField] private PlayerInventory playerInventory;
	[SerializeField] private PlayerMoney playerMoney;
	[SerializeField] private PlayerQuests playerQuests;
	[SerializeField] private PlayerStats playerStats;
	[Header ("UI")]
	[SerializeField] private GameObject menu;
	[SerializeField] private GameObject devFunctions;

	public static bool isPaused = false;
	private string savePath;

	void Awake () {
		savePath = Application.persistentDataPath + "/save";
	}

	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape)){
			if (isPaused)
				Unpause();
			else
				Pause();
		}

		if (Input.GetKeyUp (KeyCode.D)) {
			devFunctions.SetActive (!devFunctions.activeSelf);
		}
	}

	public void Pause () {
		menu.SetActive (true);
		Time.timeScale = 0;
		isPaused = true;
	}

	public void Unpause () {
		menu.SetActive (false);
		Time.timeScale = 1;
		isPaused = false;
	}

	public void SaveGame () {
		try {
			if (!Directory.Exists (savePath)) {
				Directory.CreateDirectory (savePath);
			}
		} catch (IOException ex) {
			Debug.LogError (ex.Message);
		}
		try {
			if (!Directory.Exists (savePath + "/conditions")) {
				Directory.CreateDirectory (savePath + "/conditions");
			}
		} catch (IOException ex) {
			Debug.LogError (ex.Message);
		}
		string json;
		for (int i = 0; i < allConditions.conditions.Length; i++) {
			json = JsonUtility.ToJson (allConditions.conditions [i]);
			File.WriteAllText (savePath + "/conditions/" + allConditions.conditions [i].name + ".txt", json);
		}
		json = JsonUtility.ToJson (playerEquipment);
		File.WriteAllText (savePath + "/equipment.txt", json);
		json = JsonUtility.ToJson (playerExperience);
		File.WriteAllText (savePath + "/experience.txt", json);
		json = JsonUtility.ToJson (playerHP);
		File.WriteAllText (savePath + "/hp.txt", json);
		json = JsonUtility.ToJson (playerInventory);
		File.WriteAllText (savePath + "/inventory.txt", json);
		json = JsonUtility.ToJson (playerMoney);
		File.WriteAllText (savePath + "/money.txt", json);
		json = JsonUtility.ToJson (playerQuests);
		File.WriteAllText (savePath + "/quests.txt", json);
		try {
			if (!Directory.Exists (savePath + "/stats")) {
				Directory.CreateDirectory (savePath + "/stats");
			}
		} catch (IOException ex) {
			Debug.LogError (ex.Message);
		}
		for (int i = 0; i < playerStats.stats.Length; i++) {
			json = JsonUtility.ToJson (playerStats.stats [i]);
			File.WriteAllText (savePath + "/stats/" + playerStats.stats [i].name + ".txt", json);
		}

	}

	public void LoadGame () {
		string filePath;
		if (Directory.Exists (savePath + "/conditions")) {
			for (int i = 0; i < allConditions.conditions.Length; i++) {
				filePath = savePath + "/conditions/" + allConditions.conditions[i].name + ".txt";
				if (File.Exists (filePath)) {
					string json = File.ReadAllText (filePath);
					JsonUtility.FromJsonOverwrite (json, allConditions.conditions[i]);
				}
			}

		}
		filePath = savePath + "/equipment.txt";
		if (File.Exists (filePath)) {
			string json = File.ReadAllText (filePath);
			JsonUtility.FromJsonOverwrite (json, playerEquipment);
		}
		filePath = savePath + "/experience.txt";
		if (File.Exists (filePath)) {
			string json = File.ReadAllText (filePath);
			JsonUtility.FromJsonOverwrite (json, playerExperience);
		}
		filePath = savePath + "/hp.txt";
		if (File.Exists (filePath)) {
			string json = File.ReadAllText (filePath);
			JsonUtility.FromJsonOverwrite (json, playerHP);
		}
		filePath = savePath + "/inventory.txt";
		if (File.Exists (filePath)) {
			string json = File.ReadAllText (filePath);
			JsonUtility.FromJsonOverwrite (json, playerInventory);
		}
		filePath = savePath + "/money.txt";
		if (File.Exists (filePath)) {
			string json = File.ReadAllText (filePath);
			JsonUtility.FromJsonOverwrite (json, playerMoney);
		}
		filePath = savePath + "/quests.txt";
		if (File.Exists (filePath)) {
			string json = File.ReadAllText (filePath);
			JsonUtility.FromJsonOverwrite (json, playerQuests);
		}
		if (Directory.Exists (savePath + "/stats")) {
			for (int i = 0; i < playerStats.stats.Length; i++) {
				filePath = savePath + "/stats/" + playerStats.stats[i].name + ".txt";
				if (File.Exists (filePath)) {
					string json = File.ReadAllText (filePath);
					JsonUtility.FromJsonOverwrite (json, playerStats.stats[i]);
				}
			}

		}
	}

	public void ResetPlayerBonusStats () {
		playerStats.ResetBonus ();
	}		
}
