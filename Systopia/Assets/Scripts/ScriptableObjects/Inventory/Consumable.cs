﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item {

	public int recoveryValue;
	public IntVariable recoveryStat;

	public override bool Use () {
		return recoveryStat.ApplyChange (recoveryValue);
	}
}
