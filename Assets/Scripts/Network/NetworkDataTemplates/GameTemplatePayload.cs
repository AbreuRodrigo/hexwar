namespace Hexwar
{
    [System.Serializable]
    public class GameTemplatePayload
    {
        public string id;
        public string gameName;
        public string mapSize;
        public int level;
        public int maxPlayers;
        public int currentPlayers;
        public long mapSeed;
    }
}