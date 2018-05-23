using UnityEngine;

public class ReactionCollection : MonoBehaviour {

	public Reaction [] reactions = new Reaction[0];

	// Use this for initialization
	void Start () {
		for (int i = 0; i < reactions.Length; i++) {
			reactions [i].Init ();
		}
	}
	
	public void React () {
		for (int i = 0; i < reactions.Length; i++) {
			reactions [i].React (this);
		}
	}
}
