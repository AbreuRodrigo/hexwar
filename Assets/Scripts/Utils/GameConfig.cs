using UnityEngine;

public class GameConfig
{
    public const string LAYER_HEXAGON = "Hexagon";
    public const string LAYER_IGNORE_RAYCAST = "Ignore Raycast";

    public const string PLAYER_HEXAGON_NAME = "HexagonPlayerBase";

    public static Color player1Color = new Color(0.69f, 1f, 0.69f);
    public static Color player1ColorOnHover = new Color(0.15f, 1f, 0.27f);


    public static Color openEnvironmentColor = new Color(0.69f, 0.76f, 1f);
    public static Color openEnvironmentColorOnHover = new Color(0.45f, 0.55f, 1f);
    public static Color closedEnvironmentColor = new Color(0.69f, 0.76f, 1f);
    public static Color enemyColor = new Color(1f, 0.69f, 0.69f);
    public static Color enemyColorOnHover = new Color(1f, 0.69f, 0.69f);
}