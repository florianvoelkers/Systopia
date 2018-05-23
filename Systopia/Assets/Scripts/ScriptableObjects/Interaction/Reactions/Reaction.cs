using UnityEngine;

[System.Serializable]
[CreateAssetMenu (menuName = "Interaction/Reaction")]
public abstract class Reaction : ScriptableObject {

	public void Init () {
		SpecificInit ();
	}

	protected virtual void SpecificInit () {}

	public void React (MonoBehaviour monoBehaviour) {
		ImmediateReaction ();
	}

	protected abstract void ImmediateReaction ();
}
