using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour {
    private UDPSender sender = null;
    private UDPReceiver receiver = null;

    private IPEndPoint localEndPoint = null;
    private IPEndPoint remoteEndPoint = null;

    public string ip = "127.0.0.1";
    public int port = 7777;
    public int localPort = 11777;
    public Player localPlayer;
    public int responseSearchGame;

    private Queue<Action> tasks = new Queue<Action>();
    private string response;

    public bool isLoading = false;

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
        Initialize();

        StartCoroutine(RequestClientId());
        StartCoroutine(ProcessTask());

        UILobbyManager.Instance.StartLoadingScreen();

        networkResponse = new Dictionary<short, ServerResponse>()
        {
            { GameConfig.NetworkCode.REGISTER_PLAYER, OnRegisterPlayer },
            { GameConfig.NetworkCode.CREATE_GAME, OnCreateGame },
            { GameConfig.NetworkCode.JOIN_GAME, OnJoinGame },
            { GameConfig.NetworkCode.RETRIEVE_GAMES, OnRetriveGames },
            { GameConfig.NetworkCode.SEARCH_GAME, OnSearchedGame }
        };
    }

    private void OnDestroy()
    {
        Debug.Log("Shutting down...");
        //receiver.StopReceiving();
    }

    private void Initialize()
    {
        localEndPoint = new IPEndPoint(IPAddress.Parse(ip), localPort);
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

        if(sender == null)
        {
            sender = new UDPSender(localEndPoint, remoteEndPoint);
        }
        if (receiver == null)
        {
            receiver = new UDPReceiver(localEndPoint, remoteEndPoint);
        }
    }

    public void SendPayload(short code, string message, string cliendId)
    {
        Payload p = new Payload();
        p.code = code;
        p.message = message;
        p.clientID = cliendId;

        receiver.Init();
        sender.SendPayload(p);
    }

    public void SendPayload(Payload payload)
    {
        try
        {
            string data = Serialize<Payload>(payload);
            data = FixStringMessage(data);
            sender.Send(data);
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }

    public void ProcessResponse(Response response)
    {
        if(networkResponse != null && response != null)
        {
            networkResponse[response.code](response.message);
        }
    }

    //NETWORK ACTION METHODS

    public void SearchGame(int selectedOption)
    {
        EMapSize selectedMapSize = (EMapSize)System.Enum.GetValues(typeof(EMapSize)).GetValue(selectedOption);
        
        SearchGamePayload searchPayload = new SearchGamePayload();
        searchPayload.cliendId = localPlayer.clientId;
        searchPayload.level = localPlayer.level;
        searchPayload.mapSize = (int) selectedMapSize;

        string data = JsonUtility.ToJson(searchPayload);

        SendPayload(GameConfig.NetworkCode.SEARCH_GAME, data, localPlayer.clientId);
    }

    public void RetriveGameList()
    {
        PlayerTemplatePayload playerTemplatePayload = new PlayerTemplatePayload();
        playerTemplatePayload.clientId = localPlayer.clientId;
        
        string data = JsonUtility.ToJson(playerTemplatePayload);

        SendPayload(GameConfig.NetworkCode.RETRIEVE_GAMES, data, string.Empty);
    }

    private static string Serialize<T>(T toSerialize)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        StringWriter textWriter = new StringWriter();

        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        ns.Add("", "");

        xmlSerializer.Serialize(textWriter, toSerialize, ns);
        return textWriter.ToString();
    }

    private string FixStringMessage(string strMsg)
    {
        string cliendIdStr = "<clientID />";
        string clientIdStrProper = "<clientID>null</clientID>";

        if (strMsg.Contains(cliendIdStr))
        {
            return strMsg.Replace(cliendIdStr, clientIdStrProper);
        }

        return strMsg;
    }

    //COROUTINES

    IEnumerator RequestClientId()
    {
        yield return new WaitForSecondsRealtime(3);

        PlayerTemplatePayload playerTemplate = new PlayerTemplatePayload();
        playerTemplate.playerName = localPlayer.playerName;
        playerTemplate.level = localPlayer.level;
        playerTemplate.xp = localPlayer.xp;
        playerTemplate.clientId = localPlayer.clientId;

        string data = JsonUtility.ToJson(playerTemplate);

        SendPayload(GameConfig.NetworkCode.REGISTER_PLAYER, data, localPlayer.clientId);
    }

    IEnumerator ProcessTask()
    {
        while(true)
        {
            if (tasks != null && tasks.Count > 0)
            {
                Action action = tasks.Dequeue();

                if (action != null)
                {
                    action.Invoke();
                }
            }

            yield return null;
        }        
    }


    //RESPONSES


    public void OnRegisterPlayer(string message)
    {
        if (!localPlayer.clientId.Equals(message))
        {
            localPlayer.clientId = message;
        }

        Debug.Log("OnRegisterPlayer: " + message);

        NetworkManager.Instance.RetriveGameList();
    }

    public void OnCreateGame(string message)
    {
        GameTemplatePayload gameTemplatePayload = JsonUtility.FromJson<GameTemplatePayload>(message);

        UILobbyManager.Instance.CreateNewGame(gameTemplatePayload);

        Debug.Log("OnCreateGame: " + gameTemplatePayload);
    }

    public void OnJoinGame(string message)
    {
        Debug.Log("OnJoinGame: " + message);
    }

    public void OnRetriveGames(string message)
    {
        GameListTemplatePayload gameListPayload = JsonUtility.FromJson<GameListTemplatePayload>(message);

        foreach (GameTemplatePayload game in gameListPayload.games)
        {
            UILobbyManager.Instance.EnqueueRowsItem(game);
        }
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void OnSearchedGame(string message)
    {
        if (message != null)
        {
            GameTemplatePayload gameTemplatePayload = JsonUtility.FromJson<GameTemplatePayload>(message);
            GameSetup.mapSeed = gameTemplatePayload.mapSeed;

            tasks.Enqueue(LoadGameScene);
        }
    }
}