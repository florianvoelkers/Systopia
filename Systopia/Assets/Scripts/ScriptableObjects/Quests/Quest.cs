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
	public List <ScriptableObject> rewards;
}
