using UnityEngine;

public class Interactable : MonoBehaviour {

	public Texture2D interactionCursor;
	public string interactionName;
	public Transform interactionLocation;
	public ConditionCollection [] conditionCollections = new ConditionCollection[0];
	public ReactionCollection defaultReactionCollection;

	private bool showInteractionName;
	private Font font;
	private GUIStyle infoStyle;

	private void OnEnable () {
		showInteractionName = false;
		font = Resources.Load ("Fonts/Modern Sans Serif") as Font;
		infoStyle = new GUIStyle ();
		infoStyle.normal.textColor = Color.white;
		infoStyle.alignment = TextAnchor.MiddleLeft;
		infoStyle.font = font;
		infoStyle.fontSize = 23;
	}

	public void Interact () {
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		for (int i = 0; i < conditionCollections.Length; i++) {
			if (conditionCollections [i].CheckAndReact ())
				return;
		}
		defaultReactionCollection.React ();
	}

	public void ShowCustomCursor () {
		showInteractionName = true;
		Cursor.SetCursor (interactionCursor, Vector2.zero, CursorMode.Auto);
	}

	public void HideCustomCursor () {
		showInteractionName = false;
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
	}

	void OnGUI () {
		if (showInteractionName) {
			GUI.Label (new Rect (Input.mousePosition.x + 50f, Screen.height - Input.mousePosition.y - 10f, 250, 60), interactionName, infoStyle);
		}
	}
}
