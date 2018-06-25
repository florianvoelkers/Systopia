using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	[SerializeField] private CanvasGroup faderCanvasGroup;
	[SerializeField] private float fadeDuration = 1f;
	[SerializeField] private string startingSceneName = "Taverne";
	[SerializeField] private string initialStartingPointName = "DoorToStreet";
	[SerializeField] private PlayerLocation playerLocation;
	[SerializeField] private GameObject loadScreen;

	private bool isFading;

	public void StartGame () {
		loadScreen.SetActive (true);
		StartCoroutine (ShowStartScene ());
	}

	public void StartCutscene (string cutsceneName) {
		loadScreen.SetActive (true);
		StartCoroutine (LoadSceneAndSetActive (cutsceneName));
	}

	public void StartGameFromSaveFile (bool gameStarted) {
		if (gameStarted)
			StartCoroutine (FadeAndSwitchScenes (playerLocation.currentSceneName));
		else
			StartCoroutine (StartFromSaveFile ());
	}

	private IEnumerator ShowStartScene () {
		faderCanvasGroup.alpha = 1f;
		playerLocation.startingPositionName = initialStartingPointName;
		yield return StartCoroutine (FadeAndSwitchScenes (startingSceneName));
		StartCoroutine (Fade (0f));
	}

	private IEnumerator StartFromSaveFile () {
		faderCanvasGroup.alpha = 1f;
		string currentScene = startingSceneName;
		if (playerLocation.currentSceneName != "")
			currentScene = playerLocation.currentSceneName;

		yield return StartCoroutine (LoadSceneAndSetActive (currentScene));
		StartCoroutine (Fade (0f));
	}

	public void FadeAndLoadScene (SceneReaction sceneReaction) {
		if (!isFading) {
			loadScreen.SetActive (true);
			StartCoroutine (FadeAndSwitchScenes (sceneReaction.sceneName));
		}
	}

	public void FadeAndLoadFightScene (string sceneName, System.Action callback) {
		if (!isFading) {
			loadScreen.SetActive (true);
			StartCoroutine (FadeAndSwitchScenesToFight (sceneName, callback));
		}
	}

	private IEnumerator FadeAndSwitchScenesToFight (string sceneName, System.Action callback) {
		yield return StartCoroutine (Fade (1f));
		yield return SceneManager.UnloadSceneAsync (SceneManager.GetActiveScene ().buildIndex);
		yield return StartCoroutine (LoadSceneAndSetActive (sceneName));
		yield return StartCoroutine (Fade (0f));
		callback ();
		loadScreen.SetActive (false);
	}

	private IEnumerator FadeAndSwitchScenes (string sceneName) {
		yield return StartCoroutine (Fade (1f));
		yield return SceneManager.UnloadSceneAsync (SceneManager.GetActiveScene ().buildIndex);
		yield return StartCoroutine (LoadSceneAndSetActive (sceneName));
		yield return StartCoroutine (Fade (0f));
		loadScreen.SetActive (false);
	}

	private IEnumerator LoadSceneAndSetActive (string sceneName) {
		yield return SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);
		Scene newlyLoadedScene = SceneManager.GetSceneAt (SceneManager.sceneCount - 1);
		SceneManager.SetActiveScene (newlyLoadedScene);
		loadScreen.SetActive (false);
	}

	private IEnumerator Fade (float finalAlpha) {
		isFading = true;
		faderCanvasGroup.blocksRaycasts = true;
		float fadeSpeed = Mathf.Abs (faderCanvasGroup.alpha - finalAlpha) / fadeDuration;
		while (!Mathf.Approximately (faderCanvasGroup.alpha, finalAlpha)) {
			faderCanvasGroup.alpha = Mathf.MoveTowards (faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
			yield return null;
		}
		isFading = false;
		faderCanvasGroup.blocksRaycasts = false;
	}
}
