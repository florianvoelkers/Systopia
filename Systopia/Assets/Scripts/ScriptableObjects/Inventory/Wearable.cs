using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wearable : Item {

	public enum WearableSlot {Head, Body, Legs, Finger};

	public WearableSlot wearableSlot;
	public List <StatBonus> bonusses = new List<StatBonus> ();

	public override void Use () {
		Debug.Log ("equip wearable at slot " + wearableSlot.ToString ());
	}
}
