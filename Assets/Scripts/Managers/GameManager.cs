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

    private Vector3 topPosition = new Vector3(0, 0.8f, 0);
    private Vector3 bottomPosition = new Vector3(0, -0.8f, 0);

    public Hexagon lastHexagon;
    public Hexagon selectedHexagon;

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
            playerHexagon.SetAsPlayer(localPlayer, localPlayer.troop);

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
            ClearSelection();
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
            else
            {
                ClearSelection();
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

        if(target != null && lastHexagon != null && target.id == lastHexagon.id)
        {
            ClearSelection();
            return;
        }

        if (target != null)
        {
            if (target.isPlayer)
            {
                SelectPlayerHexagon(target);
            }
            else if (targetSelection != null && selectedHexagon != null && target.state == ELandState.Visible)
            {
                ENeighborPosition neighborPosition = target.GetNeighborPositionInRelationTo(selectedHexagon);
                
                targetSelection.gameObject.SetActive(false);
                targetSelection.transform.localScale = Vector3.zero;
                targetSelection.transform.SetParent(obj.transform);
                targetSelection.transform.position = Vector3.zero;
                targetSelection.transform.localPosition = ConvertNeightPositionToActionOrientation(neighborPosition);

                targetSelection.Activate(target.isEnemy ? ETargetSelectionType.EnemyLand : ETargetSelectionType.EmptyLand);

                if(targetSelection.transform.localPosition.y > 0)
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
                    previewMove.Reposition(target.transform.position);                    
                    previewMove.gameObject.SetActive(true);

                    previewMove.ResetFunctionalities();
                    targetSelection.ResetFunctionalities();
                }

                lastHexagon = target;
            }
            else
            {
                previewMove.gameObject.SetActive(false);
                previewMove.ResetFunctionalities();

                targetSelection.gameObject.SetActive(false);
            }
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
        ValidateAddButton();
    }

    public void SubtractFromPreview()
    {
        if (previewMove != null)
        {
            previewMove.Subtract();
        }

        ValidateConfirmMoveButton();
        ValidateSubtractButton();
        ValidateAddButton();
    }

    public void ConfirmMoveTroop()
    {
        if (previewMove != null)
        {
            int amount = previewMove.Amount;

            StartCoroutine(TradeTroop(selectedHexagon, lastHexagon, amount));
        }
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

    private void ValidateAddButton()
    {
        if (previewMove != null)
        {
            if (previewMove.Amount >= selectedHexagon.troop - 1)
            {
                targetSelection.DisableAddButton();
            }
            else
            {
                targetSelection.EnableAddButton();
            }
        }
    }

    private void ClearSelection()
    {
        if (selection != null)
        {
            selection.gameObject.SetActive(false);
        }

        selectedHexagon = null;
        lastHexagon = null;

        previewMove.gameObject.SetActive(false);
        targetSelection.gameObject.SetActive(false);

        localPlayer.selectedHexagonId = -1;
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

    private void SelectPlayerHexagon(Hexagon targetHexagon)
    {
        if (targetHexagon != null && targetHexagon.isPlayer)
        {
            EnableSelection(targetHexagon.transform.position);

            selectedHexagon = targetHexagon;
            lastHexagon = targetHexagon;

            localPlayer.selectedHexagonId = selectedHexagon.id;
        }
    }

    private void EnableSelection(Vector3 pos)
    {
        if (selection != null)
        {
            selection.transform.position = pos;
            selection.gameObject.SetActive(true);
        }
    }

    private IEnumerator TradeTroop(Hexagon from, Hexagon to, int amount)
    {
        to.SetAsPlayer(localPlayer, 0);
        targetSelection.gameObject.SetActive(false);
        previewMove.gameObject.SetActive(false);

        to.SetLandSprite(MapManager.Instance.plainSprite);

        MapManager.Instance.RevealNeighbors(to);

        Vector3 bumpVec = new Vector3(1.2f, 1.2f, 1);

        while (amount > 0)
        {
            from.troop--;
            from.hud.SetTroop(from.troop);

            to.troop++;
            to.hud.SetTroop(to.troop);

            LeanTween.scale(from.hud.troopMarker.gameObject, bumpVec, 0.25f)
                     .setEasePunch();

            LeanTween.scale(to.hud.troopMarker.gameObject, bumpVec, 0.25f)
                     .setEasePunch();

            amount--;

            yield return new WaitForSecondsRealtime(0.125f);
        }

        from.hud.troopMarker.gameObject.transform.localScale = Vector3.one;
        to.hud.troopMarker.gameObject.transform.localScale = Vector3.one;

        ClearSelection();
    }
}