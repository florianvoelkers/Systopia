using UnityEngine;

public class QuestItem : Item {

	public Condition correspondingCondition;

	public override bool Use () {
		Debug.Log ("cannot use");
		return true;
	}
}
