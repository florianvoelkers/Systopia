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
		fightManager.StartFight (this);
	}
}
