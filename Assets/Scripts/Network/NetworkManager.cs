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

namespace Hexwar
{
    public class NetworkManager : MonoBehaviour
    {
        private UDPSender sender = null;
        private UDPReceiver receiver = null;

        private IPEndPoint localEndPoint = null;
        private IPEndPoint remoteEndPoint = null;

        [Header("Remote Connection")]
        public bool localConnection = true;
        public string localServer = "192.168.0.10";
        public string remoteServer = "54.241.148.177";
        public int port = 7777;

        [Header("Local Connection")]
        public string localIp = "0.0.0.0";
        public int localPort = 0;

        public Player localPlayer;
        public bool isLoading = true;

        private PlayerListTemplatePayload playersTemplatePayload;
        public PlayerReferencePayload[] PlayersTemplate
        {
            get
            {
                return playersTemplatePayload.players;
            }
        }

        private Queue<Action> tasks = new Queue<Action>();

        private Dictionary<short, ServerResponse> networkResponse = null;

        public delegate void ServerResponse(string message);

        private string movementMessage = null;

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
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
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
            { GameConfig.NetworkCode.SEARCH_GAME, OnSearchGame },
            { GameConfig.NetworkCode.START_GAMEPLAY, OnStartGameplay },
            { GameConfig.NetworkCode.RECEIVE_TURN_TOKEN, OnReceiveTurnToken },
            { GameConfig.NetworkCode.RECEIVE_OPPONENT_MOVE_ACTION, OnReceiveOpponentMove }
        };
        }

        private void OnDestroy()
        {
            Debug.Log("Shutting down...");
        }

        private void Initialize()
        {
            //TODO REMOVE HARDCODE LATER
            localEndPoint = new IPEndPoint(IPAddress.Parse(localIp), localPort);
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(localConnection ? localServer : remoteServer), port);

            if (receiver == null)
            {
                receiver = new UDPReceiver(localEndPoint, remoteEndPoint);
                localEndPoint = receiver.LocalEndPoint;
            }
            if (sender == null)
            {
                sender = new UDPSender(localEndPoint, remoteEndPoint);
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
            if (networkResponse != null && response != null)
            {
                networkResponse[response.code](response.message);
            }
        }

        //NETWORK ACTION METHODS

        public void SearchGame(int selectedOption)
        {
            EMapSize selectedMapSize = EMapSize.GIANT;//(EMapSize)System.Enum.GetValues(typeof(EMapSize)).GetValue(selectedOption);

            SearchGamePayload searchPayload = new SearchGamePayload();
            searchPayload.cliendId = localPlayer.clientId;
            searchPayload.level = localPlayer.level;
            searchPayload.mapSize = (int)selectedMapSize;

            string data = JsonUtility.ToJson(searchPayload);

            SendPayload(GameConfig.NetworkCode.SEARCH_GAME, data, localPlayer.clientId);
        }

        public void PassTurnTokenToNextPlayer()
        {
            PassTurnTemplatePayload payload = new PassTurnTemplatePayload();
            payload.playerGameName = GameSetup.currentGame;
            payload.playerTurnIndex = GameSetup.localPlayerTurnId;

            string data = JsonUtility.ToJson(payload);
            SendPayload(GameConfig.NetworkCode.RECEIVE_TURN_TOKEN, data, localPlayer.clientId);
        }

        public void PassMoveToOpponentPlayer(int source, int baseUnits, int target, int movingUnits)
        {
            PlayerMovePayload payload = new PlayerMovePayload();
            payload.clientId = localPlayer.clientId;
            payload.source = source;
            payload.baseUnits = baseUnits;
            payload.target = target;
            payload.movingUnits = movingUnits;

            string data = JsonUtility.ToJson(payload);
            SendPayload(GameConfig.NetworkCode.RECEIVE_OPPONENT_MOVE_ACTION, data, localPlayer.clientId);
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

            string cliendID = PlayerPrefs.GetString(GameConfig.PLAYER_UNIQUE_ID);

            if (string.IsNullOrEmpty(cliendID))
            {
                cliendID = SystemInfo.deviceUniqueIdentifier;
            }

            PlayerTemplatePayload playerTemplate = new PlayerTemplatePayload();
            playerTemplate.playerName = localPlayer.playerName;
            playerTemplate.level = localPlayer.level;
            playerTemplate.xp = localPlayer.xp;
            playerTemplate.clientId = cliendID;

            string data = JsonUtility.ToJson(playerTemplate);

            SendPayload(GameConfig.NetworkCode.REGISTER_PLAYER, data, cliendID);
        }

        IEnumerator ProcessTask()
        {
            while (true)
            {
                if (tasks != null && tasks.Count > 0)
                {
                    Action task = tasks.Dequeue();

                    if (task != null)
                    {
                        task.Invoke();
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

            tasks.Enqueue(SaveLocalPlayerClientID);

            isLoading = false;

            Debug.Log("OnRegisterPlayer: " + message);
        }

        private void SaveLocalPlayerClientID()
        {
            PlayerPrefs.SetString(GameConfig.PLAYER_UNIQUE_ID, localPlayer.clientId);
        }

        public void OnSearchGame(string message)
        {
            if (message != null)
            {
                GameTemplatePayload gameTemplatePayload = JsonUtility.FromJson<GameTemplatePayload>(message);
                playersTemplatePayload = JsonUtility.FromJson<PlayerListTemplatePayload>(message);

                GameSetup.mapSeed = gameTemplatePayload.mapSeed;
                GameSetup.currentGame = gameTemplatePayload.gameName;
                GameSetup.mapSize = EMapSize.GIANT;//(EMapSize)Enum.Parse(typeof(EMapSize), gameTemplatePayload.mapSize, true);

                tasks.Enqueue(WaitForOpponentsTask);
            }
        }

        public void OnStartGameplay(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                GameSetup.gameplayData = JsonUtility.FromJson<PlayerGameplayListPayload>(message);

                foreach (PlayerGameplayPayload gameplayInfo in GameSetup.gameplayData.playersData)
                {
                    if (gameplayInfo.clientId.Equals(localPlayer.clientId))
                    {
                        GameSetup.localPlayerTurnId = gameplayInfo.turnIndex;
                        GameSetup.playerColor = gameplayInfo.color;
                        localPlayer.initialHexagon = gameplayInfo.initialHexagon;
                    }
                }

                tasks.Enqueue(LoadGameSceneTask);
            }
        }

        public void OnReceiveTurnToken(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                tasks.Enqueue(ReceiveTurnTokenTask);
            }
        }

        public void OnReceiveOpponentMove(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                this.movementMessage = message;
                tasks.Enqueue(ReceiveOpponentsMovement);
            }
        }

        //TASKS
        private void LoadGameSceneTask()
        {
            SceneManager.LoadScene("Gameplay");
        }

        private void WaitForOpponentsTask()
        {
            UILobbyManager.Instance.HideSearching();
            UILobbyManager.Instance.ShowLoading();
            UILobbyManager.Instance.ShowWaitingOpponentns();
        }

        private void ReceiveTurnTokenTask()
        {
            GameManager.Instance.ReceiveTurnToken();
        }

        private void ReceiveOpponentsMovement()
        {
            if (this.movementMessage != null)
            {
                PlayerMovePayload p = JsonUtility.FromJson<PlayerMovePayload>(this.movementMessage);
                GameManager.Instance.SetOpponentsMovementPayload(p);
                GameManager.Instance.ReceiveOpponentsMovement();
            }
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
    }
}