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

	public void StartFight (FightReaction fight) {
		enemyObjects = new List <GameObject> ();
		partyObjects = new List <GameObject> ();
		enemyUIObjects = new List <GameObject> ();
		partyUIObjects = new List <GameObject> ();
		targetButtons = new List <GameObject> ();
		this.fight = fight;
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
		for (int i = 0; i < fight.enemies.Length; i++) {
			if (i <= enemySpawns.transform.childCount) {
				enemyObjects.Add(Instantiate (fight.enemies [i].npcModel));
				enemyObjects [i].transform.position = enemySpawns.transform.GetChild (i).position;
				enemyObjects [i].transform.rotation = enemySpawns.transform.GetChild (i).rotation;
				enemyUIObjects.Add(Instantiate (fightCharacterUI, enemies.transform));
				enemyUIObjects [i].transform.Find ("Name").GetComponent <Text> ().text = fight.enemies [i].npcName;
				enemyUIObjects [i].transform.Find ("CharacterSpriteBack/CharacterSprite").GetComponent <Image> ().sprite = fight.enemies [i].npcIcon;
				enemyUIObjects [i].transform.Find ("HealthBar/Fill").GetComponent <Image> ().fillAmount = (float) fight.enemies [i].currentHP / (float) fight.enemies [i].maximumHP;
				targetButtons.Add(Instantiate (targetButton, targetButtonParent.transform));
				targetButtons [i].transform.Find ("Name").GetComponent <Text> ().text = fight.enemies [i].npcName;
				targetButtons [i].transform.Find ("CharacterSpriteButton/CharacterSprite").GetComponent <Image> ().sprite = fight.enemies [i].npcIcon;
				int localIndex = i;
				UnityEngine.Events.UnityAction attackSelection = () => {
					this.Attack (localIndex);
				};
				targetButtons [i].transform.Find ("CharacterSpriteButton").GetComponent <Button> ().onClick.AddListener (attackSelection);
			}
		}

		// spawn player party
		playerPartySpawns = GameObject.Find ("PlayerPartySpawns");
		for (int i = 0; i < fight.playersParty.Length; i++) {
			if (i <= playerPartySpawns.transform.childCount) {
				partyObjects.Add(Instantiate (fight.playersParty [i].npcModel));
				partyObjects [i].transform.position = playerPartySpawns.transform.GetChild (i).position;
				partyObjects [i].transform.rotation = playerPartySpawns.transform.GetChild (i).rotation;
				partyUIObjects.Add(Instantiate (fightCharacterUI, party.transform));
				partyUIObjects [i].transform.Find ("Name").GetComponent <Text> ().text = fight.playersParty [i].npcName;
				partyUIObjects [i].transform.Find ("CharacterSpriteBack/CharacterSprite").GetComponent <Image> ().sprite = fight.playersParty [i].npcIcon;
				partyUIObjects [i].transform.Find ("HealthBar/Fill").GetComponent <Image> ().fillAmount = (float) fight.playersParty [i].currentHP / (float) fight.playersParty [i].maximumHP;
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

	public void Attack (int targetIndex) {
		targetButtonParent.SetActive (false);
		EventSystem.current.SetSelectedGameObject (null);
		int damageDealtByPlayer = player.Attack ();
		if (damageDealtByPlayer == 0) {
			Debug.Log ("miss");
		} else {
			DamageEnemy (targetIndex, damageDealtByPlayer);
		}
		for (int i = 0; i < fight.playersParty.Length; i++) {
			int damageDealt = fight.playersParty [i].Attack ();
			if (damageDealt == 0) {
				Debug.Log (fight.playersParty [i].npcName + " misses");
			} else {
				if (fight.enemies [targetIndex]) {
					DamageEnemy (targetIndex, damageDealt);
				} else {
					DamageEnemy (fight.enemies.Length - 1, damageDealt);
				}
			}
		}
		for (int i = 0; i < fight.enemies.Length; i++) {
			int damageDealt = fight.enemies [i].Attack ();
			if (damageDealt == 0) {
				Debug.Log (fight.enemies [i].npcName + " misses");
			} else {
				int randomTarget = Random.Range (0, fight.playersParty.Length);
				Debug.Log (randomTarget);
				if (randomTarget == fight.playersParty.Length) { 
					bool playerDead = player.TakeDamage (damageDealt);
					playerUI.transform.Find ("HealthBar/Fill").GetComponent <Image> ().fillAmount = (float) player.GetCurrentHealth () / (float) player.GetMaximumHealth ();
					if (playerDead) {
						PlayerLost ();
					}
				} else {
					DamageParty (randomTarget, damageDealt);
				}
			}
		}
		playerButtons.SetActive (true);
	}

	private void DamageParty (int targetIndex, int damage) {
		bool isDead = fight.playersParty [targetIndex].TakeDamage (damage);
		partyUIObjects [targetIndex].transform.Find ("HealthBar/Fill").GetComponent <Image> ().fillAmount = (float) fight.playersParty [targetIndex].currentHP / (float) fight.playersParty [targetIndex].maximumHP;
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
		bool isDead = fight.enemies [targetIndex].TakeDamage (damage);
		enemyUIObjects [targetIndex].transform.Find ("HealthBar/Fill").GetComponent <Image> ().fillAmount = (float) fight.enemies [targetIndex].currentHP / (float) fight.enemies [targetIndex].maximumHP;
		Debug.Log (enemyObjects.Count);
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
			Debug.Log (enemyObjects.Count);
			if (enemyObjects.Count == 0) {
				PlayerWon ();
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
