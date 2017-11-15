using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public EGameState gameState;

    public void ChangeToInitializingState()
    {
        gameState = EGameState.Initializing;
    }

    public void ChangeToGameplayState()
    {
        gameState = EGameState.Gameplay;
    }

    public void ChangeToPauseState()
    {
        gameState = EGameState.Pause;
    }

    public void ChangeToGameOverState()
    {
        gameState = EGameState.GameOver;
    }

    public void ChangeToLobbyState()
    {
        gameState = EGameState.Lobby;
    }
}
