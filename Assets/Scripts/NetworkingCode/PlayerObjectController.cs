﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class PlayerObjectController : NetworkBehaviour
{
    [SyncVar] public int ConnectionID;
    [SyncVar] public int PlayerIdNumber;
    [SyncVar] public ulong PlayerSteamID;
    [SyncVar(hook = nameof(PlayerNameUpdate))] public string playerName;

    private CustomNetworkManager manager;

    private CustomNetworkManager Manager
    {
        get{
            if(manager != null)
            {
                return manager;
            }
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyController.instance.FindLocalPlayer();
        LobbyController.instance.UpdateLobbyName();
    }

    public override void OnStartClient()
    {
        Manager.gamePlayers.Add(this);
        LobbyController.instance.UpdateLobbyName();
        LobbyController.instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        manager.gamePlayers.Remove(this);
        LobbyController.instance.UpdatePlayerList();
    }

    [Command]
    private void CmdSetPlayerName(string name)
    {
        this.PlayerNameUpdate(this.playerName, name);
    }

    public void PlayerNameUpdate(string oldName, string newName)
    {
        if(isServer)
        {
            this.playerName = newName;
        }

        if(isClient)
        {
            LobbyController.instance.UpdatePlayerList();
        }
    }



}
