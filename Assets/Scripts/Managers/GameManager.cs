using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Components")]
    public UIManager uiManager;
    public SelectionCircle selection;
    public Player localPlayer;
    public TargetSelection targetSelection;
    public PreviewMove previewMove;
    public GameStateManager gameStateManager;

    [Header("Opponents")]
    public List<Player> opponents;

    public EGameState GameState
    {
        get { return gameStateManager.gameState; }
    }

    [Header("Game Stats")]
    public int currentTurn = 1;
    public EGamePhase currentPhase;

    private Vector3 topPosition = new Vector3(0, 0.8f, 0);
    private Vector3 bottomPosition = new Vector3(0, -0.8f, 0);

    public Hexagon lastHexagon;
    public Hexagon selectedHexagon;

    private bool turnTimerIsRunning = false;
    
    private Dictionary<EGamePhase, EGamePhase> phaseOrder = null;
        
    void Start()
    {
        Random.InitState((int)System.Environment.TickCount);

        phaseOrder = new Dictionary<EGamePhase, EGamePhase>()
        {
            { EGamePhase.MaintenancePhase, EGamePhase.CombatOrExplorationPhase },
            { EGamePhase.CombatOrExplorationPhase, EGamePhase.ClearPhase },
            { EGamePhase.ClearPhase, EGamePhase.WaitPhase },
            { EGamePhase.WaitPhase, EGamePhase.MaintenancePhase }
        };

        if (uiManager != null && localPlayer != null)
        {
            uiManager.UpdateTopUI(localPlayer, currentTurn);
            uiManager.SetCurrentTurnUI(currentTurn.ToString());
        }

        DoPhaseTransition();
    }

    private void Update()
    {
        if (Input.touchCount == 2)
        {
            selectedHexagon = null;
            ClearSelection();
            return;
        }

        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began || Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            Collider2D collider = hit.collider;

            if (collider != null && currentPhase == EGamePhase.CombatOrExplorationPhase)
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
            if (target.isPlayer && currentTurn > target.spawningTurn)
            {
                SelectPlayerHexagon(target);
            }
            else if (targetSelection != null && selectedHexagon != null && 
                selectedHexagon.TestIsNeighbour(target)  && target.state == ELandState.Visible)
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
                }

                if(targetSelection != null && selectedHexagon.troop > 1)
                {
                    targetSelection.EnableAddButton();
                    targetSelection.DisableSubtractButton();
                    targetSelection.DisableConfirmButton();
                }

                lastHexagon = target;
            }
        }
    }

    public void AddToPreview()
    {
        if(previewMove != null && 
            selectedHexagon != null && 
            selectedHexagon.troop > 1 && 
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

    /// <summary>
    /// Click the "Move" button to move troops to the target hexagon land
    /// </summary>
    public void ConfirmMoveTroop()
    {
        if (previewMove != null && currentPhase == EGamePhase.CombatOrExplorationPhase)
        {
            int amount = previewMove.Amount;

            StartCoroutine(TradeTroop(selectedHexagon, lastHexagon, amount));           

            localPlayer.actions--;

            uiManager.UpdateTopUI(localPlayer, currentTurn);

            if(localPlayer.actions <= 0)
            {
                DoPhaseTransition();
            }
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
            if (selection != null)
            {
                selection.transform.position = targetHexagon.transform.position;
                selection.gameObject.SetActive(true);
            }

            selectedHexagon = targetHexagon;
            lastHexagon = targetHexagon;

            localPlayer.selectedHexagonId = selectedHexagon.id;
        }
    }

    public void ReceiveTurnToken()
    {
        SnoringMarkerManager.Instance.ReturnAllObjectsInUseToPool();

        currentTurn++;

        DoPhaseTransition();

        uiManager.UpdateTopUI(localPlayer, currentTurn);
    }
    
    private void DoPhaseTransition()
    {
        currentPhase = phaseOrder[currentPhase];
        ProcessPhase();
    }

    public void SkipTurn()
    {
        PassTurnTokenAhead();
    }

    public void EndTurn()
    {
        PassTurnTokenAhead();
    }

    private void PassTurnTokenAhead()
    {
        turnTimerIsRunning = false;

        localPlayer.actions = 0;

        currentPhase = EGamePhase.WaitPhase;
        ProcessPhase();        
        
        StartCoroutine(WaitAndRegainTurnToken());        
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
        to.spawningTurn = currentTurn;

        SnoringMarkerManager.Instance.RequestSnoringMarker(to);

        ClearSelection();
    }

    //TODO Remove it later
    private IEnumerator WaitAndRegainTurnToken()
    {
        yield return new WaitForSecondsRealtime(3);

        ReceiveTurnToken();
    }

    private IEnumerator TurnShiftTimer()
    {
        int initSeconds = GameConfig.BASE_TURN_TIMER + (localPlayer.level - 1) * 2;
        uiManager.SetTurnTimer(initSeconds);

        while (initSeconds >= 0 && turnTimerIsRunning)
        {
            uiManager.SetTurnTimer(initSeconds);

            initSeconds--;

            if(initSeconds == 0)
            {
                PassTurnTokenAhead();
            }

            yield return new WaitForSecondsRealtime(1);
        }
    }

    private void ProcessPhase()
    {
        switch (currentPhase)
        {
            case EGamePhase.MaintenancePhase:
                localPlayer.actions = localPlayer.initialActions;

                localPlayer.AddUnitToAllHexagonsPlayerHas();
                    
                uiManager.UpdateUiForMaintenancePhase();

                DoPhaseTransition();
                break;
            case EGamePhase.CombatOrExplorationPhase:
                turnTimerIsRunning = true;

                uiManager.UpdateUiForCombatOrExplorationPhase();

                StartCoroutine(TurnShiftTimer());
                break;
            case EGamePhase.ClearPhase:
                uiManager.UpdateUiForClearPhase();
                break;
            case EGamePhase.WaitPhase:
                turnTimerIsRunning = false;
                uiManager.UpdateUiForWaitPhase();
                break;
        }

        uiManager.UpdateTopUI(localPlayer, currentTurn);
    }
}