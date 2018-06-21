using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour {

	[SerializeField] private GameObjectData [] changeableGameObjects;
	[SerializeField] private AllConditions allConditions;

	// Use this for initialization
	private IEnumerator Start () {
		yield return null;

		for (int i = 0; i < changeableGameObjects.Length; i++) {
			for (int j = 0; j < allConditions.conditions.Length; j++) {
				if (changeableGameObjects [i].condition == allConditions.conditions [j]) {
					if (allConditions.conditions [j].satisfied == changeableGameObjects [i].state)
						changeableGameObjects [i].gameObject.SetActive (changeableGameObjects [i].state);
				}
			}
		}
	}

}
