using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;

public class KlausEndeScript : MonoBehaviour {

    private TextManager textmanager;
    private SceneController sceneController;
    float timeLeft = 16.0f;
    public GameObject videoPlayer;
    public GameObject crowd;
    public GameObject streetAmbience;
    private bool playedVideo = false;
    private bool finishedDialog = false;

    // Use this for initialization
    void Start()
    {

        textmanager = FindObjectOfType<TextManager>();
        sceneController = FindObjectOfType<SceneController>();

        textmanager.DisplayMessage("Klaus: Haltet ein ihr Narren!", Color.white, 0.5f);
        textmanager.DisplayMessage("Dies ist nicht die vorgesehene Lösung.", Color.white, 4f);
        textmanager.DisplayMessage("Alles war ein schrecklicher Fehler und wir können ihn jetzt endlich rückgängig machen.", Color.white, 8f);

        textmanager.DisplayMessage("", Color.white, 16f);


        textmanager.DisplayMessage("Als damals der TechnoLapse ausgelöst wurde, lag die Welt in Trümmern.", Color.white, 128f);
        textmanager.DisplayMessage("Ich habe lange den Zusammenhang zu unserem Projekt gar nicht gesehen.", Color.white, 136f);
        textmanager.DisplayMessage("Aber dann hab ich doch eine Analyse durchgeführt.", Color.white, 142f);
        textmanager.DisplayMessage("Es gab einen Rundungsfehler,", Color.white, 147f);
        textmanager.DisplayMessage("der extreme Lösungen unseres Problems zuließ und zu einer Spaltung des Programms führte.", Color.white, 151f);


        textmanager.DisplayMessage("Wir sind schuld an der ganzen Zerstörung.", Color.white, 160f);
        textmanager.DisplayMessage("Wegen uns sind jetzt so viele Menschen tot… ", Color.white, 165f);
        textmanager.DisplayMessage("Aber wir haben alles getan, um unseren Fehler wieder auszubügeln.", Color.white, 170f);
        textmanager.DisplayMessage("Und heute haben wir es geschafft!!", Color.white, 175f);
        textmanager.DisplayMessage("Die KI ist wieder zusammengefügt", Color.white, 179f);
        textmanager.DisplayMessage("Ihre gespaltenen Programme haben die Führung der Technokraten und der Avantgardisten übernommen.", Color.white, 185f);
        textmanager.DisplayMessage("Dieser Fehler ist nun behoben.", Color.white, 191f);
        textmanager.DisplayMessage(" Hiermit löse ich die Gruppierungen auf und erkläre den Krieg für beendet.", Color.white, 199f);

    }
    //textmanager.DisplayMessage(message,textColor,delay);
    //sceneController.StartGame ();
    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Debug.Log("keinezeit left");
            if (!playedVideo)
            {
                StartVideo();
            }
            else if (!finishedDialog && playedVideo)
            {
                EndVideo();
            }
            else {
                EndGame();
            }
        }
    }


    void StartVideo() {
        Debug.Log("Start video");
        videoPlayer.SetActive(true);
        crowd.SetActive(false);
        streetAmbience.SetActive(false);
        playedVideo = true;
        timeLeft = 120;
    }

    void EndVideo() {
        videoPlayer.SetActive(false);
        streetAmbience.SetActive(true);
        timeLeft = 90;
        finishedDialog = true;
    }

    void EndGame()
    {

		Application.Quit ();
    }
}
