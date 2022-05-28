using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;
using System.Linq;

public class LobbyController : MonoBehaviour
{
    public static LobbyController instance;

    public Text lobbyNameText;

    public GameObject playerListViewContent;
    public GameObject playerListItemPrefab;
    public GameObject localPlayerObject;

    public ulong currentLobbyID;
    public bool playerItemCreated = false;
    private List<PlayerListItem> playerListItems = new List<PlayerListItem>();
    public PlayerObjectController localPlayerController;

    private CustomNetworkManager manager;

    private CustomNetworkManager Manager
    {
        get
        {
            if(manager != null)
            {
                return manager;
            }
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void UpdateLobbyName()
    {
        currentLobbyID = Manager.GetComponent<SteamLobby>().currentLobbyID;
        lobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(currentLobbyID), "name");
    }

    public void UpdatePlayerList()
    {
        if(!playerItemCreated) CreateHostPlayerItem();

        if(playerListItems.Count < manager.gamePlayers.Count) CreateClientPlayerItem();

        if(playerListItems.Count > manager.gamePlayers.Count) RemovePlayerItem();

        if(playerListItems.Count == Manager.gamePlayers.Count) UpdatePlayerItem();
    }

    public void FindLocalPlayer()
    {
        localPlayerObject = GameObject.Find("LocalGamePlayer");
        localPlayerController = localPlayerObject.GetComponent<PlayerObjectController>();
    }

    public void CreateHostPlayerItem()
    {
        foreach(PlayerObjectController player in Manager.gamePlayers)
        {
            GameObject newPlayerItem = Instantiate(playerListItemPrefab) as GameObject;
            PlayerListItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();

            newPlayerItemScript.playerName = player.playerName;
            newPlayerItemScript.connectionId = player.ConnectionID;
            newPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
            newPlayerItemScript.SetPlayerName();

            newPlayerItem.transform.SetParent(playerListViewContent.transform);
            newPlayerItem.transform.localScale = Vector3.one;

            playerListItems.Add(newPlayerItemScript);
        }
        playerItemCreated = true;
    }

    public void CreateClientPlayerItem()
    {
        foreach(PlayerObjectController player in Manager.gamePlayers)
        {
            if(!playerListItems.Any(b => b.connectionId == player.ConnectionID))
            {
                GameObject newPlayerItem = Instantiate(playerListItemPrefab) as GameObject;
                PlayerListItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();

                newPlayerItemScript.playerName = player.playerName;
                newPlayerItemScript.connectionId = player.ConnectionID;
                newPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
                newPlayerItemScript.SetPlayerName();

                newPlayerItem.transform.SetParent(playerListViewContent.transform);
                newPlayerItem.transform.localScale = Vector3.one;

                playerListItems.Add(newPlayerItemScript);
            }
        }
    }

    public void UpdatePlayerItem()
    {
        foreach (PlayerObjectController player in Manager.gamePlayers)
        {
            foreach(PlayerListItem playerListItemScript in playerListItems)
            {
                if(playerListItemScript.connectionId == player.ConnectionID)
                {
                    playerListItemScript.playerName = player.playerName;
                    playerListItemScript.SetPlayerName();
                }
            }
        }
    }

    public void RemovePlayerItem()
    {
        List<PlayerListItem> playerListItemsToRemove = new List<PlayerListItem>();

        foreach (PlayerListItem playerListItem in playerListItems)
        {
            if(!Manager.gamePlayers.Any(b => b.ConnectionID == playerListItem.connectionId))
            {
                playerListItemsToRemove.Add(playerListItem);
            }
        }
        if(playerListItemsToRemove.Count > 0)
        {
            foreach (PlayerListItem removeItem in playerListItemsToRemove)
            {
                GameObject objectToRemove = removeItem.gameObject;
                playerListItems.Remove(removeItem);
                Destroy(objectToRemove);
                objectToRemove = null;
            }
        }
    }

}
