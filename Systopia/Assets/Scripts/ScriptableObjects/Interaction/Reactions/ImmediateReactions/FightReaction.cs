using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightReaction : Reaction {

	public FightingNPC [] enemies;
	public FightingNPC [] playersParty;
	public string sceneName;
	public string fightSceneName;
	public ReactionCollection winReaction;
	public ReactionCollection lossReaction;

	private FightManager fightManager;

	protected override void SpecificInit () {
		fightManager = FindObjectOfType <FightManager> ();
	}

	protected override void ImmediateReaction () {
		GameObject player = GameObject.Find ("PlayerCharacter");
		player.GetComponent <PlayerMovement> ().playerLocation.currentPosition = player.transform.position;
		player.GetComponent <PlayerMovement> ().playerLocation.currentRotation = player.transform.rotation;
		player.GetComponent <PlayerMovement> ().playerLocation.currentPositionSet = true;
		fightManager.StartFight (this);
	}
}
