using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamePhase
{
    protected GamePhase nextPhase;
    public EGamePhase gamePhase;
    
    public abstract void OnHandle(Player player);

    public abstract void OnInitialize();

    public abstract void OnFinish();

    public bool MoveToNextState()
    {
        if(nextPhase != null)
        {
            nextPhase.OnInitialize();
            return true;
        }

        return false;
    }
}