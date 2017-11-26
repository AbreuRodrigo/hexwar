using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

    private UDPSender sender;
    private UDPReceiver receiver;
    private IPEndPoint remoteEndPoint;

    public LoadingIcon loading;
    private bool isLoading = true;
    
    public string ip;
    public int port;
    public Player localPlayer;
        
    private Dictionary<short, ServerResponse> networkResponse = null;

    public delegate void ServerResponse(string message);

    private static NetworkManager instance;
    public static NetworkManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        StartCoroutine(RequestClientId());
        StartCoroutine(KeepLoadingUntilRetrieveClientID());

        networkResponse = new Dictionary<short, ServerResponse>()
        {
            { GameConfig.NetworkCode.REGISTER_PLAYER, OnRegisterPlayer },
            { GameConfig.NetworkCode.CREATE_GAME, OnCreateGame }
        };

        Initialize();        
    }

    private void OnDestroy()
    {
        Debug.Log("Shutting down...");
        receiver.StopReceiving();
    }

    private void Initialize()
    {
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

        if (sender == null)
        {
            sender = new UDPSender(remoteEndPoint);
        }
        if (receiver == null)
        {
            receiver = new UDPReceiver(remoteEndPoint);
        }
    }

    public void SendPayload(short code, string message)
    {
        Payload p = new Payload();
        p.code = code;
        p.message = message;
        p.clientID = localPlayer.clientId;

        sender.SendPayload(p);
    }
    
    public void ProcessResponse(Response response)
    {
        if(networkResponse != null && response != null)
        {
            networkResponse[response.code](response.message);
        }
    }

    //NETWORK RESPONSE METHODS

    private void OnRegisterPlayer(string message)
    {
        isLoading = false;

        if(!localPlayer.clientId.Equals(message))
        {
            localPlayer.clientId = message;
        }

        Debug.Log("OnRegisterPlayer: " + message);
    }
    
    private void OnCreateGame(string message)
    {
        GameTemplatePayload gameTemplatePayload = JsonUtility.FromJson<GameTemplatePayload>(message);

        UILobbyManager.Instance.CreateNewGame(gameTemplatePayload);

        Debug.Log("OnCreateGame: " + gameTemplatePayload);
    }

    IEnumerator RequestClientId()
    {
        yield return new WaitForSecondsRealtime(3);

        PlayerTemplatePayload playerTemplate = new PlayerTemplatePayload();
        playerTemplate.playerName = localPlayer.playerName;
        playerTemplate.level = localPlayer.level;
        playerTemplate.xp = localPlayer.xp;
        playerTemplate.clientId = localPlayer.clientId;

        string message = JsonUtility.ToJson(playerTemplate);

        SendPayload(GameConfig.NetworkCode.REGISTER_PLAYER, message);
    }

    IEnumerator KeepLoadingUntilRetrieveClientID()
    {
        loading.Show();

        while (isLoading)
        {
            yield return null;
        }

        loading.Hide();
    }
}