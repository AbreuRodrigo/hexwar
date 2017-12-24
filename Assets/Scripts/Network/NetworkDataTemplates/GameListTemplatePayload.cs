using System.Collections;
using System.Collections.Generic;

namespace Hexwar
{
    [System.Serializable]
    public class GameListTemplatePayload
    {
        public List<GameTemplatePayload> games = new List<GameTemplatePayload>();
    }
}