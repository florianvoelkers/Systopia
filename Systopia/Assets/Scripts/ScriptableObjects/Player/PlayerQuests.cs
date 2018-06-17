using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "Player/All Quests")]
public class PlayerQuests : ScriptableObject {

	public List <Quest> quests = new List <Quest> ();

	public void Reset () {
		quests.Clear ();
	}

	public void AddQuest (Quest quest) {
		quests.Add (quest);
	}

	public void RemoveQuest (Quest quest) {
		quests.Remove (quest);
	}
}
