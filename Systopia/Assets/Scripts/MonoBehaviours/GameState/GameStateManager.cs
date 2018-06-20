using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/*
 * What needs saving:
 * 		- All Conditions
 * 		- Player: HP, Inventory, Equipment, Experience, Money, Quests, Stats, Location
 * 		- All Locations
 */
public class GameStateManager : MonoBehaviour {

	[Header ("Save Data")]
	[SerializeField] private AllConditions allConditions;
	[SerializeField] private AllLocations allLocations;
	[SerializeField] private PlayerEquipment playerEquipment;
	[SerializeField] private PlayerExperience playerExperience;
	[SerializeField] private IntVariable playerHP;
	[SerializeField] private PlayerInventory playerInventory;
	[SerializeField] private PlayerMoney playerMoney;
	[SerializeField] private PlayerQuests playerQuests;
	[SerializeField] private PlayerStats playerStats;
	[SerializeField] private PlayerLocation playerLocation;
	[Header ("UI")]
	[SerializeField] private GameObject menu;
	[SerializeField] private Text continueStartButtonText;
	[SerializeField] private GameObject settingsIcon;
	[SerializeField] private GameObject tabletIcon;
	[SerializeField] private GameObject characterCreation;
	[SerializeField] private InputField characterName;
	[SerializeField] private GameObject loadingScreen;
	[Header ("Scene")]
	[SerializeField] private SceneController sceneController;
	[SerializeField] private GameObject creationCharacter;
	[SerializeField] private GameObject characterLight;
	[SerializeField] private GameObject characterCamera;
	[SerializeField] private GameObject persistentCamera;
	[Header ("Player Start Equipment")]
	[SerializeField] private Item [] playerStartInventory;
	[SerializeField] private Wearable [] playerStartEquipment;
	[SerializeField] private Weapon playerStartWeapon;

	private GameObject player;
	public static bool isPaused = false;
	private bool gameStarted = false;
	private string savePath;
	private WaitForSeconds wait;

	void Awake () {
		savePath = Application.persistentDataPath + "/save";
		continueStartButtonText.text = "Spiel starten";
	}

	public void StartContinueGame () {
		if (!gameStarted) {
			ResetPlayer ();
			for (int i = 0; i < playerStartInventory.Length; i++) {
				playerInventory.AddItem (playerStartInventory [i]);
			}
			for (int i = 0; i < playerStartEquipment.Length; i++) {
				playerEquipment.EquipWearable (playerStartEquipment [i], playerStartEquipment [i].wearableSlot);
			}
			playerEquipment.EquipWeapon (playerStartWeapon);
			playerMoney.SetMoney (1000);
			ShowCharacterCreation ();
		} else {
			Unpause ();
		}
	}

	private void ShowCharacterCreation () {
		characterCreation.SetActive (true);
		menu.SetActive (false);
	}

	public void FinishCharacter () {
		if (characterName.text != "") {
			loadingScreen.SetActive (true);
			playerStats.playerName = characterName.text;
			creationCharacter.SetActive (false);
			characterLight.SetActive (false);
			characterCamera.SetActive (false);
			persistentCamera.SetActive (true);
			characterCreation.SetActive (false);
			gameStarted = true;
			continueStartButtonText.text = "Fortsetzen";
			sceneController.StartGame ();
			StartCoroutine (WaitAndUnpause (1f));
		}
	}

	private IEnumerator WaitAndUnpause (float delay) {
		wait = new WaitForSeconds (delay);
		yield return wait;
		Unpause ();
	}

	public void PauseGame () {
		settingsIcon.SetActive (false);
		tabletIcon.SetActive (false);
		Time.timeScale = 0;
		isPaused = true;
	}

	public void UnpauseGame () {
		settingsIcon.SetActive (true);
		tabletIcon.SetActive (true);
		Time.timeScale = 1;
		isPaused = false;
	}

	public void ExitGame () {
		SaveGame ();
		Application.Quit ();
	}

	public void Pause () {
		menu.SetActive (true);
		settingsIcon.SetActive (false);
		tabletIcon.SetActive (false);
		Time.timeScale = 0;
		isPaused = true;
	}

	public void Unpause () {
		Time.timeScale = 1;
		isPaused = false;
		menu.SetActive (false);
		loadingScreen.SetActive (false);
		settingsIcon.SetActive (true);
		tabletIcon.SetActive (true);
	}

	public void SaveGame () {
		player = GameObject.Find ("PlayerCharacter");
		playerLocation.currentPosition = player.transform.position;
		playerLocation.currentRotation = player.transform.rotation;
		playerLocation.currentPositionSet = true;
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
		try {
			if (!Directory.Exists (savePath + "/locations")) {
				Directory.CreateDirectory (savePath + "/locations");
			}
		} catch (IOException ex) {
			Debug.LogError (ex.Message);
		}
		for (int i = 0; i < allLocations.locations.Length; i++) {
			json = JsonUtility.ToJson (allLocations.locations [i]);
			File.WriteAllText (savePath + "/locations/" + allLocations.locations [i].name + ".txt", json);
		}
		json = JsonUtility.ToJson (playerLocation);
		File.WriteAllText (savePath + "/playerLocation.txt", json);
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
		Unpause ();
	}

	public void LoadGame () {
		string filePath;
		if (Directory.Exists (savePath + "/conditions")) {
			for (int i = 0; i < allConditions.conditions.Length; i++) {
				filePath = savePath + "/conditions/" + allConditions.conditions [i].name + ".txt";
				if (File.Exists (filePath)) {
					string json = File.ReadAllText (filePath);
					JsonUtility.FromJsonOverwrite (json, allConditions.conditions [i]);
				}
			}
			gameStarted = true;
			continueStartButtonText.text = "Fortsetzen";
		}
		if (Directory.Exists (savePath + "/locations")) {
			for (int i = 0; i < allLocations.locations.Length; i++) {
				filePath = savePath + "/locations/" + allLocations.locations [i].name + ".txt";
				if (File.Exists (filePath)) {
					string json = File.ReadAllText (filePath);
					JsonUtility.FromJsonOverwrite (json, allLocations.locations [i]);
				}
			}

		}
		filePath = savePath + "/playerLocation.txt";
		if (File.Exists (filePath)) {
			string json = File.ReadAllText (filePath);
			JsonUtility.FromJsonOverwrite (json, playerLocation);
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
				filePath = savePath + "/stats/" + playerStats.stats [i].name + ".txt";
				if (File.Exists (filePath)) {
					string json = File.ReadAllText (filePath);
					JsonUtility.FromJsonOverwrite (json, playerStats.stats [i]);
				}
			}

		}
		Time.timeScale = 1;
		sceneController.StartGameFromSaveFile (gameStarted);
		StartCoroutine (WaitAndUnpause (2f));
	}

	public void ResetPlayer () {
		allConditions.Reset ();
		allLocations.ResetLocations ();
		playerEquipment.Reset ();
		playerExperience.Reset ();
		playerHP.Reset ();
		playerInventory.Reset ();
		playerMoney.Reset ();
		playerQuests.Reset ();
		playerStats.Reset ();
		playerLocation.Reset ();
	}		
}
