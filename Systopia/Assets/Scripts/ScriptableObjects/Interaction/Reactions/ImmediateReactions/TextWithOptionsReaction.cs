using UnityEngine;

public class TextWithOptionsReaction : Reaction {

	public string message;
	public Color textColor = Color.white;
	public float delay;
	public string [] options = new string[0];

	private TextManager textManager;

	protected override void SpecificInit () {
		textManager = FindObjectOfType<TextManager> ();
	}

	protected override void ImmediateReaction () {
		textManager.DisplayMessageWithOptions (message, textColor, delay, options);
	}
}
