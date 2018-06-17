public class SceneReaction : Reaction {

	public string sceneName;
	public string startingPointInLoadedScene;
	public PlayerLocation playerLocation;

	private SceneController sceneController;

	protected override void SpecificInit () {
		sceneController = FindObjectOfType <SceneController> ();
	}

	protected override void ImmediateReaction () {
		playerLocation.currentPositionSet = false;
		playerLocation.startingPositionName = startingPointInLoadedScene;
		playerLocation.currentSceneName = sceneName;
		sceneController.FadeAndLoadScene (this);
	}
}
