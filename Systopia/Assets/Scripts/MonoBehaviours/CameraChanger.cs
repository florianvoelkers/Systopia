using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour {

	[SerializeField] private GameObject firstCamera;
	[SerializeField] private GameObject secondCamera;
    [SerializeField] private GameObject player;

    private void Update()
    {
        if (player.transform.position.x < 3.5f)
        {
            firstCamera.SetActive(true);
            secondCamera.SetActive(false);
        }
        else {
            secondCamera.SetActive(true);
            firstCamera.SetActive(false);

        }
    }
}
