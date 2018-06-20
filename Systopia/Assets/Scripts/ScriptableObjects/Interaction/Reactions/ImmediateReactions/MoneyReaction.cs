using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyReaction : Reaction {

	public int amountToTake;
	public PlayerMoney playerMoney;

	protected override void ImmediateReaction () {
		playerMoney.SubtractMoney (amountToTake);
	}
}
