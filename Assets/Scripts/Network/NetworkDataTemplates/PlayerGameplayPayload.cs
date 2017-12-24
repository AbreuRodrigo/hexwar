namespace Hexwar
{
    [System.Serializable]
    public class PlayerGameplayPayload
    {
        public string clientId;
        public short turnIndex;
        public short color;
        public int level;
        public int initialHexagon;
    }
}