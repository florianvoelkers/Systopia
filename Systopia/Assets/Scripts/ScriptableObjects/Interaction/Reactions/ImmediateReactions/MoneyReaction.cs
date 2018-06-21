using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyReaction : Reaction {

	public int amountToTake;
	public PlayerMoney playerMoney;
	public ReactionCollection hasEnoughMoney;
	public ReactionCollection doesNotHaveEnoughMoney;

	protected override void ImmediateReaction () {
		if (playerMoney.SubtractMoney (amountToTake)) {
			hasEnoughMoney.React ();
		} else {
			doesNotHaveEnoughMoney.React ();
		}
	}
}
