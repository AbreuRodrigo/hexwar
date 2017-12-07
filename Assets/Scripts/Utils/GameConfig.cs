using UnityEngine;

public class GameConfig
{
    public const string LAYER_HEXAGON = "Hexagon";
    public const string LAYER_IGNORE_RAYCAST = "Ignore Raycast";

    public const string PLAYER_HEXAGON_NAME = "HexagonPlayerBase";

    public const string PLAYER_UNIQUE_ID = "hexwar_player_unique_id";

    public const int BASE_TURN_TIMER = 30;

    public static Color openEnvironmentColor = new Color(0.69f, 0.76f, 1f);
    public static Color openEnvironmentColorOnHover = new Color(0.45f, 0.55f, 1f);
    public static Color closedEnvironmentColor = new Color(0.69f, 0.76f, 1f);

    //NETWORK
    public class NetworkCode
    {
        public static short REGISTER_PLAYER = 1;
        public static short SEARCH_GAME = 2;
        public static short START_GAMEPLAY = 3;
        public static short RECEIVE_TURN_TOKEN = 4;
    }
}