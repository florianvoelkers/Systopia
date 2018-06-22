using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnfangsSzenenScript : MonoBehaviour {

    private TextManager textmanager;
	private SceneController sceneController;
    private Color red;
    private Color green;
    private Color green2;
    private Color yellow;
    float timeLeft = 34.0f;

    //rot BA0000FF
    //grün 00F448FF
    //gelb ECDC00FF
    // Use this for initialization
    void Start () {
        red = new Color(0.8f, 0.5f, 0.45f);
        green = new Color(0.4f, 0.9f, 0.5f);
        green2 = new Color(0.1f, 0.9f, 0.5f);
        yellow = new Color(0.9f, 0.95f, 0.5f);

        textmanager = FindObjectOfType<TextManager>();
		sceneController = FindObjectOfType<SceneController> ();

        textmanager.DisplayMessage("Wie könnt ihr diese Diktatur nur gutheißen ?", red, 0.5f);
        textmanager.DisplayMessage("Die Technokraten verlangen von uns unsere Freiheit und unsere Individualität aufzugeben.", red, 2f);
        textmanager.DisplayMessage("Wogapowi war der einzige, der sich ihnen in den Weg gestellt hat, er hat uns vereint im Kampf gegen die Technokratie", red, 5f);


        textmanager.DisplayMessage("Ihr Avantgardisten seid doch nur auf euren eigenen Profit aus.", green, 8.5f);
        textmanager.DisplayMessage("Ihr lebt vom Chaos, profitiert vom Leid anderer. Ihr seid eine Bande von Verbrechern.", green, 11f);

        textmanager.DisplayMessage("", red, 14f);


        textmanager.DisplayMessage("LÜGNER!!!", red, 14.5f);

        textmanager.DisplayMessage("neeeeiinnnn!!!", green2, 17.3f);

        textmanager.DisplayMessage("Engin schmeiß diesen Störenfried raus!", Color.white, 18f);

        textmanager.DisplayMessage("Geht klar Chef", yellow, 20f);

        textmanager.DisplayMessage("Raus mit dir du Schuft", yellow, 21f);

        textmanager.DisplayMessage("Argh...", red, 22f);

        textmanager.DisplayMessage("Lang lebe Wogapowi!!!", red, 28f);

    }
    //textmanager.DisplayMessage(message,textColor,delay);
    //
    // Update is called once per frame
    void Update () {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            StartGame();
        }
    }

    void StartGame() {
		sceneController.StartGame ();
    }
}
