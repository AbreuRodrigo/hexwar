using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLobbyManager : MonoBehaviour
{
    public void OnCreateGameButtonClick()
    {
        GameTemplatePayload gameTemplatePayload = new GameTemplatePayload();
        gameTemplatePayload.gameName = UILobbyManager.Instance.GetGameName();
        gameTemplatePayload.mapSize = UILobbyManager.Instance.GetMapSize();

        string jsonStr = JsonUtility.ToJson(gameTemplatePayload);

        NetworkManager.Instance.SendPayload(GameConfig.NetworkCode.CREATE_GAME, jsonStr);
    }
}