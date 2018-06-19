using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class FightManager : MonoBehaviour {

	[Header ("UI")]
	[SerializeField] private GameObject fightCharacterUI;
	[SerializeField] private GameObject enemies;
	[SerializeField] private GameObject party;
	[SerializeField] private GameObject fightComments;
	[SerializeField] private Text comments;
	[SerializeField] private GameObject playerButtons;
	[SerializeField] private GameObject settingsIcon;
	[SerializeField] private GameObject tabletIcon;
	[SerializeField] private Sprite playerIcon;
	[SerializeField] private GameObject targetButtonParent;
	[SerializeField] private GameObject targetButton;
	[Header ("Scene Objects")]
	[SerializeField] private SceneController sceneController;

	private GameObject enemySpawns;
	private GameObject playerPartySpawns;
	private GameObject playerCharacter;
	private NavMeshAgent playerNavMeshAgent;
	private Player player;
	private FightReaction fight;

	private List <GameObject> enemyObjects;
	private List <GameObject> partyObjects;
	private List <GameObject> enemyUIObjects;
	private List <GameObject> partyUIObjects;
	private GameObject playerUI;
	private List <GameObject> targetButtons;
	private bool continueRound;

	private List <FightingNPC> enemyList;
	private List <FightingNPC> partyList;

	public void StartFight (FightReaction fight) {
		enemyObjects = new List <GameObject> ();
		partyObjects = new List <GameObject> ();
		enemyUIObjects = new List <GameObject> ();
		partyUIObjects = new List <GameObject> ();
		targetButtons = new List <GameObject> ();
		enemyList = new List <FightingNPC> ();
		partyList = new List <FightingNPC> ();
		for (int i = 0; i < fight.enemies.Length; i++) {
			enemyList.Add (Instantiate (fight.enemies [i]));
		}
		for (int i = 0; i < fight.playersParty.Length; i++) {
			partyList.Add (Instantiate (fight.playersParty [i]));
		}
		this.fight = fight;
		continueRound = true;
		sceneController.FadeAndLoadFightScene (this.fight.fightSceneName, BeginFight);
	}

	public void BeginFight () {
		settingsIcon.SetActive (false);
		tabletIcon.SetActive (false);
		playerCharacter = GameObject.Find ("PlayerCharacter");
		player = playerCharacter.GetComponent <Player> ();
		playerButtons.SetActive (true);

		// spawn enemies
		enemySpawns = GameObject.Find ("EnemySpawns");
		for (int i = 0; i < enemyList.Count; i++) {
			if (i <= enemySpawns.transform.childCount) {
				enemyObjects.Add(Instantiate (enemyList [i].npcModel));
				enemyObjects [i].transform.position = enemySpawns.transform.GetChild (i).position;
				enemyObjects [i].transform.rotation = enemySpawns.transform.GetChild (i).rotation;
				enemyUIObjects.Add(Instantiate (fightCharacterUI, enemies.transform));
				enemyUIObjects [i].transform.Find ("Name").GetComponent <Text> ().text = enemyList [i].npcName;
				enemyUIObjects [i].transform.Find ("CharacterSpriteBack/CharacterSprite").GetComponent <Image> ().sprite = enemyList [i].npcIcon;
				enemyUIObjects [i].transform.Find ("HealthBar/Fill").GetComponent <Image> ().fillAmount = (float) enemyList [i].currentHP / (float) enemyList [i].maximumHP;
				targetButtons.Add(Instantiate (targetButton, targetButtonParent.transform));
				targetButtons [i].transform.Find ("Name").GetComponent <Text> ().text = enemyList [i].npcName;
				targetButtons [i].transform.Find ("CharacterSpriteButton/CharacterSprite").GetComponent <Image> ().sprite = enemyList [i].npcIcon;
				int localIndex = i;
				UnityEngine.Events.UnityAction attackSelection = () => {
					this.Attack (localIndex);
				};
				targetButtons [i].transform.Find ("CharacterSpriteButton").GetComponent <Button> ().onClick.AddListener (attackSelection);
			}
		}

		// spawn player party
		playerPartySpawns = GameObject.Find ("PlayerPartySpawns");
		for (int i = 0; i < partyList.Count; i++) {
			if (i <= playerPartySpawns.transform.childCount) {
				partyObjects.Add(Instantiate (partyList [i].npcModel));
				partyObjects [i].transform.position = playerPartySpawns.transform.GetChild (i).position;
				partyObjects [i].transform.rotation = playerPartySpawns.transform.GetChild (i).rotation;
				partyUIObjects.Add(Instantiate (fightCharacterUI, party.transform));
				partyUIObjects [i].transform.Find ("Name").GetComponent <Text> ().text = partyList [i].npcName;
				partyUIObjects [i].transform.Find ("CharacterSpriteBack/CharacterSprite").GetComponent <Image> ().sprite = partyList[i].npcIcon;
				partyUIObjects [i].transform.Find ("HealthBar/Fill").GetComponent <Image> ().fillAmount = (float) partyList [i].currentHP / (float) partyList[i].maximumHP;
			}
		}

		playerUI = Instantiate (fightCharacterUI, party.transform);
		playerUI.transform.Find ("Name").GetComponent <Text> ().text = player.GetPlayerName ();
		playerUI.transform.Find ("CharacterSpriteBack/CharacterSprite").GetComponent <Image> ().sprite = playerIcon;
		playerUI.transform.Find ("HealthBar/Fill").GetComponent <Image> ().fillAmount = (float) player.GetCurrentHealth () / (float) player.GetMaximumHealth ();
	}

	public void ShowTargets () {
		EventSystem.current.SetSelectedGameObject (null);
		playerButtons.SetActive (false);
		targetButtonParent.SetActive (true);
	}

	private void EndRound () {
		StartCoroutine (WaitForNewRound ());
	}

	private IEnumerator WaitForNewRound () {
		while (!continueRound) {
			yield return new WaitForSeconds (1f);
		}
		fightComments.SetActive (false);
		playerButtons.SetActive (true);
	}

	public void ContinueRound () {
		continueRound = true;
		fightComments.SetActive (false);
	}

	public void Attack (int targetIndex) {
		targetButtonParent.SetActive (false);
		EventSystem.current.SetSelectedGameObject (null);
		int damageDealtByPlayer = player.Attack ();
		if (damageDealtByPlayer == 0) {
			comments.text = player.GetPlayerName () + " trifft nicht.";
		} else {
			comments.text = player.GetPlayerName () + " trifft " + enemyList[targetIndex].npcName + " und richtet " + damageDealtByPlayer + " Schaden an.";
			DamageEnemy (targetIndex, damageDealtByPlayer);
		}
		// hit animation and stuff
		continueRound = false;
		fightComments.SetActive (true);

		if (partyList.Count > 0)
			StartCoroutine (LetPartyAttack (0, targetIndex));
	}

	private IEnumerator LetPartyAttack (int attackerIndex, int targetIndex) {
		while (!continueRound) {
			yield return new WaitForSeconds (1f);
		}
		PartyAttack (attackerIndex, targetIndex);
	}

	private void PartyAttack (int attackerIndex, int targetIndex) {
		int damageDealt = partyList [attackerIndex].Attack ();
		if (damageDealt == 0) {
			comments.text = partyList [attackerIndex].npcName + " trifft nicht.";
		} else {
			string npcName;
			if (enemyList [targetIndex]) {
				npcName = enemyList [targetIndex].npcName;
				DamageEnemy (targetIndex, damageDealt);
			} else {
				npcName = enemyList [enemyList.Count - 1].npcName;
				DamageEnemy (enemyList.Count - 1, damageDealt);
			}
			comments.text = partyList [attackerIndex].npcName + " trifft " + npcName + " und richtet " + damageDealt + " Schaden an.";
		}
		continueRound = false;
		fightComments.SetActive (true);
		attackerIndex++;
		if (attackerIndex < partyList.Count)
			StartCoroutine (LetPartyAttack (attackerIndex, targetIndex));
		else
			if (enemyList.Count > 0)
				StartCoroutine (LetEnemiesAttack (0));
	}

	private IEnumerator LetEnemiesAttack (int attackerIndex) {
		while (!continueRound) {
			yield return new WaitForSeconds (1f);
		}
		EnemyAttack (attackerIndex);
	}

	private void EnemyAttack (int enemyIndex) {
		int damageDealt = enemyList [enemyIndex].Attack ();
		if (damageDealt == 0) {
			comments.text = enemyList [enemyIndex].npcName + " trifft nicht.";
		} else {
			int randomTarget = Random.Range (0, partyList.Count+1);
			if (randomTarget == partyList.Count) { 
				bool playerDead = player.TakeDamage (damageDealt);
				playerUI.transform.Find ("HealthBar/Fill").GetComponent <Image> ().fillAmount = (float) player.GetCurrentHealth () / (float) player.GetMaximumHealth ();
				comments.text = enemyList [enemyIndex].npcName + " trifft " + player.GetPlayerName () + " und richtet " + damageDealt + " Schaden an.";
				if (playerDead) {
					comments.text = enemyList [enemyIndex].npcName + " trifft " + player.GetPlayerName () + ", richtet " + damageDealt + " Schaden an und schaltet damit ihn damit aus.";
					PlayerLost ();
				}
			} else {
				comments.text = enemyList [enemyIndex].npcName + " trifft " + partyList [randomTarget].npcName + " und richtet " + damageDealt + " Schaden an.";
				DamageParty (randomTarget, damageDealt);
			}
		}
		continueRound = false;
		fightComments.SetActive (true);
		enemyIndex++;
		Debug.Log (enemyIndex + " of " + enemyList.Count);
		if (enemyIndex < enemyList.Count)
			StartCoroutine (LetEnemiesAttack (enemyIndex));
		else
			EndRound ();
	}

	private void DamageParty (int targetIndex, int damage) {
		bool isDead = partyList [targetIndex].TakeDamage (damage);
		partyUIObjects [targetIndex].transform.Find ("HealthBar/Fill").GetComponent <Image> ().fillAmount = (float) partyList [targetIndex].currentHP / (float) partyList [targetIndex].maximumHP;
		if (isDead) {
			GameObject partyMemberToRemove = partyObjects [targetIndex];
			Destroy (partyMemberToRemove);
			partyObjects.Remove (partyMemberToRemove);
			GameObject partyMemberUIToRemove = partyUIObjects [targetIndex];
			Destroy (partyMemberUIToRemove);
			partyUIObjects.Remove (partyMemberUIToRemove);
		} 
	}

	private void DamageEnemy (int targetIndex, int damage) {
		bool isDead = enemyList [targetIndex].TakeDamage (damage);
		enemyUIObjects [targetIndex].transform.Find ("HealthBar/Fill").GetComponent <Image> ().fillAmount = (float) enemyList [targetIndex].currentHP / (float) enemyList [targetIndex].maximumHP;
		Debug.Log (enemyList[targetIndex].npcName + " has " + enemyList[targetIndex].currentHP + " hp left");
		if (isDead) {
			GameObject enemyToRemove = enemyObjects [targetIndex];
			Destroy (enemyToRemove);
			enemyObjects.Remove (enemyToRemove);
			GameObject enemyUIToRemove = enemyUIObjects [targetIndex];
			Destroy (enemyUIToRemove);
			enemyUIObjects.Remove (enemyUIToRemove);
			GameObject targetButtonToRemove = targetButtons [targetIndex];
			Destroy (targetButtonToRemove);
			targetButtons.Remove (targetButtonToRemove);
			FightingNPC listEnemyToRemove = enemyList [targetIndex];
			enemyList.Remove (listEnemyToRemove);
			if (enemyList.Count == 0) {
				PlayerWon ();
			}
			for (int i = 0; i < enemyList.Count; i++) {
				int localIndex = i;
				UnityEngine.Events.UnityAction attackSelection = () => {
					this.Attack (localIndex);
				};
				targetButtons [i].transform.Find ("CharacterSpriteButton").GetComponent <Button> ().onClick.AddListener (attackSelection);
			}
		} 
	}

	public void Heal () {

	}

	public void Escape () {
		sceneController.FadeAndLoadFightScene (fight.sceneName, EndFight);
	}

	private void PlayerLost () {
		Debug.Log ("player lost");
		EndFight ();
	}

	private void PlayerWon () {
		Debug.Log ("player won");
		EndFight ();
	}

	public void EndFight () {
		playerButtons.SetActive (false);
	}
}
