using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    [Header("Components")]
    public UIManager uiManager;
    public SelectionArrow selection;
    public Player localPlayer;
    public TrailManager trailManager;
    public TargetSelection targetSelection;

    [Header("Opponents")]
    public List<Player> opponents;

    [Header("Game Stats")]
    public int currentTurn = 1;
    public EGamePhase currentPhase;
   
    private int frameControl = 5;
    private int currentFrame = 0;
    private Hexagon lastHexagon;

    private Hexagon selectedHexagon;

    public GameObject trail;
    public int poolSize;

    void Awake()
    {
        instance = this;
        Random.InitState((int)System.Environment.TickCount);
    }

    void Start()
    {
        if(uiManager != null && localPlayer != null)
        {
            uiManager.UpdatePlayerUI(localPlayer);
            uiManager.SetCurrentTurnUI(currentTurn.ToString());
        }
    }

    public void SetPlayerInitialHexLand(Hexagon hexLand)
    {
        if (localPlayer != null && hexLand != null)
        {
            Hexagon playerHexagon = hexLand;
            playerHexagon.gameObject.name = GameConfig.PLAYER_HEXAGON_NAME;
            playerHexagon.SetAsPlayer(localPlayer);

            localPlayer.AddHexLand(playerHexagon);

            Vector3 p = playerHexagon.gameObject.transform.position;
            p.z = Camera.main.transform.position.z;

            Camera.main.transform.position = p;
        }
    }

    private void Update()
    {
        if (Input.touchCount == 2)
        {
            selectedHexagon = null;
            DisableSelection();
            return;
        }

        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            Collider2D collider = hit.collider;

            if (collider != null)
            {
                if(targetSelection != null)
                {
                    targetSelection.gameObject.SetActive(false);
                }

                selectedHexagon = collider.gameObject.GetComponent<Hexagon>();

                if (selectedHexagon != null && selectedHexagon.isPlayer)
                {
                    EnableSelection(selectedHexagon);

                    if (lastHexagon == null)
                    {
                        lastHexagon = selectedHexagon;
                        lastHexagon.OnRayCastHit();
                    }
                    else if (lastHexagon != null && lastHexagon.index != selectedHexagon.index)
                    {
                        selectedHexagon.OnRayCastHit();
                        lastHexagon.OnRayCastExit();
                        lastHexagon = selectedHexagon;
                    }
                    else
                    {
                        lastHexagon = selectedHexagon;
                    }
                }
                else
                {
                    if(targetSelection != null && selectedHexagon.state == ELandState.Visible)
                    {
                        targetSelection.transform.SetParent(collider.transform);
                        targetSelection.transform.position = new Vector3(0, 0, 0);
                        targetSelection.transform.localPosition = new Vector3(0, 0.7f, 0);
                        targetSelection.gameObject.SetActive(true);
                    }

                    if (lastHexagon != null)
                    {
                        lastHexagon.OnRayCastExit();
                        lastHexagon = null;
                    }

                    DisableSelection();
                }
            }
            else if (lastHexagon != null)
            {
                lastHexagon.OnRayCastExit();
                lastHexagon = null;

                DisableSelection();
            }
        }
    }

    private void EnableSelection(Hexagon targetHexagon)
    {
        if (selection != null && targetHexagon != null)
        {
            selection.transform.position = targetHexagon.transform.position;
            selection.gameObject.SetActive(true);

            if (trailManager != null)
            {
                trailManager.Enable(
                    new Vector3(selection.transform.position.x, selection.transform.position.y, -2)
                );
            }
        }
    }

    private void DisableSelection()
    {
        if (trailManager != null)
        {
            trailManager.Disable();
        }

        if (selection != null)
        {
            selection.gameObject.SetActive(false);
        }
    }
}