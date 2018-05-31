public class SceneReaction : Reaction {

	public string sceneName;
	public string startingPointInLoadedScene;

	//private SceneController sceneController;

	protected override void SpecificInit () {
		//sceneController = FindObjectOfType <SceneController> ();
	}

	protected override void ImmediateReaction () {
		//playerSaveData
		//sceneController.FadeAndLoadScene (this);
	}
}
