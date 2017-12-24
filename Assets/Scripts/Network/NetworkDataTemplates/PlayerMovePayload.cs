namespace Hexwar
{
    [System.Serializable]
    public class PlayerMovePayload
    {
        public string clientId;
        public int source;
        public int baseUnits;
        public int target;
        public int movingUnits;
    }
}