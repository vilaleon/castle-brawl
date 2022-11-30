using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkGameHandler : NetworkBehaviour
{
    public MenuUIHandler menuHandler;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        Debug.Log(clientId + " connected!");
    }

    private void OnClientDisconnectCallback(ulong clientId)
    {
        Debug.Log(clientId + " disconnected!");
    }

    [ClientRpc]
    public void ToBattleClientRpc(ClientRpcParams clientRpcParams = default)
    {
        menuHandler.LobbyToFight();
    }
}
