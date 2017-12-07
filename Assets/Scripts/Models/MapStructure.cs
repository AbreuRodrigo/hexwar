[System.Serializable]
public class MapStructure
{
    public int max;
    public int players;

    public MapStructure(int max, int players)
    {
        this.max = max;
        this.players = players;
    }
}