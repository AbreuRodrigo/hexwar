using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexwar
{
    public class CombatAndExplorePhase : GamePhase
    {
        public CombatAndExplorePhase(GamePhase nextPhase)
        {
            this.nextPhase = nextPhase;
        }

        public override void OnFinish()
        {
            throw new NotImplementedException();
        }

        public override void OnHandle(Player player)
        {
            throw new NotImplementedException();
        }

        public override void OnInitialize()
        {
            throw new NotImplementedException();
        }
    }
}