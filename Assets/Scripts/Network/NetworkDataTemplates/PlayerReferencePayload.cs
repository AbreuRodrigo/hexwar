namespace Hexwar
{
    [System.Serializable]
    public class PlayerReferencePayload
    {
        public string clientID;
        public string address;
        public int port;
        public int level;
        public long xp;
        public string name;
        public int turnIndex;
    }
}