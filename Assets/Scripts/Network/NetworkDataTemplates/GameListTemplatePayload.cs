using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameListTemplatePayload
{
    public List<GameTemplatePayload> games = new List<GameTemplatePayload>();
}
