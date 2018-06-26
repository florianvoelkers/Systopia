using UnityEngine;

public class SceneReaction : Reaction {

	public string sceneName;
	public string startingPointInLoadedScene;
	public PlayerLocation playerLocation;
	public GameObject camera;

	private SceneController sceneController;
	private StreetCameraSaver streetCameraSaver;

	protected override void SpecificInit () {
		sceneController = FindObjectOfType <SceneController> ();
		streetCameraSaver = Resources.Load <StreetCameraSaver> ("StreetCameraSaver");
	}

	protected override void ImmediateReaction () {
		if (camera != null) {
			streetCameraSaver.position = camera.transform.position;
			streetCameraSaver.rotation = camera.transform.rotation;
		}
		playerLocation.currentPositionSet = false;
		playerLocation.startingPositionName = startingPointInLoadedScene;
		playerLocation.currentSceneName = sceneName;
		sceneController.FadeAndLoadScene (this);
	}
}
