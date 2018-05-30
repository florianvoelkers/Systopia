using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu (menuName = "Quest")]
public class Quest : ScriptableObject {

	public string questTitle;
	public string questDescription;
	public bool isQuestFinished;
	public Faction fromFaction;
	public List <State> states;
	public List <Reward> rewards;

	public void AddState (State state) {
		states.Add (state);
	}

	public void AddReward (Reward newReward) {
		rewards.Add (newReward);
	}

	public void FinishState (State state) {
		for (int i = 0; i < states.Count; i++) {
			if (states [i] == state) {
				states[i].isStateFinished = true;
				if (i == states.Count - 1) {
					isQuestFinished = true;
					RewardPlayer ();
				}
			}
		}
	}

	private void RewardPlayer () {
		for (int i = 0; i < rewards.Count; i++) {
			rewards [i].RewardPlayer ();
		}
	}

}
