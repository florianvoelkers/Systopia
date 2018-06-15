using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletManager : MonoBehaviour {

	[SerializeField] private GameObject navigationSelections;
	[SerializeField] private GameObject [] tabletPanels;
	[SerializeField] private GameObject tablet;

	private GameObject[] selectors;

	private void Awake () {
		selectors = new GameObject[navigationSelections.transform.childCount];
		for (int i = 0; i < navigationSelections.transform.childCount; i++) {
			selectors[i] = navigationSelections.transform.GetChild (i).gameObject;
		}
	}

	private void Update () {
		if (Input.GetKeyDown (KeyCode.T)) {
			tablet.SetActive (!tablet.activeSelf);
		}
	}

	public void SelectTabletPanel (int selectedTabletPanel) {
		SetEveryPanelInactive ();
		selectors [selectedTabletPanel].SetActive (true);
		tabletPanels [selectedTabletPanel].SetActive (true);
	}

	private void SetEveryPanelInactive () {
		for (int i = 0; i < selectors.Length; i++) {
			selectors [i].SetActive (false);
			tabletPanels [i].SetActive (false);
		}
	}

}
