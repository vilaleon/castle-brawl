using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Netcode.Transports.UTP;
using System;

public class LobbyHandler : NetworkBehaviour
{
    private GameManager gameManager;
    private MenuUIHandler menuHandler;
    public TMP_InputField address;
    public TMP_InputField port;

    private void OnEnable()
    {
        address.text = NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address;
        port.text =  NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port.ToString();
    }

    public void UpdateConnectionData()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            address.text,
            UInt16.Parse(port.text)
        );
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void Disconnect()
    {
        NetworkManager.Singleton.Shutdown();
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        menuHandler = GetComponentInParent<MenuUIHandler>();
    }

    public void MultiplayerBattleStart()
    {
        gameManager.FighterSelectedMultiplayer();
        gameManager.GetComponent<NetworkGameHandler>().ToBattleClientRpc(new ClientRpcParams {
            Send = new ClientRpcSendParams {
                TargetClientIds = new ulong[] { 0, 1 }
            }
        });
    }
}