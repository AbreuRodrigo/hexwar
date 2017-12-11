[System.Serializable]
public class PlayerMovePayload
{
    public int source;//where did the attack came from (hexagon id)
    public int target;//where is the attack targeting (hexagon id)
    public int troop;//amount of troop units passed
}