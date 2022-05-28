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

    public void SetPlayerName()
    {
        playerNameText.text = playerName;
    }

}
