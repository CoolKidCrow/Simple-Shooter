using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

public class PlayerListItem : MonoBehaviour
{
    public string playerName;
    public int connectionId;
    public ulong PlayerSteamID;
    
    public Text playerNameText;
    public Image readyIcon;
    public bool ready;

    public void ChangeReadyStatus()
    {
        if(ready)
        {
            readyIcon.enabled = true;
        }else
        {
            readyIcon.enabled = false;
        }
    }

    public void SetPlayerName()
    {
        playerNameText.text = playerName;
        ChangeReadyStatus();
    }

}
