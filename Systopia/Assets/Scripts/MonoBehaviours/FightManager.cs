using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FightManager : MonoBehaviour {

	[Header ("UI")]
	[SerializeField] private GameObject fightCharacterUI;
	[SerializeField] private GameObject enemiesObject;
	[SerializeField] private GameObject partyObject;
	[SerializeField] private GameObject fightComments;
	[SerializeField] private Text comments;
	[SerializeField] private GameObject playerButtons;
	[SerializeField] private GameObject settingsIcon;
	[SerializeField] private GameObject tabletIcon;
	[SerializeField] private Sprite playerIcon;
	[SerializeField] private GameObject targetButtonParent;
	[SerializeField] private GameObject targetButton;
	[SerializeField] private GameObject loadScreen;
	[Header ("Scene Objects")]
	[SerializeField] private SceneController sceneController;
	[Header ("Sounds")]
	[SerializeField] private AudioSource backgroundMusic;
	[SerializeField] private AudioSource soundEffects;
	[SerializeField] private AudioClip fightWon;

	private string previousSceneName;
	private ReactionCollection winReaction;
	private ReactionCollection lossReaction;
	private AudioClip previousBackgroundMusic;
	private AudioClip fightMusic;

	private GameObject playerCharacter;
	private Player player;
	private GameObject playerUI;

	private GameObject enemySpawns;
	private List <FightingNPC> enemies = new List <FightingNPC> (); // clear at end of battle
	private List <GameObject> enemyModels = new List <GameObject> ();
	private List <GameObject> enemyUIObjects = new List <GameObject> ();
	private List <GameObject> targetButtons = new List <GameObject> ();

	private GameObject partySpawns;
	private List <FightingNPC> party = new List <FightingNPC> ();
	private List <GameObject> partyModels = new List <GameObject> ();
	private List <GameObject> partyUIObjects = new List <GameObject> ();

	private enum FightStages {Wait, PlayerAttack, PartyAttack, EnemyAttack, PlayerWon, PlayerLost };
	private FightStages currentFightStage;
	private int enemyOnTurn;
	private int partyTarget;
	private int partyMemberOnTurn;
	private bool continueRound;
	private bool playerDead;

	public void StartFight (FightReaction fight) {
		loadScreen.SetActive (true);
		for (int i = 0; i < fight.enemies.Length; i++) {
			enemies.Add (Instantiate (fight.enemies [i]));
		}
		enemyOnTurn = 0;
		for (int i = 0; i < fight.playersParty.Length; i++) {
			party.Add (Instantiate (fight.playersParty [i]));
		}
		partyMemberOnTurn = 0;
		previousSceneName = fight.sceneName;
		winReaction = fight.winReaction;
		lossReaction = fight.lossReaction;
		previousBackgroundMusic = backgroundMusic.clip;
		fightMusic = fight.fightMusic;
		currentFightStage = FightStages.Wait;
		continueRound = true;
		playerDead = false;
		sceneController.FadeAndLoadFightScene (fight.fightSceneName, BeginFight);
	}

	public void BeginFight () {
		enemySpawns = GameObject.Find ("EnemySpawns");
		partySpawns = GameObject.Find ("PlayerPartySpawns");
		settingsIcon.SetActive (false);
		tabletIcon.SetActive (false);
		playerCharacter = GameObject.Find ("PlayerCharacter");
		player = playerCharacter.GetComponent <Player> ();
		playerButtons.SetActive (true);
		backgroundMusic.clip = fightMusic;
		backgroundMusic.Play ();

		SetupEnemyUI ();
		SetupPartyUI ();
		SetupPlayerUI ();

		loadScreen.SetActive (false);
	}

	private void Update () {
		if (continueRound) {
			switch (currentFightStage) {
			case FightStages.PlayerAttack:
				currentFightStage = FightStages.Wait;
				playerButtons.SetActive (true);
				break;	
			case FightStages.PartyAttack:
				currentFightStage = FightStages.Wait;
				PartyAttack ();
				break;	
			case FightStages.EnemyAttack:
				currentFightStage = FightStages.Wait;
				EnemyAttack ();
				break;
			case FightStages.PlayerLost:
				currentFightStage = FightStages.Wait;
				PlayerLost ();
				break;
			case FightStages.PlayerWon:
				currentFightStage = FightStages.Wait;
				PlayerWon ();
				break;
			default:
				break;
			}
		}
	}

	public void Escape () {
		playerButtons.SetActive (false);
		currentFightStage = FightStages.PlayerLost;
	}

	public void Heal () {

	}

	private void EndFightScene () {
		Destroy (playerUI);
		playerUI = null;
		player = null;
		playerCharacter = null;

		for (int i = 0; i < enemies.Count; i++) {
			Destroy (enemies [i]);
		}
		enemies.Clear ();
		for (int i = 0; i < enemyModels.Count; i++) {
			Destroy (enemyModels [i]);
		}
		enemyModels.Clear ();
		for (int i = 0; i < enemyUIObjects.Count; i++) {
			Destroy (enemyUIObjects [i]);
		}
		enemyUIObjects.Clear ();
		for (int i = 0; i < targetButtons.Count; i++) {
			Destroy (targetButtons [i]);
		}
		targetButtons.Clear ();

		for (int i = 0; i < party.Count; i++) {
			Destroy (party [i]);
		}
		party.Clear ();
		for (int i = 0; i < partyModels.Count; i++) {
			Destroy (partyModels [i]);
		}
		partyModels.Clear ();
		for (int i = 0; i < partyUIObjects.Count; i++) {
			Destroy (partyUIObjects [i]);
		}
		partyUIObjects.Clear ();
		backgroundMusic.clip = previousBackgroundMusic;
		backgroundMusic.Play ();
		settingsIcon.SetActive (true);
		tabletIcon.SetActive (true);
	}

	private void FightWonReaction () {
		loadScreen.SetActive (false);
		winReaction.React ();
	}

	private void FightLostReaction () {
		loadScreen.SetActive (false);
		lossReaction.React ();
	}

	private void PlayerWon () {
		loadScreen.SetActive (true);
		soundEffects.clip = fightWon;
		soundEffects.Play ();
		EndFightScene ();
		sceneController.FadeAndLoadFightScene (previousSceneName, FightWonReaction);
	}

	private void PlayerLost () {
		loadScreen.SetActive (true);
		EndFightScene ();
		sceneController.FadeAndLoadFightScene (previousSceneName, FightLostReaction);
	}

	public void ContinueRound () {
		continueRound = true;
		fightComments.SetActive (false);
	}

	public void ShowTargets () {
		EventSystem.current.SetSelectedGameObject (null);
		playerButtons.SetActive (false);
		targetButtonParent.SetActive (true);
	}

	private void EnemyAttack () {
		int damageDealt = enemies [enemyOnTurn].Attack ();
		enemyModels [enemyOnTurn].GetComponent <Animator> ().SetTrigger ("attack");
		AudioSource source = enemyModels [enemyOnTurn].GetComponent <AudioSource> ();
		source.clip = enemies [enemyOnTurn].attackSound;
		source.Play ();

		if (damageDealt == 0) {
			comments.text = enemies [enemyOnTurn].npcName + " trifft nicht.";
			FinishEnemyAttack ();
		} else {
			int randomTarget = Random.Range (0, party.Count + 1);
			if (randomTarget == party.Count) {
				comments.text = enemies [enemyOnTurn].npcName + " trifft " + DamagePlayer (damageDealt, FinishEnemyAttack);
			} else {
				comments.text = enemies [enemyOnTurn].npcName + " trifft " + DamageParty (randomTarget, damageDealt, FinishEnemyAttack);
			}
		}
	}

	private void FinishEnemyAttack () {
		continueRound = false;
		fightComments.SetActive (true);
		enemyOnTurn++;
		if (enemies.Count > enemyOnTurn) {
			currentFightStage = FightStages.EnemyAttack;
		}
		else {
			currentFightStage = FightStages.PlayerAttack;
			enemyOnTurn = 0;
		}

		if (playerDead)
			currentFightStage = FightStages.PlayerLost;
	}

	private void PartyAttack () {
		int damageDealt = party [partyMemberOnTurn].Attack ();
		partyModels [partyMemberOnTurn].GetComponent <Animator> ().SetTrigger ("attack");
		AudioSource source = partyModels [partyMemberOnTurn].GetComponent <AudioSource> ();
		source.clip = party [partyMemberOnTurn].attackSound;
		source.Play ();
		if (damageDealt == 0) {
			comments.text = party [partyMemberOnTurn].npcName + " trifft nicht.";
			FinishPartyAttack ();
		} else {
			comments.text = party [partyMemberOnTurn].npcName + " trifft " + DamageEnemy (partyTarget, damageDealt, FinishPartyAttack);
		}
	}

	private void FinishPartyAttack () {
		continueRound = false;
		fightComments.SetActive (true);
		partyMemberOnTurn++;
		if (party.Count > partyMemberOnTurn) {
			currentFightStage = FightStages.PartyAttack;
		}
		else {
			currentFightStage = FightStages.EnemyAttack;
			partyMemberOnTurn = 0;
		}

		if (enemies.Count == 0)
			currentFightStage = FightStages.PlayerWon;
	}

	public void PlayerAttack (int targetIndex) {
		targetButtonParent.SetActive (false);
		EventSystem.current.SetSelectedGameObject (null);
		partyTarget = targetIndex;
		int damageDealtByPlayer = player.Attack ();
		playerCharacter.GetComponent <Animator> ().SetTrigger ("attack");
		AudioSource source = playerCharacter.GetComponent <AudioSource> ();
		source.clip = player.attackSound;
		source.Play ();
		if (damageDealtByPlayer == 0) {
			comments.text = player.GetPlayerName () + " trifft nicht.";
			FinishPlayerAttack ();
		} else {
			comments.text = player.GetPlayerName () + " trifft " + DamageEnemy (targetIndex, damageDealtByPlayer, FinishPlayerAttack);;
		}
	}

	private void FinishPlayerAttack () {
		continueRound = false;
		fightComments.SetActive (true);
		if (party.Count > 0)
			currentFightStage = FightStages.PartyAttack;
		else
			currentFightStage = FightStages.EnemyAttack;

		if (enemies.Count == 0)
			currentFightStage = FightStages.PlayerWon;
	}

	private string DamagePlayer (int damage, System.Action callback) {
		bool playerDead = player.TakeDamage (damage);
		UpdatePlayerHealthBar ();
		string comment = player.GetPlayerName () + " und richtet " + (damage - player.GetArmor ()) + " Schaden an.";
		if (playerDead) {
			comment = player.GetPlayerName () + " und setzt ihn außer Gefecht.";
			AudioSource source = playerCharacter.GetComponent <AudioSource> ();
			source.clip = player.dieSound;
			source.Play ();
			playerCharacter.GetComponent <Animator> ().SetTrigger ("dies");
			playerDead = true;
		} else {
			AudioSource source = playerCharacter.GetComponent <AudioSource> ();
			source.clip = player.hitSound;
			source.Play ();
			playerCharacter.GetComponent <Animator> ().SetTrigger ("hit");
		}
		callback ();
		return comment;
	}

	private string DamageParty (int targetIndex, int damage, System.Action callback) {
		bool isDead = party [targetIndex].TakeDamage (damage);
		UpdatePartyHealthBar (targetIndex);
		string comment = party [targetIndex].npcName + " und richtet " + (damage - party [targetIndex].armor) + " Schaden an.";
		if (isDead) {
			comment = party [targetIndex].npcName + " und setzt ihn außer Gefecht.";
			AudioSource source = partyModels[targetIndex].GetComponent <AudioSource> ();
			source.clip = party [targetIndex].dieSound;
			source.Play ();
			partyModels [targetIndex].GetComponent <Animator> ().SetTrigger ("dies");

			FightingNPC partyMember = party [targetIndex];
			party.Remove (partyMember);
			Destroy (partyMember);

			GameObject partyMemberModel = partyModels [targetIndex];
			partyModels.Remove (partyMemberModel);
			Destroy (partyMemberModel, 2f);

			GameObject partyMemberUI = partyUIObjects [targetIndex];
			partyUIObjects.Remove (partyMemberUI);
			Destroy (partyMemberUI, 2f);
		} else {
			AudioSource source = partyModels[targetIndex].GetComponent <AudioSource> ();
			source.clip = party [targetIndex].hitSound;
			source.Play ();
			partyModels [targetIndex].GetComponent <Animator> ().SetTrigger ("hit");
		}
		callback ();
		return comment;
	}

	private string DamageEnemy (int targetIndex, int damage, System.Action callback) {
		bool isDead = enemies [targetIndex].TakeDamage (damage);
		UpdateEnemyHealthBar (targetIndex);
		string comment = enemies [targetIndex].npcName + " und richtet " + (damage - enemies [targetIndex].armor) + " Schaden an.";
		if (isDead) {
			comment = enemies [targetIndex].npcName + " und setzt ihn außer Gefecht.";
			AudioSource source = enemyModels[targetIndex].GetComponent <AudioSource> ();
			source.clip = enemies [targetIndex].dieSound;
			source.Play ();
			enemyModels [targetIndex].GetComponent <Animator> ().SetTrigger ("dies");

			FightingNPC enemy = enemies [targetIndex];
			enemies.Remove (enemy);
			Destroy (enemy);

			GameObject enemyModel = enemyModels [targetIndex];
			enemyModels.Remove (enemyModel);
			Destroy (enemyModel, 2f);

			GameObject enemyUI = enemyUIObjects [targetIndex];
			enemyUIObjects.Remove (enemyUI);
			Destroy (enemyUI, 2f);

			for (int i = 0; i < targetButtons.Count; i++) {
				Destroy (targetButtons [i]);
			}
			targetButtons.Clear ();
			for (int i = 0; i < enemies.Count; i++) {
				SetupTargetButton (i);
			}

			partyTarget = Random.Range (0, enemies.Count);
		} else {
			AudioSource source = enemyModels[targetIndex].GetComponent <AudioSource> ();
			source.clip = enemies [targetIndex].hitSound;
			source.Play ();
			enemyModels [targetIndex].GetComponent <Animator> ().SetTrigger ("hit");
		}
		callback ();
		return comment;
	}

	private void SetupPlayerUI () {
		playerUI = Instantiate (fightCharacterUI, partyObject.transform);
		playerUI.transform.Find ("Name").GetComponent <Text> ().text = player.GetPlayerName ();
		playerUI.transform.Find ("CharacterSpriteBack/CharacterSprite").GetComponent <Image> ().sprite = playerIcon;
		UpdatePlayerHealthBar ();
	}

	private void UpdatePlayerHealthBar () {
		playerUI.transform.Find ("HealthBar/Fill").GetComponent <Image> ().fillAmount = (float) player.GetCurrentHealth () / (float) player.GetMaximumHealth ();
	}

	private void SetupPartyUI () {
		for (int i = 0; i < party.Count; i++) {
			if (i <= partySpawns.transform.childCount) {
				partyModels.Add(Instantiate (party [i].npcModel));
				partyModels [i].transform.position = partySpawns.transform.GetChild (i).position;
				partyModels [i].transform.rotation = partySpawns.transform.GetChild (i).rotation;
				partyUIObjects.Add(Instantiate (fightCharacterUI, partyObject.transform));
				partyUIObjects [i].transform.Find ("Name").GetComponent <Text> ().text = party [i].npcName;
				partyUIObjects [i].transform.Find ("CharacterSpriteBack/CharacterSprite").GetComponent <Image> ().sprite = party[i].npcIcon;
				UpdatePartyHealthBar (i);
			}
		}
	}

	private void UpdatePartyHealthBar (int i) {
		partyUIObjects [i].transform.Find ("HealthBar/Fill").GetComponent <Image> ().fillAmount = (float) party [i].currentHP / (float) party[i].maximumHP;
	}

	private void SetupEnemyUI () {
		for (int i = 0; i < enemies.Count; i++) {
			if (i <= enemySpawns.transform.childCount) {
				enemyModels.Add(Instantiate (enemies [i].npcModel));
				enemyModels [i].transform.position = enemySpawns.transform.GetChild (i).position;
				enemyModels [i].transform.rotation = enemySpawns.transform.GetChild (i).rotation;
				enemyUIObjects.Add(Instantiate (fightCharacterUI, enemiesObject.transform));
				enemyUIObjects [i].transform.Find ("Name").GetComponent <Text> ().text = enemies [i].npcName;
				enemyUIObjects [i].transform.Find ("CharacterSpriteBack/CharacterSprite").GetComponent <Image> ().sprite = enemies [i].npcIcon;
				UpdateEnemyHealthBar (i);
				SetupTargetButton (i);
			}
		}
	}

	private void UpdateEnemyHealthBar (int index) {
		enemyUIObjects [index].transform.Find ("HealthBar/Fill").GetComponent <Image> ().fillAmount = (float) enemies [index].currentHP / (float) enemies [index].maximumHP;
	}

	private void SetupTargetButton (int index) {
		targetButtons.Add(Instantiate (targetButton, targetButtonParent.transform));
		targetButtons [index].transform.Find ("Name").GetComponent <Text> ().text = enemies [index].npcName;
		targetButtons [index].transform.Find ("CharacterSpriteButton/CharacterSprite").GetComponent <Image> ().sprite = enemies [index].npcIcon;
		int localIndex = index;
		UnityEngine.Events.UnityAction attackSelection = () => {
			this.PlayerAttack (localIndex);
		};
		targetButtons [index].transform.Find ("CharacterSpriteButton").GetComponent <Button> ().onClick.AddListener (attackSelection);
	}
}