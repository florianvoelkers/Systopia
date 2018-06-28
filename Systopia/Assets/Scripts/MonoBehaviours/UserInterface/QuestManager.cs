using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour {

	[SerializeField] private PlayerQuests playerQuests;
	[SerializeField] private Text currentQuestsHeader;
	[SerializeField] private GameObject currentQuestsPanel;
	[SerializeField] private Text finishedQuestsHeader;
	[SerializeField] private GameObject finishedQuestsPanel;
	[SerializeField] private GameObject quest;
	[SerializeField] private Sprite questSprite;
	[SerializeField] private Sprite selectedQuestSprite;
	[SerializeField] private Text questTitle;
	[SerializeField] private Text questDescription;
	[SerializeField] private Text stateDescription;

	private List <GameObject> currentQuests = new List<GameObject> ();
	private List <GameObject> finishedQuests = new List<GameObject> ();

	private void OnEnable () {
		for (int i = playerQuests.quests.Count - 1; i >= 0; i--) {
			if (playerQuests.quests [i].isQuestFinished) {
				finishedQuests.Add (Instantiate (quest, finishedQuestsPanel.transform));
				finishedQuests [finishedQuests.Count - 1].GetComponentInChildren <Text> ().text = playerQuests.quests [i].questTitle;
				int localIndex = i;
				int localObjectIndex = finishedQuests.Count - 1;
				UnityEngine.Events.UnityAction questSelection = () => {
					this.SelectQuest (localIndex, localObjectIndex);
				};
				finishedQuests [finishedQuests.Count - 1].GetComponent <Button> ().onClick.AddListener (questSelection);
			} else {
				currentQuests.Add (Instantiate (quest, currentQuestsPanel.transform));
				currentQuests [currentQuests.Count - 1].GetComponentInChildren <Text> ().text = playerQuests.quests [i].questTitle;
				int localIndex = i;
				int localObjectIndex = currentQuests.Count - 1;
				UnityEngine.Events.UnityAction questSelection = () => {
					this.SelectQuest (localIndex, localObjectIndex);
				};
				currentQuests [currentQuests.Count - 1].GetComponent <Button> ().onClick.AddListener (questSelection);
			}
		}
	}

	private void OnDisable () {
		questTitle.text = "";
		questDescription.text = "";
		stateDescription.text = "";
		for (int i = 0; i < currentQuestsPanel.transform.childCount; i++) {
			Destroy (currentQuestsPanel.transform.GetChild (i).gameObject);
		}
		currentQuests.Clear ();
		for (int i = 0; i < finishedQuestsPanel.transform.childCount; i++) {
			Destroy (finishedQuestsPanel.transform.GetChild (i).gameObject);
		}
		finishedQuests.Clear ();
	}

	private void SelectQuest (int index, int objectIndex) {
		questDescription.text = "";
		for (int i = 0; i < currentQuests.Count; i++) {
			currentQuests [i].GetComponent<Image> ().sprite = questSprite;
		}
		for (int i = 0; i < finishedQuests.Count; i++) {
			finishedQuests [i].GetComponent<Image> ().sprite = questSprite;
		}
		if (playerQuests.quests[index].isQuestFinished)
			finishedQuests [objectIndex].GetComponent <Image> ().sprite = selectedQuestSprite;
		else
			currentQuests [objectIndex].GetComponent <Image> ().sprite = selectedQuestSprite;
		
		questTitle.text = playerQuests.quests [index].questTitle;
		questDescription.text = playerQuests.quests [index].questDescription;
		for (int i = 0; i < playerQuests.quests[index].states.Count; i++) {
			if (!playerQuests.quests [index].states [i].isStateFinished) {
				stateDescription.text = playerQuests.quests [index].states [i].stateDescription;
				break;
			}
		}
	}
}