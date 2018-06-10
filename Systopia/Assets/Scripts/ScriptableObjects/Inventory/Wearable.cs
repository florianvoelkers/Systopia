using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wearable : Item {

	public enum WearableSlot {Head, Body, Legs, Finger};

	public WearableSlot wearableSlot;
	public List <StatBonus> bonusses = new List<StatBonus> ();
}
