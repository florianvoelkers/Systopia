using UnityEngine;

public class QuestItem : Item {

	public Condition correspondingCondition;

	public override void Use () {
		Debug.Log ("cannot use");
	}
}
