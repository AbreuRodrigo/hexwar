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

    public SelectionArrow selection;

    private int frameControl = 5;
    private int currentFrame = 0;
    private Hexagon lastHexagon;
    private Hexagon playerHexagon;

    private Hexagon selectedHexagon;

    void Awake()
    {
        instance = this;
        Random.InitState((int)System.Environment.TickCount);
    }

    public void SetPlayerHexagon(Hexagon playerHexagon)
    {
        if (playerHexagon != null)
        {
            this.playerHexagon = playerHexagon;
            this.playerHexagon.gameObject.name = GameConfig.PLAYER_HEXAGON_NAME;

            Vector3 p = this.playerHexagon.gameObject.transform.position;
            p.z = Camera.main.transform.position.z;

            Camera.main.transform.position = p;
        }
    }

    private void Update()
    {
        if (Input.touchCount == 2)
        {
            return;
        }

        if (currentFrame % frameControl == 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            Collider2D collider = hit.collider;

            if (collider != null)
            {
                selectedHexagon = collider.gameObject.GetComponent<Hexagon>();

                if (selectedHexagon != null && selectedHexagon.isPlayer)
                {
                    if (lastHexagon == null)
                    {
                        lastHexagon = selectedHexagon;
                        lastHexagon.OnRayCastHit();
                    }
                    else if (lastHexagon != null && lastHexagon.id != selectedHexagon.id)
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
            }
            else if (lastHexagon != null)
            {
                lastHexagon.OnRayCastExit();
                lastHexagon = null;
            }

            if (currentFrame >= 60)
            {
                currentFrame = 0;
            }
        }

        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Ended ||
            Input.GetMouseButtonUp(0))
        {
            if(selectedHexagon != null && selectedHexagon.isPlayer)
            {
                EnableSelection(selectedHexagon);
            }
            else
            {
                DisableSelection();
            }
        }

        currentFrame++;
    }

    private void EnableSelection(Hexagon targetHexagon)
    {
        if(selection != null && targetHexagon != null)
        {
            selection.transform.position = targetHexagon.transform.position;
            selection.gameObject.SetActive(true);
        }
    }

    private void DisableSelection()
    { 
        if(selection != null)
        {
            selection.gameObject.SetActive(false);
        }
    }
}