﻿using UnityEngine;

// for turning on/off GameObjects
public class GameObjectReaction : DelayedReaction {

	public GameObject gameObject;
	public bool activeState;

	protected override void ImmediaReaction () {
		gameObject.SetActive (activeState);
	}
}
