using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerManager : NetworkBehaviour
{
    public GameObject playerPrefab;
    public GameObject GUI;
    public PlayerController playerController;
    public PlayerCamera playerCamera;
    public GameObject cameraHolder;

    // Start is called before the first frame update
    void Start()
    {
        playerPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Map_v1")
        {
            if(!playerPrefab.activeSelf)
            {
                playerPrefab.SetActive(true);
                if(hasAuthority)
                {
                    GUI.SetActive(true);
                    playerController.enabled = true;
                    playerCamera.enabled = true;
                    cameraHolder.SetActive(true);
                }
            }
        }
    }
}
