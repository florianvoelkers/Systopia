﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletManager : MonoBehaviour {

	[SerializeField] private GameObject navigationSelections;
	[SerializeField] private GameObject[] tabletPanels;

	private GameObject[] selectors;

	private void Awake () {
		selectors = new GameObject[navigationSelections.transform.childCount];
		for (int i = 0; i < navigationSelections.transform.childCount; i++) {
			selectors[i] = navigationSelections.transform.GetChild (i).gameObject;
		}

	}

	public void ShowInventory () {
		SetEveryPanelInactive ();
		selectors [0].SetActive (true);
		tabletPanels [0].SetActive (true);
	}

	public void ShowCharacter () {
		SetEveryPanelInactive ();
		selectors [1].SetActive (true);
	}

	public void ShowNotes () {
		SetEveryPanelInactive ();
		selectors [2].SetActive (true);
	}

	public void ShowMap () {
		SetEveryPanelInactive ();
		selectors [3].SetActive (true);
	}

	private void SetEveryPanelInactive () {
		tabletPanels [0].SetActive (false);
		for (int i = 0; i < selectors.Length; i++) {
			selectors [i].SetActive (false);
		}
	}

}
