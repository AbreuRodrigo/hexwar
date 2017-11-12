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
    public SelectionCircle selection;
    public Player localPlayer;
    public TrailManager trailManager;
    public TargetSelection targetSelection;
    public PreviewMove previewMove;

    [Header("Opponents")]
    public List<Player> opponents;

    [Header("Game Stats")]
    public int currentTurn = 1;
    public EGamePhase currentPhase;

    public Vector3 topPosition = new Vector3(0, -0.8f, 0);
    public Vector3 bottomPosition = new Vector3(0, 0.8f, 0);

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
            playerHexagon.troop = localPlayer.troop;

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
                HandleInputForHexagon(collider.gameObject);
                HandleInputForTargetSelection(collider.gameObject);
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

    private void HandleInputForTargetSelection(GameObject obj)
    {
        TargetSelection target = obj.gameObject.GetComponent<TargetSelection>();

        if(target != null)
        {
        }
    }

    private void HandleInputForHexagon(GameObject obj)
    {
        Hexagon target = obj.gameObject.GetComponent<Hexagon>();

        if (target != null)
        {
            if (target != null && target.isPlayer)
            {
                EnableSelection(target);

                selectedHexagon = target;
                localPlayer.selectedHexagonId = selectedHexagon.id;

                if (lastHexagon == null)
                {
                    lastHexagon = target;
                    lastHexagon.OnRayCastHit();
                }
                else if (lastHexagon != null && lastHexagon.id != target.id)
                {
                    target.OnRayCastHit();
                    lastHexagon.OnRayCastExit();
                    lastHexagon = target;
                }
                else
                {
                    lastHexagon = target;
                }
            }
            else if (targetSelection != null && lastHexagon != null && target.state == ELandState.Visible)
            {
                ENeighborPosition neighborPosition = target.GetNeighborPositionInRelationTo(lastHexagon);

                targetSelection.gameObject.SetActive(false);
                targetSelection.transform.localScale = Vector3.zero;
                targetSelection.transform.SetParent(obj.transform);
                targetSelection.transform.position = Vector3.zero;
                targetSelection.transform.localPosition = ConvertNeightPositionToActionOrientation(neighborPosition);

                targetSelection.Activate(target.isEnemy ? ETargetSelectionType.EnemyLand : ETargetSelectionType.EmptyLand);

                float y = targetSelection.transform.localPosition.y;

                if(y < 0)
                {
                    targetSelection.RearrangeContentTop();
                }
                else
                {
                    targetSelection.RearrangeContentBottom();
                }

                targetSelection.gameObject.SetActive(true);

                if(previewMove != null)
                {
                    previewMove.Reset(target.transform.position);
                    previewMove.gameObject.SetActive(true);
                }
            }

            if (lastHexagon != null)
            {
                //lastHexagon.OnRayCastExit();
                //lastHexagon = null;
            }

            //DisableSelection();
        }
        else if (lastHexagon != null)
        {
            //lastHexagon.OnRayCastExit();
            //lastHexagon = null;

            //DisableSelection();
        }
    }

    public void AddToPreview()
    {
        if(previewMove != null && 
            selectedHexagon != null && 
            selectedHexagon.troop > 0 && 
            previewMove.Amount < selectedHexagon.troop - 1)
        {
            previewMove.Add();
        }

        ValidateConfirmMoveButton();
        ValidateSubtractButton();
    }

    public void SubtractFromPreview()
    {
        if (previewMove != null)
        {
            previewMove.Subtract();
        }

        ValidateConfirmMoveButton();
        ValidateSubtractButton();
    }

    private void ValidateConfirmMoveButton()
    {
        if(previewMove != null)
        {
            if(previewMove.Amount <= 0)
            {
                targetSelection.DisableSubtractButton();
            }
            else
            {
                targetSelection.EnableSubtractButton();
            }            
        }
    }

    private void ValidateSubtractButton()
    {
        if (previewMove != null)
        {
            if (previewMove.Amount <= 0)
            {
                targetSelection.DisableConfirmButton();
            }
            else
            {
                targetSelection.EnableConfirmButton();
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

    private Vector3 ConvertNeightPositionToActionOrientation(ENeighborPosition neighborPosition)
    {
        Vector3 pos = Vector3.zero;

        switch(neighborPosition)
        {
            case ENeighborPosition.TopLeft:
            case ENeighborPosition.TopRight:
            case ENeighborPosition.TopMiddle:
                pos = topPosition;
                break;
            case ENeighborPosition.BottomLeft:
            case ENeighborPosition.BottomRight:
            case ENeighborPosition.BottomMiddle:
                pos = bottomPosition;
                break;
        }

        return pos;
    }
}